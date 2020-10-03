using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DGP.Snap.Connect.Services
{
    /// <summary>
    /// Directly copied from DGP.Snap Framework
    /// 100% stable
    /// </summary>
    internal class AutoStartupService
    {
        /// <summary>
        /// 快捷方式名称
        /// </summary>
        private const string ShortcutName = "Snap Connector";

        /// <summary>
        /// 程序完整路径
        /// </summary>
        private static string AppFullPath { get { return Process.GetCurrentProcess().MainModule.FileName; } }

        /// <summary>
        /// 设置开机自动启动
        /// </summary>
        /// <param name="flag">自启开关</param>
        public static void SetAutoStartState(bool flag = true)
        {
            List<string> shortcutPaths = GetAppShortcutFromFolder(Environment.GetFolderPath(Environment.SpecialFolder.Startup), AppFullPath);

            if (flag)//开机启动
            {
                //获取启动路径应用程序快捷方式的路径集合

                //存在2个以快捷方式则保留一个快捷方式-避免重复多余
                if (shortcutPaths.Count >= 2)
                {
                    for (int i = 1; i < shortcutPaths.Count; i++)
                    {
                        DeleteFile(shortcutPaths[i]);
                    }
                }
                else if (shortcutPaths.Count < 1)//不存在则创建快捷方式
                {
                    CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Startup), ShortcutName, AppFullPath, "Snap Desktop");
                }
                else
                {

                }
            }
            else//IsOff//开机不启动
            {
                //存在快捷方式则遍历全部删除
                if (shortcutPaths.Count > 0)
                {
                    for (int i = 0; i < shortcutPaths.Count; i++)
                    {
                        DeleteFile(shortcutPaths[i]);
                    }
                }
            }
        }

        public static bool IsAutorun()
        {
            List<string> shortcutPaths = GetAppShortcutFromFolder(Environment.GetFolderPath(Environment.SpecialFolder.Startup), AppFullPath);

            if (shortcutPaths.Count >= 1)//存在快捷方式，会开机自启
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///  向目标路径创建指定文件的快捷方式
        /// </summary>
        /// <param name="directory">目标目录</param>
        /// <param name="shortcutName">快捷方式名字</param>
        /// <param name="targetPath">文件完全路径</param>
        /// <param name="description">描述</param>
        /// <param name="iconLocation">图标地址</param>
        /// <returns>成功或失败</returns>
        private static bool CreateShortcut(string directory, string shortcutName, string targetPath, string description = null, string iconLocation = null)
        {
            try
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);                                                       //目录不存在则创建
                //引用 Com Windows Script Host Object Model
                string shortcutPath = Path.Combine(directory, $"{shortcutName}.lnk");                           //合成路径
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);                       //创建快捷方式对象
                shortcut.TargetPath = targetPath;                                                               //指定目标路径
                shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);                                  //设置起始位置
                shortcut.WindowStyle = 1;                                                                       //设置运行方式，默认为常规窗口
                shortcut.Description = description;                                                             //设置备注
                shortcut.IconLocation = string.IsNullOrWhiteSpace(iconLocation) ? targetPath : iconLocation;    //设置图标路径
                shortcut.Save();                                                                                //保存快捷方式
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 获取指定文件夹下指定应用程序的快捷方式路径集合
        /// </summary>
        /// <param name="directory">文件夹</param>
        /// <param name="targetPath">目标应用程序路径</param>
        /// <returns>目标应用程序的快捷方式</returns>
        private static List<string> GetAppShortcutFromFolder(string directory, string targetPath)
        {
            List<string> tempStrs = new List<string>();
            tempStrs.Clear();
            string tempStr = null;
            string[] files = Directory.GetFiles(directory, "*.lnk");
            if (files == null || files.Length < 1)
            {
                return tempStrs;
            }
            for (int i = 0; i < files.Length; i++)
            {
                //files[i] = string.Format("{0}\\{1}", directory, files[i]);
                tempStr = GetAppPathFromShortcut(files[i]);
                if (tempStr == targetPath)
                {
                    tempStrs.Add(files[i]);
                }
            }
            return tempStrs;
        }

        /// <summary>
        /// 获取快捷方式的目标文件路径-用于判断是否已经开启了自动启动
        /// </summary>
        /// <param name="shortcutPath"></param>
        /// <returns></returns>
        private static string GetAppPathFromShortcut(string shortcutPath)
        {
            //快捷方式文件的路径 = @"d:\Test.lnk";
            if (System.IO.File.Exists(shortcutPath))
            {
                WshShell shell = new WshShell();
                IWshShortcut shortct = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                //快捷方式文件指向的路径.Text = 当前快捷方式文件IWshShortcut类.TargetPath;
                //快捷方式文件指向的目标目录.Text = 当前快捷方式文件IWshShortcut类.WorkingDirectory;
                return shortct.TargetPath;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 根据路径删除文件(夹)
        /// </summary>
        /// <param name="path">路径</param>
        private static void DeleteFile(string path)
        {
            FileAttributes attr = System.IO.File.GetAttributes(path);
            if (attr == FileAttributes.Directory)
            {
                Directory.Delete(path, true);
            }
            else
            {
                System.IO.File.Delete(path);
            }
        }

    }
}
