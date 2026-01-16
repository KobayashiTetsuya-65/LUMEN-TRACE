using UnityEngine;

public static class SoundDataUtility
{
    public static class KeyConfig
    {
        public static class Se
        {

        }
        public static class Bgm
        {

        }
    }

    public enum SoundType
    {
        Bgm = 0,
        Se = 1
    }

    public static void PrepareAudioSource(this AudioSource source,SoundData soundData)
    {
        source.playOnAwake = soundData.PlayOnAwake;
        source.loop = soundData.IsLoop;
        source.clip = soundData.Clip;
        source.volume = soundData.Volume;
    }
}
