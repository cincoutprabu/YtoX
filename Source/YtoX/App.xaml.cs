//App.xaml.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;
using System.Reflection;

using YtoX.Entities;
using YtoX.Core;
using YtoX.Core.Helpers;
using YtoX.Core.Shell;
using YtoX.Core.Storage;

namespace YtoX
{
    public partial class App : Application
    {
        #region App Events

        protected override void OnStartup(StartupEventArgs e)
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            if (IsAlreadyRunning())
            {
                MessageBox.Show("YtoX is already running.");
                Application.Current.Shutdown();
                return;
            }

            ShellHelper.InstallShortcut();
            SystemHelper.SetNativeDllPath();
            SystemHelper.RegisterComDll();
            SystemHelper.SetLoadOnStartup();
            TaskBarHelper.Setup();

            if (!DAC.SetupDB())
            {
                MessageBox.Show("Database not found.", Constants.APP_ID, MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return;
            }

            Repository.Read();
            SettingsHelper.Load();
            Engine.Instance.Start();
            TaskBarHelper.ShowBalloonTip("YtoX Engine Started!\nPlease make sure you have configured the settings.");

            base.OnStartup(e);
        }

        private bool IsAlreadyRunning()
        {
            Process thisProc = Process.GetCurrentProcess();
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Events

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
            {
                Log.Error((Exception)e.ExceptionObject);
            }
            else if (e.ExceptionObject != null)
            {
                Log.Write("CurrentDomain_UnhandledException: " + e.ExceptionObject.ToString());
            }
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.ToLower().Contains("ytox.resources")) return null;
            Log.Write("AssemblyResolve failed for: " + args.Name);

            //get dll-folder path
            string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string folderPath = Path.Combine(basePath, @"Native\" + (Environment.Is64BitProcess ? "x64" : "x86"));

            //return new assembly
            string assemblyPath = Path.Combine(folderPath, new AssemblyName(args.Name).Name + ".dll");
            if (File.Exists(assemblyPath))
            {
                Log.Write("New assembly-path: " + assemblyPath);

                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                return assembly;
            }
            return null;
        }

        #endregion
    }
}
