using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin
{
    class OverlayZCorrector
    {
        private static PluginMain main = null;
        private static ILogger logger = null;
        private static Timer timer = null;

        public static void Init()
        {
            main = Registry.Resolve<PluginMain>();
            logger = Registry.Resolve<ILogger>();

            var span = TimeSpan.FromSeconds(3);
            timer = new Timer(EnsureOverlaysAreOverGame, null, span, span);
        }

        public static void DeInit()
        {
            timer.Change(0, -1);
        }

        private static void EnsureOverlaysAreOverGame(object _)
        {
            var watch = new Stopwatch();
            watch.Start();

            var xivProc = FFXIVRepository.GetCurrentFFXIVProcess();
            if (xivProc == null || xivProc.HasExited)
                return;

            var xivHandle = xivProc.MainWindowHandle;
            var overlayWindows = new List<IntPtr>();

            var handle = xivHandle;
            while (handle != IntPtr.Zero)
            {
                handle = NativeMethods.GetWindow(handle, NativeMethods.GW_HWNDPREV);
                overlayWindows.Add(handle);
            }

            foreach (var overlay in main.Overlays)
            {
                if (!overlayWindows.Contains(overlay.Handle))
                {
                    // The overlay is behind the game. Let's fix that.
                    NativeMethods.SetWindowPos(
                        overlay.Handle,
                        NativeMethods.HWND_TOPMOST,
                        0, 0, 0, 0,
                        NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOACTIVATE);

                    logger.Log(LogLevel.Info, $"ZReorder: Fixed {overlay.Name}.");
                }
            }

            // logger.Log(LogLevel.Debug, $"ZReorder: Took {watch.Elapsed.TotalSeconds}s.");
        }
    }
}
