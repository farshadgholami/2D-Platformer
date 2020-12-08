using System;
using UnityEngine;

public class CharacterPhysic : Physic
{
    protected CharacterStats stats;
    protected Gravity gravity;
    protected int fallLayerMask;
    protected override void Init()
    {
        base.Init();
        stats = GetComponent<CharacterStats>();
        gravity = GetComponent<Gravity>();
        fallLayerMask = layerMask + LayerMask.GetMask("Bridge");
        stats.DeathAction += TurnOff;
    }

    private void OnDrawGizmos()
    {
        if (raycastPointsY == null || raycastPointsY.Length == 0) return;
        foreach (var point in raycastPointsY)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere((Vector2) transform.position - point, 0.01f);
        }
    }

    protected override void Function()
    {
        gameObject.layer = LayerMask.NameToLayer("Void");
        CapGravitySpeed();
        CalculateMovment();
        CheckCollisionEnter();
        CalculateState();
        CalculateHit();
        HitActionFunction();
        ResetCalculate();
        gameObject.layer = self_layer_mask_;
    }
    
    protected virtual void CalculateState()
    {
        if (impact.HasImpact(gravity.Direction))
        {
            if (stats.FeetState != FeetStateE.OnGround)
            {
                stats.HitGround = true;
            }
            else
            {
                stats.HitGround = false;
            }
            stats.FeetState = FeetStateE.OnGround;
        }
        else if (impact.Left) stats.FeetState = FeetStateE.OnLeftWall;
        else if (impact.Right) stats.FeetState = FeetStateE.OnRightWall;
        else
        {
            Vector2 moveDirection = ((distance) * gravity.Direction).normalized;
            if (moveDirection == Toolkit.Vector2Abs(gravity.Direction))
            {
                stats.FeetState = FeetStateE.Falling;
            }
            else
            {
                stats.FeetState = FeetStateE.Jumping;
            }
        }
    }
    
    protected override void MovementCheckDown(float distance)
    {
        MoveToDirection(raycastPointsY, Vector2.down, distance, 0.01f, fallLayerMask);
    }
    
    protected override void CheckCollisionEnter()
    {
        if (!impact.Left) CheckImpact(raycastPointsX, Vector2.left, collisionCheckDistance, layerMask);
        if (!impact.Right) CheckImpact(raycastPointsX, Vector2.right, collisionCheckDistance, layerMask);
        if (!impact.Up) CheckImpact(raycastPointsY, Vector2.up, collisionCheckDistance, layerMask);
        if (!impact.Down) CheckImpact(raycastPointsY, Vector2.down, collisionCheckDistance, fallLayerMask);
        
        NotifyOnCollision();
    }
    
    public virtual void JumpDownLayerFix(bool on)
    {
        if (on)
            fallLayerMask = layerMask;
        else
            fallLayerMask = layerMask + LayerMask.GetMask("Bridge");
    }
    protected virtual void TurnOff()
    {
        enabled = false;
        collider2d.enabled = false;
    }
    protected override void Load()
    {
        base.Load();
        if (!stats.IsDead)
        {
            enabled = true;
            collider2d.enabled = true;
        }
    }
}
