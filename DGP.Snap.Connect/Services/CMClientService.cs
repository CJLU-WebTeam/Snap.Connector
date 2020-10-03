using Microsoft.Win32;
using System.Diagnostics;

namespace DGP.Snap.Connect.Services
{
    internal class CMClientService
    {
        /// <summary>
        /// get cmclient path via registry system
        /// </summary>
        /// <returns></returns>
        private static string GetCMClinetExecutableDirectory()
        {
            string clientKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\cmclient.exe";
            //we need to read the 64bit registrykey
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(clientKey))
            {
                //get the (default) key
                return baseKey.GetValue("").ToString();
            }
        }
        /// <summary>
        /// prepare for adding mouse click operation
        /// </summary>
        public static void LaunchCMClient()
        {
            StartProcess();
        }
        /// <summary>
        /// simply launch the process
        /// we require the admin when we launch ourselves so we dont have to get a UAC permission require here
        /// </summary>
        private static void StartProcess()
        {
            Process.Start(GetCMClinetExecutableDirectory());
        }
    }
}
