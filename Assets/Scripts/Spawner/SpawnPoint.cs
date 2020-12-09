using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private PatrolDirection patrolDirection;
    [SerializeField] private Side defualtMoveSide;
    [SerializeField] private float right;
    [SerializeField] private float left;
    [SerializeField] private float up;
    [SerializeField] private float down;

    public Side DefualtMoveSide => defualtMoveSide;

    private Transform _transform;
    private Vector2 UpPoint => DirectionPoint(Vector2.up, up);
    private Vector2 DownPoint => DirectionPoint(Vector2.down, down);
    private Vector2 LeftPoint => DirectionPoint(Vector2.left, left);
    private Vector2 RightPoint => DirectionPoint(Vector2.right, right);

    private Vector2 DirectionPoint(Vector2 direction, float value)
    {
        return value * direction + (Vector2) (_transform ? _transform.position : transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.color = Color.green;
        if (patrolDirection == PatrolDirection.Horizontal)
        {
            DrawSphere(LeftPoint);
            DrawSphere(RightPoint);
        }
        else if (patrolDirection == PatrolDirection.Vertical)
        {
            DrawSphere(UpPoint);
            DrawSphere(DownPoint);
        }
    }

    private void DrawSphere(Vector2 position)
    {
        Gizmos.DrawSphere(position, 0.05f);
    }

    private void Start()
    {
        _transform = transform;
        CreatPatrolPoints();
    }

    private void CreatPatrolPoints()
    {
        if (patrolDirection == PatrolDirection.Horizontal)
        {
            CreatPatrolPoint(LeftPoint, Side.Right);
            CreatPatrolPoint(RightPoint, Side.Left);
        }
        else if (patrolDirection == PatrolDirection.Vertical)
        {
            CreatPatrolPoint(UpPoint, Side.Down);
            CreatPatrolPoint(DownPoint, Side.Up);
        }
    }

    private void CreatPatrolPoint(Vector2 position, Side side)
    {
        var point = new GameObject("Patrol Point");
        point.layer = LayerMask.NameToLayer("invisable");

        var pointTransform = point.transform;
        pointTransform.position = position;
        pointTransform.SetParent(_transform);
        
        var col = point.AddComponent<BoxCollider2D>();
        col.size = Vector2.one;
        col.isTrigger = true;
        
        var patrollerDirection = point.AddComponent<PatrollerDirectionChange>();
        patrollerDirection.Side = side;
    }

    private enum PatrolDirection
    {
        Free,
        Horizontal,
        Vertical
    }
}
