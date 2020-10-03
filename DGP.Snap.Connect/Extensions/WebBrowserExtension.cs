using mshtml;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace DGP.Snap.Connect.Extensions
{
    public static class WebBrowserExtension
    {
        /// <summary>
        /// suppress script errors dialogs display
        /// </summary>
        /// <param name="webBrowser"></param>
        public static void SuppressScriptErrors(this WebBrowser webBrowser)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser != null)
            {
                object objComWebBrowser = fiComWebBrowser.GetValue(webBrowser);
                if (objComWebBrowser != null)
                    objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { true });
            }
        }
        /// <summary>
        /// inject scripts into html document
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="scripts"></param>
        public static void InjectScript(this WebBrowser browser,string scripts)
        {
            HTMLDocument htmlDoc = browser.Document as HTMLDocument;
            HTMLHeadElement head = htmlDoc.getElementsByTagName("head").Cast<HTMLHeadElement>().First();
            IHTMLScriptElement script = (IHTMLScriptElement)htmlDoc.createElement("script");
            script.text = scripts;
            head.appendChild((IHTMLDOMNode)script);
        }
    }
}
