using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using Newtonsoft.Json.Linq;

namespace RainbowMage.OverlayPlugin.EventSources
{
    partial class MiniParseEventSource : EventSourceBase
    {
        private List<string> importedLogs = new List<string>();

        private const string CombatDataEvent = "CombatData";
        private const string ImportedLogLinesEvent = "ImportedLogLines";
        private const string BroadcastMessageEvent = "BroadcastMessage";

        // Event Source

        public BuiltinEventConfig Config { get; set; }

        public MiniParseEventSource(TinyIoCContainer container) : base(container)
        {
            Name = "MiniParse";

            RegisterEventTypes(new List<string> {
                CombatDataEvent,
                ImportedLogLinesEvent,
                BroadcastMessageEvent,
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

            RegisterEventHandler("playSound", (msg) =>
            {
                var file = msg["file"]?.ToString();
                if (file == null)
                    return null;

                ActGlobals.oFormActMain.PlaySound(file);
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

            RegisterEventHandler("openWebsiteWithWS", (msg) =>
            {
                var result = new JObject();

                if (!msg.ContainsKey("url"))
                {
                    Log(LogLevel.Error, "Called openWebsiteWithWS handler without specifying a URL (\"url\" property is missing).");
                    result["$error"] = "Called openWebsiteWithWS handler without specifying a URL (\"url\" property is missing).";
                    return result;
                }

                var wsServer = container.Resolve<WSServer>();

                if (!wsServer.IsRunning())
                {
                    result["$error"] = "WSServer is not running";
                    return result;
                }

                try
                {
                    var url = wsServer.GetModernUrl(msg["url"].ToString());
                    var proc = new Process();
                    proc.StartInfo.Verb = "open";
                    proc.StartInfo.FileName = url;
                    proc.Start();
                }
                catch (Exception ex)
                {
                    Log(LogLevel.Error, $"Failed to to open website: {ex}");
                    result["$error"] = $"Failed to to open website: {ex}";
                    return result;
                }

                result["success"] = true;
                return result;
            });

            ActGlobals.oFormActMain.BeforeLogLineRead +=
                (bool isImport, LogLineEventArgs logInfo) =>
                {
                    if (isImport)
                    {
                        lock (importedLogs)
                        {
                            importedLogs.Add(logInfo.originalLogLine);
                        }
                    }
                };
        }

        private void StopACTCombat()
        {
            ActGlobals.oFormActMain.Invoke((Action)(() =>
            {
                ActGlobals.oFormActMain.EndCombat(true);
            }));
        }

        private void LogLineHandler(bool isImport, LogLineEventArgs args)
        {
            if (isImport)
            {
                try
                {
                    var line = args.originalLogLine.Split('|');

                    if (int.TryParse(line[0], out int lineTypeInt))
                    {
                        // If an imported log has split the encounter, also split it while importing.
                        // TODO: should we also consider the current user's wipe config option here for splitting,
                        // even if the original log writer did not have it set to true?
                        LogMessageType lineType = (LogMessageType)lineTypeInt;
                        if (lineType == LogMessageType.InCombat)
                        {
                            var inACTCombat = Convert.ToUInt32(line[2]);
                            if (inACTCombat == 0)
                            {
                                StopACTCombat();
                            }
                        }
                    }
                }
                catch
                {
                    return;
                }

                lock (importedLogs)
                {
                    importedLogs.Add(args.originalLogLine);
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

                    case LogMessageType.ChangeMap:
                        if (line.Length < 6) return;

                        var mapID = Convert.ToUInt32(line[2], 10);
                        var regionName = line[3];
                        var placeName = line[4];
                        var placeNameSub = line[5];

                        DispatchAndCacheEvent(JObject.FromObject(new
                        {
                            type = ChangeMapEvent,
                            mapID,
                            regionName,
                            placeName,
                            placeNameSub
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

                        // 4000000F is the new value for 6.2, 40000010 is the pre-6.2 value.
                        // When CN/KR is on 6.2, this can be removed.
                        if (line[3] == "40000010" || line[3] == "4000000F")
                        {
                            StopACTCombat();
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

        private int GetPartyType(PluginCombatant combatant)
        {
            // The PartyTypeEnum was renamed in 2.6.0.0 to work around that, we use reflection and cast the result to int.
            return (int)combatant.GetType().GetMethod("get_PartyType").Invoke(combatant, new object[] { });
        }

        private string GetWorldName(uint WorldID)
        {
            var dict = repository.GetResourceDictionary(ResourceType.WorldList_EN);
            if (dict == null)
                return null;
            if (dict.TryGetValue(WorldID, out string WorldName))
                return WorldName;
            return null;
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
            var lookupTable = new Dictionary<uint, PluginCombatant>();
            foreach (var c in combatants)
            {
                if (GetPartyType(c) != 0 /* None */)
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
                        PluginCombatant c;
                        if (lookupTable.TryGetValue(id, out c))
                        {
                            result.Add(new PartyMember
                            {
                                id = $"{id:X}",
                                name = c.Name,
                                worldId = c.WorldID,
                                job = c.Job,
                                level = c.Level,
                                inParty = GetPartyType(c) == 1 /* Party */,
                            });
                        }
                        else
                        {
                            missingPartyMembers.Add(id);
                        }
                    }

                    if (missingPartyMembers.Count > 0)
                    {
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

                // There used to be logic here to skip updating if the encounter info hasn't changed, but that's been commented
                // out since https://github.com/ngld/OverlayPlugin/commit/a56c44e85f0a5608d4185d67e13690dbad461523
                // Probably an unintentional change but it didn't break anything :shrug:


                /* // 最終更新時刻に変化がないなら更新を行わない
                 if (this.prevEncounterId == ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EncId &&
                     this.prevEndDateTime == ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EndTime &&
                     this.prevEncounterActive == ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.Active)
                 {
                     // return;
                 }

                 this.prevEncounterId = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EncId;
                 this.prevEndDateTime = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EndTime;
                 this.prevEncounterActive = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.Active;*/

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


            encounter = GetEncounterDictionary(allies);
            combatant = GetCombatantList(allies);

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

            obj["isActive"] = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter?.Active == true ? "true" : "false";

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

            //var varStopwatch = new Stopwatch();

            var combatantList = new List<KeyValuePair<CombatantData, Dictionary<string, string>>>();
            Parallel.ForEach(allies, (ally) =>
            //foreach (var ally in allies)
            {
                var valueDict = new Dictionary<string, string>();
                foreach (var exportValuePair in CombatantData.ExportVariables)
                {
                    try
                    {
                        /*varStopwatch.Reset();
                                    varStopwatch.Start();*/

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

            /// var varStopwatch = new Stopwatch();

            var encounterDict = new Dictionary<string, string>();
            foreach (var exportValuePair in EncounterData.ExportVariables)
            {
                try
                {
                    /*varStopwatch.Reset();
                    varStopwatch.Start();*/

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
                    encounterDict.Add(exportValuePair.Key, value);
                    //Log(LogLevel.Debug, $"Encounter: {exportValuePair.Key}: {varStopwatch.ElapsedMilliseconds}ms");
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
