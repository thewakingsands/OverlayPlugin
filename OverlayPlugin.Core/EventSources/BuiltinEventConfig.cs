using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RainbowMage.OverlayPlugin.EventSources
{
    [Serializable]
    public class BuiltinEventConfig
    {
        public event EventHandler UpdateIntervalChanged;
        public event EventHandler EnmityIntervalChanged;
        public event EventHandler SortKeyChanged;
        public event EventHandler SortDescChanged;
        public event EventHandler UpdateDpsDuringImportChanged;
        public event EventHandler EndEncounterAfterWipeChanged;
        public event EventHandler EndEncounterOutOfCombatChanged;
        public event EventHandler CutsceneDetectionLogChanged;

        private int updateInterval;
        public int UpdateInterval {
            get
            {
                return this.updateInterval;
            }
            set
            {
                if (this.updateInterval != value)
                {
                    this.updateInterval = value;
                    UpdateIntervalChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        private int enmityIntervalMs;
        public int EnmityIntervalMs
        {
            get
            {
                return this.enmityIntervalMs;
            }
            set
            {
                if (this.enmityIntervalMs != value)
                {
                    this.enmityIntervalMs = value;
                    EnmityIntervalChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        private string sortKey;
        public string SortKey
        {
            get
            {
                return this.sortKey;
            }
            set
            {
                if (this.sortKey != value)
                {
                    this.sortKey = value;
                    SortKeyChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        private bool sortDesc;
        public bool SortDesc
        {
            get
            {
                return this.sortDesc;
            }
            set
            {
                if (this.sortDesc != value)
                {
                    this.sortDesc = value;
                    SortDescChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        private bool updateDpsDuringImport;
        public bool UpdateDpsDuringImport
        {
            get
            {
                return this.updateDpsDuringImport;
            }
            set
            {
                if (this.updateDpsDuringImport != value)
                {
                    this.updateDpsDuringImport = value;
                    UpdateDpsDuringImportChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        private bool endEncounterAfterWipe;
        public bool EndEncounterAfterWipe
        {
            get
            {
                return this.endEncounterAfterWipe;
            }
            set
            {
                if (this.endEncounterAfterWipe != value)
                {
                    this.endEncounterAfterWipe = value;
                    EndEncounterAfterWipeChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        private bool endEncounterOutOfCombat;
        public bool EndEncounterOutOfCombat
        {
            get
            {
                return this.endEncounterOutOfCombat;
            }
            set
            {
                if (this.endEncounterOutOfCombat != value)
                {
                    this.endEncounterOutOfCombat = value;
                    EndEncounterOutOfCombatChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        private bool cutsceneDetectionLog;
        public bool CutsceneDetectionLog
        {
            get
            {
                return cutsceneDetectionLog;
            }
            set
            {
                if (this.cutsceneDetectionLog != value)
                {
                    this.cutsceneDetectionLog = value;
                    CutsceneDetectionLogChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        // Data that overlays can save/load via event handlers.
        public Dictionary<string, JToken> OverlayData = new Dictionary<string, JToken>();

        public BuiltinEventConfig()
        {
            this.updateInterval = 1;
            this.enmityIntervalMs = 100;
            this.sortKey = "encdps";
            this.sortDesc = true;
            this.updateDpsDuringImport = false;
            this.endEncounterAfterWipe = false;
            this.endEncounterOutOfCombat = false;
            this.cutsceneDetectionLog = false;
        }

        public static BuiltinEventConfig LoadConfig(IPluginConfig Config)
        {
            var result = new BuiltinEventConfig();

            if (Config.EventSourceConfigs.ContainsKey("MiniParse"))
            {
                var obj = Config.EventSourceConfigs["MiniParse"];
                
                if (obj.TryGetValue("UpdateInterval", out JToken value))
                {
                    result.updateInterval = value.ToObject<int>();
                }

                if (obj.TryGetValue("EnmityIntervalMs", out value))
                {
                    result.enmityIntervalMs = value.ToObject<int>();
                }

                if (obj.TryGetValue("SortKey", out value))
                {
                    result.sortKey = value.ToString();
                }

                if (obj.TryGetValue("SortDesc", out value))
                {
                    result.sortDesc = value.ToObject<bool>();
                }

                if (obj.TryGetValue("UpdateDpsDuringImport", out value))
                {
                    result.updateDpsDuringImport = value.ToObject<bool>();
                }

                if (obj.TryGetValue("EndEncounterAfterWipe", out value))
                {
                    result.endEncounterAfterWipe = value.ToObject<bool>();
                }

                if (obj.TryGetValue("EndEncounterOutOfCombat", out value))
                {
                    result.endEncounterOutOfCombat = value.ToObject<bool>();
                }

                if (obj.TryGetValue("OverlayData", out value))
                {
                    result.OverlayData = value.ToObject<Dictionary<string, JToken>>();
                }

                if (obj.TryGetValue("CutsceneDetctionLog", out value))
                {
                    result.cutsceneDetectionLog = value.ToObject<bool>();
                }
            }

            return result;
        }

        public void SaveConfig(IPluginConfig Config)
        {
            var newObj = JObject.FromObject(this);
            if (!JObject.DeepEquals(Config.EventSourceConfigs["MiniParse"], newObj))
            {
                Config.EventSourceConfigs["MiniParse"] = newObj;
                Config.MarkDirty();
            }
        }
    }

    public enum MiniParseSortType
    {
        None,
        StringAscending,
        StringDescending,
        NumericAscending,
        NumericDescending
    }
}
