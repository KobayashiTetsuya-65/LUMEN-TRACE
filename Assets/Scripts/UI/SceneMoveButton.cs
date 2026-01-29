using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class SceneMoveButton : MonoBehaviour,ITitleSelectable
{
    [SerializeField, Tooltip("表示する画像")] private GameObject _obj;
    [SerializeField, Tooltip("移行先のシーン")] private SceneName _sceneName;

    AudioManager _audioManager;
    Button _button;
    private bool _isFirst = true;
    private void Awake()
    {
        _obj.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioManager = AudioManager.Instance;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SceneChange);
    }

    private void SceneChange()
    {
        GameManager.Instance.SceneMove(_sceneName);
    }
    public void OnSelect()
    {
        // 色変更・拡大・SE
        _obj.SetActive(true);
        if (_isFirst)
        {
            _isFirst = false;
            return;
        }
        if(_audioManager == null) _audioManager = AudioManager.Instance;
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
        SceneChange();
    }
}
