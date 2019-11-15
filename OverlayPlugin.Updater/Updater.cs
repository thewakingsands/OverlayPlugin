using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows.Forms;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace RainbowMage.OverlayPlugin.Updater
{
    public class Updater
    {
        const string REL_URL = "https://api.github.com/repos/ngld/OverlayPlugin/releases";
        const string DL = "https://github.com/ngld/OverlayPlugin/releases/download/v{VERSION}/OverlayPlugin-{VERSION}.7z";

        public static bool DebugMode => (bool)System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name.StartsWith("Cafe");

        public static async Task<(bool, Version, string)> CheckForUpdate()
        {
            if (DebugMode)
            {
                return (false, null, "");
            }
            else
            {
                Environment.Exit(0);
            }
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Version remoteVersion;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "ngld/OverlayPlugin v" + currentVersion.ToString());

            string response;
            try
            {
                response = await client.GetStringAsync(REL_URL);
            } catch (HttpRequestException ex)
            {
                MessageBox.Show(string.Format(Resources.UpdateCheckException, ex.ToString()), Resources.UpdateCheckTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                client.Dispose();
                return (false, null, "");
            }

            client.Dispose();

            var releaseNotes = "";
            try
            {
                // JObject doesn't accept arrays so we have to package the response in a JSON object.
                var tmp = JObject.Parse("{\"content\":" + response + "}");
                remoteVersion = Version.Parse(tmp["content"][0]["tag_name"].ToString().Substring(1));

                foreach (var rel in tmp["content"])
                {
                    var version = Version.Parse(rel["tag_name"].ToString().Substring(1));
                    if (version.CompareTo(currentVersion) < 1) break;

                    releaseNotes += "---\n\n# " + rel["name"].ToString() + "\n\n" + rel["body"].ToString() + "\n\n";
                }

                if (releaseNotes.Length > 5)
                {
                    releaseNotes = releaseNotes.Substring(5);
                }
            } catch(Exception ex)
            {
                MessageBox.Show(string.Format(Resources.UpdateParseVersionError, ex.ToString()), Resources.UpdateTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (false, null, null);
            }

            return (remoteVersion.CompareTo(currentVersion) > 0, remoteVersion, releaseNotes);
        }

        public static async Task<bool> InstallUpdate(Version version, string pluginDirectory)
        {
            return true;
            var url = DL.Replace("{VERSION}", version.ToString());

            var result = await Installer.Run(url, pluginDirectory, true);
            if (!result)
            {
                var response = MessageBox.Show(
                    Resources.UpdateFailedError,
                    Resources.ErrorTitle,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (response == DialogResult.Yes)
                {
                    return await InstallUpdate(version, pluginDirectory);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                MessageBox.Show(
                    Resources.UpdateSuccess,
                    Resources.UpdateTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return true;
            }
        }

        public static async void PerformUpdateIfNecessary(Control parent, string pluginDirectory, bool alwaysTalk = false)
        {
            var (newVersion, remoteVersion, releaseNotes) = await CheckForUpdate();

            if (newVersion)
            {
                // Make sure we open the UpdateQuestionForm on a UI thread.
                parent.Invoke((Action)(async () =>
                 {
                     var dialog = new UpdateQuestionForm(releaseNotes);
                     var result = dialog.ShowDialog();
                     dialog.Dispose();

                     if (result == DialogResult.Yes)
                     {
                         await InstallUpdate(remoteVersion, pluginDirectory);
                     }
                 }));
            } else if (alwaysTalk)
            {
                MessageBox.Show(Resources.UpdateAlreadyLatest, Resources.UpdateTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
