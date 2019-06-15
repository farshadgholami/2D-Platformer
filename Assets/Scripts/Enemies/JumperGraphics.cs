using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperGraphics : MonoBehaviour
{
    private CharacterStats stats;
    private JumperBrain brain;
    private Animator animator;
    private bool hitingGround;

    private bool savedHitGround;
    private BodyStateE savedBodyState;
    private float savedAnimatorTime;

    private void Awake()
    {
        GameManager.SaveSceneAction += Save;
        GameManager.LoadSceneAction += Load;
        animator = GetComponent<Animator>();
        stats = GetComponentInParent<CharacterStats>();
        brain = GetComponentInParent<JumperBrain>();
    }
    private void Start()
    {
    }

    private void Update()
    {
        if (stats.IsDead)
        {
            Death();
            return;
        }

        if (hitingGround)
            return;

        if (stats.FeetState != FeetStateE.OnGround)
        {
            if (stats.FeetState == FeetStateE.Jumping)
                Jump();
            else if (stats.FeetState == FeetStateE.Falling)
                Fall();

            return;
        }
        else
        {
            if (stats.HitGround)
            {
                HitGround();
                return;
            }
            if (stats.BodyState == BodyStateE.Laying)
            {
                LayingDown();
                return;
            }
            if (stats.BodyState == BodyStateE.Idle)
            {
                Idle();
                return;
            }
            if (stats.BodyState == BodyStateE.StandingUp)
            {
                StandingUp();
                return;
            }
            if (stats.BodyState == BodyStateE.ChargeJump)
            {
                ChargeJump();
                return;
            }
            if (stats.BodyState == BodyStateE.Moveing)
            {
                Movement();
                return;
            }
        }
    }
    private void Death()
    {
        if (stats.MoveSide == Vector2.right)
        {
            animator.Play("Death-Right");
        }
        else
        {
            animator.Play("Death-Left");
        }
    }
    private void Jump()
    {
        if (stats.MoveSide == Vector2.right)
        {
            animator.Play("Jump-Right");
        }
        else
        {
            animator.Play("Jump-Left");
        }
    }
    private void Fall()
    {
        if (stats.MoveSide == Vector2.right)
        {
            animator.Play("Fall-Right");
        }
        else
        {
            animator.Play("Fall-Left");
        }
    }
    private void HitGround()
    {
        brain.HitGround = hitingGround = true;
        if (stats.MoveSide == Vector2.right)
        {
            animator.Play("HitGround-Right");
        }
        else
        {
            animator.Play("HitGround-Left");
        }
    }
    private void LayingDown()
    {
        if (stats.MoveSide == Vector2.right)
        {
            animator.Play("LayingDown-Right");
        }
        else
        {
            animator.Play("LayingDown-Left");
        }
    }
    private void Idle()
    {
        if (stats.MoveSide == Vector2.right)
        {
            animator.Play("Idle-Right");
        }
        else
        {
            animator.Play("Idle-Left");
        }
    }
    private void StandingUp()
    {
        if (stats.MoveSide == Vector2.right)
        {
            animator.Play("StandUp-Right");
        }
        else
        {
            animator.Play("StandUp-Left");
        }
    }
    private void ChargeJump()
    {
        if (stats.MoveSide == Vector2.right)
        {
            animator.Play("ChargeJump-Right");
        }
        else
        {
            animator.Play("ChargeJump-Left");
        }
    }
    private void Movement()
    {
        if (stats.MoveSide == Vector2.right)
        {
            animator.Play("Run-Right");
        }
        else
        {
            animator.Play("Run-Left");
        }
    }
    private void StandUpHit()
    {
        brain.HitGround = hitingGround = false;
    }
    private void StandUp()
    {
        stats.BodyState = BodyStateE.Idle;
    }
    private void Charged()
    {
        brain.Charged();
    }
    private void Save()
    {
        savedBodyState = stats.BodyState;
        savedHitGround = hitingGround;
    }
    private void Load()
    {
        stats.BodyState = savedBodyState;
        hitingGround = savedHitGround;
        if (stats.IsDead)
            return;

        if (hitingGround)
        {
            brain.HitGround = hitingGround = true;
            if (stats.MoveSide == Vector2.right)
            {
                animator.Play("HitGround-Right");
            }
            else
            {
                animator.Play("HitGround-Left");
            }
        }

    }
}
