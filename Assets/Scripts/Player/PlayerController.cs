using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
/// <summary>
/// プレイヤーの行動制御
/// </summary>
public class PlayerController : LightSourceBase,IPlayer
{
    public bool IsInvincible { get; private set; } = false;
    public bool IsDead { get; private set; } = false;

    public bool IsMovie = true;

    [Header("参照")]
    [SerializeField, Tooltip("攻撃判定")] private GameObject _attackCollider;
    [SerializeField, Tooltip("足元のライト")] private PlayerLightVisual _lightVisual;
    [SerializeField, Tooltip("夜の演出")] private Volume _nightVolume;

    [Header("数値設定")]
    [SerializeField, Tooltip("移動速度")] private float _speed = 2.0f;
    [SerializeField, Tooltip("回避時の移動距離")] private float _distanceTraveled = 2f;
    [SerializeField, Tooltip("通常時の光源の半径")] private float _normalLightRadius = 10f;
    [SerializeField, Tooltip("隠れる時の光源の半径")] private float _hideLightRadius = 3f;
    [SerializeField, Tooltip("明るさの補正値")] private float _lightAdjustmentValue = 2f;
    [SerializeField, Tooltip("最大HP")] private int _maxHP = 3;
    [SerializeField, Tooltip("被ダメ演出の効果量")] private float _intensityIncrease = 0.1f;
    [SerializeField, Tooltip("被ダメ演出の時間")] private float _damageDuration = 0.4f;
    [SerializeField, Tooltip("死亡演出の暗転度")] private float _targetPostExposure = -8f;
    [SerializeField, Tooltip("死亡時演出時間")] private float _deadEffectDuration = 1.0f;

    [Header("当たり判定調整")]
    [SerializeField, Tooltip("通常時の半径")] private float _normalRadius = 0.14f;
    [SerializeField, Tooltip("通常時の高さ")] private float _normalHeight = 0.4f;
    [SerializeField, Tooltip("回避時の高さ倍率")] private float _dodgeHeightMag = 0.8f;
    [SerializeField, Tooltip("潜伏時の半径倍率")] private float _hideRadiusMag = 0.8f;
    [SerializeField, Tooltip("潜伏時の高さ倍率")] private float _hideHeightMag = 0.35f;

    private AudioManager _audioManager;
    private Rigidbody _rb;
    private CapsuleCollider _col;
    private PlayerInput _playerInput;
    private InputAction _moveAction,_attackAction,_dodgeAction,_hideAction,_interactAction;
    private PlayerStateMachine _stateMachine;
    private PlayerSpriteAnimator _spriteAnimator;
    private Vignette _vignette;
    private ColorAdjustments _colorAdjust;
    private IInteractable _lightGimmick;

    private Vector2 _moveInput;
    private bool _isAttack,_isDodge,_isHide,_isInteract;
    private int _currentHP;
    private float _startIntensity;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _stateMachine = GetComponent<PlayerStateMachine>();
        _spriteAnimator = GetComponent<PlayerSpriteAnimator>();
        _audioManager = AudioManager.Instance;

        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];
        _dodgeAction = _playerInput.actions["Dodge"];
        _hideAction = _playerInput.actions["Hide"];
        _interactAction = _playerInput.actions["Interact"];

        if(_nightVolume.profile.TryGet(out _vignette))
        {
            _startIntensity = _vignette.intensity.value;
        }
        _nightVolume.profile.TryGet(out _colorAdjust);
        _currentHP = _maxHP;
        _attackCollider.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (IsDead) return;
        base.Update();
        float normalized = Mathf.InverseLerp(_minRadius,_maxRadius,_lightRadius);
        _lightVisual.SetLightVisual(normalized);

        if (IsMovie) return;
        PlayerInput();
    }
    private void LateUpdate()
    {
        
    }
    protected override void FixedUpdate()
    {
        if (IsDead) return;
        base.FixedUpdate();

        if(IsMovie) return;
        PlayerMove();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IInteractable>(out var gimmick))
        {
            _lightGimmick = gimmick;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<IInteractable>(out var gimmick))
        {
            if(_lightGimmick == gimmick)
                _lightGimmick = null;
        }  
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
        _isInteract = _interactAction.IsPressed();

        //ステート管理
        if(_isAttack && _stateMachine.CurrentState != PlayerState.Attack)
        {
            _stateMachine.ChangeState(PlayerState.Attack);
            Attack(true);
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

        if (_isInteract)
        {
            TryInteract();
        }

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
    /// <summary>
    /// プレイヤーの攻撃
    /// </summary>
    /// <param name="isAppear"></param>
    public void Attack(bool isAppear)
    {
        _attackCollider.SetActive(isAppear);
    }
    public void Hide(bool toHide)
    {
        float radius = toHide ? _hideLightRadius : ((_currentHP + _lightAdjustmentValue) / (_maxHP + _lightAdjustmentValue)) * _normalLightRadius;
        ChangeLightRadius(radius);
    }
    public void ChangeInvincibleState(bool toInvincible)
    {
        IsInvincible = toInvincible;
    }
    /// <summary>
    /// ダメージを受ける
    /// </summary>
    public void Damaged()
    {
        if (IsInvincible)
        {

        }
        else
        {
            _audioManager.PlaySe(SoundDataUtility.KeyConfig.Se.Damage);
            _currentHP -= 1;
            
            if(_currentHP <= 0)
            {
                IsDead = true;
                DeadEffect();
            }
            else
            {
                ChangeLightRadius(((_currentHP + _lightAdjustmentValue) / (_maxHP + _lightAdjustmentValue)) * _normalLightRadius);
                DamageEffect();
            }
                Debug.Log("ダメージを受けた");
        }
    }

    private void TryInteract()
    {
        _lightGimmick?.Interact();
    }

    public void FinishMovie()
    {
        IsMovie = false;
    }

    public void StartMovie()
    {
        IsMovie = true;
        _rb.linearVelocity = Vector3.zero;
        _stateMachine.ChangeState(PlayerState.Idle);
    }

    private void DamageEffect()
    {
        StopAllCoroutines();
        StartCoroutine(DamageVignetteEffect());
    }

    private IEnumerator DamageVignetteEffect()
    {
        float target = _startIntensity + (_intensityIncrease * (_maxHP - _currentHP));
        float t = 0f;

        while (t < _damageDuration)
        {
            t += Time.deltaTime;
            _vignette.intensity.value = Mathf.Lerp(_startIntensity, target, t / _damageDuration);
            yield return null;
        }

        t = 0f;

        // 元に戻す
        while (t < _damageDuration)
        {
            t += Time.deltaTime;
            _vignette.intensity.value = Mathf.Lerp(target, _startIntensity, t / _damageDuration);
            yield return null;
        }
    }

    private void DeadEffect()
    {
        _audioManager.StopBGM();
        _stateMachine.ChangeState(PlayerState.Idle);
        StopAllCoroutines();
        StartCoroutine(DeadBlackoutEffect());
    }

    private IEnumerator DeadBlackoutEffect()
    {
        IsMovie = true;
        _rb.linearVelocity = Vector3.zero;

        float t = 0f;
        float start = _colorAdjust.postExposure.value;

        while (t < _deadEffectDuration)
        {
            t += Time.deltaTime;
            _colorAdjust.postExposure.value = Mathf.Lerp(start, _targetPostExposure, t / _deadEffectDuration);
            yield return null;
        }

        _colorAdjust.postExposure.value = _targetPostExposure;

        yield return new WaitForSeconds(0.5f); // 少し余韻

        GameManager.Instance.SceneMove(SceneName.Title);
    }
}
