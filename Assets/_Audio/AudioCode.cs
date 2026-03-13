using System;

namespace GOA.Audio{
    public class AudioCodeParser
    {
        public static AudioCode FromString(string name)
        {
            //name = name.ToLower();
            name = name.Substring(0,1).ToLower() + name.Substring(1);
            return (AudioCode)Enum.Parse(typeof(AudioCode), name);
        }
    }

    public enum AudioCode
    {
        unknowAudio = 0,
        baseMusic,
        mouseClick,
    }
}