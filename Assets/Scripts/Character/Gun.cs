using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float distance;
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private float drawTime;

    private float timer;

    private bool orderqueued;
    private Animator handAnimator;

    private CharacterStats stats;
    // Start is called before the first frame update
    void Start()
    {
        stats = transform.parent.parent.GetComponent<CharacterStats>();
        handAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(stats.HandState == HandStateE.Drawed)
        {
            timer -= Time.deltaTime;
            {
                if(timer <= 0)
                {
                    Hide();
                }
            }
        }
    }

    public void shoot()
    {
        if (stats.HandState == HandStateE.Drawed)
        {
            stats.HandState = HandStateE.Shooting;
            BulletManager.Shoot((Vector2)transform.position + (offset * stats.MoveSide), stats.MoveSide, speed, distance);
            orderqueued = false;
        }
        else if (stats.HandState == HandStateE.Shooting)
        {
            orderqueued = true;
        }
        else
        {
            orderqueued = true;
            Draw();
        }
    }
    public void Draw()
    {
        if(stats.HandState == HandStateE.Drawed)
        {
            timer = drawTime;
        }
        else if(stats.HandState != HandStateE.Shooting)
        {
            stats.HandState = HandStateE.DrawingGun;
        }
    }
    private void Hide()
    {
        stats.HandState = HandStateE.HidingGun;
    }
    private void DrawCompelete()
    {
        stats.HandState = HandStateE.Drawed;
        handAnimator.Play("Drawed");
        timer = drawTime;
        if (orderqueued)
        {
            shoot();
        }
    }
    private void HideCompelete()
    {
        stats.HandState = HandStateE.Idle;
    }
}
