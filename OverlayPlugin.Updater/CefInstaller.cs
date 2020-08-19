using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace RainbowMage.OverlayPlugin.Updater
{
    public class CefInstaller
    {
        const string CEF_DL = "https://github.com/ngld/OverlayPlugin/releases/download/v0.7.0/CefSharp-{CEF_VERSION}-{ARCH}.DO_NOT_DOWNLOAD";
        const string CEF_VERSION = "75.1.14";

        public static string GetUrl()
        {
            return CEF_DL.Replace("{CEF_VERSION}", CEF_VERSION).Replace("{ARCH}", Environment.Is64BitProcess ? "x64" : "x86");
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

            if (File.Exists(manifest))
            {
                var installed = File.ReadAllText(manifest).Trim();
                if (installed == CEF_VERSION)
                {
                    return true;
                }
            }

            return await InstallCef(cefPath);
        }

        public static async Task<bool> InstallCef(string cefPath, string archivePath = null)
        {
            var tempNuget = Path.Combine(Path.GetTempPath(), "OverlayPlugin-cefredist");
            Directory.CreateDirectory(cefPath);

            var result = await Installer.Run(GetNupkgUrl("cef.redist.x64", "75.1.14"), tempNuget, "OverlayPluginCef.tmp1");
            if (result)
            {
                CopyFiles(Path.Combine(tempNuget, "CEF"), cefPath);
            }

            var result2 = await Installer.Run(GetNupkgUrl("CefSharp.Common", "75.1.141"), tempNuget, "OverlayPluginCef.tmp2");
            if (result2)
            {
                CopyFiles(Path.Combine(tempNuget, "CefSharp", "x64"), cefPath);
            }

            var result3 = await Installer.Run(GetNupkgUrl("CefSharp.OffScreen", "75.1.141"), tempNuget, "OverlayPluginCef.tmp3");
            if (result3)
            {
                CopyFiles(Path.Combine(tempNuget, "CefSharp", "x64"), cefPath);
            }

            if (!result || !result2 || !result3 || !Directory.Exists(cefPath))
            {
                MessageBox.Show(
                    Resources.UpdateCefDlFailed,
                    Resources.ErrorTitle,
                    MessageBoxButtons.OK
                );
                
                return false;
            } else
            {
                File.WriteAllText(Path.Combine(cefPath, "version.txt"), CEF_VERSION);
                return true;
            }
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
