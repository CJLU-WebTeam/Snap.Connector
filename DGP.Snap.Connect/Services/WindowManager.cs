using System.Windows;

namespace DGP.Snap.Connect.Services
{
    public class WindowManager
    {
        /// <summary>
        /// 查找 <see cref="Application.Current.Windows"/> 集合中的对应 <typeparamref name="TWindow"/> 类型的 Window
        /// </summary>
        /// <returns>返回唯一的窗口，未找到返回新实例</returns>
        public static Window GetOrAddWindow<TWindow>() where TWindow : Window
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window.GetType() == typeof(TWindow))
                {
                    return window;
                }
            }
            return (Window)typeof(TWindow).Assembly.CreateInstance(typeof(TWindow).FullName);
            //return Singleton<TWindow>.Instance as System.Windows.Window;
        }
    }
}
