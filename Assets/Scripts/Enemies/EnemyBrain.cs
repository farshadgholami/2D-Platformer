using UnityEngine;

public abstract class EnemyBrain : MonoBehaviour
{
    [SerializeField]
    protected Side direction;
    
    protected Vector2 moveDirection;
    protected CharacterStats stats;
    protected CharacterPhysic physic;
    protected CharacterMovement move;
    protected Gravity gravity;

    protected Vector2 target;
    protected bool targetAvailable;

    protected Vector2 savedMoveDirection;
    protected Vector2 savedTarget;
    protected bool savedTargetStatus;

    public Side Direction
    {
        get => direction;
        set => direction = value;
    }

    protected virtual void Awake()
    {
        stats = GetComponent<CharacterStats>();
        physic = GetComponent<CharacterPhysic>();
        move = GetComponent<CharacterMovement>();
        gravity = GetComponent<Gravity>();
    }

    protected virtual void ResetBrain()
    {
        stats.ResetState();
        physic.TurnOn();
        CalculateDirection();
    }
    protected abstract void CalculateDirection();
}
