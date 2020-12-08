using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] private Vector2[] pathPoints;
    [SerializeField] private float pointOffset;
    [SerializeField] private bool isPingPong;
    [SerializeField] private float moveSpeed;
    
    private Transform _transform;
    private int _pointIndex;
    private bool _isClockWise = true;
    private int _pingPongTargetIndex;
    private Physic _physic;
    private Vector2 _startPosition;

    private void Start()
    {
        _physic = GetComponent<Physic>();
        _transform = transform;
        _startPosition = _transform.localPosition;
        _transform.localPosition = GetTargetPoint();
        _pingPongTargetIndex = pathPoints.Length - 1;
    }

    private void OnDrawGizmosSelected()
    {
        if (pathPoints.Length == 0) return;
        Gizmos.color = Color.green;
        foreach (var point in pathPoints) Gizmos.DrawSphere(point + (_startPosition.Equals(Vector2.zero) ? (Vector2) transform.localPosition : _startPosition), 0.05f);
    }
    
    private void Update()
    {
        if (IsArrivedTarget()) CalculateNextPointIndex();

        var speed = GetTargetDirection().normalized * moveSpeed;
        _physic.AddSpeed(speed);
    }

    private Vector2 GetTargetDirection()
    {
        return GetTargetPoint() - (Vector2) _transform.localPosition;
    }

    private bool IsArrivedTarget()
    {
        var a = Vector2.Distance(GetTargetPoint(), _transform.localPosition);
        //print($"Distance {a} Point Index {_pointIndex}");
        return a < pointOffset;
    }

    private Vector2 GetTargetPoint()
    {
        return _startPosition + pathPoints[_pointIndex];
    }

    private void CalculateNextPointIndex()
    {
        if (isPingPong)
            CalcuteNextPointIndexPingPong();
        else
        {
            _pointIndex++;
            if (_pointIndex == pathPoints.Length) _pointIndex = 0;
        }
    }

    private void CalcuteNextPointIndexPingPong()
    {
        if (_pointIndex == _pingPongTargetIndex)
        {
            _isClockWise = !_isClockWise;
            _pingPongTargetIndex = _isClockWise ? pathPoints.Length - 1 : 0;
        }
        _pointIndex = _isClockWise ? _pointIndex + 1 : _pointIndex - 1;
    }
}