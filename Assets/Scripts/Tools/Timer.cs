using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    public delegate void Delegate();
    private static Timer instance;
    private Hashtable timerTable = new Hashtable();
    private List<int> timerTableIdList = new List<int>();
    private int currentTimerId;
    // Use this for initialization
    private void Awake()
    {
        currentTimerId = 0;
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        TimerCheck();
    }
    private void TimerCheck()
    {
        for (int i = 0; i < timerTableIdList.Count; i++)
        {
            TimerAction timer = ((TimerAction)timerTable[timerTableIdList[i]]);
            if (!timer.deactive)
            {
                if (Time.time - timer.startTime >= timer.time)
                {
                    timer.function();
                    timer.deactive = true;
                }
            }
        }
    }
    public static void AddTimer(Delegate function, float time, int id)
    {
        if (!instance.timerTable.Contains(id))
        {
            instance.timerTable.Add(id, new TimerAction(function, time, Time.time));
            instance.timerTableIdList.Add(id);
        }
        else
        {
            ((TimerAction)instance.timerTable[id]).time = time;
            ((TimerAction)instance.timerTable[id]).startTime = Time.time;
            ((TimerAction)instance.timerTable[id]).function = function;
            ((TimerAction)instance.timerTable[id]).deactive = false;
        }
    }
    public static void ResetTimer(int id)
    {
        if (instance.timerTable.Contains(id))
        {
            ((TimerAction)instance.timerTable[id]).startTime = Time.time;
        }
    }
    public static void StopTimer(int id)
    {
        if (instance.timerTable.Contains(id))
        {
            ((TimerAction)instance.timerTable[id]).deactive = true;
        }
    }
    public static int GetTimerId()
    {
        return instance.currentTimerId++;
    }
}
public class TimerAction
{
    public Timer.Delegate function;
    public float time;
    public float startTime;
    public bool deactive;
    public TimerAction(Timer.Delegate function, float time, float startTime)
    {
        this.function = function;
        this.time = time;
        this.startTime = startTime;
    }
}
