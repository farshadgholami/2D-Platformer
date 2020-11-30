using System.Collections;
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
    [SerializeField]
    private bool hasDoubleJump;
    [SerializeField]
    private bool hasWallJump;
    [SerializeField]
    private float wallJumpSpeedBase = 5;
    [SerializeField] 
    private float wallJumpTime;

    private float currentAccelerateTime;
    private bool accelerate;

    private CharacterPhysic physic;
    private CharacterStats stats;
    private Gravity gravity;
    private bool jumpCommand;
    private int jumpCount;
    
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
        stats.DeathAction += jumpDownReset;
        stats.onGroundAction += ChargeJump;
        stats.onWallAction += ChargeJumpOnWall;
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
        if (CanJump()) 
            JumpUp();
    }

    private bool CanJump()
    {
        if (stats.IsOnWallJump) return false;
        if (jumpCount == 0 && (stats.FeetState == FeetStateE.OnGround || hasWallJump && stats.IsFeetOnWall())) return true;
        if (hasDoubleJump && jumpCount == 1) return true;
        return false;
    }

    private void JumpUp()
    {
        ResetJump();
        stats.Jumped = true;
        jumpCount++;
        if (hasWallJump && stats.IsFeetOnWall())
            JumpOnWall();
        else
        {
            accelerate = true;
            physic.AddForce(-gravity.Direction * (jumpSpeedBase) * physic.Weight);
        }
    }

    private void JumpOnWall()
    {
        StopAllCoroutines();
        StartCoroutine(JWall());
    }

    private IEnumerator JWall()
    {
        stats.IsOnWallJump = true;
        stats.BodyState = BodyStateE.WallJump;
        var wallJumpForce = GetWallJumpDirection() * (wallJumpSpeedBase * physic.Weight);
        physic.AddSpeed(-physic.Speed);
        physic.AddForce(wallJumpForce);
        var moveSide = wallJumpForce.x >= 0 ? Vector2.right : Vector2.left;
        stats.MoveSide = moveSide;
        
        yield return new WaitForSeconds(wallJumpTime);
        physic.AddForce(Vector2.right * -physic.Force.x);

        stats.IsOnWallJump = false;
        if (stats.BodyState == BodyStateE.WallJump) stats.BodyState = BodyStateE.Idle;
    }

    private Vector2 GetWallJumpDirection()
    {
        return -(gravity.Direction + (stats.FeetState == FeetStateE.OnLeftWall ? Vector2.left : Vector2.right));
    }
    
    public void HitJump()
    {
        if (CanJump())
        {
            ResetJump();
            jumpCount++;
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
        jumpCount = 0;
    }
    private void ChargeJumpOnWall()
    {
        if (hasWallJump)
        {
            jumpCount = hasDoubleJump ? 1 : 0;
            stats.IsOnWallJump = false;
            if (stats.BodyState == BodyStateE.WallJump) stats.BodyState = BodyStateE.Idle;
        }
    }
    public void JumpDown(bool on)
    {
        physic.JumpDownLayerFix(on);
    }
    private void jumpDownReset()
    {
        physic.JumpDownLayerFix(false);
    }
}
