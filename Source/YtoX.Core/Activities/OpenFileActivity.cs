//OpenFileActivity.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Windows.Foundation;
using Windows.System;
using Windows.ApplicationModel;
using Windows.Storage;

namespace YtoX.Core.Activities
{
    public class OpenFileActivity : IActivity
    {
        #region Methods

        public void Execute(Dictionary<string, object> arguments)
        {
            try
            {
                var filePath = (string)arguments["FilePath"];

                ProcessStartInfo process = new ProcessStartInfo(filePath);
                process.UseShellExecute = true;
                Process.Start(process);

                //Launch(filePath);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        private void Launch(string filePath)
        {
            Package.Current.InstalledLocation.GetFileAsync(filePath).Completed += delegate(IAsyncOperation<StorageFile> result, AsyncStatus status)
            {
                var file = result.GetResults();

                LauncherOptions options = new LauncherOptions();
                options.DisplayApplicationPicker = false;
                Launcher.LaunchFileAsync(file, options).Completed += delegate(IAsyncOperation<bool> result1, AsyncStatus status1)
                {
                    var success = result1.GetResults();
                    if (!success)
                    {
                    }
                };
            };
        }

        #endregion
    }
}
