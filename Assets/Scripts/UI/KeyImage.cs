using UnityEngine;

public class KeyImage : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _fullDisplayDistance = 1f;
    [SerializeField] private float _fadeDistance = 1f;

    private SpriteRenderer _sr;
    private float _alpha,_distance,_center;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _center = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        _distance = Mathf.Abs(_player.position.x - _center);

        if(_distance > _fadeDistance + _fullDisplayDistance)
        {
            _alpha = 0f;
        }
        else if(_distance < _fullDisplayDistance)
        {
            _alpha = 1f;
        }
        else
        {
            _alpha = 1f - ((_distance - _fullDisplayDistance) / _fadeDistance);
        }

        _sr.color = new Color(1f, 1f, 1f, _alpha);
    }
}
