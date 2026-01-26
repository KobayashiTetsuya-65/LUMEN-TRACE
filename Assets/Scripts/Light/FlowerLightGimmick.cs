using UnityEngine;

public class FlowerLightGimmick : LightSourceBase, ILightAffectable, IInteractable
{
    [Header("éQè∆")]
    [SerializeField, Tooltip("ïœâªÇ∑ÇÈâÊëú")] private Sprite[] _flowerSprites;
    [SerializeField] private SpriteRenderer _sr;

    [Header("èåè")]
    [SerializeField] private float _requiredTime = 3f;

    [Header("ÇµÇ⁄Çﬁë¨Ç≥")]
    [SerializeField] private float _shrinkSpeed = 1f;

    private float _currentTime = 0f;
    private bool _isInteracting = false, _isBloomed = false;

    protected override void Awake()
    {
        base.Awake();
        ChangeLightRadius(0);
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

    public void AddLight(float value, float direction)
    {

    }

    public void Interact()
    {
        _isInteracting = true;
        Debug.Log("kaika");
    }
    private void UpdateFlowerSprite()
    {
        float rate = _currentTime / _requiredTime;
        int index = Mathf.FloorToInt(rate * (_flowerSprites.Length - 1));
        _sr.sprite = _flowerSprites[index];
    }

    private void Bloom()
    {
        _isBloomed = true;
        ChangeLightRadius(_maxRadius);
        Debug.Log("â‘Ç™ñûäJÇ…Ç»Ç¡ÇΩÅI");
    }
}
