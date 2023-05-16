using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using Newtonsoft.Json.Linq;
using RainbowMage.HtmlRenderer;
using RainbowMage.OverlayPlugin.MemoryProcessors;
using RainbowMage.OverlayPlugin.NetworkProcessors;

namespace RainbowMage.OverlayPlugin.EventSources
{
    public class CactbotEventSource : EventSourceBase
    {
        public CactbotEventSourceConfig Config { get; private set; }

        private static int kFastTimerMilli = 16;
        private static int kSlowTimerMilli = 300;
        private static int kUberSlowTimerMilli = 3000;
        private static int kRequiredNETVersionMajor = 4;
        private static int kRequiredNETVersionMinor = 6;

        private SemaphoreSlim log_lines_semaphore_ = new SemaphoreSlim(1);
        // Not thread-safe, as OnLogLineRead may happen at any time. Use |log_lines_semaphore_| to access it.
        private List<string> log_lines_ = new List<string>(40);
        // Used on the fast timer to avoid allocing List every time.
        private List<string> last_log_lines_ = new List<string>(40);

        // When true, the update function should reset notify state back to defaults.
        private bool reset_notify_state_ = false;

        private System.Timers.Timer fast_update_timer_;
        // Held while the |fast_update_timer_| is running.
        private FFXIVProcess ffxiv_;
        private FateWatcher fate_watcher_;

        private Version overlayVersion_;
        private Version ffxivPluginVersion_;
        private Version actVersion_;
        private GameRegion gameRegion_;
        private string language_ = null;
        private string pc_locale_ = null;
        private List<FileSystemWatcher> watchers;

        private PluginMain pluginMain;

        public const string ForceReloadEvent = "onForceReload";
        public const string GameExistsEvent = "onGameExistsEvent";
        public const string GameActiveChangedEvent = "onGameActiveChangedEvent";
        public const string LogEvent = "onLogEvent";
        public const string ImportLogEvent = "onImportLogEvent";
        public const string InCombatChangedEvent = "onInCombatChangedEvent";
        public const string ZoneChangedEvent = "onZoneChangedEvent";
        public const string PlayerDiedEvent = "onPlayerDied";
        public const string PartyWipeEvent = "onPartyWipe";
        public const string FateEvent = "onFateEvent";
        public const string CEEvent = "onCEEvent";
        public const string PlayerChangedEvent = "onPlayerChangedEvent";
        public const string SendSaveDataEvent = "onSendSaveData";
        public const string DataFilesReadEvent = "onDataFilesRead";
        public const string InitializeOverlayEvent = "onInitializeOverlay";

        public void Wipe()
        {
            ActGlobals.oFormActMain.EndCombat(false);
            DispatchToJS(new JSEvents.PartyWipeEvent());
        }

