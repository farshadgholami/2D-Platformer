using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Physic physic;
    protected Vector2 direction;
    protected float force;
    protected bool disabled;

    private void Awake()
    {
        physic = GetComponent<Physic>();
        force = GameManager.instance.GravityForce;
        direction = GameManager.instance.GravityDirection;
    }
    private void Update()
    {
        Function();
    }
    protected virtual void Function()
    {
        if (disabled)
            return;

        physic.AddForce(force * direction * physic.Weight * Time.deltaTime);
    }
    public bool Disabled { get { return disabled; } set { disabled = value; } }
    public Vector2 Direction { get { return direction; } }
}



