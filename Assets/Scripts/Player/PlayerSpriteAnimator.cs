using UnityEngine;

public class PlayerSpriteAnimator : MonoBehaviour
{
    [Header("QÆ")]
    [SerializeField, Tooltip("Œ©‚½–Ú")] private SpriteRenderer _sr;

    [Header("‰æ‘œ")]
    [SerializeField, Tooltip("’Êí")] private Sprite[] _idleSprites;
    [SerializeField, Tooltip("•às")] private Sprite[] _walkSprites;
    [SerializeField, Tooltip("UŒ‚")] private Sprite[] _attackSprites;
    [SerializeField, Tooltip("‰ñ”ğ")] private Sprite[] _dodgeSprites;

    [Header("”’lİ’è")]
    [SerializeField, Tooltip("’Êí‚ÌŠÔŠu")] private float _idleFrame = 0.15f;
    [SerializeField, Tooltip("•às‚ÌŠÔŠu")] private float _walkFrame = 0.15f;
    [SerializeField, Tooltip("UŒ‚‚ÌŠÔŠu")] private float _attackFrame = 0.15f;
    [SerializeField, Tooltip("‰ñ”ğ‚ÌŠÔŠu")] private float _dodgeFrame = 0.15f;

    public bool _isRightFacing { get; private set; } = true;

    private PlayerStateMachine _stateMachine;

    private PlayerState _prevState;
    private float _timer, _frameTime;
    private int _index;

    private void Awake()
    {
        _stateMachine = GetComponent<PlayerStateMachine>();
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
                _index++;
                if (_index >= sprites.Length)
                {
                    _index = 0;
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
    private float GetTimeByState()
    {
        return _stateMachine.CurrentState switch
        {
            PlayerState.Idle => _idleFrame,
            PlayerState.Walk => _walkFrame,
            PlayerState.Attack => _attackFrame,
            PlayerState.Dodge => _dodgeFrame,
            _ => 0.1f
        };
    }
    /// <summary>
    /// ƒvƒŒƒCƒ„[‚ÌŒü‚«‚ğØ‚è‘Ö‚¦‚é
    /// </summary>
    /// <param name="input"></param>
    public void ChangeSpriteFlipX(float input)
    {
        if(input > 0.01f)
            _sr.flipX = false;
        else if(input < -0.01f)
            _sr.flipX = true;

        _isRightFacing = !_sr.flipX;
    }
}
