//OpenUrlActivity.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace YtoX.Core.Activities
{
    public class OpenUrlActivity : IActivity
    {
        #region Methods

        public void Execute(Dictionary<string, object> arguments)
        {
            try
            {
                string url = (string)arguments["Url"];

                ProcessStartInfo process = new ProcessStartInfo(url);
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
