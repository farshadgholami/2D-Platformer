using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperBrain : MonoBehaviour
{
    [SerializeField]
    private Side direction;
    [SerializeField]
    private float visionRadius;
    [SerializeField]
    private float jumpDistance;
    [SerializeField]
    private float movementSpeedIncreas;
    private float currentJumpDistance;


    private CharacterStats stats;
    private CharacterPhysic physic;
    private CharacterMovement move;
    private CharacterJump jump;
    private Gravity gravity;

    protected int layerMask;

    protected Vector2 moveDirection;
    protected Vector2 target;
    protected bool targetAvailable;
    protected bool targetVisible;
    private bool hitGround;

    private Vector2 savedMoveDirection;
    private Vector2 savedTarget;
    private bool savedTargetStatus;
    private bool savedTargetVisibilityStatus;
    private bool saveHitGround;

    private void Awake()
    {
        GameManager.SaveSceneAction += Save;
        GameManager.LoadSceneAction += Load;
    }
    void Start()
    {
        Init();
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
    protected virtual void Init()
    {
        currentJumpDistance = jumpDistance;
        moveDirection = Toolkit.SideToVector(direction);
        if (moveDirection != Vector2.right)
        {
            moveDirection = Vector2.left;
        }
        stats = GetComponent<CharacterStats>();
        physic = GetComponent<CharacterPhysic>();
        move = GetComponent<CharacterMovement>();
        jump = GetComponent<CharacterJump>();
        gravity = GetComponent<Gravity>();
        physic.Layer += LayerMask.GetMask("Player");
        stats.MoveSide = moveDirection;
        layerMask = LayerMask.GetMask("Player", "Block");
        stats.BodyState = BodyStateE.Laying;
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
        jump.JumpStart();
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
