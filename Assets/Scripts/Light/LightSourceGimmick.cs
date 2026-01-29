using UnityEngine;

public class LightSourceGimmick : LightSourceBase,ILightAffectable,IInteractable
{
    [Header("参照")]
    [SerializeField] private GameObject _particle;
    [SerializeField] private GameObject _sprite;

    [Header("条件")]
    [SerializeField] private float _requiredLight = 0.8f;

    PlayerController _playerController;

    private float _currentLight;
    private bool _canInteract = false,_isLit = false;

    protected override void Awake()
    {
        base.Awake();
        ChangeLightRadius(0); // 最初は消灯
        _playerController = FindAnyObjectByType<PlayerController>();
    }

    private void LateUpdate()
    {
        if(_currentLight >= _requiredLight)
        {
            _canInteract = true;
            //後で演出追加したっていい
        }

        _currentLight = 0;
    }

    public void AddLight(float value, float direction)
    {
        _currentLight = Mathf.Max(_currentLight, value);
    }

    public void Interact()
    {
        if (!_canInteract || _isLit) return;

        TurnOn();
    }
    private void TurnOn()
    {
        _isLit = true;
        ChangeLightRadius(_maxRadius);
        _playerController.Recovery();

        // 見た目演出
        _particle.SetActive(true);
        _sprite.SetActive(true);
        Debug.Log("ランプが点灯！");
    }
}
