using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyIn : ImpactEffect
{
    private Lock attachedLock;

    protected override void Init()
    {
        base.Init();
        attachedLock = GetComponent<Lock>();
    }
    protected override void Effect(GameObject impacted)
    {
        if (attachedLock.State != LockState.Close)
            return;


        PlayerStats stats = impacted.GetComponent<PlayerStats>();
        if(attachedLock.Color == KeyColor.Red)
        {
            if(stats.RedKey > 0)
            {
                stats.RedKey--;
                attachedLock.Open();
            }
        }
        else if (attachedLock.Color == KeyColor.Yellow)
        {
            if (stats.YellowKey > 0)
            {
                stats.YellowKey--;
                attachedLock.Open();
            }
        }
        else if (attachedLock.Color == KeyColor.Blue)
        {
            if (stats.BlueKey > 0)
            {
                stats.BlueKey--;
                attachedLock.Open();
            }
        }
    }
}
