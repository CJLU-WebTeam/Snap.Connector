using DGP.Snap.Connect.Extensions;
using mshtml;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DGP.Snap.Connect.Services
{
    internal static class ConnectionService
    {
        /// <summary>
        /// triggered after the connect operation finished
        /// </summary>
        public static Action<ConnectionState> Connected;

        static ConnectionService()
        {
            Browser.LoadCompleted += HandleNavigation;
        }

        public static WebBrowser Browser;
        public static bool IsLoggedIn { get; private set; }

        private static string _account;
        private static string _password;
        /// <summary>
        /// attmpt to login with your ticket
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public static void Connect(string account, string password)
        {
            _account = account;
            _password = password;
            Browser.LoadCompleted += HandleConnect;
            if(Browser.Source != new Uri(@"http://portal2.cjlu.edu.cn/a70.htm"))
                Browser.Source = new Uri(@"http://portal2.cjlu.edu.cn/a70.htm");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void HandleNavigation(object sender, NavigationEventArgs e)
        {
            //sometimes work...
            if (Browser.Source.ToString().Contains("2.htm"))
                IsLoggedIn = false;
            //tried to connect the school net but failed
            if (!IsConnectedToInternet())
                IsLoggedIn = false;
            if (Browser.Source.ToString().Contains("a70.htm")&&IsLoggedIn==false)
                IsLoggedIn = false;
        }

        /// <summary>
        /// do not use it
        /// </summary>
        public static void DisConnect()
        {
            InjectScript("window.confirm = function(msg) { return true; }");
            HTMLDocument htmlDoc = Browser.Document as HTMLDocument;
            htmlDoc.getElementById("logout").click();
            //TO DO
        }

        private static void HandleConnect(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("HandleConnect called");
            Thread.Sleep(1000);
            if (!IsLoggedIn)
            {
                if (Browser.Source == new Uri(@"https://portal2.cjlu.edu.cn/3.htm"))
                {
                    IsLoggedIn = true;
                    return;
                }
                try
                {
                    //make no confirm dialog
                    Browser.InjectScript("window.confirm = function(msg) { return true; }");

                    HTMLDocument htmlDoc = Browser.Document as HTMLDocument;
                    htmlDoc.getElementById("VipDefaultAccount").setAttribute("value", _account);//账号
                    htmlDoc.getElementById("VipDefaultPassword").setAttribute("value", _password);//密码
                    htmlDoc.getElementsByName("union").Cast<IHTMLElement>().First().click();//联网
                    htmlDoc.getElementById("login").click();//登陆
                    Connected?.Invoke(ConnectionState.Connected);
                    Debug.WriteLine(Browser.Source);
                }
                catch (NullReferenceException)
                {
                    //already logged in,but we don't know
                    //mannual logout should do work
                    Connected?.Invoke(ConnectionState.RequireManualLogout);
                }
                finally
                {
                    Browser.LoadCompleted -= HandleConnect;
                }
            }
        }

        public static bool IsConnectedToInternet()
        {
            PingReply reply;
            Ping ping = new Ping();
            try
            {
                reply = ping.Send("www.baidu.com");
                return reply.Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsConnectedToSchoolnet()
        {
            PingReply reply;
            Ping ping = new Ping();
            try
            {
                reply = ping.Send("www.cjlu.edu.cn");
                return reply.Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
