using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "SoundData",menuName = "Audio/SoundData",order = 1)]
public class SoundDataBase : ScriptableObject
{
    [SerializeField] private List<SoundData> _soundMap = new List<SoundData>();

    public SoundData GetSoundData(string key)
    {
        return _soundMap.FirstOrDefault(x => x.Key == key);
    }
}

[Serializable]
public class SoundData
{
    [SerializeField, Header("キー")] private string _key;
    [SerializeField, Header("種類")] private SoundDataUtility.SoundType _type;
    [SerializeField, Header("ループ再生")] private bool _isLoop;
    [SerializeField, Header("即再生")] private bool _playOnAwake;
    [SerializeField, Range(0f, 1f), Header("音量調整")] private float _volume;
    [SerializeField, Header("音源")] private AudioClip _clip;

    public string Key => _key;
    public SoundDataUtility.SoundType Type => _type;
    public bool IsLoop => _isLoop;
    public bool PlayOnAwake => _playOnAwake;
    public float Volume => _volume;
    public AudioClip Clip => _clip;
}
