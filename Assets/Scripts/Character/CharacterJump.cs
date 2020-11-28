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
        if (jumpCount == 0 && stats.FeetState == FeetStateE.OnGround) return true;
        if (hasDoubleJump && jumpCount == 1) return true;
        return false;
    }

    private void JumpUp()
    {
        ResetJump();
        jumpCount++;
        stats.Jumped = true;
        accelerate = true;
        physic.AddForce(-gravity.Direction * (jumpSpeedBase) * physic.Weight);
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
    public void JumpDown(bool on)
    {
        physic.JumpDownLayerFix(on);
    }
    private void jumpDownReset()
    {
        physic.JumpDownLayerFix(false);
    }
}
