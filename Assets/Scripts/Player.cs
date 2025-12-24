using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("参照")]
    [SerializeField, Tooltip("見た目")] private SpriteRenderer _sr;
    [SerializeField, Tooltip("通常時画像")] private Sprite[] _normalSprites;

    [Header("数値設定")]
    [SerializeField, Tooltip("移動速度")] private float _speed = 2.0f;
    [SerializeField, Tooltip("画像の入れ替え速度")] private float _spriteChangeDistance = 0.2f;

    private Rigidbody _rb;
    private PlayerInput _playerInput;
    private InputAction _moveAction;

    private Vector2 _moveInput;
    private float _timer = 0;
    private int _normalSpritesIndex = 0;
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
        _timer += Time.deltaTime;
        if (_timer >= _spriteChangeDistance)
        {
            _sr.sprite = _normalSprites[_normalSpritesIndex];
            _normalSpritesIndex++;
            _timer = 0;
            if(_normalSpritesIndex >= _normalSprites.Length)
            {
                _normalSpritesIndex = 0;
            }
        }
        if (_moveInput.x > 0.01f)
            _sr.flipX = false;
        else if (_moveInput.x < -0.01f)
            _sr.flipX = true;
    }
}
