using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerController : LightSourceBase
{
    [Header("数値設定")]
    [SerializeField, Tooltip("移動速度")] private float _speed = 2.0f;
    [SerializeField, Tooltip("回避時の移動距離")] private float _distanceTraveled = 2f;

    [Header("当たり判定調整")]
    [SerializeField, Tooltip("通常時の半径")] private float _normalRadius = 0.14f;
    [SerializeField, Tooltip("通常時の高さ")] private float _normalHeight = 0.4f;
    [SerializeField, Tooltip("回避時の高さ倍率")] private float _dodgeHeightMag = 0.8f;
    [SerializeField, Tooltip("潜伏時の半径倍率")] private float _hideRadiusMag = 0.8f;
    [SerializeField, Tooltip("潜伏時の高さ倍率")] private float _hideHeightMag = 0.35f;

    private Rigidbody _rb;
    private CapsuleCollider _col;
    private PlayerInput _playerInput;
    private InputAction _moveAction,_attackAction,_dodgeAction,_hideAction;
    private PlayerStateMachine _stateMachine;
    private PlayerSpriteAnimator _spriteAnimator;

    private Vector2 _moveInput;
    private bool _isAttack,_isDodge,_isHide;

    void Start()
    {
        _tr = transform;
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _stateMachine = GetComponent<PlayerStateMachine>();
        _spriteAnimator = GetComponent<PlayerSpriteAnimator>();

        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];
        _dodgeAction = _playerInput.actions["Dodge"];
        _hideAction = _playerInput.actions["Hide"];
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
        if (_stateMachine.CurrentState == PlayerState.Attack
            || _stateMachine.CurrentState == PlayerState.Hide)
        {
            return;
        }
        if (_stateMachine.CurrentState == PlayerState.Dodge)
        {
            Vector3 dodge = new Vector3(_distanceTraveled, 0, 0);
            if (!_spriteAnimator.IsRightFacing)
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
        _isHide = _hideAction.IsPressed();

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

        if (_isHide)
        {
            _stateMachine.ChangeState(PlayerState.Hide);
            return;
        }
        else if (_spriteAnimator.IsEnterHide)
        {
            _spriteAnimator.IsEnterHide = false;
            return;
        }
        else if(_stateMachine.CurrentState == PlayerState.Hide)
        {
            return;
        }

        if (_stateMachine.CurrentState == PlayerState.Attack
            || _stateMachine.CurrentState == PlayerState.Dodge) return;

        if (_moveInput.magnitude > 0.1f)
        {
            _stateMachine.ChangeState(PlayerState.Walk);
            _spriteAnimator.ChangeSpriteFlipX(_moveInput.x);
        }  
        else
            _stateMachine.ChangeState(PlayerState.Idle);

    }
    /// <summary>
    /// 状態に合わせて高さを調整する
    /// </summary>
    /// <param name="state"></param>
    public void ChangeColliderHeight(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Dodge:
                if(_col.height == _normalHeight * _dodgeHeightMag)return;
                _col.height = _normalHeight * _dodgeHeightMag;
                break;
            case PlayerState.Hide:
                if (_col.height == _normalHeight * _hideHeightMag) return;
                _col.height = _normalHeight * _hideHeightMag;
                _col.radius = _normalRadius * _hideRadiusMag;
                break;
            default:
                if(_col.height == _normalHeight) return;
                _col.height = _normalHeight;
                _col.radius = _normalRadius;
                break;
        }
    }
}
