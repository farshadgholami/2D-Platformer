using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagOutPlayer : DamageOut
{
    protected int lastJump;
    private CharacterJump jump;
    private CharacterStats stats;
    protected override void Init()
    {
        base.Init();
        jump = GetComponent<CharacterJump>();
        stats = GetComponent<CharacterStats>();
    }
    protected override void Effect(GameObject impacted)
    {
        if (lastJump != Time.frameCount)
        {
            lastJump = Time.frameCount;
            stats.FeetState = FeetStateE.OnGround;
            jump.HitJump();
        }
        if (!impacted.CompareTag(gameObject.tag))
            impacted.GetComponent<CharacterStats>().Health--;
    }
    protected override void ImpactCheck()
    {
        foreach (ImpactProperty impactproperty in physic.ImpactProperties)
        {
            if (effectiveSides.Contains(Toolkit.VectorToSide(impactproperty.impactSide)))
            {
                if (impactproperty.impactEffect is DamageIn)
                {
                    jump.HitJump();
                    impactproperty.impactEffect.EffectCheck(impactproperty.impactSide, gameObject);
                }
            }
        }
    }
}
