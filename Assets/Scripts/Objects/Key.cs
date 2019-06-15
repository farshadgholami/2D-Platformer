using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    [SerializeField]
    private KeyColor color;
    // Start is called before the first frame update
    protected override void PickUpFuntion(PlayerStats stats)
    {
        base.PickUpFuntion(stats);
        if(color == KeyColor.Yellow)
        {
            stats.YellowKey++;
        }
        else if(color == KeyColor.Red)
        {
            stats.RedKey++;
        }
        else if (color == KeyColor.Blue)
        {
            stats.BlueKey++;
        }
    }
}
public enum KeyColor { Yellow, Red, Blue }
