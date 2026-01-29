using UnityEngine;

public class FlowerLightGimmick : LightSourceBase, IInteractable
{
    [Header("éQè∆")]
    [SerializeField, Tooltip("ïœâªÇ∑ÇÈâÊëú")] private Sprite[] _flowerSprites;
    [SerializeField] private SpriteRenderer _sr;

    [Header("èåè")]
    [SerializeField] private float _requiredTime = 3f;

    [Header("ÇµÇ⁄Çﬁë¨Ç≥")]
    [SerializeField] private float _shrinkSpeed = 1f;

    [Header("ñæÇÈÇ≥ê›íË")]
    [SerializeField] private float _minIntensity = 0f;
    [SerializeField] private float _maxIntensity = 1.5f;

    PlayerController _playerController;

    private float _currentTime = 0f;
    private bool _isInteracting = false, _isBloomed = false;

    protected override void Awake()
    {
        base.Awake();
        ChangeLightRadius(0);
        _playerController = FindAnyObjectByType<PlayerController>();
    }
    protected override void Update()
    {
        base.Update();

        if (_isBloomed) return;

        if (_isInteracting)
        {
            _currentTime += Time.deltaTime;
        }
        else
        {
            _currentTime -= Time.deltaTime * _shrinkSpeed;
        }

        _currentTime = Mathf.Clamp(_currentTime, 0, _requiredTime);

        UpdateFlowerSprite();

        if (_currentTime >= _requiredTime)
        {
            Bloom();
        }

        _isInteracting = false;
    }

    public void Interact()
    {
        _isInteracting = true;
    }
    private void UpdateFlowerSprite()
    {
        float rate = _currentTime / _requiredTime;
        int index = Mathf.FloorToInt(rate * (_flowerSprites.Length - 1));
        if (_sr.sprite != _flowerSprites[index])
            AudioManager.Instance.PlaySe(SoundDataUtility.KeyConfig.Se.Flower);

        _sr.sprite = _flowerSprites[index];

        if (_light != null)
        {
            float intensity = Mathf.Lerp(_minIntensity, _maxIntensity, rate);
            _light.intensity = intensity;
        }

        ChangeLightRadius(Mathf.Lerp(_minRadius, _maxRadius, rate));
    }

    private void Bloom()
    {
        _isBloomed = true;
        ChangeLightRadius(_maxRadius);
        if (_light != null)
            _light.intensity = _maxIntensity;
        _playerController.Recovery();

        Debug.Log("â‘Ç™ñûäJÇ…Ç»Ç¡ÇΩÅI");
    }
}
