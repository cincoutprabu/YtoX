//AudioRecordActivity.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YtoX.Entities;
using YtoX.Core.Helpers;

using NAudio;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;

namespace YtoX.Core.Activities
{
    public class AudioRecordActivity : IActivity
    {
        public static AudioRecordActivity Instance = new AudioRecordActivity();

        WaveIn _inStream;
        WaveFileWriter _writer;

        #region Methods

        public void Execute(Dictionary<string, object> arguments)
        {
            try
            {
                Start();

                //stop after specified duration
                long duration = long.Parse(arguments["Duration"].ToString());
                UIHelper.Run(() => { this.Stop(); }, duration);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        void inStream_RecordingStopped(object sender, EventArgs e)
        {
            Stop();
        }

        void inStream_DataAvailable(object sender, WaveInEventArgs e)
        {
            _writer.Write(e.Buffer, 0, e.BytesRecorded);
        }

        public void Start()
        {
            try
            {
                _inStream = new WaveIn();
                _inStream.DataAvailable += inStream_DataAvailable;
                _inStream.RecordingStopped += inStream_RecordingStopped;

                string fileName = string.Format("YtoX Recording [{0}].WAV", DateTime.Now.ToString(Constants.FILENAME_DATE_FORMAT));
                string audioPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), fileName);
                _writer = new WaveFileWriter(audioPath, _inStream.WaveFormat);

                _inStream.StartRecording();

                Log.Write("AudioRecord Started");
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
                if (_inStream != null)
                {
                    _inStream.StopRecording();
                    _inStream.Dispose();

                    Log.Write("AudioRecord Stopped");
                }

                if (_writer != null)
                {
                    _writer.Close();
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        #endregion
    }
}
