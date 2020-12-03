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
    [SerializeField]
    private bool hasDoubleJump;
    [SerializeField]
    private bool hasWallJump;
    [SerializeField]
    private float wallJumpSpeedBase = 5;
    [SerializeField] 
    private float wallJumpTime;
    [SerializeField]
    private float jumpInputBufferingTime;
    
    private CharacterPhysic physic;
    private CharacterStats stats;
    private Gravity gravity;
    private int jumpCount;
    private IEnumerator accelerateEnumerator;
    private IEnumerator jumpBufferControllerEnumerator;
    private IEnumerator pressDurationEnumerator;
    private readonly Queue<JumpProperty> jumpBuffer = new Queue<JumpProperty>();
    private readonly Stack<JumpProperty> jumpPool = new Stack<JumpProperty>();
    private JumpProperty lastJump;

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        PlayerControl.OnPressInput += CheckJumpBuffer;
    }

    private void OnDisable()
    {
        PlayerControl.OnPressInput -= CheckJumpBuffer;
    }

    protected virtual void Init()
    {
        physic = GetComponent<CharacterPhysic>();
        stats = GetComponent<CharacterStats>();
        gravity = GetComponent<Gravity>();
        lastJump = new JumpProperty();
        ReleaseJumpProperty(lastJump);
        
        stats.DeathAction += CancelJump;
        stats.DeathAction += JumpDownReset;
        stats.onGroundAction += ChargeJump;
        stats.onWallAction += ChargeJumpOnWall;
    }
    
    public void StartJumpUp()
    {
        CreateJumpBuffer(JumpType.Up, stats.MoveSide);
    }
    
    public void StartJumpDown()
    {
        CreateJumpBuffer(JumpType.Down, Vector2.down);
    }
    public void EndJump()
    {
        StopCoroutineSafe(pressDurationEnumerator);
    }
    
    private void JumpDownReset()
    {
        physic.JumpDownLayerFix(false);
    }

    private void CancelJump()
    {
        StopCoroutineSafe(jumpBufferControllerEnumerator);
        StopCoroutineSafe(accelerateEnumerator);
        StopCoroutineSafe(pressDurationEnumerator);
    }

    private void CreateJumpBuffer(JumpType type, Vector2 moveSide, bool isImmediate = false)
    {
        var jumpProperty = GetJumpProperty(type, moveSide);
        jumpBuffer.Enqueue(jumpProperty);
        
        if (!isImmediate)
        {
            StopCoroutineSafe(pressDurationEnumerator);
            StartCoroutine(pressDurationEnumerator = PressDurationCounter(jumpProperty));
        }

        if (jumpBuffer.Count <= 1) StartCoroutine(jumpBufferControllerEnumerator = ControlJumpBuffer());
    }
    
    private IEnumerator ControlJumpBuffer()
    {
        while (jumpBuffer.Count > 0)
        {
            var passedTime = 0f;
            while (!CanJump(jumpBuffer.Peek().Type))
            {
                passedTime += Time.deltaTime;
                if (passedTime > jumpInputBufferingTime) CancelBuffer();
                yield return null;
                if (jumpBuffer.Count == 0) yield break;
            }
            
            Jump(jumpBuffer.Dequeue());
        }
    }

    private bool CanJump(JumpType jumpType)
    {
        if (jumpType == JumpType.Up)
            return CanJumpUp();
        return !lastJump.IsInProgress;
    }

    private bool CanJumpUp()
    {
        if (stats.BodyState == BodyStateE.WallJump) return false;
        if (jumpCount == 0 && (stats.FeetState == FeetStateE.OnGround || hasWallJump && stats.IsFeetOnWall())) return true;
        if (hasDoubleJump && jumpCount == 1) return true;
        return false;
    }

    private void Jump(JumpProperty jumpProperty)
    {
        lastJump = jumpProperty;
        jumpProperty.IsInProgress = true;
        if (jumpProperty.Type == JumpType.Down)
            StartCoroutine(JumpDown(jumpProperty));
        else
            JumpUp(jumpProperty);
    }

    private void JumpUp(JumpProperty jumpProperty)
    {
        stats.Jumped = true;
        jumpCount++;
        if (hasWallJump && stats.IsFeetOnWall())
            StartCoroutine(JumpOnWall(jumpProperty));
        else
        {
            StartCoroutine(accelerateEnumerator = AccelerateSpeed(jumpProperty));
            physic.AddForce(-gravity.Direction * (jumpSpeedBase * physic.Weight));
        }
    }

    private IEnumerator JumpDown(JumpProperty jumpProperty)
    {
        physic.JumpDownLayerFix(true);
        var timePassed = 0f;
        while (timePassed < jumpProperty.PressDuration)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        physic.JumpDownLayerFix(false);
        ReleaseJumpProperty(jumpProperty);
    }

    private IEnumerator AccelerateSpeed(JumpProperty jumpProperty)
    {
        var acceleratTimePassed = 0f;
        while (acceleratTimePassed < jumpProperty.PressDuration)
        {
            if(acceleratTimePassed + Time.deltaTime < jumpAccelerateTime)
            {
                acceleratTimePassed += Time.deltaTime;
                physic.AddForce(-gravity.Direction * (jumpAcceleration * physic.Weight * Time.deltaTime));
                yield return null;
            }
            else
            {
                physic.AddForce(-gravity.Direction * (jumpAcceleration * physic.Weight * (jumpAccelerateTime - acceleratTimePassed)));
                break;
            }
        }
        ReleaseJumpProperty(jumpProperty);
    }

    private IEnumerator JumpOnWall(JumpProperty jumpProperty)
    {
        stats.BodyState = BodyStateE.WallJump;
        var wallJumpForce = GetWallJumpDirection() * (wallJumpSpeedBase * physic.Weight);
        physic.AddSpeed(-physic.Speed);
        physic.AddForce(wallJumpForce);
        var moveSide = wallJumpForce.x >= 0 ? Vector2.right : Vector2.left;
        stats.MoveSide = moveSide;
        
        yield return new WaitForSeconds(wallJumpTime);
        physic.AddForce(Vector2.right * -physic.Force.x);

        if (stats.BodyState == BodyStateE.WallJump) stats.BodyState = BodyStateE.Idle;
        ReleaseJumpProperty(jumpProperty);
    }

    private Vector2 GetWallJumpDirection()
    {
        return -(gravity.Direction + (stats.FeetState == FeetStateE.OnLeftWall ? Vector2.left : Vector2.right));
    }
    
    public void HitJump()
    {
        if (CanJumpUp())
        {
            jumpCount++;
            stats.Jumped = true;
            physic.AddForce(-gravity.Direction * ((hitJumpBaseSpeed - physic.Force.y ) * physic.Weight));
        }
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
            if (stats.BodyState == BodyStateE.WallJump) stats.BodyState = BodyStateE.Idle;
        }
    }

    private void StopCoroutineSafe(IEnumerator coroutine)
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }

    private void CheckJumpBuffer(InputType inputType)
    {
        switch (inputType)
        {
            case InputType.Shoot: 
            case InputType.UpArrow:
                CancelBuffer();
                break;
            case InputType.LeftArrow:
                if (jumpBuffer.Count == 0 || jumpBuffer.Count > 0 && jumpBuffer.Peek().MoveSide.Equals(Vector2.left)) break;
                CancelBuffer();
                break;
            case InputType.RightArrow:
                if (jumpBuffer.Count == 0 || jumpBuffer.Count > 0 && jumpBuffer.Peek().MoveSide.Equals(Vector2.right)) break;
                CancelBuffer();
                break;
            case InputType.DownArrow:
                if (jumpBuffer.Count == 0 || jumpBuffer.Count > 0 && jumpBuffer.Peek().MoveSide.Equals(Vector2.down)) break;
                CancelBuffer();
                break;
        }
    }

    private void CancelBuffer()
    {
        while (jumpBuffer.Count > 0)
        {
            var jumpProperty = jumpBuffer.Dequeue();
            jumpProperty.IsInProgress = false;
            jumpPool.Push(jumpProperty);
        }
    }

    private IEnumerator PressDurationCounter(JumpProperty jumpProperty)
    {
        while (true)
        {
            jumpProperty.PressDuration += Time.deltaTime;
            yield return null;
        }
    }

    private JumpProperty GetJumpProperty(JumpType type, Vector2 moveSide, float pressDuration = 0f, bool isInProgress = false)
    {
        if (jumpPool.Count == 0) return new JumpProperty(type, moveSide, pressDuration, isInProgress);
        
        var jumpProperty = jumpPool.Pop();
        jumpProperty.SetProperty(type, moveSide, pressDuration, isInProgress);
        return jumpProperty;
    }

    private void ReleaseJumpProperty(JumpProperty jumpProperty)
    {
        jumpProperty.IsInProgress = false;
        jumpPool.Push(jumpProperty);
    }
    
    private class JumpProperty
    {
        public JumpType Type;
        public float PressDuration;
        public Vector2 MoveSide;
        public bool IsInProgress;

        public JumpProperty()
        {
            IsInProgress = false;
        }
        
        public JumpProperty(JumpType type, Vector2 moveSide, float pressDuration = 0f, bool isInProgress = false)
        {
            SetProperty(type, moveSide, pressDuration, isInProgress);
        }

        public void SetProperty(JumpType type, Vector2 moveSide, float pressDuration, bool isInProgress)
        {
            Type = type;
            MoveSide = moveSide; 
            PressDuration = pressDuration;
            IsInProgress = isInProgress;
        }
    }
    
    private enum JumpType
    {
        Up,
        Down
    }
}
