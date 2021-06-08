using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Reflection;

namespace RainbowMage.OverlayPlugin.Updater
{
    public class CefInstaller
    {
        const string CEF_VERSION = "90.6.70";

        public static string GetUrl()
        {
            return "https://ffcafe.org/";
        }

        public static async Task<bool> EnsureCef(string cefPath)
        {
            // Ensure we have a working MSVCRT first
            var lib = IntPtr.Zero;
            while (true)
            {
                lib = NativeMethods.LoadLibrary("msvcp140.dll");
                if (lib != IntPtr.Zero)
                {
                    NativeMethods.FreeLibrary(lib);
                    break;
                }

                var response = MessageBox.Show(
                    Resources.MsvcrtMissing,
                    Resources.OverlayPluginTitle,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (response == DialogResult.Yes)
                {
                    await InstallMsvcrt();

                    return false;
                }
                else
                {
                    return false;
                }
            }

            var manifest = Path.Combine(cefPath, "version.txt");
            var importantFiles = new List<string>() { "CefSharp.dll", "CefSharp.Core.dll", "CefSharp.OffScreen.dll", "CefSharp.BrowserSubprocess.exe", "CefSharp.BrowserSubprocess.Core.dll", "libcef.dll", "libEGL.dll", "libGLESv2.dll" };

            if (File.Exists(manifest))
            {
                var installed = File.ReadAllText(manifest).Trim();
                if (installed == CEF_VERSION)
                {
                    // Verify all important files exist
                    var itsFine = true;
                    foreach (var name in importantFiles)
                    {
                        if (!File.Exists(Path.Combine(cefPath, name)))
                        {
                            itsFine = false;
                            break;
                        }
                    }

                    if (itsFine) return true;
                }
            }

            return await InstallCef(cefPath);
        }

        public static async Task<bool> InstallCef(string cefPath, string archivePath = null)
        {
            var tempNuget = Path.Combine(Path.GetTempPath(), "OverlayPlugin-cefredist");
            Directory.CreateDirectory(cefPath);
            try
            {
                var result = await Installer.DownloadAndExtractTo(GetNupkgUrl("CefSharp.Common", "90.6.70"), "OverlayPluginCef.tmp2", cefPath, "CefSharp/x64/", "第1个，共3个", "lib/net452/");
                if (!result) throw new Exception("下载失败1");

                result = await Installer.DownloadAndExtractTo(GetNupkgUrl("CefSharp.OffScreen", "90.6.70"), "OverlayPluginCef.tmp3", cefPath, "lib/net452/", "第2个，共3个");
                if (!result) throw new Exception("下载失败2");

                result = await Installer.DownloadAndExtractTo(GetNupkgUrl("cef.redist.x64", "90.6.7"), "OverlayPluginCef.tmp1", cefPath, "CEF/", "第3个，共3个");
                if (!result) throw new Exception("下载失败3");

                File.WriteAllText(Path.Combine(cefPath, "version.txt"), CEF_VERSION);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "安装悬浮窗浏览器组件失败，请尝试：\r\n1. 禁用 WebSocket 悬浮窗插件后重启 ACT\r\n2. 关闭360/腾讯电脑管家等安全软件\r\n3. 重启电脑\r\n如果还不能成功，请加群 163386335",
                    Resources.ErrorTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return false;
            }
            return true;
        }

        public static async Task<bool> InstallMsvcrt()
        {
            Process.Start("https://www.yuque.com/ffcafe/act/downloadvc");

            return false;
        }

        public static string GetNupkgUrl(string packageName, string version)
        {
            return $"https://repo.huaweicloud.com/repository/nuget/v3-flatcontainer/{packageName.ToLower()}/{version}/{packageName.ToLower()}.{version}.nupkg";
        }

        private static void CopyFiles(string SourcePath, string DestinationPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
        }
    }
}
