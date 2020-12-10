using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformPhysic : NormalPhysic
{
    protected override void Function()
    {
        CalculateMovment();
        ResetCalculate();
    }
    
    protected override void CalculateMovment()
    {
        impactProperties.Clear();
        impacted_objects_.Clear();
        
        distance = (force + speed) / Weight * Time.deltaTime;

        if (distance.x > 0)
            MovementCheckRight(distance.x);
        else if (distance.x < 0) MovementCheckLeft(-distance.x);
    }
    
    protected override void MovementCheckRight(float distance)
    {
        HorizontalMove(distance, Vector2.right);
        ApplyMovement(Vector2.right * distance);
    }
    
    protected override void MovementCheckLeft(float distance)
    {
        HorizontalMove(distance, Vector2.left);
        ApplyMovement(Vector2.left * distance);
    }

    private void HorizontalMove(float distance, Vector2 direction)
    {
        CheckImpact(raycastPointsY, Vector2.up, collisionCheckDistance, layerMask);
        if (impact.Up) MoveSidewardHitObjects(impact.UpHits, direction, distance);
        
        CheckImpact(raycastPointsX, direction, distance, layerMask);
        if (impact.HasImpact(direction)) MoveHitObjects(impact.GetHits(direction), direction, distance);
    }
    
    private void MoveSidewardHitObjects(List<RaycastHit2D> hitObjects, Vector2 direction, float platformMovement)
    {
        foreach (var hitObject in hitObjects)
        {
            var physic = hitObject.collider.GetComponent<Physic>();
            physic.Move(direction * platformMovement);
        }
    }

    private void MoveHitObjects(List<RaycastHit2D> hitObjects, Vector2 direction, float platformMovement)
    {
        foreach (var hitObject in hitObjects)
        {
            var physic = hitObject.collider.GetComponent<Physic>();
            physic.Move(direction * (platformMovement));
        }
    }
}
