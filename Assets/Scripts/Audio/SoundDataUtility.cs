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
            public static readonly string PlayerDamage = "PlayerDamage";
            public static readonly string Dodge = "Dodge";
            public static readonly string Hide = "Hide";
            public static readonly string Select = "Select";
            public static readonly string Submit = "Submit";
            public static readonly string Recovery = "Recovery";
            public static readonly string Flower = "Flower";
            public static readonly string NightEnemyAttack = "NightEnemyAttack";
            public static readonly string WolfEnemyAttack = "WolfEnemyAttack";
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
