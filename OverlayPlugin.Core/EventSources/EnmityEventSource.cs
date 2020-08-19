using Advanced_Combat_Tracker;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RainbowMage.OverlayPlugin.MemoryProcessors;

namespace RainbowMage.OverlayPlugin.EventSources
{
    public class EnmityEventSource : EventSourceBase
    {
        private EnmityMemory memory;
        private List<EnmityMemory> memoryCandidates;
        private bool memoryValid = false;

        const int MEMORY_SCAN_INTERVAL = 3000;

        // General information about the target, focus target, hover target.  Also, enmity entries for main target.
        private const string EnmityTargetDataEvent = "EnmityTargetData";
        // All of the mobs with aggro on the player.  Equivalent of the sidebar aggro list in game.
        private const string EnmityAggroListEvent = "EnmityAggroList";
        // State of combat, both act and game.
        private const string InCombatEvent = "InCombat";

        [Serializable]
        internal class InCombatDataObject {
            public string type = InCombatEvent;
            public bool inACTCombat = false;
            public bool inGameCombat = false;
        };
        InCombatDataObject sentCombatData;

        // Unlike "sentCombatData" which caches sent data, this variable caches each update.
        private bool lastInGameCombat = false;
        private const int endEncounterOutOfCombatDelayMs = 5000;
        CancellationTokenSource endEncounterToken;

        public BuiltinEventConfig Config { get; set; }

        public EnmityEventSource(TinyIoCContainer container) : base(container)
        {
            var repository = container.Resolve<FFXIVRepository>();

            if (repository.GetLanguage() == FFXIV_ACT_Plugin.Common.Language.Chinese)
            {
                memoryCandidates = new List<EnmityMemory>()
                {
                    new EnmityMemory52(container)
                };
            }
            else if (repository.GetLanguage() == FFXIV_ACT_Plugin.Common.Language.Korean)
            {
                memoryCandidates = new List<EnmityMemory>()
                {
                    new EnmityMemory50(container)
                };
            }
            else
            {
                memoryCandidates = new List<EnmityMemory>()
                {
                    new EnmityMemory53(container)
                };
            }

            RegisterEventTypes(new List<string> {
                EnmityTargetDataEvent, EnmityAggroListEvent,
            });
            RegisterCachedEventType(InCombatEvent);
        }

        public override Control CreateConfigControl()
        {
            return null;
        }

        public override void LoadConfig(IPluginConfig cfg)
        {
            this.Config = container.Resolve<BuiltinEventConfig>();

            this.Config.EnmityIntervalChanged += (o, e) =>
            {
                if (memory != null)
                    timer.Change(0, this.Config.EnmityIntervalMs);
            };
        }

        public override void Start()
        {
            memoryValid = false;
            timer.Change(0, MEMORY_SCAN_INTERVAL);
        }

        public override void SaveConfig(IPluginConfig config)
        {
        }

