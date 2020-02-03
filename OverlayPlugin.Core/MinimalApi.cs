using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RainbowMage.HtmlRenderer;
using Newtonsoft.Json;

namespace RainbowMage.OverlayPlugin
{
    public class MinimalApi : IEventReceiver
    {
        public string Name => "";
        private readonly Renderer renderer;

        public MinimalApi(Renderer r)
        {
            renderer = r;

            r.BrowserStartLoading += (o, e) =>
            {
                EventDispatcher.UnsubscribeAll(this);
            };

            r.BrowserConsoleLog += (o, e) =>
            {
                Registry.Resolve<ILogger>().Log(LogLevel.Info, $"OverlayControl: {e.Message} ({e.Source})");
            };
        }

        public static void AttachTo(Renderer r)
        {
            r.SetApi(new MinimalApi(r));
        }

        public void HandleEvent(JObject e)
        {
            renderer.ExecuteScript("if(window.__OverlayCallback) __OverlayCallback(" + e.ToString(Formatting.None) + ");");
        }

        // Also handles (un)subscription to make switching between this and WS easier.
        public void callHandler(string data, object callback)
        {
            Task.Run(() => {
                var result = EventDispatcher.ProcessHandlerMessage(this, data);
                if (callback != null)
                {
                    Renderer.ExecuteCallback(callback, result?.ToString(Formatting.None));
                }
            });
        }
    }
}
