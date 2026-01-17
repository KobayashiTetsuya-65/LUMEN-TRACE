using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("参照")]
    [SerializeField, Tooltip("ターゲット")] private Transform _target;
    [SerializeField, Tooltip("カメラの距離")] private Vector3 _offset;
    [SerializeField, Tooltip("プレイヤー")] private Transform _playerTr;

    [Header("数値設定")]
    [SerializeField, Tooltip("追従速度")] private float _followSpeed = 2f;

    private Transform _tr;
    private Vector3 _targetPos,_offsetLeft;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tr = transform;
        _offsetLeft = new Vector3(-_offset.x, _offset.y, _offset.z);
    }

    // Update is called once per frame
    void Update()
    {
        FollowObj();
    }
    public void FollowObj()
    {
        if (_playerTr.localScale.x < 0)
        {
            _targetPos = _target.position + _offsetLeft;
        }
        else
        {
            _targetPos = _target.position + _offset;
        }
            _tr.position = Vector3.Lerp(
                _tr.position, _targetPos, _followSpeed * Time.deltaTime);
    }
}
