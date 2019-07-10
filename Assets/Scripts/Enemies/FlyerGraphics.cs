using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerGraphics : MonoBehaviour
{
    private CharacterStats stats;
    private Animator animator;
    private Vector2 lastHorizontalMovement = Vector2.right;
    private void Start()
    {
        stats = GetComponent<CharacterStats>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (stats.IsDead)
        {
            Death();
            return;
        }

        Fly();
    }
    private void Fly()
    {
        if (stats.MoveSide == Vector2.right)
        {
            animator.Play("Fly-Left");
            lastHorizontalMovement = stats.MoveSide;
        }
        else if (stats.MoveSide == Vector2.left)
        {
            animator.Play("Fly-Right");
            lastHorizontalMovement = stats.MoveSide;
        }
        else
        {
            if (lastHorizontalMovement == Vector2.right)
            {
                animator.Play("Fly-Left");
            }
            else if (lastHorizontalMovement == Vector2.left)
            {
                animator.Play("Fly-Right");
            }
        }
    }
    private void Death()
    {
        if (stats.MoveSide == Vector2.right)
        {
            animator.Play("Death-Right");
            lastHorizontalMovement = stats.MoveSide;
        }
        else if (stats.MoveSide == Vector2.left)
        {
            animator.Play("Death-Left");
            lastHorizontalMovement = stats.MoveSide;
        }
        else
        {
            if (lastHorizontalMovement == Vector2.right)
            {
                animator.Play("Death-Right");
            }
            else if (lastHorizontalMovement == Vector2.left)
            {
                animator.Play("Death-Left");
            }
        }
    }
}
