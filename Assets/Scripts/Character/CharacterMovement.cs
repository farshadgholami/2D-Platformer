﻿using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeedBase;
    [SerializeField]
    protected float moveSpeedMax;
    [SerializeField]
    protected float moveAcceleration;
    [SerializeField]
    private bool hasSprint;
    [SerializeField]
    private float sprintSpeedMultiplier;

    protected float moveSpeed;
    protected bool accelerate;
    protected float increment;
    protected CharacterStats stats;
    protected Physic physic;
    protected Gravity gravity;
    protected Vector2 side;

    public bool IsRun { get; set; }

    public bool HasSprint
    {
        get => hasSprint;
        set => hasSprint = value;
    }

    private void Awake()
    {
        Init();
        stats.DeathAction += ForceStop;
    }
    protected virtual void Init()
    {
        stats = GetComponent<CharacterStats>();
        physic = GetComponent<Physic>();
        gravity = GetComponent<Gravity>();
    }

    private void OnEnable()
    {
        moveSpeed = moveSpeedBase + increment;
    }

    private void Update()
    {
        Function();
    }
    protected virtual void Function()
    {
        if (stats.BodyState == BodyStateE.Moveing)
        {
            if (accelerate && stats.FeetState == FeetStateE.OnGround)
            {
                moveSpeed += moveAcceleration * Time.deltaTime;
                if (moveSpeed >= moveSpeedMax)
                {
                    moveSpeed = moveSpeedMax;
                    accelerate = false;
                }
                stats.SpeedMult = moveSpeed / moveSpeedBase;
            }
            physic.AddSpeed(moveSpeed * (IsRun && hasSprint ? sprintSpeedMultiplier : 1) * side);
        }
    }

    public virtual void MoveStart(Vector2 direction)
    {
        if (!(direction * gravity.Direction).Equals(Vector2.zero)) return;
        if (stats.BodyState != BodyStateE.Moveing)
        {
            MoveReset();
            stats.BodyState = BodyStateE.Moveing;
            SetMovementSide(direction);
        }
        else if (!side.Equals(direction)) SetMovementSide(direction);
    }

    private void SetMovementSide(Vector2 direction)
    {
        side = direction;
        stats.MoveSide = direction;
    }
    
    public virtual void MoveStop(Vector2 direction)
    {
        if (direction == side)
        {
            stats.SpeedMult = 1;
            stats.BodyState = BodyStateE.Idle;
        }
    }
    public virtual void ForceStop()
    {
        stats.SpeedMult = 1;
        stats.BodyState = BodyStateE.Idle;
    }
    public virtual void MoveReset()
    {
        moveSpeed = moveSpeedBase + increment;
    }
    public virtual void Stop()
    {
        MoveReset();
        stats.BodyState = BodyStateE.Idle;
    }

    public bool Accelerate
    {
        get => accelerate;
        set => accelerate = value;
    }

    public float Increment { get { return increment; } set { increment = value; } }
}
