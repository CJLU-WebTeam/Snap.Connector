using DGP.Snap.Connect.Services;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace DGP.Snap.Connect
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Start(object sender, StartupEventArgs e)
        {
            //实现单实例
            EnsureSingleInstance();
            //托盘图标
            NotifyIconManager.GetInstance();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (!_isFirstInstance)
            {
                return;
            }
            _isRunning.ReleaseMutex();

            NotifyIconManager.GetInstance().Dispose();
            base.OnExit(e);
        }

        #region 单例
        private bool _isFirstInstance;
        private Mutex _isRunning;
        private void EnsureSingleInstance()
        {
        #if DEBUG
            if (Debugger.IsAttached)//调试模式
            {
                _isRunning = new Mutex(true, "DGP.Snap.Mutex.Debug.Debuging", out _isFirstInstance);
            }
            else
            {
                _isRunning = new Mutex(true, "DGP.Snap.Mutex.Debug", out _isFirstInstance);
            }

            if (!_isFirstInstance)
            {
                Shutdown();
                return;
            }
        #else
            _isRunning = new Mutex(true, "DGP.Snap.Mutex.Release", out _isFirstInstance);

            if (!_isFirstInstance)
            {
                Shutdown();
                return;
            }
        #endif
        }
        #endregion
    }
}