        public CactbotEventSource(TinyIoCContainer container)
            : base(container)
        {
            pluginMain = container.Resolve<PluginMain>();

            Name = "Cactbot";

            RegisterPresets();

            RegisterEventTypes(new List<string>()
            {
                "onForceReload",
                "onGameExistsEvent",
                "onGameActiveChangedEvent",
                "onLogEvent",
                "onImportLogEvent",
                "onInCombatChangedEvent",
                "onZoneChangedEvent",
                "onFateEvent",
                "onCEEvent",
                "onPlayerDied",
                "onPartyWipe",
                "onPlayerChangedEvent",
                "onUserFileChanged",
            });

            // Broadcast onConfigChanged when a cactbotNotifyConfigChanged message occurs.
            RegisterEventHandler("cactbotReloadOverlays", (msg) =>
            {
                DispatchToJS(new JSEvents.ForceReloadEvent());
                return null;
            });
            RegisterEventHandler("cactbotLoadUser", FetchUserFiles);
            RegisterEventHandler("cactbotReadDataFiles", FetchDataFiles);
            RegisterEventHandler("cactbotRequestPlayerUpdate", (msg) =>
            {
                notify_state_.player = null;
                return null;
            });
            RegisterEventHandler("cactbotRequestState", (msg) =>
            {
                reset_notify_state_ = true;
                return null;
            });
            RegisterEventHandler("cactbotSay", (msg) =>
            {
                ActGlobals.oFormActMain.TTS(msg["text"].ToString());
                return null;
            });
            RegisterEventHandler("cactbotSaveData", (msg) =>
            {
                Config.OverlayData[msg["overlay"].ToString()] = msg["data"];
                Config.OnUpdateConfig();
                return null;
            });
            RegisterEventHandler("cactbotLoadData", (msg) =>
            {
                if (Config.OverlayData.ContainsKey(msg["overlay"].ToString()))
                {
                    var ret = new JObject();
                    ret["data"] = Config.OverlayData[msg["overlay"].ToString()];
                    return ret;
                }
                else
                {
                    return null;
                }
            });
            RegisterEventHandler("cactbotChooseDirectory", (msg) =>
            {
                var ret = new JObject();
                string data = (string)ActGlobals.oFormActMain.Invoke((ChooseDirectoryDelegate)ChooseDirectory);
                if (data != null)
                    ret["data"] = data;
                return ret;
            });
        }

        private void Log(LogLevel level, string msg)
        {
            logger.Log(level, "Cactbot: " + msg);
        }

        private delegate string ChooseDirectoryDelegate();

        private string ChooseDirectory()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog(ActGlobals.oFormActMain);
            if (result != DialogResult.OK)
                return null;
            return dialog.SelectedPath;
        }

        public override Control CreateConfigControl()
        {
            var control = new OverlayControl();
            var initDone = false;

            var url = pluginMain.OfflineCactbotConfigHtmlFile;
            control.VisibleChanged += (o, e) =>
            {
                if (initDone)
                    return;
                initDone = true;
                control.Init(url);
                MinimalApi.AttachTo(control.Renderer, container);
            };
            return control;
        }

        public override void LoadConfig(IPluginConfig config)
        {
            Config = CactbotEventSourceConfig.LoadConfig(config, logger);
            if (Config.OverlayData == null)
                Config.OverlayData = new Dictionary<string, JToken>();
        }

        public override void SaveConfig(IPluginConfig config)
        {
            Config.SaveConfig(config);
        }

