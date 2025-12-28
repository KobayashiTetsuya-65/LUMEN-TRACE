using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("数値設定")]
    [SerializeField, Tooltip("移動速度")] private float _speed = 2.0f;
    [SerializeField, Tooltip("明るさ半径")] private float _lightRadius = 4f;

    private Transform _tr;
    private Rigidbody _rb;
    private PlayerInput _playerInput;
    private InputAction _moveAction,_attackAction;
    private PlayerStateMachine _stateMachine;
    private PlayerSpriteAnimator _spriteAnimator;

    private Vector2 _moveInput;
    private bool _isAttack;

    void Start()
    {
        _tr = transform;
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _stateMachine = GetComponent<PlayerStateMachine>();
        _spriteAnimator = GetComponent<PlayerSpriteAnimator>();

        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }
    private void LateUpdate()
    {
        
    }
    private void FixedUpdate()
    {
        PlayerMove();
    }
    /// <summary>
    /// プレイヤー挙動
    /// </summary>
    public void PlayerMove()
    {
        if (_stateMachine.CurrentState == PlayerState.Attack ||
            _stateMachine.CurrentState == PlayerState.Dodge)
            return;
        Vector3 move = new Vector3(_moveInput.x,0,0);
        _rb.linearVelocity = move * _speed;
    }
    /// <summary>
    /// プレイヤー入力
    /// </summary>
    public void PlayerInput()
    {
        //入力
        _moveInput = _moveAction.ReadValue<Vector2>();
        _isAttack = _attackAction.WasPressedThisFrame();

        //ステート管理
        if(_isAttack && _stateMachine.CurrentState != PlayerState.Attack)
        {
            _stateMachine.ChangeState(PlayerState.Attack);
            return;
        }

        if(_stateMachine.CurrentState == PlayerState.Attack) return;

        if (_moveInput.magnitude > 0.1f)
        {
            _stateMachine.ChangeState(PlayerState.Walk);
            _spriteAnimator.ChangeSpriteFlipX(_moveInput.x);
        }  
        else
            _stateMachine.ChangeState(PlayerState.Idle);

    }
    void OnTriggerStay(Collider other)
    {
        if (Time.frameCount % 3 != 0) return;
        if (other.TryGetComponent<ILightAffectable>(out var affectable))
        {
            float dist = Vector3.Distance(_tr.position, other.transform.position);
            affectable.SetLightPower(1f - dist / _lightRadius);
        }
    }
}
