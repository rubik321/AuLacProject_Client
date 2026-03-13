using UnityEngine;

namespace Rubik.Common.AudioHelper
{
    public class AudioRecorder : MonoBehaviour {
        public AudioClip audioClip;
        public bool isUseMicrophone = true;
        public string selectedDevice;

        static public AudioRecorder Instance;
        private string fileName;
        private int timeLimited;

        private void Awake()
        {
            Instance = this;
            if (isUseMicrophone)
            {
                if (Microphone.devices.Length > 0)
                {
                    selectedDevice = Microphone.devices[0];
                }
                else
                {
                    isUseMicrophone = false;
                }
            }
        }

        void Start () 
        {
        
        }

        public void SetTimeLimited(int time)
        {
            if (time > 0)
            {
                timeLimited = time;
            }
            else
            {
                timeLimited = 10;
            }
        }

        public void StartRecord(string filename)
        {
            if (isUseMicrophone)
            {
                fileName = filename;
                audioClip = Microphone.Start(selectedDevice, true, timeLimited, 44100);
            }
        }

        public void StopRecord(float duration)
        {
            if (IsRecording())
            {
                Microphone.End(selectedDevice);
                SavWav.Save(fileName, TrimClip(audioClip, duration));
            }
        }

        public bool IsRecording()
        {
            return isUseMicrophone && Microphone.IsRecording(selectedDevice);
        }

        private AudioClip TrimClip(AudioClip recordedClip, float duration)
        {
            try
            {
                if (duration > 0 && duration < recordedClip.length)
                {
                    var soundData = new float[recordedClip.samples * recordedClip.channels];
                    recordedClip.GetData(soundData, 0);

                    //Copy datas trimed
                    int sampleCount = (int)(duration * recordedClip.frequency * recordedClip.channels);
                    var newData = new float[sampleCount];
                    for (int i = 0; i < newData.Length; i++)
                    {
                        newData[i] = soundData[i];
                    }

                    //Create new clip
                    var clip = AudioClip.Create(recordedClip.name, sampleCount, recordedClip.channels, recordedClip.frequency, false, false);
                    clip.SetData(newData, 0);
                    return clip;
                }
            }
            catch
            {
                Debug.Log("Error to try trim audio clip");
            }
            return recordedClip;
        }

    }
}
