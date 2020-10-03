using DGP.Snap.Connect.Properties;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DGP.Snap.Connect.Services
{
    internal class NotifyIconManager : IDisposable
    {
        private MenuItem MenuItemSeparator => new MenuItem("-");

        public NotifyIcon NotifyIcon { get; }

        private readonly MenuItem itemAutorun =
            new MenuItem("开机启动",
            (sender, e) =>
            {
                if (AutoStartupService.IsAutorun())
                    AutoStartupService.SetAutoStartState(false);
                else
                    AutoStartupService.SetAutoStartState(true);
            });

        private readonly MenuItem itemSchoolnetConnectionState =
            new MenuItem("校园网:未连接", (sender, e) => { });

        private readonly MenuItem itemInternetConnectionState =
            new MenuItem("Internet:未连接", (sender, e) => { });

        private NotifyIconManager()
        {
            itemSchoolnetConnectionState.Enabled = false;
            itemInternetConnectionState.Enabled = false;
            NotifyIcon = new NotifyIcon
            {
                Text = "Snap Connector",
                Icon = Resources.Connector,
                Visible = true,
                ContextMenu = new ContextMenu(new[]
                {
                    new MenuItem("打开 Snap Connector", (sender, e) => WindowManager.GetOrAddWindow<MainWindow>().Show()),
                    itemSchoolnetConnectionState,
                    itemInternetConnectionState,
                    MenuItemSeparator,
                    new MenuItem("检查更新...",(sender, e) => Process.Start(@"https://github.com/CJLU-WebTeam/Snap.Connector/releases")),
                    itemAutorun,//自动启动
                    MenuItemSeparator,
                    new MenuItem("退出", (sender, e) => App.Current.Shutdown())
                })
            };

            NotifyIcon.Click +=
                (sender, e) =>
                {
                    if (((MouseEventArgs)e).Button == MouseButtons.Left)
                    {
                        System.Windows.Window mainWindow = WindowManager.GetOrAddWindow<MainWindow>();
                        if (mainWindow.IsVisible)
                            mainWindow.Close();
                        else
                            mainWindow.Show();
                    }
                };
            //设置check
            NotifyIcon.ContextMenu.Popup +=
                (sender, e) =>
                {
                    itemAutorun.Checked = AutoStartupService.IsAutorun();
                    itemSchoolnetConnectionState.Text = ConnectionService.IsLoggedIn ? "校园网:已连接" : "校园网:未连接";
                    itemInternetConnectionState.Text = ConnectionService.IsConnectedToInternet() ? "Internet:已连接" : "Internet:未连接";
                };

        }

        #region 单例
        private static NotifyIconManager instance;
        private static object _lock = new object();
        public static NotifyIconManager GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new NotifyIconManager();
                    }
                }
            }
            return instance;
        }
        #endregion

        /// <summary>
        /// 实现 <see cref="IDisposable"/> 接口
        /// </summary>
        public void Dispose()
        {
            //_itemAutorun.Dispose();
            NotifyIcon.Visible = false;
        }
    }
}
