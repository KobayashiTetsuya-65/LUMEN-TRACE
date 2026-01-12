using UnityEngine;

public abstract class LightSourceBase : MonoBehaviour
{
    [Header("åıåπê›íË")]
    [SerializeField] protected float _lightRadius = 10f;
    [SerializeField] protected float _lightPower = 1f;

    protected Transform _tr;
    protected virtual void Awake()
    {
        _tr = transform;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        EmitLight();
    }

    protected void EmitLight()
    {
        Collider[] targets = Physics.OverlapSphere(_tr.position, _lightRadius);

        foreach (Collider target in targets)
        {
            if (target.TryGetComponent<ILightAffectable>(out var lightingObj))
            {
                Transform targetPos = target.transform;
                float dist = 1f;
                float dir;

                if (_tr.position.x >= targetPos.position.x)
                {
                    dir = _tr.position.x - targetPos.position.x;
                    dist = Mathf.Clamp01(dir / _lightRadius);
                }
                else
                {
                    dir = targetPos.position.x - _tr.position.x;
                    dist = Mathf.Clamp01(dir / _lightRadius);
                }

                lightingObj.AddLight(1f - dist,dir);
            }
        }
    }
}
