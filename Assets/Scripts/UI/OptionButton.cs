using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class OptionButton : MonoBehaviour,ITitleSelectable
{
    [SerializeField, Tooltip("表示する画像")] private GameObject _obj;
    [SerializeField, Tooltip("設定画面")] private GameObject _panel;

    AudioManager _audioManager;
    Button _button;
    PlayerInput _playerInput;
    OptionMenuController _optionMenuController;
    private void Awake()
    {
        _obj.SetActive(false);
        _panel.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioManager = AudioManager.Instance;
        _button = GetComponent<Button>();
        _playerInput = FindAnyObjectByType<PlayerInput>();
        _optionMenuController = FindAnyObjectByType<OptionMenuController>();
        _button.onClick.AddListener(OptionDisplay);
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

        StartCoroutine(DelayRegister());
    }
    IEnumerator DelayRegister()
    {
        yield return null;
        OptionDisplay();
        _playerInput.SwitchCurrentActionMap("Option");
        _optionMenuController.RegisterAction();
    }
    private void OptionDisplay()
    {
        _panel.SetActive(true);
    }
}
