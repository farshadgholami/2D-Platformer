using System;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    [SerializeField] private LevelData levelData;
    private int points;
    private int yellowKeyNumber;
    private int redKeyNumber;
    private int blueKeyNumber;

    private int savedPoints;
    private int savedyellowKey;
    private int savedRedKey;
    private int savedBlueKey;

    private void Awake()
    {
        levelData.data.bestScore = 0;
    }

    public int Points
    {
        get => points;
        set
        {
            points = value;
            levelData.data.bestScore = value;
        }
    }

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
        savedPoints = Points;
        savedyellowKey = yellowKeyNumber;
        savedRedKey = redKeyNumber;
        savedBlueKey = blueKeyNumber;
    }
    protected override void Load()
    {
        base.Load();
        Points = savedPoints;
        yellowKeyNumber = savedyellowKey;
        redKeyNumber = savedRedKey;
        blueKeyNumber = savedBlueKey;
    }
}
