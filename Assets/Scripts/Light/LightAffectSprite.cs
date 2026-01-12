using UnityEngine;

public class LightAffectSprite : MonoBehaviour,ILightAffectable
{
    [Header("参照")]
    [SerializeField, Tooltip("見た目")] private SpriteRenderer _sr;

    [Header("数値設定")]
    [SerializeField, Tooltip("最低明度")] private float _minBrightness = 0.2f;
    [SerializeField, Tooltip("最大明度")] private float _maxBrightness = 1.0f;

    private float _currentLight;
    private void Start()
    {
        _sr.color = new Color(_minBrightness, _minBrightness, _minBrightness,1f);
    }
    void LateUpdate()
    {
        float finalLight = Mathf.Clamp(_currentLight, _minBrightness, _maxBrightness);
        _sr.color = new Color(finalLight, finalLight, finalLight, 1f);

        _currentLight = 0;
    }
    public void AddLight(float value,float range)
    {
        value = Mathf.Clamp(value, _minBrightness, _maxBrightness);
        _currentLight += value;

    }
}
