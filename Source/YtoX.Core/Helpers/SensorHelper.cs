//SensorHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.Devices.Sensors;

using YtoX.Entities;

namespace YtoX.Core.Helpers
{
    public class SensorHelper
    {
        private static Accelerometer accelerometer = null;
        private static SimpleOrientationSensor orientationSensor;
        //private static LightSensor lightSensor = null;

        #region Methods

        public static void StartTracking()
        {
            try
            {
                if (accelerometer == null) accelerometer = Accelerometer.GetDefault();
                if (accelerometer != null)
                {
                    //accelerometer.ReadingChanged += accelerometer_ReadingChanged;
                    accelerometer.Shaken += accelerometer_Shaken;
                }

                if (orientationSensor == null) orientationSensor = SimpleOrientationSensor.GetDefault();
                if (orientationSensor != null)
                {
                    orientationSensor.OrientationChanged += orientationSensor_OrientationChanged;
                }

                //if (lightSensor == null) lightSensor = LightSensor.GetDefault();
                //if (lightSensor != null)
                //{
                //    lightSensor.ReadingChanged += lightSensor_ReadingChanged;
                //}

                Log.Write("Sensors Tracking Started");
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void StopTracking()
        {
            try
            {
                if (accelerometer != null)
                {
                    //accelerometer.ReadingChanged -= accelerometer_ReadingChanged;
                    accelerometer.Shaken -= accelerometer_Shaken;
                }

                if (orientationSensor != null)
                {
                    orientationSensor.OrientationChanged -= orientationSensor_OrientationChanged;
                }

                //if (lightSensor != null)
                //{
                //    lightSensor.ReadingChanged -= lightSensor_ReadingChanged;
                //}

                Log.Write("Sensors Tracking Stopped");
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        #endregion

        #region Events

        static void accelerometer_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            //var r = args.Reading;
            //Log.Write("Accelerometer Reading: " + r.AccelerationX + "," + r.AccelerationY + "," + r.AccelerationZ);
        }

        static void accelerometer_Shaken(Accelerometer sender, AccelerometerShakenEventArgs args)
        {
            Log.Write("Device Shaken");

            Execution.ExecuteFavoriteAppRecipe();
        }

        static void lightSensor_ReadingChanged(LightSensor sender, LightSensorReadingChangedEventArgs args)
        {
            //Log.Write("LightSensor Reading: " + args.Reading.IlluminanceInLux);
        }

        static void orientationSensor_OrientationChanged(SimpleOrientationSensor sender, SimpleOrientationSensorOrientationChangedEventArgs args)
        {
            Log.Write("Orientation Changed: " + args.Orientation);

            switch (args.Orientation)
            {
                //case SimpleOrientation.NotRotated:
                //case SimpleOrientation.Rotated180DegreesCounterclockwise:
                case SimpleOrientation.Rotated270DegreesCounterclockwise:
                case SimpleOrientation.Rotated90DegreesCounterclockwise:
                    {
                        Execution.ExecuteLandscapeRecipe();
                    }
                    break;
            }
        }

        #endregion
    }
}
