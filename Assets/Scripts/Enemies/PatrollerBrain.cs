using UnityEngine;

public class PatrollerBrain : EnemyBrain
{
    protected Vector2 reservedDirection;
    private Vector2 savedReserevedDirection;

    protected override void Awake()
    {
        base.Awake();
        GameManager.SaveSceneAction += Save;
        GameManager.LoadSceneAction += Load;
        
        physic.Layer += LayerMask.GetMask("Player");
        physic.HitAction += HitCheck;
    }

    private void OnEnable()
    {
        ResetBrain();
    }

    protected override void ResetBrain()
    {
        base.ResetBrain();
        MovementCommand();
    }
    
    protected override void CalculateDirection()
    {
        moveDirection = Toolkit.SideToVector(direction);
        if(moveDirection != Vector2.right)
        {
            moveDirection = Vector2.left;
        }
    }

    private void Update()
    {
        if (targetAvailable)
        {
            if ((target - (Vector2)transform.position).magnitude <= 0.1f)
            {
                targetAvailable = false;
                moveDirection = reservedDirection;
                MovementCommand();
            }
        }
    }
    private void HitCheck()
    {
        if (physic.HasImpact(moveDirection))
        {
            moveDirection *= -1;
            targetAvailable = false;
            MovementCommand();
        }
    }

    private void MovementCommand()
    {
        if (!stats.IsDead)
            move.MoveStart(moveDirection);
    }
    private void Save()
    {
        savedMoveDirection = moveDirection;
        savedTarget = target;
        savedTargetStatus = targetAvailable;
        savedReserevedDirection = reservedDirection;
    }
    private void Load()
    {
        moveDirection = savedMoveDirection;
        target = savedTarget;
        targetAvailable = savedTargetStatus;
        reservedDirection = savedReserevedDirection;
        MovementCommand();
    }

    public virtual void SetTarget(Vector2 target, Vector2 reservedDirection)
    {
        if (reservedDirection == moveDirection)
            return;
        if ((reservedDirection * gravity.Direction) != Vector2.zero)
            return;

        Vector2 temp1 = target * Toolkit.Transpose2(Toolkit.Vector2Abs(gravity.Direction));
        Vector2 temp2 = transform.position * Toolkit.Vector2Abs(gravity.Direction);
        this.target = temp1 + temp2;

        this.reservedDirection = reservedDirection;
        targetAvailable = true;
    }
}
