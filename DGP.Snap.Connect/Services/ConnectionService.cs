using mshtml;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DGP.Snap.Connect.Services
{
    static class ConnectionService
    {
        public static Action<ConnectionState> Connected;

        static ConnectionService()
        {
            Browser.LoadCompleted += HandleNavigation;
        }

        public static WebBrowser Browser=new WebBrowser();
        public static bool IsLoggedIn { get; private set; }

        private static string _account;
        private static string _password;

        public static void Connect(string account,string password)
        {
            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(account);
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(password);
            _account = account;
            _password = password;
            Browser.LoadCompleted += HandleConnect;
            Browser.Source = new Uri(@"http://portal2.cjlu.edu.cn/a70.htm");
        }

        private static void HandleNavigation(object sender, NavigationEventArgs e)
        {
            //TODO detect the connection state to mark the islogin to false

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
            if (!IsLoggedIn)
            {
                if(Browser.Source == new Uri(@"https://portal2.cjlu.edu.cn/3.htm"))
                {
                    IsLoggedIn = true;
                    return;
                }
                try
                {
                    //make no confirm dialog
                    InjectScript("window.confirm = function(msg) { return true; }");

                    HTMLDocument htmlDoc = Browser.Document as HTMLDocument;
                    htmlDoc.getElementById("VipDefaultAccount").setAttribute("value", _account);
                    htmlDoc.getElementById("VipDefaultPassword").setAttribute("value", _password);
                    htmlDoc.getElementsByName("union").Cast<IHTMLElement>().First().click();
                    htmlDoc.getElementById("login").click();
                    IsLoggedIn = true;
                    Connected?.Invoke(ConnectionState.Succeeded);
                    Debug.WriteLine(Browser.Source);
                }
                catch(NullReferenceException)
                {
                    //already logged in,but we don't know
                    //mannual logout should do work
                    Connected?.Invoke(ConnectionState.UnAuthed);
                }
                finally
                {
                    Browser.LoadCompleted -= HandleConnect;
                }
            }
        }

        private static void InjectScript(string scripts)
        {
            HTMLDocument htmlDoc = Browser.Document as HTMLDocument;
            HTMLHeadElement head = htmlDoc.getElementsByTagName("head").Cast<HTMLHeadElement>().First();
            IHTMLScriptElement script = (IHTMLScriptElement)htmlDoc.createElement("script");
            script.text = scripts;
            head.appendChild((IHTMLDOMNode)script);
        }
    }
}
