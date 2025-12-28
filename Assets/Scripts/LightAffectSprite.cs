using UnityEngine;

public class LightAffectSprite : MonoBehaviour,ILightAffectable
{
    [Header("参照")]
    [SerializeField, Tooltip("見た目")] private SpriteRenderer _sr;

    [Header("数値設定")]
    [SerializeField, Tooltip("最低明度")] private float _minBrightness = 0.2f;
    [SerializeField, Tooltip("最大明度")] private float _maxBrightness = 1.0f;

    private float _current = 0f;
    private void Start()
    {
        _sr.color = new Color(_minBrightness, _minBrightness, _minBrightness,1f);
    }
    public void SetLightPower(float lightValue)
    {
        lightValue = Mathf.Clamp(lightValue, _minBrightness, _maxBrightness);
        _sr.color = new Color(lightValue, lightValue, lightValue, 1f);
    }
}
