using System;
using System.Collections.Generic;
using Advanced_Combat_Tracker;
using RainbowMage.OverlayPlugin.EventSources;
using RainbowMage.OverlayPlugin.NetworkProcessors;

namespace RainbowMage.OverlayPlugin.Integration
{
    public class UnstableNewLogLines
    {
        private bool inCutscene = false;
        private FFXIVRepository repo = null;
        private NetworkParser parser = null;

        public UnstableNewLogLines(TinyIoCContainer container)
        {
            repo = container.Resolve<FFXIVRepository>();
            parser = container.Resolve<NetworkParser>();
            var config = container.Resolve<BuiltinEventConfig>();

            config.CutsceneDetectionLogChanged += (o, e) =>
            {
                if (config.CutsceneDetectionLog)
                {
                    Enable();
                } else
                {
                    Disable();
                }
            };

            if (config.CutsceneDetectionLog)
            {
                Enable();
            }
        }

        public void Enable()
        {
            parser.OnOnlineStatusChanged += OnOnlineStatusChange;
        }

        public void Disable()
        {
            parser.OnOnlineStatusChanged -= OnOnlineStatusChange;
        }

        private void OnOnlineStatusChange(object sender, OnlineStatusChangedArgs ev)
        {
            if (ev.Target != repo.GetPlayerID())
                return;

            var cutsceneStatus = ev.Status == 15;
            if (cutsceneStatus != inCutscene)
            {
                inCutscene = cutsceneStatus;
                string msg;

                if (cutsceneStatus)
                {
                    msg = "Entered cutscene";
                } else
                {
                    msg = "Left cutscene";
                }

                var time = DateTime.Now;
                var line = new string[] { "00", time.ToString(), "c0fe", "", "OPLine: " + msg, ""};
                ActGlobals.oFormActMain.ParseRawLogLine(false, time, string.Join("|", line));
            }
        }
    }
}
