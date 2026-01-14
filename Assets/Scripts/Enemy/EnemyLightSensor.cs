using UnityEngine;
/// <summary>
/// 明度を感知して敵自身の明るさを変える
/// </summary>
public class EnemyLightSensor : MonoBehaviour,ILightAffectable
{
    [Header("参照")]
    [SerializeField, Tooltip("見た目")] private SpriteRenderer _sr;

    [Header("数値設定")]
    [SerializeField, Tooltip("最低明度")] private float _minBrightness = 0.2f;
    [SerializeField, Tooltip("最大明度")] private float _maxBrightness = 1.0f;
    [SerializeField, Tooltip("光が減衰する速度")] private float _decaySpeed = 1.5f;

    public float CurrentLight { get; private set; }
    public float BrightestRange { get; private set; }

    private float _nextLight,_nextDir;
    private void Awake()
    {
        _sr.color = new Color(_minBrightness, _minBrightness, _minBrightness, 1f);
    }
    private void Update()
    {
        if(_nextLight > CurrentLight)
        {
            CurrentLight = _nextLight;
            BrightestRange = _nextDir;
        }
        else
        {
            CurrentLight = Mathf.MoveTowards(
                CurrentLight,
                0f,
                _decaySpeed * Time.deltaTime
            );
        }

        float finalLight = Mathf.Clamp(CurrentLight, _minBrightness, _maxBrightness);
        _sr.color = new Color(finalLight, finalLight, finalLight, 1f);

        _nextLight = 0f;
    }

    public void AddLight(float value,float dir)
    {
        value = Mathf.Clamp(value, _minBrightness, _maxBrightness);
        if (value > CurrentLight)
        {
            _nextLight = value;
            _nextDir = dir;
        }
    }
}
