using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformPhysic : NormalPhysic
{
    protected override void Function()
    {
        CalculateMovment();
        ResetCalculate();
    }
    
    private void OnDrawGizmos()
    {
        if (raycastPointsY == null || raycastPointsY.Length == 0) return;
        foreach (var point in raycastPointsY)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere((Vector2) transform.position + point, 0.01f);
        }
    }
    
    protected override void CalculateMovment()
    {
        impactProperties.Clear();
        impacted_objects_.Clear();
        
        distance = (force + speed) / Weight * Time.deltaTime;
        
        if (distance.y > 0)
            MovementCheckUp(distance.y);
        else if (distance.y < 0) MovementCheckDown(-distance.y);
        
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
        if (this.distance.y <= 0) CheckImpact(raycastPointsY, Vector2.up, collisionCheckDistance, layerMask);
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
    
    protected override void MovementCheckUp(float distance)
    {
        VerticalMove(distance, Vector2.up);
        ApplyMovement(Vector2.up * distance);
    }

    protected override void MovementCheckDown(float distance)
    {
        VerticalMove(distance, Vector2.down);
        ApplyMovement(Vector2.down * distance);
    }

    private void VerticalMove(float distance, Vector2 direction)
    {
        CheckImpact(raycastPointsY, direction, distance, layerMask);
        if (impact.HasImpact(direction)) MoveHitObjects(impact.GetHits(direction), direction, distance);
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
