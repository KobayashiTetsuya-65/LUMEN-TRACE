using UnityEngine;

public class PlayerSpriteAnimator : MonoBehaviour
{
    [Header("参照")]
    [SerializeField, Tooltip("見た目")] private SpriteRenderer _sr;

    [Header("画像")]
    [SerializeField, Tooltip("通常時")] private Sprite[] _idleSprites;
    [SerializeField, Tooltip("歩行時")] private Sprite[] _walkSprites;
    [SerializeField, Tooltip("攻撃時")] private Sprite[] _attackSprites;
    [SerializeField, Tooltip("回避時")] private Sprite[] _dodgeSprites;

    [Header("数値設定")]
    [SerializeField, Tooltip("差し替え間隔")] private float _frameTime = 0.15f;

    private PlayerStateMachine _stateMachine;

    private PlayerState _prevState;
    private float _timer;
    private int _index;

    private void Awake()
    {
        _stateMachine = GetComponent<PlayerStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_stateMachine.CurrentState != _prevState)
        {
            _index = 0;
            _timer = 0;
            _prevState = _stateMachine.CurrentState;
        }

        Sprite[] sprites = GetSpritesByState();
        if(sprites == null || sprites.Length == 0) return;

        _timer += Time.deltaTime;
        if(_timer >= _frameTime)
        {
            _sr.sprite = sprites[_index];
            _timer = 0;

            if(_stateMachine.CurrentState == PlayerState.Attack ||
                _stateMachine.CurrentState == PlayerState.Dodge)
            {
                _index++;
                if (_index >= sprites.Length)
                {
                    _stateMachine.ChangeState(PlayerState.Idle);
                    return;
                }
            }
            else
            {
                _index = (_index + 1) % sprites.Length;
            }
        }
    }
    private Sprite[] GetSpritesByState()
    {
        return _stateMachine.CurrentState switch
        {
            PlayerState.Idle => _idleSprites,
            PlayerState.Walk => _walkSprites,
            PlayerState.Attack => _attackSprites,
            PlayerState.Dodge => _dodgeSprites,
            _ => null
        };
    }
    /// <summary>
    /// プレイヤーの向きを切り替える
    /// </summary>
    /// <param name="input"></param>
    public void ChangeSpriteFlipX(float input)
    {
        if(input > 0.01f)
            _sr.flipX = false;
        else if(input < -0.01f)
            _sr.flipX = true;
    }
}
