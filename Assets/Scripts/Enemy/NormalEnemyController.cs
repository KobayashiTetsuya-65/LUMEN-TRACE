using System.Collections;
using UnityEngine;
/// <summary>
/// 敵の行動制御
/// </summary>
public class NormalEnemyController : MonoBehaviour,IEnemy
{
    public bool IsDead { get; private set; } = false;
    public bool IsMovie = false;
    public EnemyType MyEnemyType;
    public System.Action OnDead;

    [Header("参照")]
    [SerializeField, Tooltip("攻撃判定")] private GameObject _attackCollider;
    [SerializeField, Tooltip("中心")] private Transform _centerPoint;
    [SerializeField, Tooltip("体の判定")] private CapsuleCollider _col;
    [SerializeField, Tooltip("親オブジェクト")] private Transform _tr;
    [SerializeField] private EnemyStateMachine _stateMachine;
    [SerializeField] private EnemySpriteAnimator _spriteAnimator;
    [SerializeField] private EnemyPlayerDetector _playerDetector;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject _effect;
    [SerializeField] private Transform _effectPos;

    [Header("数値設定")]
    [SerializeField, Tooltip("移動速度")] private float _speed = 2.0f;
    [SerializeField, Tooltip("光源判定明度")] private float _lightDetectionValue = 0.2f;
    [SerializeField, Tooltip("攻撃判定距離")] private float _attackDetectionRange = 2f;
    [SerializeField, Tooltip("攻撃クールタイム")] private float _attackCooldown = 0.5f;
    [SerializeField, Tooltip("反転しない距離")] private float _flipDeadZone = 0.1f;
    [SerializeField, Tooltip("ヒットストップ時間")] private float _hitStopTime = 0.05f;
    [SerializeField, Tooltip("HP")] private int _enemyMaxHP = 5;

    [Header("徘徊設定")]
    [SerializeField, Tooltip("静止時間")] private float _wanderIdleTime = 2f;
    [SerializeField, Tooltip("歩く時間")] private float _wanderWalkTime = 2f;
    [SerializeField, Tooltip("見失うまでの時間")] private float _sarchInterval = 1.0f;

    [Header("障害物検知")]
    [SerializeField, Tooltip("障害物確認距離")] private float _wallCheckDistance = 0.3f;
    [SerializeField, Tooltip("障害物レイヤー")] private LayerMask _wallLayer;
    [SerializeField, Tooltip("飛ばし始める場所")] private Transform _wallCheckPoint;

    [Header("当たり判定調整")]
    [SerializeField, Tooltip("通常時の半径")] private float _normalRadius = 0.2f;
    [SerializeField, Tooltip("通常時の高さ")] private float _normalHeight = 0.6f;

    private AudioManager _audioManager;
    private GameManager _gameManager;
    private EnemyLightSensor _lightSensor;
    private HitStopManager _hitStopManager;
    private PlayerController _playerController;

    RaycastHit _hit;
    private Vector3 _dir;
    private Transform _target;
    private float _lastAttackTime, _wanderTimer = 0f, _chaseTimer = 0;
    private int _currentHP;
    void Start()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
        _lightSensor = GetComponent<EnemyLightSensor>();
        _hitStopManager = HitStopManager.Instance;
        _audioManager = AudioManager.Instance;
        _gameManager = GameManager.Instance;

        _col.radius = _normalRadius;
        _col.height = _normalHeight;
        _currentHP = _enemyMaxHP;

