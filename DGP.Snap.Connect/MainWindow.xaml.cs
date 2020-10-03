using DGP.Snap.Connect.Extensions;
using DGP.Snap.Connect.Services;
using System.Diagnostics;
using System.Windows;

namespace DGP.Snap.Connect
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Host.Navigated += (s, e) =>
            {
                Debug.WriteLine(Host.Source);
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Host.SuppressScriptErrors();
            //sync the state of checkbox and text
            Setting settingCheck = new Setting();
            RememberMeCheckBox.IsChecked = settingCheck.Get(Setting.RememberMe);

            if ((bool)RememberMeCheckBox.IsChecked)
            {
                Setting settingText = new Setting();
                Account.Text = (string)settingText[Setting.Account];
                Password.Password = (string)settingText[Setting.Password];
            }

            AutoStartupCheckBox.IsChecked = AutoStartupService.IsAutorun();

            AutoLoginCheckBox.IsChecked = settingCheck.Get(Setting.AutoLogin);
            AutoClientCheckBox.IsChecked = settingCheck.Get(Setting.AutoCMClient);
            AutoMinimizeCheckBox.IsChecked = settingCheck.Get(Setting.AutoMinimize);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NotificationManager.ShowNotification("Snap Connector", "正在连接至校园网...");
            ConnectionService.Browser = Host;

            ConnectionService.Connected += OnAfterConnect;
            ConnectionService.Connect("1900303205", "Login087452");
            Setting setting = new Setting();
            setting[Setting.Account] = Account.Text;
            setting[Setting.Password] = Password.Password;
        }

        private void OnAfterConnect(ConnectionState state)
        {
            if (state == ConnectionState.Connected)
            {
                if ((bool)AutoClientCheckBox.IsChecked)
                {
                    NotificationManager.ShowNotification("Snap Connector", "正在启动随e行...");
                    CMClientService.LaunchCMClient();
                }

                if ((bool)AutoMinimizeCheckBox.IsChecked)
                    this.Hide();
            }
            ConnectionService.Connected -= OnAfterConnect;
        }

        private void AutoLoginCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)AutoLoginCheckBox.IsChecked)
            {
                RememberMeCheckBox.IsChecked = true;
            }
            Setting setting = new Setting();
            setting[Setting.AutoLogin] = AutoLoginCheckBox.IsChecked;
            setting[Setting.RememberMe] = RememberMeCheckBox.IsChecked;
        }

        private void RememberMeCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)RememberMeCheckBox.IsChecked)
            {
                AutoLoginCheckBox.IsChecked = false;
            }
            Setting setting = new Setting();
            setting[Setting.AutoLogin] = AutoLoginCheckBox.IsChecked;
            setting[Setting.RememberMe] = RememberMeCheckBox.IsChecked;
        }

        private void AutoStartupCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)AutoStartupCheckBox.IsChecked)
                AutoStartupService.SetAutoStartState(true);
            else
                AutoStartupService.SetAutoStartState(false);
        }

        private void OnInput(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Account.Text) || string.IsNullOrEmpty(Password.Password))
            {
                LoginButton.IsEnabled = false;
                return;
            }

            LoginButton.IsEnabled = true;
        }

        private void AutoMinimizeCheckBox_Click(object sender, RoutedEventArgs e)
        {
            new Setting()[Setting.AutoMinimize] = AutoMinimizeCheckBox.IsChecked;
        }

        private void AutoClientCheckBox_Click(object sender, RoutedEventArgs e)
        {
            new Setting()[Setting.AutoCMClient] = AutoClientCheckBox.IsChecked;
        }
    }
}
