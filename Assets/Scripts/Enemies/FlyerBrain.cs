using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerBrain : PatrollerBrain
{
    public override void SetTarget(Vector2 target, Vector2 reservedDirection)
    {
        if (reservedDirection == moveDirection)
            return;

        Vector2 temp1 = transform.position * Toolkit.Transpose2(Toolkit.Vector2Abs(moveDirection));
        Vector2 temp2 = target * Toolkit.Vector2Abs(moveDirection);
        this.target = temp1 + temp2;
        this.reservedDirection = reservedDirection;
        targetAvailable = true;
    }
    protected override void Init()
    {
        moveDirection = Toolkit.SideToVector(direction);
    }
}
