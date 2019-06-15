﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOut : ImpactEffect
{
    protected override void Effect(GameObject impacted)
    {
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
                    impactproperty.impactEffect.EffectCheck(impactproperty.impactSide, gameObject);
                }
            }
        }
    }
}
