using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyOut : ImpactEffect
{
    protected override void ImpactCheck()
    {
        foreach (ImpactProperty impactproperty in physic.ImpactProperties)
        {
            if (effectiveSides.Contains(Toolkit.VectorToSide(impactproperty.impactSide)))
            {
                if (impactproperty.impactEffect is KeyIn)
                {
                    impactproperty.impactEffect.EffectCheck(impactproperty.impactSide, gameObject);
                }
            }
        }
    }
}
