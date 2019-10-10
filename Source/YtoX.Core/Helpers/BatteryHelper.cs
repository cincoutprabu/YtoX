//BatteryHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAPICodePack.ApplicationServices;
using Microsoft.WindowsAPICodePack.Net;

namespace YtoX.Core.Helpers
{
    public class BatteryHelper
    {
        public static int BatteryLifePercent = int.MaxValue;
        public static bool SystemInUse = false;

        #region Methods

        public static void StartTracking()
        {
            PowerManager.PowerSourceChanged += PowerManager_PowerSourceChanged;
            PowerManager.BatteryLifePercentChanged += PowerManager_BatteryLifePercentChanged;
            PowerManager.SystemBusyChanged += PowerManager_SystemBusyChanged;

            Log.Write("Battery Tracking Started");
        }

        public static void StopTracking()
        {
            PowerManager.PowerSourceChanged -= PowerManager_PowerSourceChanged;
            PowerManager.BatteryLifePercentChanged -= PowerManager_BatteryLifePercentChanged;
            PowerManager.SystemBusyChanged -= PowerManager_SystemBusyChanged;

            Log.Write("Battery Tracking Stopped");
        }

        public static void GetBatteryRemaining()
        {
            BatteryState batteryState = PowerManager.GetCurrentBatteryState();
            bool isInternet = NetworkListManager.IsConnectedToInternet;
        }

        static void PowerManager_PowerSourceChanged(object sender, EventArgs e)
        {
            Log.Write("PowerSource changed to: " + PowerManager.PowerSource.ToString());

            //start engine once power is connected
            if (PowerManager.PowerSource == PowerSource.AC && !Engine.Instance.IsRunning())
            {
                Engine.Instance.Start();
            }
            else if (PowerManager.PowerSource == PowerSource.Battery)
            {
                PowerManager_BatteryLifePercentChanged(null, null);
            }
        }

        static void PowerManager_BatteryLifePercentChanged(object sender, EventArgs e)
        {
            BatteryLifePercent = PowerManager.BatteryLifePercent;
            Log.Write("BatteryLifePercent changed to: " + BatteryLifePercent);

            BatteryState state = PowerManager.GetCurrentBatteryState();
            if (state.CurrentCharge <= state.SuggestedCriticalBatteryCharge)
            {
                if (Engine.Instance.IsRunning()) Engine.Instance.Stop();
            }
            else
            {
                if (!Engine.Instance.IsRunning()) Engine.Instance.Start();
            }
        }

        static void PowerManager_SystemBusyChanged(object sender, EventArgs e)
        {
            //Log.Write("SystemBusy Changed.");

            SystemInUse = true;
        }

        #endregion
    }
}
