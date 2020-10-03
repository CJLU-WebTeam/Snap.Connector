using System;
using System.Windows.Forms;

namespace DGP.Snap.Connect.Services
{
    public class NotificationManager
    {
        /// <summary>
        /// 显示Windows系统通知
        /// </summary>
        /// <param name="title">显示的标题，一般为 Snap Desktop</param>
        /// <param name="content">显示的内容</param>
        /// <param name="clickEvent">点击通知触发的<see cref="Action"/></param>
        /// <param name="closeEvent">通知消失时触发的<see cref="Action"/></param>
        /// <param name="timedout">通知显示的时间，以毫秒为单位</param>
        public static void ShowNotification(string title, string content, Action clickEvent = null, Action closeEvent = null, int timedout = 2000)
        {
            NotifyIcon icon = NotifyIconManager.GetInstance().NotifyIcon;
            icon.ShowBalloonTip(timedout, title, content, ToolTipIcon.None);
            icon.BalloonTipClicked += OnIconOnBalloonTipClicked;
            icon.BalloonTipClosed += OnIconOnBalloonTipClosed;

            void OnIconOnBalloonTipClicked(object sender, EventArgs e)
            {
                clickEvent?.Invoke();
                icon.BalloonTipClicked -= OnIconOnBalloonTipClicked;
            }

            void OnIconOnBalloonTipClosed(object sender, EventArgs e)
            {
                closeEvent?.Invoke();
                icon.BalloonTipClosed -= OnIconOnBalloonTipClosed;
            }
        }

    }
}