        public override void Start()
        {
            var ffxiv = container.Resolve<FFXIVRepository>();
            if (!ffxiv.IsFFXIVPluginPresent())
            {
                Log(LogLevel.Error, "FFXIV plugin not found. Not initializing.");
                return;
            }

            // Our own timer with a higher frequency than OverlayPlugin since we want to see
            // the effect of log messages quickly.
            // TODO: Cleanup; Log messages are distributed through events and skip the update
            //   loop. Memory scanning needs a high frequencey but that's handled by the
            //   MemoryProcessor classes which raise events.
            //   Everything else should be handled through events to avoid unnecessary polling.
            //   -- ngld
            fast_update_timer_ = new System.Timers.Timer();
            fast_update_timer_.Elapsed += (o, args) =>
            {
                int timer_interval = kSlowTimerMilli;
                try
                {
                    timer_interval = SendFastRateEvents();
                }
                catch (Exception e)
                {
                    // SendFastRateEvents holds this semaphore until it exits.
                    LogError("Exception in SendFastRateEvents: " + e.Message);
                    LogError("Stack: " + e.StackTrace);
                    LogError("Source: " + e.Source);
                }
                fast_update_timer_.Interval = timer_interval;
            };
            fast_update_timer_.AutoReset = false;

            language_ = "cn";
            pc_locale_ = System.Globalization.CultureInfo.CurrentUICulture.Name;

            overlayVersion_ = typeof(IOverlay).Assembly.GetName().Version;
            ffxivPluginVersion_ = ffxiv.GetPluginVersion();
            actVersion_ = typeof(ActGlobals).Assembly.GetName().Version;
            gameRegion_ = GameRegion.Chinese;

            // Print out version strings and locations to help users debug.
            LogInfo("OverlayPlugin: {0} {1}", overlayVersion_.ToString(), typeof(IOverlay).Assembly.Location);
            LogInfo("FFXIV Plugin: {0} {1}", ffxivPluginVersion_.ToString(), ffxiv.GetPluginPath());
            LogInfo("ACT: {0} {1}", actVersion_.ToString(), typeof(ActGlobals).Assembly.Location);

            if (language_ == null)
            {
                LogInfo("Parsing Plugin Language: {0}", "(unknown)");
            }
            else
            {
                LogInfo("Parsing Plugin Language: {0}", language_);
            }
            if (pc_locale_ == null)
            {
                LogInfo("System Locale: {0}", "(unknown)");
            }
            else
            {
                LogInfo("System Locale: {0}", pc_locale_);
            }


            ffxiv_ = new FFXIVProcessCn(container);

            // Avoid initialization races by always calling OnProcessChanged with the current process
            // in case the ffxiv plugin has already sent this event and it never changes again.

            // Comment this since it has done in FFXIVProcess and RainbowMage.OverlayPlugin.FFXIVRepository.RegisterProcessChangedHandler
            // plugin_helper.RegisterProcessChangedHandler(ffxiv_.OnProcessChanged);
            // ffxiv_.OnProcessChanged(plugin_helper.GetCurrentProcess());

            fate_watcher_ = new FateWatcher(container);
            fate_watcher_.OnFateChanged += (o, e) => DispatchToJS(new JSEvents.FateEvent(e.eventType, e.fateID, e.progress));
            fate_watcher_.OnCEChanged += (o, e) => DispatchToJS(new JSEvents.CEEvent(e.eventType, JObject.FromObject(e.data)));

            // Incoming events.
            ActGlobals.oFormActMain.OnLogLineRead += OnLogLineRead;

            fast_update_timer_.Interval = kFastTimerMilli;
            fast_update_timer_.Start();

            string net_version_str = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(int).Assembly.Location).ProductVersion;
            string[] net_version = net_version_str.Split('.');
            if (int.Parse(net_version[0]) < kRequiredNETVersionMajor || int.Parse(net_version[1]) < kRequiredNETVersionMinor)
                Log(LogLevel.Error, "Requires .NET 4.6 or above. Using " + net_version_str);

            // Start watching files after the update check.
            Config.WatchFileChangesChanged += (o, e) =>
            {
                if (Config.WatchFileChanges)
                {
                    StartFileWatcher();
                }
                else
                {
                    StopFileWatcher();
                }
            };

