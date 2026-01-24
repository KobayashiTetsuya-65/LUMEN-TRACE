using UnityEngine;

public class EventManager : MonoBehaviour
{
    GameManager _gameManager;
    private void Awake()
    {
        _gameManager = GameManager.Instance;
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
}
