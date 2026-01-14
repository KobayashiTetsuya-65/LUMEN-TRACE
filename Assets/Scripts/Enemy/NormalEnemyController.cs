using System.Runtime.CompilerServices;
using UnityEngine;
/// <summary>
/// 敵の行動制御
/// </summary>
public class NormalEnemyController : MonoBehaviour,IEnemy
{
    [Header("参照")]
    [SerializeField, Tooltip("攻撃判定")] private GameObject _attackCollider;
    [SerializeField, Tooltip("中心")] private Transform _centerPoint;

    [Header("数値設定")]
    [SerializeField, Tooltip("移動速度")] private float _speed = 2.0f;
    [SerializeField, Tooltip("光源判定明度")] private float _lightDetectionValue = 0.2f;
    [SerializeField, Tooltip("攻撃判定距離")] private float _attackDetectionRange = 2f;
    [SerializeField, Tooltip("攻撃クールタイム")] private float _attackCooldown = 0.5f;
    [SerializeField, Tooltip("反転しない距離")] private float _flipDeadZone = 0.1f;
    [SerializeField, Tooltip("ヒットストップ時間")] private float _hitStopTime = 0.05f;

    [Header("当たり判定調整")]
    [SerializeField, Tooltip("通常時の半径")] private float _normalRadius = 0.2f;
    [SerializeField, Tooltip("通常時の高さ")] private float _normalHeight = 0.6f;

    private Transform _tr;
    private Rigidbody _rb;
    private CapsuleCollider _col;
    private EnemyStateMachine _stateMachine;
    private EnemySpriteAnimator _spriteAnimator;
    private EnemyLightSensor _lightSensor;
    private EnemyPlayerDetector _playerDetector;
    private HitStopManager _hitStopManager;
    
    private Transform _target;
    private float _lastAttackTime;

    void Start()
    {
        _tr = transform;
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _stateMachine = GetComponent<EnemyStateMachine>();
        _spriteAnimator = GetComponent<EnemySpriteAnimator>();
        _lightSensor = GetComponent<EnemyLightSensor>();
        _playerDetector = GetComponentInChildren<EnemyPlayerDetector>();
        _hitStopManager = HitStopManager.Instance;

        _col.radius = _normalRadius;
        _col.height = _normalHeight;

        _attackCollider.SetActive(false);
    }

    void Update()
    {
        _target = _playerDetector.CurrentTarget;
        SearchPlayer();

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
        Move();
    }
    /// <summary>
    /// 行動選択
    /// </summary>
    public void ThinkingPatterns()
    {
        if(_target == null)
        {
            _stateMachine.ChangeState(EnemyState.Idle);
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
        if (!isAppear)
        {
            EnemyAttackDetection enemyAttack = _attackCollider.GetComponent<EnemyAttackDetection>();
            enemyAttack.IsAttack = false;
        }
        _attackCollider.SetActive(isAppear);
    }
    public void Damaged()
    {
        _hitStopManager.RequestHitStop(_hitStopTime);
        Debug.Log("ダメージを与えた！");
    }
    /// <summary>
    /// プレイヤーを探索し、発見したらターゲットする
    /// </summary>
    public void SearchPlayer()
    {
        if (_target == null) return;

        float diff = _target.position.x - _tr.position.x;

        if (Mathf.Abs(diff) < _flipDeadZone)
            return;

        bool isLeft = _target.position.x < _tr.position.x;
        _spriteAnimator.ChangeSpriteFlipX(isLeft);
    }
}
