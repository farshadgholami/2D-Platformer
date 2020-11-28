using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatManGraphics : MonoBehaviour
{
    private CharacterStats stats;
    private Animator bodyAnimator;
    private Animator handAnimator;
    private SpriteRenderer handSprite;
    private Vector2 side;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponentInParent<CharacterStats>();
        bodyAnimator = transform.GetChild(0).GetComponent<Animator>();
        handAnimator = transform.GetChild(1).GetComponent<Animator>();
        handSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        side = stats.MoveSide;
        if (stats.IsDead)
        {
            Death();
            return;
        }
        if (stats.FeetState == FeetStateE.Jumping)
        {
            Jump();
        }
        else if (stats.FeetState == FeetStateE.Falling)
        {
            Fall();
        }
        else if (stats.FeetState == FeetStateE.OnGround)
        {
            if (stats.BodyState == BodyStateE.Moveing)
            {
                Movement();
            }
            else
            {
                Idle();
            }
        }
        Hand();
    }
    private void Movement()
    {
        if (side == Vector2.right)
        {
            bodyAnimator.Play("Run-Right");
            handSprite.flipX = false;
        }
        else if (side == Vector2.left)
        {
            bodyAnimator.Play("Run-Left");
            handSprite.flipX = true;
        }
        if(stats.HandState == HandStateE.Idle)
        {
            handAnimator.Play("Run", 0, bodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
    }
    private void Jump()
    {
        if (side == Vector2.right)
        {
            bodyAnimator.Play("Jump-Right");
            handSprite.flipX = false;
        }
        else if (side == Vector2.left)
        {
            bodyAnimator.Play("Jump-Left");
            handSprite.flipX = true;
        }
        if (stats.HandState == HandStateE.Idle)
        {
            handAnimator.Play("Jump", 0, bodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
    }
    private void Fall()
    {
        if (side == Vector2.right)
        {
            bodyAnimator.Play("Fall-Right");
            handSprite.flipX = false;
        }
        else if (side == Vector2.left)
        {
            bodyAnimator.Play("Fall-Left");
            handSprite.flipX = true;
        }
        if (stats.HandState == HandStateE.Idle)
        {
            handAnimator.Play("Fall", 0, bodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }

    }
    private void Idle()
    {
        if (side == Vector2.right)
        {
            bodyAnimator.Play("Idle-Right");
            handSprite.flipX = false;
        }
        else if (side == Vector2.left)
        {
            bodyAnimator.Play("Idle-Left");
            handSprite.flipX = true;
        }
        if (stats.HandState == HandStateE.Idle)
        {
            handAnimator.Play("Idle", 0, bodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }

    }
    private void Hand()
    {
        if (stats.HandState == HandStateE.DrawingGun)
        {
            handAnimator.Play("Draw");
        }
        if (stats.HandState == HandStateE.HidingGun)
        {
            handAnimator.Play("Hide");
        }
        if(stats.HandState == HandStateE.Shooting)
        {
            handAnimator.Play("Shoot");
        }
    }
    private void Death()
    {
        if (side == Vector2.right)
        {
            bodyAnimator.Play("Death-Right");
            handSprite.flipX = false;
        }
        else if (side == Vector2.left)
        {
            bodyAnimator.Play("Death-Left");
            handSprite.flipX = true;
        }
        handAnimator.Play("Death");
    }
}
