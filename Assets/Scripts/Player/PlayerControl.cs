using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static event Action<InputType> OnPressInput;
    private CharacterMovement move;
    private CharacterJump jump;
    private Gun gun;
    private PlayerStats stats;
    private INGameMenu menu;
    private bool paused;

    // Use this for initialization
    void Start()
    {
        move = GetComponent<CharacterMovement>();
        jump = GetComponent<CharacterJump>();
        gun = GetComponentInChildren<Gun>();
        stats = GetComponent<PlayerStats>();
        menu = INGameMenu.sSingletone;
    }
    // Update is called once per frame
    void Update()
    {
        if (!stats.IsDead)
            GamePlay();
        else
            Death(); 
    }
    private void GamePlay()
    {
        MoveInput();
        JumpInput();
        GunInput();
        Menu();
    }

    private void Menu()
    {
        if (menu != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menu.show();
            }
        }
    }
    private void Death()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stats.LoadlastcCheckPoint();
        }
    }
    private void MoveInput()
    {
        if (stats.BodyState == BodyStateE.WallJump) return;
        
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            move.MoveStart(Vector2.right);
            PressInput(InputType.RightArrow);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            move.MoveStart(Vector2.right);
            PressInput(InputType.RightArrow);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            move.MoveStart(Vector2.left);
            PressInput(InputType.LeftArrow);
        }
        
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            move.MoveStop(Vector2.right);
        }
        
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            move.MoveStop(Vector2.left);
        }

        AccelerateInput();
        SprintInput();
    }

    private void AccelerateInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            move.Accelerate = true;
            PressInput(InputType.Accelerate);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            move.Accelerate = false;
    }

    private void SprintInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            move.IsRun = true;
            PressInput(InputType.Sprint);
        }
        else if (Input.GetKeyUp(KeyCode.Z))
            move.IsRun = false;
    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PressInput(InputType.Space);
            jump.StartJumpUp();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            jump.EndJump();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PressInput(InputType.DownArrow);
            jump.StartJumpDown();
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            jump.EndJump();
        }
    }
    private void GunInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            gun.shoot();
            PressInput(InputType.Shoot);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gun.Draw();
            PressInput(InputType.UpArrow);
        }
    }

    private void PressInput(InputType inputType)
    {
        OnPressInput?.Invoke(inputType);
    }
}

public enum InputType
{
    RightArrow,
    LeftArrow,
    UpArrow,
    DownArrow,
    Space,
    Shoot,
    Sprint,
    Accelerate
}

