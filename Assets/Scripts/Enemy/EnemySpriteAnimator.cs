using System.Collections;
using UnityEngine;
/// <summary>
/// 敵のアニメーション制御
/// </summary>
public class EnemySpriteAnimator : MonoBehaviour
{
    public bool IsLeftFacing { get; private set; } = true;
    public bool IsAttackFinished { get; private set; }

    [Header("参照")]
    [SerializeField, Tooltip("左右反転用の親オブジェクト")] private Transform _spriteRoot;
    [SerializeField, Tooltip("見た目")] private SpriteRenderer _sr;

    [Header("画像")]
    [SerializeField, Tooltip("通常時")] private Sprite[] _idleSprites;
    [SerializeField, Tooltip("歩行時")] private Sprite[] _walkSprites;
    [SerializeField, Tooltip("攻撃時")] private Sprite[] _attackSprites;
    [SerializeField, Tooltip("死亡時")] private Sprite[] _deadSprites;

    [Header("数値設定")]
    [SerializeField, Tooltip("通常時の間隔")] private float _idleFrame = 0.15f;
    [SerializeField, Tooltip("歩行時の間隔")] private float _walkFrame = 0.06f;
    [SerializeField, Tooltip("攻撃時の間隔")] private float _attackFrame = 0.08f;
    [SerializeField, Tooltip("死亡時の間隔")] private float _deadFrame = 0.15f;
    [SerializeField, Tooltip("ダメージエフェクト表示時間")] private float _damageDuration = 0.2f;
    [SerializeField, Tooltip("スケール")] private float _scale = 3f;
    [SerializeField, Tooltip("一回目の攻撃判定フレーム")] private int _firstAttackFrame = 4;
    [SerializeField, Tooltip("一回目の攻撃判定の持続フレーム数")] private int _sustainedFirstAttackFrame = 5;

    private AudioManager _audioManager;
    private EnemyStateMachine _stateMachine;
    private NormalEnemyController _controller;

    private EnemyState _prevState;
    private float _timer, _frameTime;
    private int _index;

    private void Awake()
    {
        _stateMachine = GetComponent<EnemyStateMachine>();
        _controller = GetComponentInChildren<NormalEnemyController>();
    }
    private void Start()
    {
        _audioManager = AudioManager.Instance;
    }
    // Update is called once per frame
    void Update()
    {
        Sprite[] sprites = GetSpritesByState();
        if (sprites == null || sprites.Length == 0) return;

        if (_stateMachine.CurrentState != _prevState)
        {
            _index = 0;
            _timer = 0;
            _prevState = _stateMachine.CurrentState;
        }

        _frameTime = GetTimeByState();

        _timer += Time.deltaTime;
        if (_timer >= _frameTime)
        {
            _sr.sprite = sprites[_index];
            _timer = 0;

            if (_stateMachine.CurrentState == EnemyState.Attack ||
                _stateMachine.CurrentState == EnemyState.Dead)
            {
                if(_stateMachine.CurrentState == EnemyState.Attack)
                {
                    if (_controller.IsDead)
                    {
                        _stateMachine.ChangeState(EnemyState.Dead);
                        _index = 0;
                        return;
                    }

                    if(_index == _firstAttackFrame)
                    {
                        _controller.Attack(true);
                    }
                    else if(_index == _firstAttackFrame + _sustainedFirstAttackFrame)
                    {
                        _controller.Attack(false);
                    }
                }
                _index++;
                if (_index >= sprites.Length)
                {
                    _index = 0;
                    if(_stateMachine.CurrentState == EnemyState.Attack)
                    {
                        IsAttackFinished = true;
                        return;
                    }
                    else
                    {
                        //死亡処理
                        _controller.Dead();
                    }
                }
            }
            else
            {
                _index = (_index + 1) % sprites.Length;
            }
        }
    }
    /// <summary>
    /// エネミーの状態からアニメーションさせる画像を変化
    /// </summary>
    /// <returns></returns>
    private Sprite[] GetSpritesByState()
    {
        return _stateMachine.CurrentState switch
        {
            EnemyState.Idle => _idleSprites,
            EnemyState.Walk => _walkSprites,
            EnemyState.Attack => _attackSprites,
            EnemyState.Dead => _deadSprites,
            _ => null
        };
    }
    /// <summary>
    /// エネミーの状態によってアニメーションフレームを変化
    /// </summary>
    /// <returns></returns>
    private float GetTimeByState()
    {
        return _stateMachine.CurrentState switch
        {
            EnemyState.Idle => _idleFrame,
            EnemyState.Walk => _walkFrame,
            EnemyState.Attack => _attackFrame,
            EnemyState.Dead => _deadFrame,
            _ => 0.08f
        };
    }
    /// <summary>
    /// エネミーの向きを切り替える
    /// </summary>
    /// <param name="input"></param>
    public void ChangeSpriteFlipX(bool isLeftFacing)
    {
        IsLeftFacing = isLeftFacing;

        float x = IsLeftFacing ? 1f : -1f;
        _spriteRoot.localScale = new Vector3(x * _scale, _scale, _scale);
    }
    public void ResetAttack()
    {
        IsAttackFinished = false;
    }
}
