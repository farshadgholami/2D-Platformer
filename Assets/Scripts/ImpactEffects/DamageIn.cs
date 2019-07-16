using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIn : ImpactEffect
{
    protected CharacterStats stats;

    protected override void Init()
    {
        base.Init();
        stats = GetComponent<CharacterStats>();
    }
    protected override void Effect(GameObject impacted)
    {
        if (!impacted.CompareTag(gameObject.tag))
            stats.Health--;
    }
    protected override void ImpactCheck()
    {
        foreach (ImpactProperty impactproperty in physic.ImpactProperties)
        {
            if (impactproperty.impactEffect is DamageOut)
            {
                if (effectiveSides.Contains(Toolkit.VectorToSide(impactproperty.impactSide)))
                {
                    impactproperty.impactEffect.EffectCheck(impactproperty.impactSide , gameObject);
                }
            }
        }
    }
}
