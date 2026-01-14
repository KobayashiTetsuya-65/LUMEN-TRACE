using System.IO.Compression;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpriteAnimator : MonoBehaviour
{
    [Header("参照")]
    [SerializeField, Tooltip("見た目の親オブジェクト")] private Transform _visualRoot;
    [SerializeField, Tooltip("見た目")] private SpriteRenderer _sr;

    [Header("画像")]
    [SerializeField, Tooltip("通常時")] private Sprite[] _idleSprites;
    [SerializeField, Tooltip("歩行時")] private Sprite[] _walkSprites;
    [SerializeField, Tooltip("攻撃時")] private Sprite[] _attackSprites;
    [SerializeField, Tooltip("回避時")] private Sprite[] _dodgeSprites;
    [SerializeField, Tooltip("潜伏時")] private Sprite[] _hideSprites;

    [Header("数値設定")]
    [SerializeField, Tooltip("通常時の間隔")] private float _idleFrame = 0.15f;
    [SerializeField, Tooltip("歩行時の間隔")] private float _walkFrame = 0.15f;
    [SerializeField, Tooltip("攻撃時の間隔")] private float _attackFrame = 0.15f;
    [SerializeField, Tooltip("回避時の間隔")] private float _dodgeFrame = 0.15f;
    [SerializeField, Tooltip("回避時の間隔")] private float _hideFrame = 0.15f;
    [SerializeField, Tooltip("スケール")] private float _scale = 3f;
    [SerializeField, Tooltip("一回目の攻撃判定フレーム")] private int _firstAttackFrame = 1;
    [SerializeField, Tooltip("二回目の攻撃判定フレーム")] private int _secondAttackFrame = 5;
    [SerializeField, Tooltip("攻撃判定の持続フレーム数")] private int _sustainedAttackFrame = 2;


    public bool IsRightFacing { get; private set; } = true;
    public bool IsFinishAction { get; private set; } = false;
    public bool IsEnterHide = true;

    private PlayerStateMachine _stateMachine;
    private PlayerController _controller;

    private PlayerState _prevState;
    private float _timer, _frameTime;
    private int _index;

    private void Awake()
    {
        _stateMachine = GetComponent<PlayerStateMachine>();
        _controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        Sprite[] sprites = GetSpritesByState();
        if(sprites == null || sprites.Length == 0) return;

        if (_stateMachine.CurrentState != _prevState)
        {
            _index = 0;
            _timer = 0;
            _prevState = _stateMachine.CurrentState;
        }

        _frameTime = GetTimeByState();

        _timer += Time.deltaTime;
        if(_timer >= _frameTime)
        {
            _sr.sprite = sprites[_index];
            _timer = 0;

            if(_stateMachine.CurrentState == PlayerState.Attack ||
                _stateMachine.CurrentState == PlayerState.Dodge)
            {
                if(_stateMachine.CurrentState == PlayerState.Attack)
                {
                    if(_index == _firstAttackFrame ||
                        _index == _secondAttackFrame)
                    {
                        _controller.Attack(true);
                    }
                    else if(_index == _firstAttackFrame + _sustainedAttackFrame ||
                        _index == _secondAttackFrame + _sustainedAttackFrame)
                    {
                        _controller.Attack(false);
                    }
                }
                _index++;
                if (_index >= sprites.Length)
                {
                    _index = 0;
                    FinishAction(true);
                    
                    return;
                }
            }
            else if(_stateMachine.CurrentState == PlayerState.Hide)
            {
                if (IsEnterHide)
                {
                    _index++;
                    if (_index >= _hideSprites.Length)
                    {
                        _index = _hideSprites.Length - 1;
                        return;
                    }
                }
                else
                {
                    _index--;
                    if(_index <= 0)
                    {
                        _index = 0;
                        FinishAction(true);
                        IsEnterHide = true;
                        return;
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
    /// プレイヤーの状態からアニメーションさせる画像を変化
    /// </summary>
    /// <returns></returns>
    private Sprite[] GetSpritesByState()
    {
        return _stateMachine.CurrentState switch
        {
            PlayerState.Idle => _idleSprites,
            PlayerState.Walk => _walkSprites,
            PlayerState.Attack => _attackSprites,
            PlayerState.Dodge => _dodgeSprites,
            PlayerState.Hide => _hideSprites,
            _ => null
        };
    }
    /// <summary>
    /// プレイヤーの状態によってアニメーションフレームを変化
    /// </summary>
    /// <returns></returns>
    private float GetTimeByState()
    {
        return _stateMachine.CurrentState switch
        {
            PlayerState.Idle => _idleFrame,
            PlayerState.Walk => _walkFrame,
            PlayerState.Attack => _attackFrame,
            PlayerState.Dodge => _dodgeFrame,
            PlayerState.Hide => _hideFrame,
            _ => 0.08f
        };
    }
    /// <summary>
    /// プレイヤーの向きを切り替える
    /// </summary>
    /// <param name="input"></param>
    public void ChangeSpriteFlipX(float input)
    {
        if(input > 0.01f)
        {
            IsRightFacing = true;
            _visualRoot.localScale = new Vector3(1f * _scale,_scale,_scale);
        }
        else if(input < -0.01f)
        {
            IsRightFacing = false;
            _visualRoot.localScale = new Vector3(-1f * _scale, _scale, _scale);
        }
    }
    /// <summary>
    /// アクション終了フラグ
    /// </summary>
    /// <param name="isFinish"></param>
    public void FinishAction(bool isFinish)
    {
        IsFinishAction = isFinish;
    }
}
