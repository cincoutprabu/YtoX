//LaunchHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YtoX.Entities;
using YtoX.Core.Activities;

namespace YtoX.Core.Helpers
{
    public class LaunchHelper
    {
        #region Methods

        public static void LaunchApp(string appName)
        {
            try
            {
                new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", Constants.AppUrls[appName] } });
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        #endregion
    }
}
