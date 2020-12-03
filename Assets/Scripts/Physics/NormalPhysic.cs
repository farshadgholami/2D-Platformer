using System.Collections.Generic;
using UnityEngine;

public class NormalPhysic : Physic
{
    [SerializeField] private LayerMask layer;
    protected override void Init()
    {
        self_layer_mask_ = gameObject.layer;
        collider2d = GetComponent<BoxCollider2D>();
        size = transform.localScale * collider2d.size;
        layerMask += layer.value;
        CalculateRayCastPoints();
        impactProperties = new List<ImpactProperty>();
        impacted_objects_ = new List<GameObject>();
        v_raycast_list_ = new List<RaycastHit2D>();
        h_raycast_list_ = new List<RaycastHit2D>();
    }
}
