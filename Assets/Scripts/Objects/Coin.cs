using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    [SerializeField]
    private int value;

    protected override void PickUpFuntion(PlayerStats stats)
    {
        base.PickUpFuntion(stats);
        stats.Points += value;
    }
}
