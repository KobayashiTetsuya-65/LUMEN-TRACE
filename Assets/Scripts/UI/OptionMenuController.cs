using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionMenuController : MonoBehaviour
{
    [SerializeField] private List<Slider> _sliders;
    [SerializeField] private GameObject _panel;
    [SerializeField] private float _changeAmount = 0.01f;
    [SerializeField] private float _sliderSizeMag = 1.2f;
    private int _currentIndex = 0,_index;

    private AudioManager _audioManager;
    private PlayerInput _playerInput;
    private InputAction _upAction, _downAction, _leftAction, _rightAction, _submitAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _audioManager = AudioManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_panel.activeSelf)
        {
            if (_upAction.WasPressedThisFrame())
                ChangeIndex(-1);

            if (_downAction.WasPressedThisFrame())
                ChangeIndex(1);

            if (_leftAction.IsPressed())
                ChangeValue(-_changeAmount);

            if (_rightAction.IsPressed())
                ChangeValue(_changeAmount);

            if (_submitAction.WasPressedThisFrame())
                Close();
        }
    }
    void ChangeIndex(int delta)
    {
        _sliders[_currentIndex].transform.localScale = Vector3.one * 0.3f;
        _index = _currentIndex;
        _currentIndex = Mathf.Clamp(_currentIndex + delta, 0, _sliders.Count - 1);
        if (_index != _currentIndex) _audioManager.PlaySe(SoundDataUtility.KeyConfig.Se.Select);
        _sliders[_currentIndex].transform.localScale = Vector3.one * _sliderSizeMag;
    }

    void ChangeValue(float amount)
    {
        _sliders[_currentIndex].value += amount;
    }

    void Close()
    {
        _audioManager.PlaySe(SoundDataUtility.KeyConfig.Se.Submit);
        _playerInput.SwitchCurrentActionMap("Player");
        _panel.SetActive(false);
    }
    public void RegisterAction()
    {
        _upAction = _playerInput.actions["UP"];
        _downAction = _playerInput.actions["Down"];
        _leftAction = _playerInput.actions["Left"];
        _rightAction = _playerInput.actions["Right"];
        _submitAction = _playerInput.actions["Enter"];
    }
}
