using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using RainbowMage.OverlayPlugin.NetworkProcessors;

namespace RainbowMage.OverlayPlugin
{
    class OverlayHider
    {
        private bool gameActive = true;
        private bool inCutscene = false;
        private IPluginConfig config;
        private ILogger logger;
        private PluginMain main;
        private FFXIVRepository repository;

        public OverlayHider(TinyIoCContainer container)
        {
            this.config = container.Resolve<IPluginConfig>();
            this.logger = container.Resolve<ILogger>();
            this.main = container.Resolve<PluginMain>();
            this.repository = container.Resolve<FFXIVRepository>();

            container.Resolve<NativeMethods>().ActiveWindowChanged += ActiveWindowChangedHandler;
            container.Resolve<NetworkParser>().OnOnlineStatusChanged += OnlineStatusChanged;
        }

        public void UpdateOverlays()
        {
            if (!config.HideOverlaysWhenNotActive)
                gameActive = true;

            if (!config.HideOverlayDuringCutscene)
                inCutscene = false;

            try
            {
                foreach (var overlay in main.Overlays)
                {
                    if (overlay.Config.IsVisible) overlay.Visible = gameActive && !inCutscene;
                }
            } catch (Exception ex)
            {
                logger.Log(LogLevel.Error, $"OverlayHider: Failed to update overlays: {ex}");
            }
        }

        private void ActiveWindowChangedHandler(object sender, IntPtr changedWindow)
        {
            if (!config.HideOverlaysWhenNotActive || changedWindow == IntPtr.Zero) return;
            try
            {
                try
                {
                    NativeMethods.GetWindowThreadProcessId(NativeMethods.GetForegroundWindow(), out uint pid);

                    if (pid == 0)
                        return;

                    var exePath = Process.GetProcessById((int)pid).MainModule.FileName;
                    var fileName = Path.GetFileName(exePath.ToString());
                    gameActive = (fileName == "ffxiv.exe" || fileName == "ffxiv_dx11.exe" ||
                                    exePath.ToString() == Process.GetCurrentProcess().MainModule.FileName);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    // Ignore access denied errors. Those usually happen if the foreground window is running with
                    // admin permissions but we are not.
                    if (ex.ErrorCode == -2147467259)  // 0x80004005
                    {
                        gameActive = false;
                    }
                    else
                    {
                        logger.Log(LogLevel.Error, "XivWindowWatcher: {0}", ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "XivWindowWatcher: {0}", ex.ToString());
            }

            UpdateOverlays();
        }

        private void OnlineStatusChanged(object sender, OnlineStatusChangedArgs e)
        {
            if (!config.HideOverlayDuringCutscene || e.Target != repository.GetPlayerID()) return;

            inCutscene = e.Status == 15;
            UpdateOverlays();
        }
    }
}
