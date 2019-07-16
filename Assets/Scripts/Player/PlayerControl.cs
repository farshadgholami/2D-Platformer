using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
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
        //MoveUp
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // move.MovePressed(Vector2.up);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            //move.MoveReleased(Vector2.up);
        }
        //Move Down
        if (Input.GetKeyDown(KeyCode.S))
        {
            //move.MovePressed(Vector2.down);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            //move.MoveReleased(Vector2.down);
        }
        //Move Right
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            move.MoveStart(Vector2.right);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            move.MoveStop(Vector2.right);
        }
        //Move left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move.MoveStart(Vector2.left);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            move.MoveStop(Vector2.left);
        }
    }
    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump.JumpStart();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            jump.JumpStop();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            jump.JumpDown(true);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            jump.JumpDown(false);
        }
    }
    private void GunInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            gun.shoot();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gun.Draw();
        }
    }
}

