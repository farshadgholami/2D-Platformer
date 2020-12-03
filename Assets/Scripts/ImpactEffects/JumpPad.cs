using System.Collections.Generic;
using UnityEngine;

public class JumpPad : ImpactEffect
{
    [SerializeField] private List<string> effectedTags;
    protected int lastJump;
    
    protected override void Effect(GameObject impacted)
    {
        if(!effectedTags.Contains(impacted.tag)) return;
        var jump = impacted.GetComponent<CharacterJump>();
        var stats = impacted.GetComponent<CharacterStats>();
        if (lastJump != Time.frameCount)
        {
            lastJump = Time.frameCount;
            stats.FeetState = FeetStateE.OnGround;
            jump.HitJump();
        }
    }
    protected override void ImpactCheck()
    {
        foreach (ImpactProperty impactproperty in physic.ImpactProperties)
        {
            if (!effectedTags.Contains(impactproperty.impactEffect.tag)) continue;
            if (effectiveSides.Contains(Toolkit.VectorToSide(impactproperty.impactSide))) return;
            Effect(impactproperty.impactEffect.gameObject);
        }
    }
}
