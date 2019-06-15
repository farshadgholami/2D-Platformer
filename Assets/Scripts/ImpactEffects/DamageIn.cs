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
            if (effectiveSides.Contains(Toolkit.VectorToSide(impactproperty.impactSide)))
            {
                if (impactproperty.impactEffect is DamageOut)
                {
                    impactproperty.impactEffect.EffectCheck(impactproperty.impactSide, gameObject);
                }
            }
        }
    }
}