        protected override void Update()
        {
            try
            {
#if TRACE
                var stopwatch = new Stopwatch();
                stopwatch.Start();
#endif

                if (memory == null)
                {
                    foreach (var candidate in memoryCandidates)
                    {
                        if (candidate.IsValid())
                        {
                            memory = candidate;
                            memoryCandidates = null;
                            break;
                        }
                    }
                }

                if (memory == null || !memory.IsValid())
                {
                    if (memoryValid)
                    {
                        timer.Change(MEMORY_SCAN_INTERVAL, MEMORY_SCAN_INTERVAL);
                        memoryValid = false;
                    }

                    return;
                } else if (!memoryValid)
                {
                    // Increase the update interval now that we found our memory
                    timer.Change(this.Config.EnmityIntervalMs, this.Config.EnmityIntervalMs);
                    memoryValid = true;
                }

                // Handle optional "end encounter of combat" logic.
                bool inGameCombat = memory.GetInCombat();
                // If we've transitioned to being out of combat, start a delayed task to end the ACT encounter.
                if (Config.EndEncounterOutOfCombat && lastInGameCombat && !inGameCombat)
                {
                    endEncounterToken = new CancellationTokenSource();
                    Task.Run(async delegate
                    {
                        await Task.Delay(endEncounterOutOfCombatDelayMs, endEncounterToken.Token);
                        ActGlobals.oFormActMain.Invoke((Action)(() =>
                        {
                            ActGlobals.oFormActMain.EndCombat(true);
                        }));
                    });
                }
                // If combat starts again, cancel any outstanding tasks to stop the ACT encounter.
                // If the task has already run, this will not do anything.
                if (inGameCombat && endEncounterToken != null)
                {
                    endEncounterToken.Cancel();
                    endEncounterToken = null;
                }
                lastInGameCombat = inGameCombat;

                if (HasSubscriber(InCombatEvent))
                {
                    bool inACTCombat = Advanced_Combat_Tracker.ActGlobals.oFormActMain.InCombat;
                    if (sentCombatData == null || sentCombatData.inACTCombat != inACTCombat || sentCombatData.inGameCombat != inGameCombat)
                    {
                        if (sentCombatData == null)
                            sentCombatData = new InCombatDataObject();
                        sentCombatData.inACTCombat = inACTCombat;
                        sentCombatData.inGameCombat = inGameCombat;
                        this.DispatchAndCacheEvent(JObject.FromObject(sentCombatData));
                    }
                }

                bool targetData = HasSubscriber(EnmityTargetDataEvent);
                bool aggroList = HasSubscriber(EnmityAggroListEvent);
                if (!targetData && !aggroList)
                    return;

                var combatants = memory.GetCombatantList();

                if (targetData)
                {
                    // See CreateTargetData() below
                    this.DispatchEvent(CreateTargetData(combatants));
                }
                if (aggroList)
                {
                    this.DispatchEvent(CreateAggroList(combatants));
                }

#if TRACE
                Log(LogLevel.Trace, "UpdateEnmity: {0}ms", stopwatch.ElapsedMilliseconds);
#endif
            }
            catch (Exception ex)
            {
                Log(LogLevel.Error, "UpdateEnmity: {0}", ex.ToString());
            }
        }

        [Serializable]
        internal class EnmityTargetDataObject
        {
            public string type = EnmityTargetDataEvent;
            public Combatant Target;
            public Combatant Focus;
            public Combatant Hover;
            public Combatant TargetOfTarget;
            public List<EnmityEntry> Entries;
        }

        [Serializable]
        internal class EnmityAggroListObject
        {
            public string type = EnmityAggroListEvent;
            public List<AggroEntry> AggroList;
        }

        internal JObject CreateTargetData(List<Combatant> combatants)
        {
            EnmityTargetDataObject enmity = new EnmityTargetDataObject();
            try
            {
                var mychar = memory.GetSelfCombatant();
                enmity.Target = memory.GetTargetCombatant();
                if (enmity.Target != null)
                {
                    if (enmity.Target.TargetID > 0)
                    {
                        enmity.TargetOfTarget = combatants.FirstOrDefault((Combatant x) => x.ID == (enmity.Target.TargetID));
                    }
                    enmity.Target.Distance = mychar.DistanceString(enmity.Target);

                    if (enmity.Target.Type == ObjectType.Monster)
                    {
                        enmity.Entries = memory.GetEnmityEntryList(combatants);
                    }
                }

                enmity.Focus = memory.GetFocusCombatant();
                enmity.Hover = memory.GetHoverCombatant();
                if (enmity.Focus != null)
                {
                    enmity.Focus.Distance = mychar.DistanceString(enmity.Focus);
                }
                if (enmity.Hover != null)
                {
                    enmity.Hover.Distance = mychar.DistanceString(enmity.Hover);
                }
                if (enmity.TargetOfTarget != null)
                {
                    enmity.TargetOfTarget.Distance = mychar.DistanceString(enmity.TargetOfTarget);
                }
            }
            catch (Exception ex)
            {
                this.logger.Log(LogLevel.Error, "CreateTargetData: {0}", ex);
            }
            return JObject.FromObject(enmity);
        }

        internal JObject CreateAggroList(List<Combatant> combatants)
        {
            EnmityAggroListObject enmity = new EnmityAggroListObject();
            try
            {
                enmity.AggroList = memory.GetAggroList(combatants);
            }
            catch (Exception ex)
            {
                this.logger.Log(LogLevel.Error, "CreateAggroList: {0}", ex);
            }
            return JObject.FromObject(enmity);
        }
    }
}
