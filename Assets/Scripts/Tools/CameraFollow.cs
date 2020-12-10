using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float smoothness;

    private Vector2 leftDown;
    private Vector2 rightUp;
    private Vector2 offset;
    private Transform target;
    private Vector3 targetPos;
    private bool boundless;
    private float targetSize;
    private static CameraFollow instance;

    private Vector2 savedLeftDown;
    private Vector2 savedRightUp;
    private Vector2 savedoffset;
    private bool savedBoundless;
    private float savedTargetSize;

    public float Smoothness
    {
        get => smoothness;
        set => smoothness = value;
    }

    public static CameraFollow Instance
    {
        get
        {
            if (!instance) instance = FindObjectOfType<CameraFollow>();
            return instance;
        }
    }

    private void Awake()
    {
        GameManager.SaveSceneAction += Save;
        GameManager.LoadSceneAction += Load;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetSize = Camera.main.orthographicSize;
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        CameraMovement();
        CameraZoom();
    }
    private void CameraMovement()
    {
        targetPos = target.position + (Vector3)offset;
        if (!boundless)
        {
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, leftDown.x, rightUp.x), Mathf.Clamp(targetPos.y, leftDown.y, rightUp.y));
        }
        targetPos.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothness);
    }
    private void CameraZoom()
    {
        if (Camera.main.orthographicSize != targetSize)
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, smoothness);
    }
    public void ChangeBoundries(Vector2 leftDown, Vector2 rightUp, Vector2 offset, bool boundless)
    {
        this.offset = offset;
        this.boundless = boundless;
        this.leftDown = leftDown;
        this.rightUp = rightUp;
    }
    public void ChangeZoom(float size)
    {
        targetSize = size;
    }
    private void Save()
    {
        savedLeftDown = leftDown;
        savedRightUp = rightUp;
        savedoffset = offset;
        savedBoundless = boundless;
        savedTargetSize = targetSize;
    }
    private void Load()
    {
        leftDown = savedLeftDown;
        rightUp = savedRightUp;
        offset = savedoffset;
        boundless = savedBoundless;
        targetSize = savedTargetSize;
    }
}
