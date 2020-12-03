using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollerBrain : MonoBehaviour
{
    [SerializeField]
    protected Side direction;

    protected Vector2 moveDirection;
    private CharacterStats stats;
    private CharacterPhysic physic;
    private CharacterMovement move;
    private Gravity gravity;

    protected Vector2 target;
    protected bool targetAvailable;
    protected Vector2 reservedDirection;

    private Vector2 savedMoveDirection;
    private Vector2 savedTarget;
    private Vector2 savedReserevedDirection;
    private bool savedTargetStatus;
    private void Awake()
    {
        Init();
        GameManager.SaveSceneAction += Save;
        GameManager.LoadSceneAction += Load;
    }

    private void Start()
    {
        stats = GetComponent<CharacterStats>();
        physic = GetComponent<CharacterPhysic>();
        move = GetComponent<CharacterMovement>();
        gravity = GetComponent<Gravity>();
        physic.Layer += LayerMask.GetMask("Player");
        physic.HitAction += HitCheck;
        MovementCommand();
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
    protected virtual void Init()
    {
        moveDirection = Toolkit.SideToVector(direction);
        if(moveDirection != Vector2.right)
        {
            moveDirection = Vector2.left;
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
