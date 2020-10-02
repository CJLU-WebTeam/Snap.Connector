using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DGP.Snap.Connect.Services
{
    internal partial class Setting
    {
        public const string AutoLogin = "AutoLogin";
        public const string RememberMe = "RememberMe";
        public const string Account = "Account";
        public const string Password = "Password";
        public const string AutoCMClient = "AutoCMClient";
        public const string AutoMinimize = "AutoMinimize";

    }
    internal partial class Setting
    {
        public bool Get(string key)
        {
            using (RegistryKey setting = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE", true).CreateSubKey("Snap Connector", true))
            {
                Debug.WriteLine(setting.GetValue(key));

                return bool.TryParse((string)setting.GetValue(key), out bool result) ? result : false;
            }
        }

        public object this[string key]
        {
            get
            {
                using (RegistryKey setting = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,RegistryView.Registry64).OpenSubKey("SOFTWARE",true).CreateSubKey("Snap Connector",true))
                {
                    Debug.WriteLine(setting.GetValue(key));
                    return setting.GetValue(key);
                }
            }
            set
            {
                using (RegistryKey setting = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE",true).CreateSubKey("Snap Connector",true))
                {
                    setting.SetValue(key, value);
                }
            }
        }
    }
}
