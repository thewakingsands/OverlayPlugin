using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RainbowMage.HtmlRenderer;

namespace RainbowMage.OverlayPlugin.Overlays
{
    public partial class MiniParseOverlay : OverlayBase<MiniParseOverlayConfig>
    {
        protected bool modernApi = false;
        protected DateTime lastUrlChange;
        protected string lastLoadedUrl;

        public MiniParseOverlay(MiniParseOverlayConfig config, string name)
            : base(config, name)
        {
            Config.ActwsCompatibilityChanged += (o, e) =>
            {
                if (lastLoadedUrl != null) Navigate(lastLoadedUrl);
            };

            Overlay.Renderer.BrowserStartLoading += PrepareWebsite;
            Overlay.Renderer.BrowserLoad += FinishLoading;
            Overlay.Renderer.BrowserConsoleLog += Renderer_BrowserConsoleLog;
        }

        public override Control CreateConfigControl()
        {
            return new MiniParseConfigPanel(this);
        }

        private void Renderer_BrowserConsoleLog(object sender, BrowserConsoleLogEventArgs e)
        {
            if (Config.ActwsCompatibility && e.Message.Contains("ws://127.0.0.1/") && (e.Message.Contains("SecurityError:") || e.Message.Contains("ERR_CONNECTION_")))
            {
                Overlay.Reload();
            }
        }

        private void FinishLoading(object sender, BrowserLoadEventArgs e)
        {
            if (Config.ActwsCompatibility)
            {
                var charName = JsonConvert.SerializeObject(FFXIVRepository.GetPlayerName() ?? "YOU");
                var charID = JsonConvert.SerializeObject(FFXIVRepository.GetPlayerID());

                ExecuteScript(@"if (window.__OverlayPlugin_ws_faker) {
                    __OverlayPlugin_ws_faker({ type: 'broadcast', msgtype: 'SendCharName', msg: { charName: " + charName + ", charID: " + charID + @" }});
                }");
            }
        }

        private void PrepareWebsite(object sender, BrowserLoadEventArgs e)
        {
            lastLoadedUrl = e.Url;
            Config.Url = e.Url;

            if (Config.ActwsCompatibility)
            {
                var shimMsg = Resources.ActwsShimEnabled;

                // Install a fake WebSocket so we can directly call the event handler.
                ExecuteScript(@"(function() {
                    let realWS = window.WebSocket;
                    let queue = [];
                    window.__OverlayPlugin_ws_faker = (msg) => queue.push(msg);

                    window.WebSocket = function(url) {
                        if (url.indexOf('ws://127.0.0.1/fake/') > -1)
                        {
                            window.__OverlayPlugin_ws_faker = (msg) => {
                                if (this.onmessage) this.onmessage({ data: JSON.stringify(msg) });
                            };
                            this.close = () => null;

                            console.log(" + JsonConvert.SerializeObject(shimMsg) + @");

                            if (queue !== null) {
                                setTimeout(() => {
                                    queue.forEach(__OverlayPlugin_ws_faker);
                                    queue = null;
                                }, 100);
                            }
                        }
                        else
                        {
                            return new realWS(url);
                        }
                    };
                })();");

                Subscribe("CombatData");
                Subscribe("LogLine");
                Subscribe("ChangeZone");
                Subscribe("ChangePrimaryPlayer");

                // ACTWS overlays always accept input focus.
                SetAcceptFocus(true);
            } else {
                // Subscriptions are cleared on page navigation so we have to restore this after every load.

                modernApi = false;
                Subscribe("CombatData");
                Subscribe("LogLine");

                // Reset the focus setting to make sure that a previously loaded overlay doesn't affect a different one.
                SetAcceptFocus(false);
            }
        }

        public override void Navigate(string url)
        {
            if (Config.ActwsCompatibility)
            {
                if (!url.Contains("HOST_PORT="))
                {
                    if (!url.EndsWith("?"))
                    {
                        if (url.Contains("?"))
                        {
                            url += "&";
                        } else
                        {
                            url += "?";
                        }
                    }
                    url += "HOST_PORT=ws://127.0.0.1/fake/";
                }
            } else
            {
                int pos = url.IndexOf("HOST_PORT=");
                if (pos > -1 && url.Contains("/fake/"))
                {
                    url = url.Substring(0, pos).Trim(new char[] { '?', '&' });
                }
            }

            // If this URL was just loaded (see PrepareWebsite), ignore this request since we're loading that URL already.
            if (url == lastLoadedUrl) return;

            lastUrlChange = DateTime.Now;
            if (url != Overlay.Url)
            {
                base.Navigate(url);
            }
        }

        public override void Reload()
        {
            // If the user changed the URL less than a second ago, ignore the reload since it would interrupt
            // the currently loading overlay and end up with an empty page.
            // The user probably just wanted to load the page so doing nothing here (in that case) is fine.

            if (DateTime.Now - lastUrlChange > new TimeSpan(0, 0, 1))
            {
                base.Reload();
            }
        }

        public override void Start()
        {
        }

        public override void Stop()
        {
        }

        public override void HandleEvent(JObject e)
        {
            if (modernApi)
            {
                base.HandleEvent(e);
            }
            else if (Config.ActwsCompatibility)
            {
                // NOTE: Keep this in sync with WSServer's LegacyHandler.
                switch (e["type"].ToString())
                {
                    case "CombatData":
                        ExecuteScript("__OverlayPlugin_ws_faker({'type': 'broadcast', 'msgtype': 'CombatData', 'msg': " + e.ToString(Formatting.None) + " });");
                        break;
                    case "LogLine":
                        ExecuteScript("__OverlayPlugin_ws_faker({'type': 'broadcast', 'msgtype': 'Chat', 'msg': " + JsonConvert.SerializeObject(e["rawLine"].ToString()) + " });");
                        break;
                    case "ChangeZone":
                        ExecuteScript("__OverlayPlugin_ws_faker({'type': 'broadcast', 'msgtype': 'ChangeZone', 'msg': " + e.ToString(Formatting.None) + " });");
                        break;
                    case "ChangePrimaryPlayer":
                        ExecuteScript("__OverlayPlugin_ws_faker({'type': 'broadcast', 'msgtype': 'SendCharName', 'msg': " + e.ToString(Formatting.None) + " });");
                        break;
                }
            }
            else
            {
                // Old OverlayPlugin API
                switch (e["type"].ToString())
                {
                    case "CombatData":
                        ExecuteScript("document.dispatchEvent(new CustomEvent('onOverlayDataUpdate', { detail: " + e.ToString(Formatting.None) + " }));");
                        break;
                    case "LogLine":
                        ExecuteScript("document.dispatchEvent(new CustomEvent('onLogLine', { detail: " + JsonConvert.SerializeObject(e["line"].ToString(Formatting.None)) + " }));");
                        break;
                }
            }
        }

        public override void InitModernAPI()
        {
            if (!modernApi)
            {
                // Clear the subscription set in PrepareWebsite().
                Unsubscribe("CombatData");
                Unsubscribe("LogLine");
                modernApi = true;
            }
        }

        protected override void Update()
        {
            
        }
    }
}
