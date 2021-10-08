﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Advanced_Combat_Tracker;
using System.Diagnostics;
using System.Windows.Forms;
using FFXIV_ACT_Plugin.Common.Models;
using RainbowMage.OverlayPlugin.NetworkProcessors;
using System.IO;

namespace RainbowMage.OverlayPlugin.EventSources
{
    partial class MiniParseEventSource : EventSourceBase
    {
        private string prevEncounterId { get; set; }
        private DateTime prevEndDateTime { get; set; }
        private bool prevEncounterActive { get; set; }

        private List<string> importedLogs = new List<string>();
        private ReadOnlyCollection<uint> cachedPartyList = null;
        private List<uint> missingPartyMembers = new List<uint>();
        private bool ffxivPluginPresent = false;
        private static Dictionary<uint, string> StatusMap = new Dictionary<uint, string>
        {
            { 0, "Online" },
            { 12, "Busy" },
            { 15, "InCutscene" },
            { 17, "AFK" },
            { 21, "LookingToMeld" },
            { 22, "RP" },
            { 23, "LookingForParty" },
        };

        private readonly List<string> DefaultCombatantFields = new List<string>
        {
            "CurrentWorldID",
            "WorldID",
            "WorldName",
            "BNpcID",
            "BNpcNameID",
            "PartyType",
            "ID",
            "OwnerID",
            "type",
            "Job",
            "Level",
            "Name",
            "CurrentHP",
            "MaxHP",
            "CurrentMP",
            "MaxMP",
            "PosX",
            "PosY",
            "PosZ",
            "Heading"
        };

        private Dictionary<string, System.Reflection.PropertyInfo> CachedCombatantPropertyInfos
            = new Dictionary<string, System.Reflection.PropertyInfo>();

        private const string CombatDataEvent = "CombatData";
        private const string LogLineEvent = "LogLine";
        private const string ImportedLogLinesEvent = "ImportedLogLines";
        private const string ChangeZoneEvent = "ChangeZone";
        private const string ChangePrimaryPlayerEvent = "ChangePrimaryPlayer";
        private const string FileChangedEvent = "FileChanged";
        private const string OnlineStatusChangedEvent = "OnlineStatusChanged";
        private const string PartyChangedEvent = "PartyChanged";
        private const string BroadcastMessageEvent = "BroadcastMessage";

        private FFXIVRepository repository;

        // Event Source

        public BuiltinEventConfig Config { get; set; }

        public MiniParseEventSource(TinyIoCContainer container) : base(container)
        {
            this.Name = "MiniParse";
            this.repository = container.Resolve<FFXIVRepository>();

            // FileChanged isn't actually raised by this event source. That event is generated in MiniParseOverlay directly.
            RegisterEventTypes(new List<string> {
                CombatDataEvent,
                FileChangedEvent,
                LogLineEvent,
                ImportedLogLinesEvent,
                BroadcastMessageEvent,
            });

            // These events need to deliver cached values to new subscribers.
            RegisterCachedEventTypes(new List<string> {
                ChangePrimaryPlayerEvent,
                ChangeZoneEvent,
                OnlineStatusChangedEvent,
                PartyChangedEvent,
            });

            RegisterEventHandler("getLanguage", (msg) =>
            {
                var lang = repository.GetLanguage();
                return JObject.FromObject(new
                {
                    language = lang.ToString("g"),
                    languageId = lang.ToString("d"),
                });
            });

            RegisterEventHandler("getCombatants", (msg) =>
            {
                List<uint> ids = new List<uint>();

                if (msg["ids"] != null)
                {
                    foreach (var id in ((JArray)msg["ids"]))
                    {
                        ids.Add(id.ToObject<uint>());
                    }
                }

                List<string> names = new List<string>();

                if (msg["names"] != null)
                {
                    foreach (var name in ((JArray)msg["names"]))
                    {
                        names.Add(name.ToString());
                    }
                }

                List<string> props = new List<string>();

                if (msg["props"] != null)
                {
                    foreach (var prop in ((JArray)msg["props"]))
                    {
                        props.Add(prop.ToString());
                    }
                }

                var combatants = GetCombatants(ids, names, props);
                return JObject.FromObject(new
                {
                    combatants
                });
            });

            RegisterEventHandler("saveData", (msg) =>
            {
                var key = msg["key"]?.ToString();
                if (key == null)
                    return null;

                Config.OverlayData[key] = msg["data"];
                return null;
            });

            RegisterEventHandler("loadData", (msg) =>
            {
                var key = msg["key"]?.ToString();
                if (key == null)
                    return null;

                if (!Config.OverlayData.ContainsKey(key))
                    return null;

                var ret = new JObject();
                ret["key"] = key;
                ret["data"] = Config.OverlayData[key];
                return ret;
            });

            RegisterEventHandler("say", (msg) =>
            {
                var text = msg["text"]?.ToString();
                if (text == null)
                    return null;

                ActGlobals.oFormActMain.TTS(text);
                return null;
            });

            RegisterEventHandler("broadcast", (msg) =>
            {
                if (!msg.ContainsKey("msg") || !msg.ContainsKey("source"))
                {
                    Log(LogLevel.Error, "Called broadcast handler without specifying a source or message (\"source\" or \"msg\" property are missing).");
                    return null;
                }

                if (msg["source"].Type != JTokenType.String)
                {
                    Log(LogLevel.Error, "The source passed to the broadcast handler must be a string!");
                    return null;
                }

                DispatchEvent(JObject.FromObject(new
                {
                    type = BroadcastMessageEvent,
                    source = msg["source"],
                    msg = msg["msg"],
                }));

                return null;
            });

            try
            {
                InitFFXIVIntegration();
            } catch (FileNotFoundException)
            {
                // The FFXIV plugin hasn't been loaded.
            }

            ActGlobals.oFormActMain.BeforeLogLineRead += LogLineHandler;
            container.Resolve<NetworkParser>().OnOnlineStatusChanged += (o, e) =>
            {
                var obj = new JObject();
                obj["type"] = OnlineStatusChangedEvent;
                obj["target"] = e.Target;
                obj["rawStatus"] = e.Status;
                obj["status"] = StatusMap.ContainsKey(e.Status) ? StatusMap[e.Status] : "Unknown";

                DispatchAndCacheEvent(obj);
            };
        }

