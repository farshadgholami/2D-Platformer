using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    [SerializeField]
    protected List<Side> effectiveSides;

    protected Physic physic;
    private void Start()
    {
        Init();
    }
    protected virtual void Init()
    {
        physic = GetComponent<Physic>();
        if (physic)
        {
            physic.HitActionEffects += ImpactCheck;
        }
    }

    public void EffectCheck(Vector2 sideVector,GameObject impacted)
    {
        if (effectiveSides.Contains(Toolkit.VectorToSide(-sideVector)))
        {
            Effect(impacted);
        }
    }
    protected virtual void Effect(GameObject impacted)
    {

    }
    protected virtual void ImpactCheck()
    {

    }
}
public class ImpactProperty
{
    public ImpactEffect impactEffect;
    public Vector2 impactSide;
    public ImpactProperty(ImpactEffect impactEffect, Vector2 impactSide)
    {
        this.impactEffect = impactEffect;
        this.impactSide = impactSide;
    }
}
