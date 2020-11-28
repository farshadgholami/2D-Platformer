using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private int points;
    private int yellowKeyNumber;
    private int redKeyNumber;
    private int blueKeyNumber;

    private int savedPoints;
    private int savedyellowKey;
    private int savedRedKey;
    private int savedBlueKey;
    protected override void Death()
    {
        base.Death();
    }
    public int Points { get { return points; } set { points = value; } }
    public int YellowKey { get { return yellowKeyNumber; } set { yellowKeyNumber = value; } }
    public int RedKey { get { return redKeyNumber; } set { redKeyNumber = value; } }
    public int BlueKey { get { return blueKeyNumber; } set { blueKeyNumber = value; } }
    public void LoadlastcCheckPoint()
    {
        if (IsDead)
        {
            GameManager.LoadLastCheckPoint();
        }
    }
    protected override void Save()
    {
        base.Save();
        savedPoints = points;
        savedyellowKey = yellowKeyNumber;
        savedRedKey = redKeyNumber;
        savedBlueKey = blueKeyNumber;
    }
    protected override void Load()
    {
        base.Load();
        points = savedPoints;
        yellowKeyNumber = savedyellowKey;
        redKeyNumber = savedRedKey;
        blueKeyNumber = savedBlueKey;
    }
}
