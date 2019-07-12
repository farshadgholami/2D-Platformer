using System.Collections;
using System.Collections.Generic;
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
    protected override void Function()
    {
        gameObject.layer = LayerMask.NameToLayer("Void");
        CapGravitySpeed();
        CalculateMovment();
        CalculateState();
        CalculateHit();
        HitActionFunction();
        ResetCalculate();
        gameObject.layer = self_layer_mask_;
    }
    protected virtual void CalculateState()
    {
        if ((impactedSides - gravity.Direction) * gravity.Direction == Vector2.zero)
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
    protected override void MovementCheckDown()
    {
        float threshold = 0.01f;
        float leastDistance = -distance.y + threshold;
        for (int i = 0 ; i < raycastPointsY.Length ; i++)
        {
            hitPoint = Physics2D.Raycast((Vector2)transform.position - raycastPointsY[i] + (threshold * Vector2.down) , Vector2.down , leastDistance , fallLayerMask , 0 , 0);
            if (hitPoint.collider != null && hitPoint.distance <= leastDistance)
            {
                impactedSides.y = -1;
                leastDistance = hitPoint.distance;
                v_raycast_list_.Add(hitPoint);
            }
        }
        v_raycast_list_.RemoveAll(delegate (RaycastHit2D ray)
        {
            return ray.distance > leastDistance;
        });
        UpdateImpactProperties(v_raycast_list_ , Vector2.down);
        ApplyMovement(Vector2.down * leastDistance);
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