        IsMovie = false;
        _attackCollider.SetActive(false);
    }

    void Update()
    {
        if (IsDead) return;

        SearchPlayer();

        if (IsMovie) return;

        UpdateFacing();

        if (_gameManager.IsMovie)
        {
            _stateMachine.ChangeState(EnemyState.Dead);
            return;
        }



        if (_stateMachine.CurrentState == EnemyState.Attack)
        {
            if (_spriteAnimator.IsAttackFinished)
            {
                _spriteAnimator.ResetAttack();
                _stateMachine.ChangeState(EnemyState.Idle);
            }
            return;
        }

        ThinkingPatterns();
    }
    private void LateUpdate()
    {

    }
    private void FixedUpdate()
    {
        if (IsDead) return;
        Move();
    }
    /// <summary>
    /// 行動選択
    /// </summary>
    public void ThinkingPatterns()
    {
        if(_target == null)
        {
            Wander();
            return;
        }

        _chaseTimer += Time.deltaTime;

        if(_chaseTimer >= _sarchInterval)
        {
            _target = null;
            _chaseTimer = 0f;
            Wander();
            return;
        }

        //攻撃判定
        float distanceX = Mathf.Abs(_target.position.x - _centerPoint.position.x);
        if (distanceX <= _attackDetectionRange &&
            Time.time >= _lastAttackTime + _attackCooldown)
        {
            _lastAttackTime = Time.time;
            _stateMachine.ChangeState(EnemyState.Attack);
            return;
        }

        //光源判定
        if (_lightSensor.CurrentLight > _lightDetectionValue &&
            distanceX > _attackDetectionRange)
        {
            _stateMachine.ChangeState(EnemyState.Walk);
            
            return;
        }

        _stateMachine.ChangeState(EnemyState.Idle);
    }

    public void Move()
    {
        if (_stateMachine.CurrentState != EnemyState.Walk)
        {
            _rb.linearVelocity = Vector3.zero;
            return;
        }

        
        Vector3 move = _spriteAnimator.IsLeftFacing ? Vector3.left : Vector3.right;
        _rb.linearVelocity = _speed * move;
    }
    public void Attack(bool isAppear)
    {
        if(_playerController.IsDead) return;

        if (!isAppear)
        {
            EnemyAttackDetection enemyAttack = _attackCollider.GetComponent<EnemyAttackDetection>();
            enemyAttack.IsAttack = false;
        }
        else
        {
            _audioManager.PlaySe(SoundDataUtility.KeyConfig.Se.EnemyAttack);
        }
            _attackCollider.SetActive(isAppear);
    }
    public void Damaged()
    {
        //演出
        Instantiate(_effect, _effectPos.position + new Vector3(0f,0f,-1f), Quaternion.identity);
        _hitStopManager.RequestHitStop(_hitStopTime);
        _audioManager.PlaySe(SoundDataUtility.KeyConfig.Se.Damage);

        _currentHP -= 1;
        if(_currentHP <= 0)
        {
            IsDead = true;
            _col.gameObject.SetActive(false);
            _rb.useGravity = false;
            _stateMachine.ChangeState(EnemyState.Dead);
        }
    }

    public void Dead()
    {
        OnDead?.Invoke();
    }
    /// <summary>
    /// プレイヤーを探索し、発見したらターゲットする
    /// </summary>
    public void SearchPlayer()
    {
        if (_target == null && _playerDetector.CurrentTarget != null)
        {
            _target = _playerDetector.CurrentTarget;
            _chaseTimer = 0f;
        }
        else if (_playerDetector.CurrentTarget == null)
        {
            _target = null;
        }

        if (_target == null) return;

        float diff = _target.position.x - _tr.position.x;

        if (Mathf.Abs(diff) < _flipDeadZone)
            return;
    }

    /// <summary>
    /// 向きを1か所で決定
    /// </summary>
    private void UpdateFacing()
    {
        if (_target != null)
        {
            float diff = _target.position.x - _tr.position.x;

            if (Mathf.Abs(diff) < _flipDeadZone)
                return;

            bool isLeft = diff < 0;
            SetFacing(isLeft);
            return;
        }

        if (_stateMachine.CurrentState == EnemyState.Walk && IsWallAhead())
        {
            SetFacing(!_spriteAnimator.IsLeftFacing);
        }
    }
    private bool IsWallAhead()
    {
        _dir = _spriteAnimator.IsLeftFacing ? Vector3.left : Vector3.right;

        return Physics.Raycast(_wallCheckPoint.position, _dir, _wallCheckDistance, _wallLayer);
    }
    /// <summary>
    /// 向き変更の唯一の入口
    /// </summary>
    private void SetFacing(bool isLeft)
    {
        if (_spriteAnimator.IsLeftFacing == isLeft) return;

        _spriteAnimator.ChangeSpriteFlipX(isLeft);
    }

    private void Wander()
    {
        _wanderTimer += Time.deltaTime;

        if (_stateMachine.CurrentState == EnemyState.Idle)
        {
            if (_wanderTimer >= _wanderIdleTime)
            {
                _wanderTimer = 0f;
                _wanderIdleTime = Random.Range(1f, 3f);
                _wanderWalkTime = Random.Range(1f, 3f);

                _stateMachine.ChangeState(EnemyState.Walk);
            }
        }
        else if (_stateMachine.CurrentState == EnemyState.Walk)
        {
            if (_wanderTimer >= _wanderWalkTime)
            {
                _wanderTimer = 0f;
                _wanderIdleTime = Random.Range(1f, 3f);
                _wanderWalkTime = Random.Range(1f, 3f);
                _stateMachine.ChangeState(EnemyState.Idle);
            }
        }
        else
        {
            _stateMachine.ChangeState(EnemyState.Idle);
        }
    }

    public void FinishMovie()
    {
        IsMovie = false;
        Destroy(_tr.gameObject);
    }

    public void StartMovie()
    {
        IsMovie = true;
    }
}
