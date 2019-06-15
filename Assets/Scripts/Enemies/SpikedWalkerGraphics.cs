using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedWalkerGraphics : MonoBehaviour
{
    private CharacterStats stats;
    private Animator animator;

    private void Start()
    {
        stats = GetComponent<CharacterStats>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (stats.IsDead)
        {
            if (stats.MoveSide == Vector2.right)
            {
                animator.Play("Death-Right");
            }
            else
            {
                animator.Play("Death-Left");
            }
            return;
        }

        if (stats.MoveSide == Vector2.right)
        {
            animator.Play("Run-Right");
        }
        else
        {
            animator.Play("Run-Left");
        }
    }
}
