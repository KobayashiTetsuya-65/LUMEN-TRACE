using UnityEngine;

public class ClearPoint : MonoBehaviour
{
    GameManager _gameManager;
    private void Start()
    {
        _gameManager = GameManager.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _gameManager.SceneMove(SceneName.Title);
        }
    }
}
