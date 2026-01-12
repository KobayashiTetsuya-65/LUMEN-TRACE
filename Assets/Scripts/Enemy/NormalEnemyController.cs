using UnityEngine;

public class NormalEnemyController : MonoBehaviour,IEnemy
{
    [Header("”’lİ’è")]
    [SerializeField, Tooltip("ˆÚ“®‘¬“x")] private float _speed = 2.0f;
    [SerializeField, Tooltip("ŒõŒ¹”»’è–¾“x")] private float _lightDetectionValue = 0.2f;

    [Header("“–‚½‚è”»’è’²®")]
    [SerializeField, Tooltip("’Êí‚Ì”¼Œa")] private float _normalRadius = 0.2f;
    [SerializeField, Tooltip("’Êí‚Ì‚‚³")] private float _normalHeight = 0.6f;

    private Transform _tr;
    private Rigidbody _rb;
    private CapsuleCollider _col;
    private EnemyStateMachine _stateMachine;
    private EnemySpriteAnimator _spriteAnimator;
    private EnemyLightSensor _lightSensor;
    
    private bool _isChase = false;
    private Transform _target;
    private Vector3 _center;


    void Start()
    {
        _tr = transform;
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _stateMachine = GetComponent<EnemyStateMachine>();
        _spriteAnimator = GetComponent<EnemySpriteAnimator>();
        _lightSensor = GetComponent<EnemyLightSensor>();
        _center = _col.center;
    }

    void Update()
    {
        ThinkingPatterns();

    }
    private void LateUpdate()
    {
        
    }
    private void FixedUpdate()
    {
        Move();
    }
    public void ThinkingPatterns()
    {
        //ŒõŒ¹”»’è
        if (_lightSensor.CurrentLight > _lightDetectionValue)
        {
            _stateMachine.ChangeState(EnemyState.Walk);
        }
        else
        {
            _stateMachine.ChangeState(EnemyState.Idle);
        }
    }
    public void Move()
    {
        if (_stateMachine.CurrentState != EnemyState.Walk) return;

        Vector3 move = _spriteAnimator.IsLeftFacing ? new Vector3(-1f,0,0) : new Vector3(1f,0,0);
        _rb.linearVelocity = _speed * move;
    }
}
