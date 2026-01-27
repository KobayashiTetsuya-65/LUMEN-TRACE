using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class EventManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director;

    GameManager _gameManager;
    InputAction _skipAction;
    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }
    void Start()
    {
        PlayerInput input = FindAnyObjectByType<PlayerInput>();
        _skipAction = input.actions["Skip"];
        _skipAction.Enable();
    }

    void Update()
    {
        if (_skipAction.WasPressedThisFrame())
        {
            SkipTimeline();
        }
    }
    public void StartMovie()
    {
        _gameManager.StartMovie();
    }

    public void FinishMovie()
    {
        _gameManager.FinishMovie();
    }

    public void ClearGame()
    {
        _gameManager.ClearGame();
    }

    private void SkipTimeline()
    {
        _director.time = _director.duration;
        _director.Evaluate(); // ã≠êßîΩâf
        _director.Stop();
        FinishMovie();
    }
}
