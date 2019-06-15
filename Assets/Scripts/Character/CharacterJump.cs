using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    [SerializeField]
    private float hitJumpBaseSpeed;
    [SerializeField]
    private float jumpSpeedBase;
    [SerializeField]
    private float jumpAcceleration;
    [SerializeField]
    private float jumpAccelerateTime;

    private float currentAccelerateTime;
    private bool accelerate;

    private CharacterPhysic physic;
    private CharacterStats stats;
    private Gravity gravity;
    private bool jumpCommand;
    private bool canJump = true;
    // Use this for initialization
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        Function();
    }
    protected virtual void Init()
    {
        physic = GetComponent<CharacterPhysic>();
        stats = GetComponent<CharacterStats>();
        gravity = GetComponent<Gravity>();
        stats.DeathAction += JumpStop;
        stats.onGroundAction += ChargeJump;
    }
    protected virtual void Function()
    {
        if (accelerate)
        {
            if(currentAccelerateTime + Time.deltaTime < jumpAccelerateTime)
            {
                currentAccelerateTime += Time.deltaTime;
                physic.AddForce(-gravity.Direction * jumpAcceleration * physic.Weight * Time.deltaTime);
            }
            else
            {
                physic.AddForce(-gravity.Direction * jumpAcceleration *physic.Weight *(jumpAccelerateTime - currentAccelerateTime));
                accelerate = false;
            }
        }
    }
    public void JumpStart()
    {
        jumpCommand = true;
        if (stats.FeetState == FeetStateE.OnGround && canJump)
        {
            ResetJump();
            canJump = false;
            stats.Jumped = true;
            accelerate = true;
            physic.AddForce(-gravity.Direction * (jumpSpeedBase) * physic.Weight);
        }
    }
    public void HitJump()
    {
        if (stats.FeetState == FeetStateE.OnGround && canJump)
        {
            ResetJump();
            canJump = false;
            stats.Jumped = true;
            accelerate = jumpCommand;
            physic.AddForce(-gravity.Direction * (hitJumpBaseSpeed - physic.Force.y ) * physic.Weight);
        }
    }
    public void JumpStop()
    {
        jumpCommand = false;
        accelerate = false;
    }
    private void ResetJump()
    {
        currentAccelerateTime = 0;
    }
    private void ChargeJump()
    {
        canJump = true;
    }
    public void JumpDown(bool on)
    {
        physic.JumpDownLayerFix(on);
    }
}
