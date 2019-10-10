//OpenAppActivity.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace YtoX.Core.Activities
{
    public class OpenAppActivity : IActivity
    {
        #region Methods

        public void Execute(Dictionary<string, object> arguments)
        {
            try
            {
                var appName = (string)arguments["AppName"];

                ProcessStartInfo process = new ProcessStartInfo(appName);
                process.UseShellExecute = true;
                Process.Start(process);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        #endregion
    }
}
