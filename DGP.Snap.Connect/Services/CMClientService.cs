using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGP.Snap.Connect.Services
{
    class CMClientService
    {
        private static string GetCMClinetExecutableDirectory()
        {
            string clientKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\cmclient.exe";
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(clientKey))
            {
                return baseKey.GetValue("").ToString();
            }
        }

        public static void LaunchCMClient()
        {
            StartProcess();
        }

        private static void StartProcess()
        {
            Process.Start(GetCMClinetExecutableDirectory());
        }
    }
}