        private void InitFFXIVIntegration()
        {
            foreach (var propName in DefaultCombatantFields)
            {
                CachedCombatantPropertyInfos.Add(propName, typeof(Combatant).GetProperty(propName));
            }

            repository.RegisterPartyChangeDelegate((partyList, partySize) => DispatchPartyChangeEvent(partyList, partySize));
            ffxivPluginPresent = true;
        }

        private List<Dictionary<string, object>> GetCombatants(List<uint> ids, List<string> names, List<string> props)
        {
            List<Dictionary<string, object>> filteredCombatants = new List<Dictionary<string, object>>();

            var combatants = repository.GetCombatants();

            foreach (var combatant in combatants)
            {
                if (combatant.ID == 0)
                {
                    continue;
                }

                bool include = false;

                var combatantName = CachedCombatantPropertyInfos["Name"].GetValue(combatant);

                if (ids.Count == 0 && names.Count == 0)
                {
                    include = true;
                }
                else
                {
                    foreach (var id in ids)
                    {
                        if (combatant.ID == id)
                        {
                            include = true;
                            break;
                        }
                    }

                    if (!include)
                    {
                        foreach (var name in names)
                        {
                            if (combatantName.Equals(name))
                            {
                                include = true;
                                break;
                            }
                        }
                    }
                }

                if (include)
                {
                    Dictionary<string, object> filteredCombatant = new Dictionary<string, object>();
                    if (props.Count > 0)
                    {
                        foreach (var prop in props)
                        {
                            if (CachedCombatantPropertyInfos.ContainsKey(prop))
                            {
                                filteredCombatant.Add(prop, CachedCombatantPropertyInfos[prop].GetValue(combatant));
                            }
                        }
                    }
                    else
                    {
                        foreach (var prop in CachedCombatantPropertyInfos.Keys)
                        {
                            filteredCombatant.Add(prop, CachedCombatantPropertyInfos[prop].GetValue(combatant));
                        }
                    }
                    filteredCombatants.Add(filteredCombatant);
                }
            }

            return filteredCombatants;
        }

