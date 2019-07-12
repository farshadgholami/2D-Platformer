using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class GameManager : MonoBehaviour
{
    private Action loadSceneAction;
    private Action lateLoadSceneAction;
    private Action saveSceneAction;
    private Action lateStartAction;

    private int latestartCount = 1;
    [SerializeField]
    private float minSize;
    [SerializeField]
    private float gravityForce;
    [SerializeField]
    private Vector2 gravityDirection;
    [SerializeField]
    private float max_gravity_speed_ = 12.5f;

    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SaveScene();
    }
    private void Update()
    {
        if (latestartCount <= 0)
        {
            lateStartAction?.Invoke();
        }
        else
        {
            latestartCount--;
        }
    }
    public static void ReloadScene()
    {
        instance.loadSceneAction?.Invoke();
    }
    public static void LoadLastCheckPoint()
    {
        instance.loadSceneAction?.Invoke();
        instance.lateLoadSceneAction?.Invoke();
    }
    public static void SaveScene()
    {
        instance.saveSceneAction?.Invoke();
    }
    public static Action LoadSceneAction { get { return instance.loadSceneAction; } set { instance.loadSceneAction = value; } }
    public static Action SaveSceneAction { get { return instance.saveSceneAction; } set { instance.saveSceneAction = value; } }
    public static Action LateLoadSceneAction { get { return instance.lateLoadSceneAction; } set { instance.lateLoadSceneAction = value; } }
    public static Action LateStartAction { get { return instance.lateStartAction; } set { instance.lateStartAction = value; } }
    public static float MinSize { get { return instance.minSize; } }
    public Vector2 GravityDirection { get { return gravityDirection; } set { gravityDirection = value; } }
    public float pMaxGravitySpeed { get { return max_gravity_speed_; } }
    public float GravityForce { get { return gravityForce; } }
}
