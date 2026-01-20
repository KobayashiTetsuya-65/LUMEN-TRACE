using UnityEngine;

public static class SoundDataUtility
{
    public static class KeyConfig
    {
        public static class Se
        {
            public static readonly string PlayerAttack = "PlayerAttack";
            public static readonly string EnemyAttack = "EnemyAttack";
            public static readonly string AttackHit = "AttackHit";
            public static readonly string Damage = "Damage";
            public static readonly string Dodge = "Dodge";
            public static readonly string Hide = "Hide";
        }
        public static class Bgm
        {
            public static readonly string Title = "Title";
            public static readonly string InGame = "InGame";
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
