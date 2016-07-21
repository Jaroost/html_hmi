using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace WCFLib
{
    public class ApplicationTools
    {
        public delegate void BeforeExitEventHandler(int exitCode);
        public static event BeforeExitEventHandler BeforeExit;

        private static void OnBeforeExit(int exitCode)
        {
            BeforeExit?.Invoke(exitCode);
        }

        public static void ExitApplication(int exitCode)
        {
            OnBeforeExit(exitCode);
            Environment.Exit(exitCode);
        }

        public static string CombineFromStartupPath(params string[] paths)
        {
            List<string> list = new List<string>(paths);
            list.Insert(0, ApplicationTools.StartupPath);
            return Path.Combine(list.ToArray());
        }

        public static string StartupPath
        {
            get
            {
                return Application.StartupPath;
            }
        }

        public static string ExecutablePath
        {
            get
            {
                return Application.ExecutablePath;
            }
        }

        public static string ApplicationName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(ExecutablePath);
            }
        }
    }
}
