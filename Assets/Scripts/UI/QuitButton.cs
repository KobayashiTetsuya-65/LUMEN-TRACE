using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class QuitButton : MonoBehaviour,ITitleSelectable
{
    [SerializeField, Tooltip("表示する画像")] private GameObject _obj;
    AudioManager _audioManager;
    Button _button;
    private void Awake()
    {
        _obj.SetActive(false);
    }
    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(QuitGame);
    }
    public void OnSelect()
    {
        // 色変更・拡大・SE
        _obj.SetActive(true);
        if (_audioManager == null) _audioManager = AudioManager.Instance;
        _audioManager.PlaySe(SoundDataUtility.KeyConfig.Se.Select);
    }

    public void OnDeselect()
    {
        // 元に戻す
        _obj.SetActive(false);
    }

    public void OnSubmit()
    {
        if (_audioManager == null) _audioManager = AudioManager.Instance;
        _audioManager.PlaySe(SoundDataUtility.KeyConfig.Se.Submit);
        QuitGame();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
}
