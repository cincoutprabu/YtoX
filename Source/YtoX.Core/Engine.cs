//Engine.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

using YtoX.Entities;
using YtoX.Core;
using YtoX.Core.Helpers;
using YtoX.Core.Shell;
using YtoX.Core.Location;
using YtoX.Core.Weather;

namespace YtoX.Core
{
    public class Engine
    {
        public static Engine Instance = new Engine();

        #region Fields

        private System.Timers.Timer executionTimer;
        private bool executionInProgress = false;
        private CancellationTokenSource cancelTokenSource = null;

        #endregion

        #region Constructors

        private Engine()
        {
        }

        #endregion

        #region Methods

        public void Start()
        {
            try
            {
                StartListeners();
                StartTimer();
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public void Stop()
        {
            try
            {
                StopListeners();
                StopTimer();
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public bool IsRunning()
        {
            return executionTimer == null ? false : executionTimer.Enabled;
        }

        #endregion

        #region Internal-Methods

        private void StartListeners()
        {
            try
            {
                LocationHelper.StartTracking();
                SensorHelper.StartTracking();
                BatteryHelper.StartTracking();
                GTalkHelper.StartTracking();
                SkypeHelper.StartTracking();
                OutlookHelper.StartTracking();
                DropboxHelper.StartTracking();
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        private void StopListeners()
        {
            try
            {
                LocationHelper.StopTracking();
                SensorHelper.StopTracking();
                BatteryHelper.StopTracking();
                GTalkHelper.StopTracking();
                SkypeHelper.StopTracking();
                OutlookHelper.StopTracking();
                DropboxHelper.StopTracking();
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public void StartTimer()
        {
            try
            {
                //validate inputs
                if (string.IsNullOrEmpty(Constants.NEWS_EMAIL))
                {
                    Log.Write("News EmailId is missing.");
                }

                //setup timer
                executionTimer = new System.Timers.Timer(Constants.ENGINE_INTERVAL * 1000);
                executionTimer.Elapsed += new ElapsedEventHandler(executionTimer_Elapsed);
                executionTimer.AutoReset = true;
                executionTimer.Start();

                Log.Write("Engine Timer Started");
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        private void StopTimer()
        {
            try
            {
                if (executionTimer != null)
                {
                    executionTimer.Stop();
                    executionInProgress = false;

                    Log.Write("Engine Timer Stopped");
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        async void executionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (cancelTokenSource != null)
            {
                cancelTokenSource.Cancel();
            }

            try
            {
                Log.Write("Engine Timer Elapsed");

                if (executionInProgress)
                {
                    Log.Write("Ignoring engine-execution, as previous execution in progress.");
                    return;
                }

                executionInProgress = true;
                cancelTokenSource = new CancellationTokenSource();

                //Geolocator locator = new Geolocator();
                //await locator.GetGeopositionAsync().AsTask(cancelTokenSource.Token);

                await Execution.Run();
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
            finally
            {
                executionInProgress = false;
            }
        }

        #endregion
    }
}