        private void LogLineHandler(bool isImport, LogLineEventArgs args)
        {
            if (isImport)
            {
                if (HasSubscriber(ImportedLogLinesEvent))
                {
                    lock (importedLogs)
                    {
                        importedLogs.Add(args.originalLogLine);
                    }
                }
                return;
            }

            try
            {
                LogMessageType lineType;
                var line = args.originalLogLine.Split('|');

                if (!int.TryParse(line[0], out int lineTypeInt))
                {
                    return;
                }

                try
                {
                    lineType = (LogMessageType)lineTypeInt;
                }
                catch
                {
                    return;
                }

                switch (lineType)
                {
                    case LogMessageType.ChangeZone:
                        if (line.Length < 3) return;

                        var zoneID = Convert.ToUInt32(line[2], 16);
                        var zoneName = line[3];

                        DispatchAndCacheEvent(JObject.FromObject(new
                        {
                            type = ChangeZoneEvent,
                            zoneID,
                            zoneName,
                        }));
                        break;

                    case LogMessageType.ChangePrimaryPlayer:
                        if (line.Length < 4) return;

                        var charID = Convert.ToUInt32(line[2], 16);
                        var charName = line[3];

                        DispatchAndCacheEvent(JObject.FromObject(new
                        {
                            type = ChangePrimaryPlayerEvent,
                            charID,
                            charName,
                        }));
                        break;

                    case LogMessageType.Network6D:
                        if (!Config.EndEncounterAfterWipe) break;
                        if (line.Length < 4) break;

                        if (line[3] == "40000010")
                        {
                            ActGlobals.oFormActMain.Invoke((Action)(() =>
                            {
                                ActGlobals.oFormActMain.EndCombat(true);
                            }));
                        }
                        break;
                }

                DispatchEvent(JObject.FromObject(new
                {
                    type = LogLineEvent,
                    line,
                    rawLine = args.originalLogLine,
                }));
            }
            catch (Exception e)
            {
                Log(LogLevel.Error, "Failed to process log line: " + e.ToString());
            }
        }

        struct PartyMember
        {
            // Player id in hex (for ease in matching logs).
            public string id;
            public string name;
            public uint worldId;
            // Raw job id.
            public int job;
            public int level;
            // In immediate party (true), vs in alliance (false).
            public bool inParty;
        }

        private void DispatchPartyChangeEvent(ReadOnlyCollection<uint> partyList, int partySize)
        {
            cachedPartyList = partyList;
            var combatants = repository.GetCombatants();
            if (combatants == null)
                return;

            // This is a bit of a hack.  The goal is to return a set of party
            // and alliance players, along with their jobs, ids, and names.
            //
            // |partySize| is only the size of your party, but the list of ids
            // contains ids from both party and alliance members.
            //
            // Additionally, there is a race where |combatants| is not updated
            // by the time this function is called.  However, this only seems
            // to happen in the case of disconnects and never when zoning in.
            // As a workaround, we use data retrieved from the NetworkAdd/RemoveCombatant
            // lines and keep track of all combatants which are missing from
            // the memory combatant list (the network lines are missing the
            // party status). Once per second (in Update()) we check if all
            // missing members have appeared and once they do, we dispatch
            // a PartyChangedEvent. This should result in immediate events
            // whenever the party changes and a second delayed event for each
            // change that updates the inParty field.
            //
            // Alternatives:
            // * poll GetCombatants until all party members exist (infinitely?)
            // * find better memory location of party list
            // * make this function only return the values from the delegate
            // * make callers handle this via calling GetCombatants explicitly

            // Build a lookup table of currently known combatants
            var lookupTable = new Dictionary<uint, Combatant>();
            foreach (var c in combatants)
            {
                if (c.PartyType != PartyTypeEnum.None)
                {
                    lookupTable[c.ID] = c;
                }
            }

            // Accumulate party members from cached info.  If they don't exist,
            // still send *something*, since it's better than nothing.
            List<PartyMember> result = new List<PartyMember>(24);
            lock (missingPartyMembers) lock (partyList)
            {
                missingPartyMembers.Clear();

                foreach (var id in partyList)
                {
                    Combatant c;
                    if (lookupTable.TryGetValue(id, out c))
                    {
                        result.Add(new PartyMember
                        {
                            id = $"{id:X}",
                            name = c.Name,
                            worldId = c.WorldID,
                            job = c.Job,
                            inParty = c.PartyType == PartyTypeEnum.Party
                        });
                    }
                    else
                    {
                        missingPartyMembers.Add(id);
                    }
                }

                if (missingPartyMembers.Count > 0) {
                    Log(LogLevel.Debug, "Party changed event delayed until members are available");
                    return;
                }
            }

            Log(LogLevel.Debug, "party list: {0}", JObject.FromObject(new { party = result }).ToString());

            DispatchAndCacheEvent(JObject.FromObject(new
            {
                type = PartyChangedEvent,
                party = result,
            }));
        }

