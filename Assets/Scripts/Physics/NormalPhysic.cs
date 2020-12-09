using System.Collections.Generic;
using UnityEngine;

public class NormalPhysic : Physic
{
    [SerializeField] private LayerMask layer;
    protected override void Init()
    {
        impact.Initial();
        self_layer_mask_ = gameObject.layer;
        size = transform.localScale * Collider.size;
        layerMask += layer.value;
        CalculateRayCastPoints();
        impactProperties = new List<ImpactProperty>();
        impacted_objects_ = new List<GameObject>();
    }
}
