﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using RainbowMage.HtmlRenderer;
using RainbowMage.OverlayPlugin.Updater;

namespace RainbowMage.OverlayPlugin
{
    public class PluginLoader : IActPluginV1, IDisposable
    {
        PluginMain pluginMain;
        Logger logger;
        static AssemblyResolver asmResolver;
        string pluginDirectory;
        TabPage pluginScreenSpace;
        Label pluginStatusText;
        bool initFailed = false;
        private bool _disposed;

        public TinyIoCContainer Container { get; private set; }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            pluginDirectory = GetPluginDirectory();

            if (asmResolver == null)
            {
                asmResolver = new AssemblyResolver(new List<string>
                {
                    Path.Combine(pluginDirectory, "libs"),
                    Path.Combine(pluginDirectory, "addons"),
                    Path.Combine(pluginDirectory, "libs", Environment.Is64BitProcess ? "x64" : "x86"),
                    GetCefPath()
                });
            }

            this.pluginScreenSpace = pluginScreenSpace;
            this.pluginStatusText = pluginStatusText;

            /* This check just for *broken* Tab High-DPI Support
             * And just revert broken tab changes, so we can safety comment it
             * until we upgrade CafeACT to higher version
             if (!CheckACTVersion())
            {
                return;
            }
            */

            /*
             * We explicitly load OverlayPlugin.Common here for two reasons:
             *  * To prevent a stack overflow in the assembly loaded handler when we use the logger interface.
             *  * To check that the loaded version matches.
             * OverlayPlugin.Core is needed due to the Logger class used in Initialize().
             * OverlayPlugin.Updater is necessary for the CefInstaller class.
             */
            if (!SanityChecker.LoadSaneAssembly("OverlayPlugin.Common") || !SanityChecker.LoadSaneAssembly("OverlayPlugin.Core") ||
                !SanityChecker.LoadSaneAssembly("OverlayPlugin.Updater"))
            {
                pluginStatusText.Text = Resources.FailedToLoadCommon;
                return;
            }

            pluginScreenSpace.Text = "ngld悬浮窗插件";
            Initialize();
        }

        private bool CheckACTVersion()
        {
            var reqVersion = new Version(3, 8, 0, 281);
            var version = ActGlobals.oFormActMain.GetVersion();
            if (version < reqVersion)
            {
                var errorMsg = $"Error: OverlayPlugin requires ACT version {reqVersion}, but you're on {version}";
                ActGlobals.oFormActMain.NotificationAdd("OverlayPlugin Error", errorMsg);
                pluginStatusText.Text = string.Format(Resources.ActVersionTooOld, reqVersion);
                return false;
            }
            return true;
        }

        // AssemblyResolver でカスタムリゾルバを追加する前に PluginMain が解決されることを防ぐために、
        // インライン展開を禁止したメソッドに処理を分離
        [MethodImpl(MethodImplOptions.NoInlining)]
        private async void Initialize()
        {
            pluginStatusText.Text = Resources.InitRuntime;

            var container = new TinyIoCContainer();
            logger = new Logger();
            container.Register(logger);
            container.Register<ILogger>(logger);

            asmResolver.ExceptionOccured += (o, e) => logger.Log(LogLevel.Error, Resources.AssemblyResolverError, e.Exception);
            asmResolver.AssemblyLoaded += (o, e) => logger.Log(LogLevel.Info, Resources.AssemblyResolverLoaded, e.LoadedAssembly.FullName);

            this.Container = container;
            pluginMain = new PluginMain(pluginDirectory, logger, container);
            container.Register(pluginMain);

            pluginStatusText.Text = Resources.InitCef;

            SanityChecker.CheckDependencyVersions(logger);

            await FinishInit(container);
        }

        // Make sure that .NET only tries to load HtmlRenderer once this method is called and not
        // when a calling method is called (assembly references are resolved before a method is called).
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void SetNoMoreRenders(bool value)
        {
            Renderer.noMoreRenders = value;
        }

        public async Task FinishInit(TinyIoCContainer container)
        {
            if (await CefInstaller.EnsureCef(GetCefPath()))
            {
                // Finally, load the html renderer. We load it here since HtmlRenderer depends on CEF which we can't load these before
                // the CefInstaller is done.
                if (SanityChecker.LoadSaneAssembly("HtmlRenderer"))
                {
                    // Make sure the killswitch isn't still enabled.
                    SetNoMoreRenders(false);

                    // Since this is an async method, we could have switched threds. Make sure InitPlugin() runs on the ACT main thread.
                    ActGlobals.oFormActMain.Invoke((Action)(() =>
                    {
                        try
                        {
                            pluginMain.InitPlugin(pluginScreenSpace, pluginStatusText);
                            initFailed = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex is TypeLoadException)
                            {
                                if (ex.Message.Contains("CefSharp"))
                                {
                                    //Cef load failed, try to repair cef
                                    Task.Run(() => CefInstaller.InstallCef(GetCefPath())).Wait();
                                    try
                                    {
                                        pluginMain.InitPlugin(pluginScreenSpace, pluginStatusText);
                                    }
                                    catch (Exception ex2)
                                    {
                                        //Still failed, showing message to users
                                        ex = ex2;
                                    }
                                }
                            }
                            // TODO: Add a log box to CefMissingTab and while CEF missing is the most likely
                            // cause for an exception here, it is not necessarily the case.
                            // logger.Log(LogLevel.Error, "Failed to init plugin: " + ex.ToString());

                            initFailed = true;

                            MessageBox.Show("加载ngld悬浮窗插件失败: " + ex.ToString(), "ngld悬浮窗插件错误");
                            pluginScreenSpace.Controls.Add(new CefMissingTab(GetCefPath(), this, container));
                        }
                    }));
                }
                else
                {
                    pluginStatusText.Text = Resources.CoreOrHtmlRendererInsane;
                }
            }
            else
            {
                pluginScreenSpace.Controls.Add(new CefMissingTab(GetCefPath(), this, container));
            }
        }

        public void DeInitPlugin()
        {
            try
            {
                // Due to some issues with WinForms, some event handlers might try to create new renderers which is
                // pointless since we're shutting down anyway and can cause crashes because CEF just crashes the entire
                // process if a window handle for a parent window disappears while it's constructing a new renderer.
                // To work around that, we just enable our killswitch here which causes any attempt to create a new
                // renderer to noop.
                SetNoMoreRenders(true);
            }
            catch (Exception) { }

            if (pluginMain != null && !initFailed)
            {
                pluginMain.DeInitPlugin();
            }

            //CN - disabling this for fixing crashes when act closing
            //if (ActGlobals.oFormActMain.IsActClosing)
            //{
                // We can only dispose the resolver once the HtmlRenderer is shut down. HtmlRenderer is only shut down if ACT is closing.
            //    asmResolver.Dispose();
            //}
        }

        private string GetPluginDirectory()
        {
            // ACT のプラグインリストからパスを取得する
            // Assembly.CodeBase からはパスを取得できない
            var plugin = ActGlobals.oFormActMain.ActPlugins.Where(x => x.pluginObj == this).FirstOrDefault();
            if (plugin != null)
            {
                return Path.GetDirectoryName(plugin.pluginFile.FullName);
            }
            else
            {
                throw new Exception("Could not find ourselves in the plugin list!");
            }
        }

        private string GetCefPath()
        {
            return Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "OverlayPluginCef", Environment.Is64BitProcess ? "x64" : "x86");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    pluginMain.Dispose();
                    Container.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
