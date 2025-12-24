using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("QÆ")]
    [SerializeField, Tooltip("Œ©‚½–Ú")] private SpriteRenderer _sr;
    [SerializeField, Tooltip("’Êí‰æ‘œ")] private Sprite[] _normalSprits;

    [Header("”’lİ’è")]
    [SerializeField, Tooltip("ˆÚ“®‘¬“x")] private float _speed = 2.0f;

    private Rigidbody _rb;
    private PlayerInput _playerInput;
    private InputAction _moveAction;

    private Vector2 _moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        UpdateSprite();
    }
    private void LateUpdate()
    {
        PlayerMove();
    }
    public void PlayerMove()
    {
        Vector3 move = new Vector3(_moveInput.x,0,0);
        _rb.linearVelocity = move * _speed;
    }
    public void PlayerInput()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
    }
    private void UpdateSprite()
    {
        if (_moveInput.x > 0.01f)
            _sr.flipX = false;
        else if (_moveInput.x < -0.01f)
            _sr.flipX = true;
    }
}
