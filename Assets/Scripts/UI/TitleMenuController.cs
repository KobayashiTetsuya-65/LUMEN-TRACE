using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class TitleMenuController : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _buttons;

    private List<ITitleSelectable> _selectables = new();
    private PlayerInput _playerInput;
    private InputAction _upAction, _downAction,_submitAction;

    private int _currentIndex = 0;
    private void Awake()
    {
        foreach (MonoBehaviour b in _buttons)
        {
            if(b.TryGetComponent<ITitleSelectable>(out var component))
            {
                _selectables.Add(component);
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.SwitchCurrentActionMap("Player");
        _upAction = _playerInput.actions["UP"];
        _downAction = _playerInput.actions["Down"];
        _submitAction = _playerInput.actions["Submit"];

        _selectables[0].OnSelect();
    }

    // Update is called once per frame
    void Update()
    {
        if (_upAction.WasPressedThisFrame())
        {
            if (_currentIndex > 0)
                ChangeSelection(-1);
        }
        else if (_downAction.WasPressedThisFrame())
        {
            if (_currentIndex < _selectables.Count - 1)
                ChangeSelection(1);
        }
        else if (_submitAction.WasPressedThisFrame())
        {
            _selectables[_currentIndex].OnSubmit();
        }
    }
    void ChangeSelection(int next)
    {
        _selectables[_currentIndex].OnDeselect();
        _currentIndex += next;
        _selectables[_currentIndex].OnSelect();
    }
}
