using System;
using UnityEngine;

public class JumperBrain : EnemyBrain
{
    [SerializeField]
    private float visionRadius;
    [SerializeField]
    private float jumpDistance;
    [SerializeField]
    private float movementSpeedIncreas;

    private float currentJumpDistance;
    private int layerMask;
    private bool targetVisible;
    private CharacterJump jump;
    private bool hitGround;
    private bool savedTargetVisibilityStatus;
    private bool saveHitGround;

    protected override void Awake()
    {
        base.Awake();
        gravity = GetComponent<Gravity>();
        GameManager.SaveSceneAction += Save;
        GameManager.LoadSceneAction += Load;

        physic.Layer += LayerMask.GetMask("Player");
        layerMask = LayerMask.GetMask("Player", "Block");
    }

    private void OnEnable()
    {
        ResetBrain();
    }

    protected override void ResetBrain()
    {
        base.ResetBrain();
        targetVisible = false;
        hitGround = false;
        stats.MoveSide = moveDirection;
        stats.BodyState = BodyStateE.Laying;
        currentJumpDistance = jumpDistance;
    }

    protected override void CalculateDirection()
    {
        moveDirection = Toolkit.SideToVector(direction);
        if (moveDirection != Vector2.right)
        {
            moveDirection = Vector2.left;
        }
    }
    void Update()
    {
        if (stats.IsDead)
            return;
        targetVisible = false;
        Vision();
        Movement();
        Jump();
    }
    protected virtual void Vision()
    {
        if (stats.FeetState != FeetStateE.OnGround)
            return;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, visionRadius, layerMask, 0, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                if (Toolkit.IsVisible(transform.position, collider.transform.position, layerMask, collider))
                {
                    target = collider.transform.position;
                    target.y = transform.position.y;
                    targetAvailable = true;
                    targetVisible = true;
                    moveDirection = (target - (Vector2)transform.position).normalized;
                }
            }
        }
    }
    protected void Movement()
    {
        if (hitGround)
        {
            if (stats.BodyState == BodyStateE.Moveing)
            {
                move.ForceStop();
            }
            return;
        }
        if (!targetAvailable)
            return;


        target.y = transform.position.y;
        stats.MoveSide = moveDirection;
        if (stats.BodyState == BodyStateE.Idle || stats.BodyState == BodyStateE.Moveing)
        {
            move.MoveStart(moveDirection);
        }
        if (stats.BodyState == BodyStateE.Laying)
        {
            stats.BodyState = BodyStateE.StandingUp;
        }
        if ((target - (Vector2)transform.position).magnitude < 0.1f)
        {
            move.ForceStop();
        }
    }
    private void Jump()
    {
        if (hitGround)
            return;
        if ((target - (Vector2)transform.position).magnitude < currentJumpDistance)
        {
            if (targetVisible)
            {
                if (stats.FeetState == FeetStateE.OnGround && (stats.BodyState == BodyStateE.Idle || stats.BodyState == BodyStateE.Moveing))
                {
                    stats.BodyState = BodyStateE.ChargeJump;
                }
            }
        }
    }
    public void Charged()
    {
        currentJumpDistance = 0;
        stats.BodyState = BodyStateE.Idle;
        move.Increment = movementSpeedIncreas;
        jump.StartJumpUp();
    }
    public bool HitGround
    {
        get { return hitGround; }
        set
        {
            hitGround = value;
            if (value)
            {
                move.Increment = 0;
                currentJumpDistance = jumpDistance;
            }
        }
    }
    private void Save()
    {
        savedMoveDirection = moveDirection;
        savedTarget = target;
        savedTargetStatus = targetAvailable;
        saveHitGround = hitGround;
        savedTargetVisibilityStatus = targetVisible;

    }
    private void Load()
    {
        moveDirection = savedMoveDirection;
        target = savedTarget;
        targetAvailable = savedTargetStatus;
        hitGround = saveHitGround;
        targetVisible = savedTargetVisibilityStatus;
    }
}
