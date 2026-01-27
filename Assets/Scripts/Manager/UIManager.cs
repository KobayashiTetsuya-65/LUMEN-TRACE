using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IUIManager
{
    [Header("ŽQÆ")]
    [SerializeField] private Image _img;

    [Header("‰‰oŽžŠÔÝ’è")]
    [SerializeField] private float _duration = 1f;

    GameManager _gameManager;
    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.RegisterUIManager(this);
    }
    public void FadePanel(bool toInvisible, SceneName sceneName)
    {
        if (toInvisible)
        {
            _img.DOFade(1f, _duration)
                .OnComplete(() => _gameManager.ChangeScene(sceneName));
        }
        else
        {
            _img.DOFade(0f, _duration * 0.5f);
        }
    }
}
