using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advanced_Combat_Tracker;
using Newtonsoft.Json.Linq;

namespace RainbowMage.OverlayPlugin
{
    static class TriggIntegration
    {
        public delegate void CustomCallbackDelegate(object o, string param);

        public static void Init()
        {
            try
            {
                var trigg = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x => x.lblPluginTitle.Text == "Triggernometry.dll");
                if (trigg == null || trigg.pluginObj == null)
                    return;

                var triggType = trigg.pluginObj.GetType();
                var deleType = triggType.GetNestedType("CustomCallbackDelegate");
                if (deleType == null)
                    return;

                var dele = Delegate.CreateDelegate(deleType, typeof(TriggIntegration).GetMethod("SendOverlayMessage"));
                triggType.GetMethod("RegisterNamedCallback")?.Invoke(trigg.pluginObj, new object[] { "OverlayPluginMessage", dele, null });
            } catch (Exception ex)
            {
                Registry.Resolve<ILogger>().Log(LogLevel.Error, $"Failed to register Triggernometry callback: {ex}");
            }
        }

        public static void SendOverlayMessage(object _, string msg)
        {
            var pos = msg.IndexOf('|');
            if (pos < 1) return;

            var overlayName = msg.Substring(0, pos);
            msg = msg.Substring(pos + 1);

            var plugin = Registry.Resolve<PluginMain>();
            foreach (var overlay in plugin.Overlays)
            {
                if (overlay.Name == overlayName)
                {
                    ((IEventReceiver)overlay).HandleEvent(JObject.FromObject(new
                    {
                        type = "Triggernometry",
                        message = msg
                    }));
                    break;
                }
            }
        }
    }
}
