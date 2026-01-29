using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private SoundDataBase _soundDataBase;
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private Transform _seRoot;
    [SerializeField] private int _sePoolSize = 10;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioMixerGroup _seGroup;

    private Queue<AudioSource> _seAudioSourcePools = new Queue<AudioSource>();

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        LoadVolume();
    }
    public void CreateAudioSource()
    {
        if (_seRoot == null)
        {
            _seRoot = transform;
        }

        for (int i = 0; i < _sePoolSize; i++)
        {
            GameObject instance = new GameObject("SeAudioSource_" + i, typeof(AudioSource));
            instance.transform.SetParent(_seRoot);
            AudioSource audioSource = instance.GetComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = _seGroup;
            audioSource.playOnAwake = false;
            instance.gameObject.SetActive(false);
            _seAudioSourcePools.Enqueue(audioSource);
        }
    }
    public void PlayBGM(string key)
    {
        StopBGM();
        SoundData soundData = _soundDataBase.GetSoundData(key);

        if (soundData == null)
        {
            Debug.LogWarning("Sound Data not found: " + key);
            return;
        }

        _bgmSource.PrepareAudioSource(soundData);

        _bgmSource.Play();
    }

    public void StopBGM()
    {
        if (_bgmSource.isPlaying)
        {
            _bgmSource.Stop();
        }
    }

    /// <summary>
    /// 指定のSEを出す
    /// </summary>
    /// <param name="key"></param>
    public void PlaySe(string key)
    {
        SoundData soundData = _soundDataBase.GetSoundData(key);
        if (soundData == null)
        {
            Debug.LogWarning("Sound Data not found: " + key);
            return;
        }

        AudioSource seAudioSource = default;
        if (_seAudioSourcePools.TryDequeue(out AudioSource source))
        {
            seAudioSource = source;
        }
        else
        {
            seAudioSource = new GameObject("seAudioSource_" + "NewInstance", typeof(AudioSource)).GetComponent<AudioSource>();
        }

        seAudioSource.PrepareAudioSource(soundData);
        seAudioSource.gameObject.SetActive(true);
        seAudioSource.Play();
        StartCoroutine(ReturnToPoolAfterPlaying(seAudioSource));
    }

    private void LoadVolume()
    {
        string[] parameters = { "Master", "BGM", "SE" };

        foreach (var p in parameters)
        {
            if (PlayerPrefs.HasKey(p))
            {
                float v = PlayerPrefs.GetFloat(p, 1f);
                float dB = Mathf.Log10(Mathf.Clamp(v, 0.0001f, 1f)) * 20f;
                _mixer.SetFloat(p, dB);
            }
        }
    }
    private IEnumerator ReturnToPoolAfterPlaying(AudioSource source)
    {
        if (source == null)yield break;
        yield return new WaitWhile(() => source.isPlaying);
        source.gameObject.SetActive(false);
        _seAudioSourcePools.Enqueue(source);
    }
    /// <summary>
    /// 指定シーンのBGMキーを返す
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public string ReturnBGMKey(SceneName scene)
    {
        return scene switch
        {
            SceneName.Title => SoundDataUtility.KeyConfig.Bgm.Title,
            SceneName.InGame => SoundDataUtility.KeyConfig.Bgm.InGame,
            _ => null
        };
    }

    public void SetVolume(string param, float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        _mixer.SetFloat(param, dB);
        PlayerPrefs.SetFloat(param, value);
    }
}