        public override Control CreateConfigControl()
        {
            return null;
        }

        public override void LoadConfig(IPluginConfig config)
        {
            this.Config = container.Resolve<BuiltinEventConfig>();

            this.Config.UpdateIntervalChanged += (o, e) =>
            {
                this.Start();
            };
        }

        public override void SaveConfig(IPluginConfig config)
        {

        }

        public override void Start()
        {
            this.timer.Change(0, this.Config.UpdateInterval * 1000);
        }

        protected override void Update()
        {
            var importing = ActGlobals.oFormImportProgress?.Visible == true;

            if (CheckIsActReady() && (!importing || this.Config.UpdateDpsDuringImport))
            {
                if (!HasSubscriber(CombatDataEvent))
                {
                    return;
                }

                // 最終更新時刻に変化がないなら更新を行わない
                if (this.prevEncounterId == ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EncId &&
                    this.prevEndDateTime == ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EndTime &&
                    this.prevEncounterActive == ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.Active)
                {
                    return;
                }

                this.prevEncounterId = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EncId;
                this.prevEndDateTime = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EndTime;
                this.prevEncounterActive = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.Active;

                DispatchEvent(this.CreateCombatData());
            }

            if (HasSubscriber(ImportedLogLinesEvent))
            {
                List<string> logs = null;
                lock (importedLogs)
                {
                    if (importedLogs.Count > 0)
                    {
                        logs = importedLogs;
                        importedLogs = new List<string>();
                    }
                }
                if (logs != null)
                {
                    DispatchEvent(JObject.FromObject(new
                    {
                        type = ImportedLogLinesEvent,
                        logLines = logs
                    }));
                }
            }

            if (ffxivPluginPresent)
            {
                UpdateMissingPartyMembers();
            }
        }

        private void UpdateMissingPartyMembers()
        {
            lock(missingPartyMembers)
            {
                // If we are looking for missing party members, check if they are present by now.
                if (missingPartyMembers.Count > 0)
                {
                    var combatants = repository.GetCombatants();
                    if (combatants != null)
                    {
                        foreach (var c in combatants)
                        {
                            if (missingPartyMembers.Contains(c.ID))
                            {
                                missingPartyMembers.Remove(c.ID);
                            }
                        }
                    }

                    // Send an update event once all party members have been found.
                    if (missingPartyMembers.Count == 0)
                    {
                        DispatchPartyChangeEvent(cachedPartyList, 0);
                    }
                }
            }
        }

        internal JObject CreateCombatData()
        {
            if (!CheckIsActReady())
            {
                return new JObject();
            }

#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif

            var allies = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.GetAllies();
            Dictionary<string, string> encounter = null;
            List<KeyValuePair<CombatantData, Dictionary<string, string>>> combatant = null;

            var encounterTask = Task.Run(() =>
            {
                encounter = GetEncounterDictionary(allies);
            });
            var combatantTask = Task.Run(() =>
            {
                combatant = GetCombatantList(allies);
            });
            Task.WaitAll(encounterTask, combatantTask);

            if (encounter == null || combatant == null) return new JObject();

            JObject obj = new JObject();

            obj["type"] = "CombatData";
            obj["Encounter"] = JObject.FromObject(encounter);
            obj["Combatant"] = new JObject();

            if (this.Config.SortKey != null && this.Config.SortKey != "")
            {
                int factor = this.Config.SortDesc ? -1 : 1;
                var key = this.Config.SortKey;

                try
                {
                    combatant.Sort((a, b) =>
                    {
                        try
                        {
                            var aValue = float.Parse(a.Value[key]);
                            var bValue = float.Parse(b.Value[key]);

                            return factor * aValue.CompareTo(bValue);
                        }
                        catch (FormatException)
                        {
                            return 0;
                        }
                        catch (KeyNotFoundException)
                        {
                            return 0;
                        }
                    });
                }
                catch (Exception e)
                {
                    Log(LogLevel.Error, Resources.ListSortFailed, key, e);
                }
            }

            foreach (var pair in combatant)
            {
                JObject value = new JObject();
                foreach (var pair2 in pair.Value)
                {
                    value.Add(pair2.Key, Util.ReplaceNaNString(pair2.Value, "---"));
                }

                obj["Combatant"][pair.Key.Name] = value;
            }

            obj["isActive"] = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.Active ? "true" : "false";

#if TRACE
            stopwatch.Stop();
            Log(LogLevel.Trace, "CreateUpdateScript: {0} msec", stopwatch.Elapsed.TotalMilliseconds);
#endif
            return obj;
        }

