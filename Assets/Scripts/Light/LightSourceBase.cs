using UnityEngine;

public abstract class LightSourceBase : MonoBehaviour
{
    [Header("éQè∆")]
    [SerializeField] protected Light _light;

    [Header("åıåπê›íË")]
    [SerializeField] protected float _maxRadius = 10f;
    [SerializeField] protected float _minRadius = 3f;
    [SerializeField] protected float _lightRadius = 10f;
    [SerializeField] protected float _lightPower = 1f;
    [SerializeField] protected float _radiusChangeSpeed = 5f;

    protected Transform _tr;
    protected float _targetRadius;
    protected virtual void Awake()
    {
        _tr = transform;
        _targetRadius = _lightRadius;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        _lightRadius = Mathf.Lerp(
            _lightRadius,
            _targetRadius,
            Time.deltaTime * _radiusChangeSpeed
        );

        EmitLight();
    }
    protected virtual void FixedUpdate()
    {
        
    }

    protected void EmitLight()
    {
        Collider[] targets = Physics.OverlapSphere(_tr.position, _lightRadius);

        foreach (Collider target in targets)
        {
            if (target.TryGetComponent<ILightAffectable>(out var lightingObj))
            {
                Transform targetPos = target.transform;
                float dir = Mathf.Abs(_tr.position.x - targetPos.position.x);
                float dist = Mathf.Clamp01(dir / _lightRadius);
                lightingObj.AddLight(1f - dist,dir);
            }
        }
    }
    protected void ChangeLightRadius(float radius)
    {
        radius = Mathf.Clamp(radius,_minRadius,_maxRadius);
        _targetRadius = radius;
    }
}
