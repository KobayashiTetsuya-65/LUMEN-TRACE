using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, Tooltip("ターゲット")] private Transform _target;
    [SerializeField, Tooltip("カメラの距離")] private Vector3 _offset;
    [SerializeField, Tooltip("追従速度")] private float _followSpeed = 2f;

    private Transform _tr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        FollowObj();
    }
    public void FollowObj()
    {
        Vector3 targetPos = _target.position + _offset;
        _tr.position = Vector3.Lerp(
            _tr.position, targetPos,_followSpeed * Time.deltaTime);
    }
}