            if (Config.WatchFileChanges)
            {
                StartFileWatcher();
            }
        }

        public override void Stop()
        {
            if (fast_update_timer_ != null)
                fast_update_timer_.Stop();

            ActGlobals.oFormActMain.OnLogLineRead -= OnLogLineRead;
        }

        protected override void Update()
        {
            // Nothing to do since this is handled in SendFastRateEvents.
        }

        public void ClearFateWatcherDictionaries()
        {
            fate_watcher_.RemoveAndClearCEs();
            fate_watcher_.RemoveAndClearFates();
        }

        private void OnLogLineRead(bool isImport, LogLineEventArgs args)
        {
            log_lines_semaphore_.Wait();
            if (!isImport)
                log_lines_.Add(args.logLine);
            log_lines_semaphore_.Release();
        }

        // Sends an event called |event_name| to javascript, with an event.detail that contains
        // the fields and values of the |detail| structure.
        public void DispatchToJS(JSEvent detail)
        {
            JObject ev = new JObject();
            ev["type"] = detail.EventName();
            ev["detail"] = JObject.FromObject(detail);
            DispatchEvent(ev);
        }

        // Events that we want to update as soon as possible.  Return next time this should be called.
        private int SendFastRateEvents()
        {
            if (reset_notify_state_)
                notify_state_ = new NotifyState();
            reset_notify_state_ = false;

            // Loading dance:
            // * OverlayPlugin loads addons and initializes event sources.
            // * OverlayPlugin loads its configuration.
            // * Event sources are told to load their configuration and start (LoadConfig and Start are called).
            // * Overlays are initialised and the browser instances are started. At this points the overlays start loading.
            // * At some point the overlay's JavaScript is executed and OverlayPluginApi is injected. This order isn't
            //   deterministic and depends on what the ACT process is doing at that point in time. During startup the
            //   OverlayPluginApi is usually injected after the overlay is done loading while an overlay that's reloaded or
            //   loaded later on will see the OverlayPluginApi before the page has loaded.
            // * The overlay JavaScript sets up the initial event handlers and calls the cactbotLoadUser handler through
            //   getUserConfigLocation. These actions are queued by the JS implementation in overlay_plugin_api.js until OverlayPluginApi
            //   (or the WebSocket) is available. Once it is, the event subscriptions and handler calls are transmitted.
            // * OverlayPlugin stores the event subscriptions and executes the C# handlers which in this case means
            //   FetchUserFiles is called. That method loads the user files and returns them. The result is now transmitted
            //   back to the overlay that called the handler and the Promise in JS is resolved with the result.
            // * getUserConfigLocation processes the received information and calls the passed callback. This constructs the
            //   overlay specific objects and registers additional event handlers. Finally, the cactbotRequestState handler
            //   is called.
            // * OverlayPlugin processes the new event subscriptions and executes the cactbotRequestState handler.
            // * The next time SendFastRateEvents is called, it resets notify_state_ (since the previous handler set
            //   reset_notify_state_ to true) which causes it to dispatch all state events again. These events are now
            //   dispatched to all subscribed overlays. However, this means that overlays can receive state events multiple
            //   times during startup. If the user has three Cactbot overlays, all of them will call cactbotRequestState and
            //   thus cause this to happen one to three times depending on their timing. This shouldn't cause any issues but
            //   it's a waste of CPU cycles.
            // * Since this only happens during startup, it's probably not worth fixing though. Not sure.

            bool game_exists = ffxiv_.HasProcess();
            if (game_exists != notify_state_.game_exists)
            {
                notify_state_.game_exists = game_exists;
                DispatchToJS(new JSEvents.GameExistsEvent(game_exists));
            }

            bool game_active = game_active = ffxiv_.IsActive();
            if (game_active != notify_state_.game_active)
            {
                notify_state_.game_active = game_active;
                DispatchToJS(new JSEvents.GameActiveChangedEvent(game_active));
            }

            // Silently stop sending other messages if the ffxiv process isn't around.
            if (!game_exists)
            {
                return kUberSlowTimerMilli;
            }

            // onInCombatChangedEvent: Fires when entering or leaving combat.
            bool in_act_combat = ActGlobals.oFormActMain.InCombat;
            bool in_game_combat = ffxiv_.GetInGameCombat();
            if (!notify_state_.in_act_combat.HasValue || in_act_combat != notify_state_.in_act_combat ||
                !notify_state_.in_game_combat.HasValue || in_game_combat != notify_state_.in_game_combat)
            {
                notify_state_.in_act_combat = in_act_combat;
                notify_state_.in_game_combat = in_game_combat;
                DispatchToJS(new JSEvents.InCombatChangedEvent(in_act_combat, in_game_combat));
            }

            // onZoneChangedEvent: Fires when the player changes their current zone.
            string zone_name = ActGlobals.oFormActMain.CurrentZone;
            if (notify_state_.zone_name == null || !zone_name.Equals(notify_state_.zone_name))
            {
                notify_state_.zone_name = zone_name;
                DispatchToJS(new JSEvents.ZoneChangedEvent(zone_name));
                ClearFateWatcherDictionaries();
            }

            DateTime now = DateTime.Now;
            // The |player| can be null, such as during a zone change.
            FFXIVProcess.EntityData player = ffxiv_.GetSelfData();

            // onPlayerDiedEvent: Fires when the player dies. All buffs/debuffs are
            // lost.
            if (player != null)
            {
                bool dead = player.hp == 0;
                if (dead != notify_state_.dead)
                {
                    notify_state_.dead = dead;
                    if (dead)
                        DispatchToJS(new JSEvents.PlayerDiedEvent());
                }
            }

            // onPlayerChangedEvent: Fires when current player data changes.
            if (player != null)
            {
                bool send = false;
                if (!player.Equals(notify_state_.player))
                {
                    // Clear the FateWatcher dictionaries if we switched characters
                    if (notify_state_.player != null && !player.name.Equals(notify_state_.player.name))
                    {
                        ClearFateWatcherDictionaries();
                    }
                    notify_state_.player = player;
                    send = true;
                }
                var job = ffxiv_.GetJobSpecificData(player.job);
                if (job != null)
                {
                    if (send || !JObject.DeepEquals(job, notify_state_.job_data))
                    {
                        notify_state_.job_data = job;
                        var ev = new JSEvents.PlayerChangedEvent(player);
                        ev.jobDetail = job;
                        DispatchToJS(ev);
                    }
                }
                else if (send)
                {
                    // No job-specific data.
                    DispatchToJS(new JSEvents.PlayerChangedEvent(player));
                }
            }

            // onLogEvent: Fires when new combat log events from FFXIV are available. This fires after any
            // more specific events, some of which may involve parsing the logs as well.
            List<string> logs;
            log_lines_semaphore_.Wait();
            logs = log_lines_;
            log_lines_ = last_log_lines_;

            log_lines_semaphore_.Release();

            if (logs.Count > 0)
            {
                DispatchToJS(new JSEvents.LogEvent(logs));
                logs.Clear();
            }

            last_log_lines_ = logs;

            return game_active ? kFastTimerMilli : kSlowTimerMilli;
        }

        // ILogger implementation.
        public void LogDebug(string format, params object[] args)
        {
            this.Log(LogLevel.Debug, format, args);
        }
        public void LogError(string format, params object[] args)
        {
            this.Log(LogLevel.Error, format, args);
        }
        public void LogWarning(string format, params object[] args)
        {
            this.Log(LogLevel.Warning, format, args);
        }
        public void LogInfo(string format, params object[] args)
        {
            this.Log(LogLevel.Info, format, args);
        }

        private Dictionary<string, string> GetDataFiles(string url)
        {
            // Uri is not smart enough to strip the query args here, so we'll do it manually?
            var idx = url.IndexOf('?');
            if (idx > 0)
                url = url.Substring(0, idx);

            // If file is a remote pointer, load that file explicitly so that the manifest
            // is relative to the pointed to url and not the local file.
            if (url.StartsWith("file:///"))
            {
                var html = File.ReadAllText(new Uri(url).LocalPath);
                var match = System.Text.RegularExpressions.Regex.Match(html, @"<meta http-equiv=""refresh"" content=""0; url=(.*)?""\/?>");
                if (match.Groups.Count > 1)
                {
                    url = match.Groups[1].Value;
                }
            }

            // TODO: Reimplement
            // return new Dictionary<string, string>();

            var web = new System.Net.WebClient();
            web.Encoding = System.Text.Encoding.UTF8;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

            var data_file_paths = new List<string>();
            try
            {
                var data_dir_manifest = new Uri(new Uri(url), "data/manifest.txt");
                var manifest_reader = new StringReader(web.DownloadString(data_dir_manifest));
                for (var line = manifest_reader.ReadLine(); line != null; line = manifest_reader.ReadLine())
                {
                    line = line.Trim();
                    if (line.Length > 0)
                        data_file_paths.Add(line);
                }
            }
            catch (System.Net.WebException e)
            {
                if (e.Status == System.Net.WebExceptionStatus.ProtocolError &&
                    e.Response is System.Net.HttpWebResponse &&
                    ((System.Net.HttpWebResponse)e.Response).StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Ignore file not found.
                }
                else if (e.InnerException != null &&
                (e.InnerException is FileNotFoundException || e.InnerException is DirectoryNotFoundException))
                {
                    // Ignore file not found.
                }
                else if (e.InnerException != null && e.InnerException.InnerException != null &&
                (e.InnerException.InnerException is FileNotFoundException || e.InnerException.InnerException is DirectoryNotFoundException))
                {
                    // Ignore file not found.
                }
                else
                {
                    LogError("Unable to read manifest file: " + e.Message);
                }
            }
            catch (Exception e)
            {
                LogError("Unable to read manifest file: " + e.Message);
            }

            if (data_file_paths.Count > 0)
            {
                var file_data = new Dictionary<string, string>();
                foreach (string data_filename in data_file_paths)
                {
                    try
                    {
                        var file_path = new Uri(new Uri(url), "data/" + data_filename);
                        file_data[data_filename] = web.DownloadString(file_path);
                        LogDebug("Read file " + data_filename);
                    }
                    catch (Exception e)
                    {
                        LogError("Unable to read data file: " + e.Message);
                    }
                }

                return file_data;
            }

            return null;
        }

        private JObject FetchDataFiles(JObject msg)
        {
            var result = GetDataFiles(msg["source"].ToString());

            var container = new JObject();
            container["files"] = result == null ? null : JObject.FromObject(result);

            var output = new JObject();
            output["detail"] = container;

            return output;
        }

        private Dictionary<string, string> GetLocalUserFiles(string config_dir, string overlay_name)
        {
            // TODO: probably should sanity check overlay_name for no * or ? wildcards as well
            // as GetInvalidPathChars.
            if (String.IsNullOrEmpty(config_dir))
                return null;

            var user_files = new Dictionary<string, string>();
            string top_dir;
            string sub_dir = null;
            try
            {
                top_dir = new Uri(config_dir).LocalPath;
            }
            catch (UriFormatException)
            {
                // This can happen e.g. "http://localhost:8000".  Thanks, Uri constructor.  /o\
                return null;
            }

            // Returning null here means we failed to find anything meaningful (or error), and so try
            // again with a different directory.  In the future when this is in OverlayPlugin, we will
            // probably just abort entirely.
            try
            {
                if (!Directory.Exists(top_dir))
                {
                    return null;
                }

                if (overlay_name != null)
                {
                    sub_dir = Path.Combine(top_dir, overlay_name);
                    if (!Directory.Exists(sub_dir))
                        sub_dir = null;
                }

            }
            catch (Exception e)
            {
                LogError("用户配置目录检查错误 {0}", e.ToString());
                return null;
            }

            // Hack for backwards compat with older js that doesn't provide overlay_name,
            // just in case.  Remove this in any future version and require overlay_name.
            if (overlay_name == null)
                overlay_name = "*";

            try
            {
                var filenames = Directory.EnumerateFiles(top_dir, $"{overlay_name}.js").Concat(
                  Directory.EnumerateFiles(top_dir, $"{overlay_name}.css"));
                if (sub_dir != null)
                {
                    filenames = filenames.Concat(
                      Directory.EnumerateFiles(sub_dir, "*.js", SearchOption.AllDirectories)).Concat(
                      Directory.EnumerateFiles(sub_dir, "*.css", SearchOption.AllDirectories));
                }
                foreach (string filename in filenames)
                {
                    user_files[GetRelativePath(top_dir, filename)] = File.ReadAllText(filename) +
                      $"\n//# sourceURL={filename}";
                }

                var textFilenames = Directory.EnumerateFiles(top_dir, "*.txt");
                if (sub_dir != null)
                {
                    textFilenames = textFilenames.Concat(Directory.EnumerateFiles(sub_dir, "*.txt", SearchOption.AllDirectories));
                }
                foreach (string filename in textFilenames)
                {
                    user_files[GetRelativePath(top_dir, filename)] = File.ReadAllText(filename);
                }
            }
            catch (Exception e)
            {
                LogError("用户目录加载失败 {0}", e.ToString());
            }

            return user_files;
        }

        private void GetUserConfigDirAndFiles(string source, string overlay_name, out string config_dir, out Dictionary<string, string> local_files)
        {
            local_files = null;
            config_dir = null;

            if (Config.UserConfigFile != null && Config.UserConfigFile != "")
            {
                // Explicit user config directory specified.
                config_dir = Config.UserConfigFile;
                local_files = GetLocalUserFiles(config_dir, overlay_name);
            }
            else
            {
                if (source != null && source != "")
                {
                    // First try a user directory relative to the html.
                    try
                    {
                        config_dir = Path.Combine(pluginMain.OfflineCactbotDirectory, "user");
                        local_files = GetLocalUserFiles(config_dir, overlay_name);
                    }
                    catch (Exception e)
                    {
                        LogError("Error checking html rel dir: {0}: {1}", source, e.ToString());
                        config_dir = null;
                        local_files = null;
                    }
                }
            }
            // Set any implicitly discovered cactbot user config dirs as explicit.
            // This will help in the future when there aren't local plugins or html.
            if (config_dir != null)
                Config.UserConfigFile = config_dir;
        }

        private JObject FetchUserFiles(JObject msg)
        {
            Dictionary<string, string> user_files;
            var overlay_name = msg.ContainsKey("overlayName") ? msg["overlayName"].ToString() : null;
            GetUserConfigDirAndFiles(msg["source"].ToString(), overlay_name, out string config_dir, out user_files);

            var result = new JObject();
            result["userLocation"] = config_dir;
            result["localUserFiles"] = user_files == null ? null : JObject.FromObject(user_files);

            result["parserLanguage"] = language_;
            result["systemLocale"] = pc_locale_;
            result["displayLanguage"] = Config.DisplayLanguage;
            // For backwards compatibility:
            result["language"] = language_;

            //It's unknown for ffcafe, but leave stub here should be better incase some overlays using that
            result["cactbotVersion"] = "0.28.20.0";
            result["overlayPluginVersion"] = overlayVersion_.ToString();
            result["ffxivPluginVersion"] = ffxivPluginVersion_.ToString();
            result["actVersion"] = actVersion_.ToString();
            result["gameRegion"] = gameRegion_.ToString();

            var response = new JObject();
            response["detail"] = result;
            return response;
        }

        private void StartFileWatcher()
        {
            watchers = new List<FileSystemWatcher>();
            var paths = new List<string>();

            paths.Add(Config.UserConfigFile);

            foreach (var path in paths)
            {
                if (String.IsNullOrEmpty(path))
                    continue;

                var watchDir = "";
                try
                {
                    // Get canonical url for paths so that Directory.Exists will work properly.
                    watchDir = Path.GetFullPath(Path.GetDirectoryName(new Uri(path).LocalPath));
                }
                catch
                {
                    continue;
                }

                if (!Directory.Exists(watchDir))
                    continue;

                var watcher = new FileSystemWatcher()
                {
                    Path = watchDir,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                    IncludeSubdirectories = true,
                };

                // We only care about file changes. New or renamed files don't matter if we don't have a reference to them
                // and adding a new reference causes an existing file to change.
                watcher.Changed += (o, e) =>
                {
                    DispatchEvent(JObject.FromObject(new
                    {
                        type = "onUserFileChanged",
                        file = e.FullPath,
                    }));
                };

                watcher.EnableRaisingEvents = true;
                watchers.Add(watcher);

                LogInfo("Started watching {0}", watchDir);
            }
        }

        private void StopFileWatcher()
        {
            foreach (var watcher in watchers)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }

            watchers = null;
        }

        struct OverlayPreset : IOverlayPreset
        {
            public string Name { get; set; }
            public string Type { get { return "MiniParse"; } }
            public string Url { get; set; }
            public int[] Size { get; set; }
            public bool Locked { get; set; }
            public List<string> Supports { get { return new List<string> { "modern" }; } }
        }

        private void RegisterPreset(string dirName, int width, int height, string nameOverride = null, string fileOverride = null)
        {
            string lc = dirName.ToLowerInvariant();
            var name = nameOverride != null ? nameOverride : dirName;
            var filename = fileOverride != null ? fileOverride : dirName;

            Registry.RegisterOverlayPreset(new OverlayPreset
            {
                Name = $"Cactbot {name}",
                Url = Path.Combine(pluginMain.OfflineCactbotDirectory, "ui", lc, $"{filename}.html"), // Path.Combine("https://quisquous.github.io/cactbot", "ui", lc, $"{filename}.html"),
                Size = new int[] { width, height },
                Locked = false,
            });
        }

        private static string GetRelativePath(string top_dir, string filename)
        {
            // TODO: .net 5.0 / .net core 2.0 has Path.GetRelativePath.
            // There's also a win api function we could call, but that's a bit gross.
            // However, this is an easy case where filename is known to be rooted in top_dir,
            // so use this hacky solution for now.  Hi, ngld.
            string initial = filename;
            filename = filename.Replace(top_dir, "");
            // top_dir may or may not have a trailing slash, so remove that as well.
            // user_config.js expects filenames to not have a beginning slash.
            while (filename[0] == '\\' || filename[0] == '/')
                filename = filename.Substring(1);
            return filename;
        }


        private void RegisterDpsPreset(string name, string file, int width, int height, string displayName)
        {
            string lc = name.ToLowerInvariant();
            Registry.RegisterOverlayPreset(new OverlayPreset
            {
                Name = $"Cactbot {displayName}",
                Url = Path.Combine(pluginMain.OfflineCactbotDirectory, "ui", "dps", lc, $"{file}.html"), // Path.Combine("https://quisquous.github.io/cactbot", "ui", "dps", lc, $"{file}.html"),
                Size = new int[] { width, height },
                Locked = false,
            });
        }

        private void RegisterPresets()
        {
            RegisterPreset("Raidboss", width: 1100, height: 300, "副本辅助(时间轴+触发器)", "raidboss");
            RegisterPreset("Raidboss", width: 1100, height: 300, "副本辅助(单触发器)", "raidboss_alerts_only");
            RegisterPreset("Raidboss", width: 320, height: 220, "副本辅助(单时间轴)", "raidboss_timeline_only");
            RegisterPreset("Jobs", width: 600, height: 300, "职业辅助");
            RegisterPreset("Eureka", width: 400, height: 400, "优雷卡辅助");
            RegisterPreset("Fisher", width: 500, height: 500, "捕鱼辅助");
            RegisterPreset("OopsyRaidsy", width: 400, height: 400, "犯错监控");
            RegisterPreset("PullCounter", width: 200, height: 200, "团灭计数");
            RegisterPreset("Radar", width: 300, height: 400, "狩猎雷达");
            RegisterPreset("Test", width: 300, height: 300, "测试");
            // FIXME: these should be consistently named.
            RegisterDpsPreset("Xephero", "xephero-cactbot", width: 600, height: 400, "Xephero DPS 悬浮窗");
            RegisterDpsPreset("Rdmty", "dps", width: 600, height: 400, "Rdmty DPS 悬浮窗");
        }

        // State that is tracked and sent to JS when it changes.
        private class NotifyState
        {
            public bool added_dom_content_listener = false;
            public bool dom_content_loaded = false;
            public bool sent_data_dir = false;
            public bool game_exists = false;
            public bool game_active = false;
            public bool? in_act_combat;
            public bool? in_game_combat;
            public bool dead = false;
            public string zone_name = null;
            public JObject job_data = new JObject();
            public FFXIVProcess.EntityData player = null;
        }
        private NotifyState notify_state_ = new NotifyState();
    }

}  // namespace Cactbot
