using System;
using System.Runtime.InteropServices;

namespace DGP.Snap.Connect.Services
{
    internal class NativeMewthods
    {
        public delegate bool CallBack(IntPtr hwnd, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr FindWindow(string strClass, string strWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr EnumChildWindows(IntPtr hWndParent, CallBack lpEnumFunc, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowRect(IntPtr hwnd, RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SetCursorPos(int x, int y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        public const int MOUSEEVENTF_LEFTDOWN = 0x2;
        public const int MOUSEEVENTF_LEFTUP = 0x4;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hwnd, string lpString, int cch);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hwnd);

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static bool EnumWindowsProc(IntPtr h, int lParam)
        {

            string strText = new string((char)0, 255);
            RECT hRect = new RECT();

            int Ret = 0;

            Ret = GetWindowTextLength(h);
            string sSave = new string((char)9, Ret);
            GetWindowText(h, sSave, Ret + 1);
            if (sSave.IndexOf("Click Me", 0) > -1)
            {
                //GetWindowRect(h, hRect);
                //SetCursorPos((hRect.Left + hRect.Right) / 2, (hRect.Top + hRect.Bottom) / 2);
                SetCursorPos(200, 160);//这是用Spy++直接获取的，VS都带这个工具
                SetForegroundWindow(h);
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                return false;
            }
            return true;
        }
    }
}
