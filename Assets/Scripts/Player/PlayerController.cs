using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : LightSourceBase
{
    [Header("数値設定")]
    [SerializeField, Tooltip("移動速度")] private float _speed = 2.0f;
    [SerializeField, Tooltip("回避時の移動距離")] private float _distanceTraveled = 2f;

    private Rigidbody _rb;
    private PlayerInput _playerInput;
    private InputAction _moveAction,_attackAction,_dodgeAction;
    private PlayerStateMachine _stateMachine;
    private PlayerSpriteAnimator _spriteAnimator;

    private Vector2 _moveInput;
    private bool _isAttack,_isDodge;

    void Start()
    {
        _tr = transform;
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _stateMachine = GetComponent<PlayerStateMachine>();
        _spriteAnimator = GetComponent<PlayerSpriteAnimator>();

        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];
        _dodgeAction = _playerInput.actions["Dodge"];
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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
        if (_stateMachine.CurrentState == PlayerState.Attack)
        {
            return;
        }
        if (_stateMachine.CurrentState == PlayerState.Dodge)
        {
            Vector3 dodge = new Vector3(_distanceTraveled, 0, 0);
            if (!_spriteAnimator._isRightFacing)
            {
                dodge = new Vector3(-_distanceTraveled, 0, 0);
            }
                
            _rb.linearVelocity = dodge * _speed;
            return;
        }

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
        _isDodge = _dodgeAction.WasPressedThisFrame();

        //ステート管理
        if(_isAttack && _stateMachine.CurrentState != PlayerState.Attack)
        {
            _stateMachine.ChangeState(PlayerState.Attack);
            return;
        }

        if(_isDodge && _stateMachine.CurrentState != PlayerState.Dodge)
        {
            _stateMachine.ChangeState(PlayerState.Dodge);
            return;
        }

        if(_stateMachine.CurrentState == PlayerState.Attack
            || _stateMachine.CurrentState == PlayerState.Dodge) return;

        if (_moveInput.magnitude > 0.1f)
        {
            _stateMachine.ChangeState(PlayerState.Walk);
            _spriteAnimator.ChangeSpriteFlipX(_moveInput.x);
        }  
        else
            _stateMachine.ChangeState(PlayerState.Idle);

    }
}
