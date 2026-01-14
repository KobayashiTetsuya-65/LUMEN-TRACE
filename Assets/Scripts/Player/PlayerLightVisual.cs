using UnityEngine;
/// <summary>
/// プレイヤーの足元ライトの状態管理
/// </summary>
public class PlayerLightVisual : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private Light _pointLight;

    [Header("Emission設定")]
    [SerializeField] private float _maxEmission = 2.5f;
    [SerializeField] private float _minEmission = 0.2f;
    [SerializeField] private float _visualSmoothSpeed = 8f;

    private float _currentEmission;
    private float _targetEmission;

    void Awake()
    {
        _currentEmission = _minEmission;
        _targetEmission = _minEmission;
    }

    void Update()
    {
        _currentEmission = Mathf.Lerp(
            _currentEmission,
            _targetEmission,
            Time.deltaTime * _visualSmoothSpeed
        );

        if (_pointLight != null)
        {
            _pointLight.intensity = _currentEmission;
        }
    }

    /// <summary>
    /// 外部から見た目の明るさを指定
    /// </summary>
    public void SetLightVisual(float normalized)
    {
        _targetEmission = Mathf.Lerp(
            _minEmission,
            _maxEmission,
            Mathf.Clamp01(normalized)
        );
    }
}
