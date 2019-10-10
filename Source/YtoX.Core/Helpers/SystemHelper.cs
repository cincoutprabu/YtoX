//SystemHelper.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;

using NAudio.CoreAudioApi;

using YtoX.Entities;

namespace YtoX.Core.Helpers
{
    public class SystemHelper
    {
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SetDllDirectory(string pathName);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        const int APPCOMMAND_VOLUME_UP = 0xA0000;
        const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        const int WM_APPCOMMAND = 0x319;

        static IntPtr MainHandle = Process.GetCurrentProcess().MainWindowHandle;

        #region Methods

        public static void SetNativeDllPath()
        {
            try
            {
                var folderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Native\" + (Environment.Is64BitProcess ? "x64" : "x86"));
                string dllPath = Path.Combine(folderPath, "System.Data.SQLite.dll");
                if (Directory.Exists(folderPath) && File.Exists(dllPath))
                {
                    //Assembly.Load(dllPath);
                    //SetDllDirectory(path);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void RegisterComDll()
        {
            try
            {
                var dllPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Native\Skype4COM.dll");
                //var dllPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Skype4COM.dll");

                ProcessStartInfo process = new ProcessStartInfo("regsvr32", "/s " + dllPath);
                process.UseShellExecute = false;
                process.CreateNoWindow = true;
                process.RedirectStandardOutput = true;
                Process.Start(process);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void SetLoadOnStartup()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (key.GetValue(Constants.APP_ID) == null)
            {
                string processFileName = Process.GetCurrentProcess().MainModule.FileName;
                if (!processFileName.ToLower().Contains("vshost") && !Debugger.IsAttached) //ignore if running from Visual Studio
                {
                    string appPath = "\"" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"YtoX.exe") + "\" /minimized /regrun";
                    key.SetValue(Constants.APP_ID, appPath);
                }
            }

            //to remove app from startup
            //key.DeleteValue(Constants.APP_ID, false);
        }

        public static TimeSpan GetSystemUpTime()
        {
            using (var counter = new PerformanceCounter("System", "System Up Time"))
            {
                counter.NextValue();
                TimeSpan uptime = TimeSpan.FromSeconds(counter.NextValue());
                return uptime;
            }
        }

        public static float GetMasterVolume()
        {
            MMDevice audioDevice = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            return audioDevice.AudioEndpointVolume.MasterVolumeLevelScalar;
        }

        public static bool IsMute()
        {
            MMDevice audioDevice = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            return audioDevice.AudioEndpointVolume.Mute;
        }

        public static void Mute()
        {
            SendMessageW(MainHandle, WM_APPCOMMAND, MainHandle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }

        public static void IncreaseVolume()
        {
            SendMessageW(MainHandle, WM_APPCOMMAND, MainHandle, (IntPtr)APPCOMMAND_VOLUME_UP);
        }

        public static void DecreaseVolume()
        {
            SendMessageW(MainHandle, WM_APPCOMMAND, MainHandle, (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        public static void SetBrightness(byte brightness)
        {
            try
            {
                ManagementScope scope = new ManagementScope("root\\WMI");
                SelectQuery query = new SelectQuery("WmiMonitorBrightnessMethods");
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    using (ManagementObjectCollection objectCollection = searcher.Get())
                    {
                        foreach (ManagementObject mObj in objectCollection)
                        {
                            mObj.InvokeMethod("WmiSetBrightness", new object[] { UInt32.MaxValue, brightness });
                            //mObj.InvokeMethod("WmiSetALSBrightnessState", new object[] { brightness });
                            break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        #endregion
    }
}