        private List<KeyValuePair<CombatantData, Dictionary<string, string>>> GetCombatantList(List<CombatantData> allies)
        {
#if TRACE
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif

            var combatantList = new List<KeyValuePair<CombatantData, Dictionary<string, string>>>();
            Parallel.ForEach(allies, (ally) =>
            //foreach (var ally in allies)
            {
                var valueDict = new Dictionary<string, string>();
                foreach (var exportValuePair in CombatantData.ExportVariables)
                {
                    try
                    {
                        // NAME タグには {NAME:8} のようにコロンで区切られたエクストラ情報が必要で、
                        // プラグインの仕組み的に対応することができないので除外する
                        if (exportValuePair.Key.StartsWith("NAME"))
                        {
                            continue;
                        }

                        // ACT_FFXIV_Plugin が提供する LastXXDPS は、
                        // ally.Items[CombatantData.DamageTypeDataOutgoingDamage].Items に All キーが存在しない場合に、
                        // プラグイン内で例外が発生してしまい、パフォーマンスが悪化するので代わりに空の文字列を挿入する
                        if (exportValuePair.Key == "Last10DPS" ||
                            exportValuePair.Key == "Last30DPS" ||
                            exportValuePair.Key == "Last60DPS" ||
                            exportValuePair.Key == "Last180DPS")
                        {
                            if (!ally.Items[CombatantData.DamageTypeDataOutgoingDamage].Items.ContainsKey("All"))
                            {
                                valueDict.Add(exportValuePair.Key, "");
                                continue;
                            }
                        }

                        var value = exportValuePair.Value.GetExportString(ally, "");
                        valueDict.Add(exportValuePair.Key, value);
                    }
                    catch (Exception e)
                    {
                        Log(LogLevel.Debug, "GetCombatantList: {0}: {1}: {2}", ally.Name, exportValuePair.Key, e);
                        continue;
                    }
                }

                lock (combatantList)
                {
                    combatantList.Add(new KeyValuePair<CombatantData, Dictionary<string, string>>(ally, valueDict));
                }
            }
            );

#if TRACE
            stopwatch.Stop();
            Log(LogLevel.Trace, "GetCombatantList: {0} msec", stopwatch.Elapsed.TotalMilliseconds);
#endif

            return combatantList;
        }

        private Dictionary<string, string> GetEncounterDictionary(List<CombatantData> allies)
        {
#if TRACE
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif

            var encounterDict = new Dictionary<string, string>();
            foreach (var exportValuePair in EncounterData.ExportVariables)
            {
                try
                {
                    // ACT_FFXIV_Plugin が提供する LastXXDPS は、
                    // ally.Items[CombatantData.DamageTypeDataOutgoingDamage].Items に All キーが存在しない場合に、
                    // プラグイン内で例外が発生してしまい、パフォーマンスが悪化するので代わりに空の文字列を挿入する
                    if (exportValuePair.Key == "Last10DPS" ||
                        exportValuePair.Key == "Last30DPS" ||
                        exportValuePair.Key == "Last60DPS" ||
                        exportValuePair.Key == "Last180DPS")
                    {
                        if (!allies.All((ally) => ally.Items[CombatantData.DamageTypeDataOutgoingDamage].Items.ContainsKey("All")))
                        {
                            encounterDict.Add(exportValuePair.Key, "");
                            continue;
                        }
                    }

                    var value = exportValuePair.Value.GetExportString(
                        ActGlobals.oFormActMain.ActiveZone.ActiveEncounter,
                        allies,
                        "");
                    //lock (encounterDict)
                    //{
                    encounterDict.Add(exportValuePair.Key, value);
                    //}
                }
                catch (Exception e)
                {
                    Log(LogLevel.Debug, "GetEncounterDictionary: {0}: {1}", exportValuePair.Key, e);
                }
            }

#if TRACE
            stopwatch.Stop();
            Log(LogLevel.Trace, "GetEncounterDictionary: {0} msec", stopwatch.Elapsed.TotalMilliseconds);
#endif

            return encounterDict;
        }

        private static bool CheckIsActReady()
        {
            if (ActGlobals.oFormActMain?.ActiveZone?.ActiveEncounter != null &&
                EncounterData.ExportVariables != null &&
                CombatantData.ExportVariables != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
