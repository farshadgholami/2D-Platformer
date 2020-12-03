using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysic : Physic
{
    private float radius;
    private CircleCollider2D circleCollider;
    private RaycastHit2D hit;
    private Bullet bullet;

    protected override void Init()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        radius = transform.localScale.x * circleCollider.radius;
        layerMask = LayerMask.GetMask("Block","Enemy");
        bullet = GetComponent<Bullet>();
    }
    protected override void Function()
    {
        CalculateMovment();
        CheckCollisionEnter();
        CalculateHit();
    }
    protected override void CalculateMovment()
    {
        distance = force / Weight * Time.deltaTime;
        if (distance.magnitude <= 0)
            return;

        hit = Physics2D.CircleCast(transform.position, radius, distance.normalized,distance.magnitude, layerMask, 0, 0);
        if (hit.collider)
        {
            transform.position += hit.distance * (Vector3) distance.normalized;
            bullet.hit(hit);
        }
        else
        {
            transform.position += (Vector3)distance;
            bullet.DistanceCheck(distance.magnitude);
        }
    }
    protected override void CalculateHit()
    {
        if (hit.collider)
        {
            force = Vector2.zero;
        }
    }
    protected override void Load()
    {
       
    }
    protected override void Save()
    {
        
    }
}
