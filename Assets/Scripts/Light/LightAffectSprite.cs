using UnityEngine;

public class LightAffectSprite : MonoBehaviour,ILightAffectable
{
    [Header("参照")]
    [SerializeField, Tooltip("見た目")] private SpriteRenderer _sr;

    [Header("数値設定")]
    [SerializeField, Tooltip("最低明度")] private float _minBrightness = 0.2f;
    [SerializeField, Tooltip("最大明度")] private float _maxBrightness = 1.0f;
    [SerializeField, Tooltip("明度の変化速度")] private float _decaySpeed = 1.5f;

    private float _currentLight;
    private float _nextLight;
    private void Start()
    {
        _sr.color = new Color(_minBrightness, _minBrightness, _minBrightness,1f);
    }
    void LateUpdate()
    {
        _currentLight = Mathf.MoveTowards(_currentLight,_nextLight,_decaySpeed * Time.deltaTime);

        float finalLight = Mathf.Clamp(_currentLight, _minBrightness, _maxBrightness);
        _sr.color = new Color(finalLight, finalLight, finalLight, 1f);

        _nextLight = 0;
    }
    public void AddLight(float value,float range)
    {
        value = Mathf.Clamp01(value);
        if (value > _nextLight)
        {
            _nextLight = value;
        }
    }
}
