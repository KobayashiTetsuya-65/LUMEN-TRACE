using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public SceneName CurrentScene { get; private set; }

    public bool IsMovie { get; private set; } = false;

    [SerializeField] private SceneName _startSceneName = SceneName.Title;

    private AudioManager _audioManager;

    private bool _isPlayingBGM = false,_first = false;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
        CurrentScene = _startSceneName;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioManager = AudioManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPlayingBGM)
        {
            if (!_first)
            {
                _audioManager.CreateAudioSource();
                if (_startSceneName == SceneName.InGame) IsMovie = true;
                _first = true;
            }

            if (IsMovie) return;

            _audioManager.PlayBGM(_audioManager.ReturnBGMKey(CurrentScene));
            _isPlayingBGM = true;
        }
    }
    public void SceneMove(SceneName scene)
    {
        SceneManager.LoadScene(SceneStringName(scene));
        CurrentScene = scene;
        _isPlayingBGM = false;
    }
    private string SceneStringName(SceneName scene)
    {
        return scene switch
        {
            SceneName.Title => "Title",
            SceneName.InGame => "InGame",
            _ => null
        };
    }

    public void FinishMovie()
    {
        IsMovie = false;
    }

    public void StartMovie()
    {
        IsMovie = true;
        _audioManager.StopBGM();
    }
}
