using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] private float pointOffset;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float right;
    [SerializeField] private float left;
    
    private Transform _transform;
    private Vector2 _startPoint;
    private Physic _physic;
    private bool _isRightTarget;

    private Vector2 LeftPoint => DirectionPoint(Vector2.left, left);
    private Vector2 RightPoint => DirectionPoint(Vector2.right, right);

    private Vector2 DirectionPoint(Vector2 direction, float value)
    {
        return value * direction + (_startPoint.Equals(Vector2.zero) ? (Vector2) (_transform ? _transform.position : transform.position) : _startPoint);
    }

    private void Start()
    {
        _physic = GetComponent<Physic>();
        _transform = transform;
        _startPoint = _transform.position;
        _transform.position = GetTargetPoint();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(LeftPoint, 0.05f);
        Gizmos.DrawSphere(RightPoint, 0.05f);
    }
    
    private void Update()
    {
        if (IsArrivedTarget()) _isRightTarget = !_isRightTarget;

        _physic.AddSpeed(GetTargetDirection().normalized * moveSpeed);
    }

    private Vector2 GetTargetDirection()
    {
        return GetTargetPoint() - (Vector2) _transform.position;
    }

    private bool IsArrivedTarget()
    {
        var a = Vector2.Distance(GetTargetPoint(), _transform.position);
        return a < pointOffset;
    }

    private Vector2 GetTargetPoint()
    {
        return _isRightTarget ? RightPoint : LeftPoint;
    }
}