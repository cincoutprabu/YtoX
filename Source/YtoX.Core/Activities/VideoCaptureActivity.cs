//VideoCaptureActivity.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;

using YtoX.Entities;
using YtoX.Core.Helpers;

using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
//using AForge.Video.FFMPEG;

namespace YtoX.Core.Activities
{
    public class VideoCaptureActivity
    {
        public static VideoCaptureActivity Instance = new VideoCaptureActivity();

        bool _photoOnly;
        VideoCaptureDevice _source;
        //VideoFileWriter _writer;

        #region Methods

        public void Execute(Dictionary<string, object> arguments)
        {
            try
            {
                var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (devices.Count == 0)
                {
                    Log.Write("No Camera Detected");
                    return;
                }

                _source = new VideoCaptureDevice(devices[0].MonikerString);
                _source.NewFrame += source_NewFrame;
                _source.Start();
                Log.Write("VideoCapture Started");

                _photoOnly = (bool)arguments["PhotoOnly"];
                if (!_photoOnly)
                {
                    string fileName = string.Format("YtoX Video [{0}].MP4", DateTime.Now.ToString(Constants.FILENAME_DATE_FORMAT));
                    string videoPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), fileName);

                    //_writer = new VideoFileWriter();
                    //_writer.Open(videoPath, 320, 240, 25, VideoCodec.MPEG4);
                }

                //stop after specified duration
                long duration = long.Parse(arguments["Duration"].ToString());
                UIHelper.Run(() => { this.Stop(); }, duration);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        void source_NewFrame(object sender, NewFrameEventArgs e)
        {
            Bitmap frame = e.Frame;

            if (_photoOnly)
            {
                string fileName = string.Format("YtoX Photo [{0}].JPG", DateTime.Now.ToString(Constants.FILENAME_DATE_FORMAT));
                string photoPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), fileName);
                frame.Save(photoPath);
            }
            else
            {
                //_writer.WriteVideoFrame(frame);
            }
        }

        public void Stop()
        {
            try
            {
                _source.SignalToStop();
                if (!_photoOnly)
                {
                    //_writer.Close();
                }

                Log.Write("VideoCapture Ended");
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}
