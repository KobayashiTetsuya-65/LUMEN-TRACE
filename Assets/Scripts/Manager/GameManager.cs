using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public SceneName CurrentScene { get; private set; }
    public IUIManager CurrentUIManager { get; private set; }

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
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioManager = AudioManager.Instance;
        CurrentScene = _startSceneName;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPlayingBGM)
        {
            if (!_first)
            {
                _audioManager.CreateAudioSource();
                if (CurrentScene == SceneName.InGame) IsMovie = true;
                _first = true;
            }

            if (IsMovie) return;

            _audioManager.PlayBGM(_audioManager.ReturnBGMKey(CurrentScene));
            _isPlayingBGM = true;
        }
    }
    public void SceneMove(SceneName scene)
    {
        CurrentUIManager.FadePanel(true,scene);
    }

    public void ChangeScene(SceneName scene)
    {
        CurrentScene = scene;
        _isPlayingBGM = false;
        _first = false;
        _audioManager.StopBGM();
        SceneManager.LoadScene(SceneStringName(scene));
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
        _isPlayingBGM = false;
        Debug.Log("ゲーム開始！！！");
    }

    public void StartMovie()
    {
        IsMovie = true;
        _audioManager.StopBGM();
    }

    public void ClearGame()
    {
        SceneMove(SceneName.Title);
    }

    public void RegisterUIManager(IUIManager manager)
    {
        CurrentUIManager = manager;
    }
}
