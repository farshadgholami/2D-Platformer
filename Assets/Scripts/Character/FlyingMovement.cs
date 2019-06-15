using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMovement : CharacterMovement
{
    public override void MoveStart(Vector2 direction)
    {
        MoveReset();
        stats.BodyState = BodyStateE.Moveing;
        side = direction;
        stats.MoveSide = direction;
    }
}
