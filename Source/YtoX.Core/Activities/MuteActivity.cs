//MuteActivity.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YtoX.Core.Helpers;

namespace YtoX.Core.Activities
{
    public class MuteActivity : IActivity
    {
        #region Methods

        public void Execute(Dictionary<string, object> arguments)
        {
            SystemHelper.Mute();
            //SystemHelper.IncreaseVolume();
        }

        #endregion
    }
}
