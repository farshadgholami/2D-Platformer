using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletPhysic physic;
    private float distance;
    private float currentDistance;
    private SpriteRenderer sprite;
    private TrailRenderer trail;
    private Animator animator;
    private Transform effect;
    // Start is called before the first frame update
    private void Start()
    {
        physic = GetComponent<BulletPhysic>();
        sprite = GetComponent<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();
        effect = transform.GetChild(0);
        Deactivate();
    }
    private void Activate()
    {
        sprite.enabled = true;
        physic.enabled = true;
        trail.enabled = true;
        physic.ResetForce();
    }
    private void Deactivate()
    {
        sprite.enabled = false;
        physic.enabled = false;
        trail.enabled = false;
        physic.ResetForce();
    }
    private void HideBullet()
    {
        sprite.enabled = false;
    }
    private void Reload()
    {
        animator.Play("Idle");
        Deactivate();
        BulletManager.Reload(this);
    }
    private void Expire()
    {
        animator.Play("Expire");
    }
    public void hit(RaycastHit2D hit)
    {
        if (hit.collider.GetComponent<CharacterStats>())
        {
            hit.collider.GetComponent<CharacterStats>().Health--;
        }
        Vector2 temp = hit.point - (Vector2)transform.position;
        effect.rotation = Quaternion.FromToRotation(Vector2.down, Toolkit.Side(temp));
        animator.Play("Explode");
    }
    public void DistanceCheck(float distance)
    {
        this.distance -= distance;
        if(this.distance <= 0)
        {
            physic.ResetForce();
            Expire();
        }
    }
    public void Shoot(Vector2 position, Vector2 direction, float speed, float distance)
    {
        this.distance = distance;
        transform.position = position;
        Activate();
        physic.AddForce(direction * speed * physic.Weight);
    }
}
