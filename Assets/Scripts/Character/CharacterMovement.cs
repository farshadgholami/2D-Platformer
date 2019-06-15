using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeedBase;
    [SerializeField]
    protected float moveSpeedMax;
    [SerializeField]
    protected float moveAcceleration;

    protected float moveSpeed;
    protected bool accelerate;
    protected float increment;
    protected CharacterStats stats;
    protected Physic physic;
    protected Gravity gravity;
    protected Vector2 side;
    // Use this for initialization
    private void Start()
    {
        Init();
        stats.DeathAction += ForceStop;
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
            physic.AddSpeed(moveSpeed * side);
        }
    }
    protected virtual void Init()
    {
        moveSpeed = moveSpeedBase + increment;
        stats = GetComponent<CharacterStats>();
        physic = GetComponent<Physic>();
        gravity = GetComponent<Gravity>();
    }
    public virtual void MoveStart(Vector2 direction)
    {
        if ((direction * gravity.Direction).Equals(Vector2.zero))
        {
            MoveReset();
            stats.BodyState = BodyStateE.Moveing;
            side = direction;
            stats.MoveSide = direction;
        }
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

    public float Increment { get { return increment; } set { increment = value; } }
}
