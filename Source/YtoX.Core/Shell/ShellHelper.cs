//ShellHelper.cs

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MS.WindowsAPICodePack.Internal;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

using YtoX.Entities;

namespace YtoX.Core.Shell
{
    public class ShellHelper
    {
        //In order to display toasts, a desktop application must have a shortcut on the Start menu.
        //The shortcut should be created as part of the installer.
        //Note: AppUserModelID must be set on that shortcut.
        public static void InstallShortcut()
        {
            try
            {
                string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Microsoft\Windows\Start Menu\Programs\YtoX.lnk";
                if (!File.Exists(shortcutPath))
                {
                    //get current executable path
                    string exePath = Process.GetCurrentProcess().MainModule.FileName;
                    IShellLinkW newShortcut = (IShellLinkW)new CShellLink();

                    Log.Write("Shortcut to EXE: " + exePath);

                    //Create a shortcut to the exe
                    ErrorHelper.VerifySucceeded(newShortcut.SetPath(exePath));
                    ErrorHelper.VerifySucceeded(newShortcut.SetArguments(""));

                    //Open the shortcut property store, set the AppUserModelId property
                    IPropertyStore newShortcutProperties = (IPropertyStore)newShortcut;
                    using (PropVariant appIdProp = new PropVariant(Constants.APP_ID))
                    {
                        ErrorHelper.VerifySucceeded(newShortcutProperties.SetValue(SystemProperties.System.AppUserModel.ID, appIdProp));
                        ErrorHelper.VerifySucceeded(newShortcutProperties.Commit());
                    }

                    Log.Write("Shortcut filepath: " + shortcutPath);

                    //Save the shortcut to disk
                    IPersistFile newShortcutSave = (IPersistFile)newShortcut;
                    ErrorHelper.VerifySucceeded(newShortcutSave.Save(shortcutPath, true));
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }
    }
}
