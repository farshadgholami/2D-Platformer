using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyOut : ImpactEffect
{
    protected override void ImpactCheck()
    {
        foreach (ImpactProperty impactproperty in physic.ImpactProperties)
        {

            if (impactproperty.impactEffect is KeyIn)
            {
                if (effectiveSides.Contains(Toolkit.VectorToSide(impactproperty.impactSide)))
                {
                    impactproperty.impactEffect.EffectCheck(impactproperty.impactSide , gameObject);
                }
            }
        }
    }
}
