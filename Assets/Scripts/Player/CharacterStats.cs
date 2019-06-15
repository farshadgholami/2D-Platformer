using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    public Action onGroundAction;
    public Action fallAction;
    public Action DeathAction;

    [SerializeField]
    private int health;
    private bool dead;
    private Gravity gravity;
    private int currentHealth;

    private float speedMult = 1;
    private bool jumped;
    private bool hitGround;
    private bool shot;
    private bool shooting;
    private Vector2 moveSide = Vector2.right;
    private Vector2 lookSide;
    private SoundProperty soundProperty;
    private int layerMask;

    private MoveTypeE moveType = MoveTypeE.Ground;
    private FeetStateE feetState = FeetStateE.OnGround;
    private BodyStateE bodyState = BodyStateE.Idle;
    private HandStateE handState = HandStateE.Idle;

    //Save
    protected int savedHealth;
    protected bool savedHealthStatus;


    private void Awake()
    {
        layerMask = gameObject.layer;
        currentHealth = health;
        GameManager.SaveSceneAction += Save;
        GameManager.LoadSceneAction += Load;
    }
    protected virtual void Save()
    {
        savedHealth = currentHealth;
        savedHealthStatus = dead;
    }
    protected virtual void Load()
    {
        currentHealth = savedHealth;
        dead = savedHealthStatus;
        if (!dead)
        {
            gameObject.layer = layerMask;
        }
    }
    protected virtual void Death()
    {
        dead = true;
        gameObject.layer = LayerMask.NameToLayer("Void");

        DeathAction?.Invoke();
    }
    public FeetStateE FeetState
    {
        get { return feetState; }
        set
        {
            if (value == FeetStateE.Falling)
            {
                fallAction?.Invoke();
            }
            else if (value == FeetStateE.OnGround)
            {
                onGroundAction?.Invoke();
            }
            feetState = value;
        }
    }
    public BodyStateE BodyState { get { return bodyState; } set { bodyState = value; } }
    public HandStateE HandState { get { return handState; } set { handState = value; } }
    public MoveTypeE MoveType { get { return moveType; } set { moveType = value; } }
    public float Health
    {
        get { return currentHealth; }
        set
        {
            currentHealth = (int)Mathf.Clamp(value, 0, health);
            if (currentHealth == 0 && !dead)
            {
                Death();
            }
        }
    }
    public float MaxHealth { get { return health; } }
    public Vector2 MoveSide { get { return moveSide; } set { moveSide = value; } }
    public Vector2 LookSide { get { return lookSide; } set { lookSide = value; } }
    public bool Shooting { get { return shooting; } set { shooting = value; } }
    public SoundProperty SoundProperty { get { return soundProperty; } set { soundProperty = value; } }
    public bool Jumped { get { return jumped; } set { jumped = value; } }
    public bool HitGround { get { return hitGround; } set { hitGround = value; } }
    public bool Shot { get { return shot; } set { shot = value; } }
    public float SpeedMult { get { return speedMult; } set { speedMult = value; } }
    public bool IsDead { get { return dead; } }
}
public enum FeetStateE { OnGround, Jumping, Falling }
public enum BodyStateE { Idle, Moveing, Laying, ChargeJump, StandingUp }
public enum MoveTypeE { Ground, Flying }
public enum HandStateE { Idle, DrawingGun, HidingGun, Drawed, Shooting }