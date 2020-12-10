using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraProperties : MonoBehaviour
{
    [SerializeField]
    private Vector2 leftDown;
    [SerializeField]
    private Vector2 rightUp;
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private float zoom;
    [SerializeField]
    private bool boundless;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        if (GetComponent<CameraFollow>())
        {
            ChangeCameraProperties();
        }
    }
    private void ChangeCameraProperties()
    {
        CameraFollow.Instance.ChangeBoundries(leftDown, rightUp, offset, boundless);
        if (zoom > 0)
        {
            CameraFollow.Instance.ChangeZoom(zoom);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ChangeCameraProperties();
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (boundless)
            return;

        float drawZoom;
        if (zoom > 0)
            drawZoom = zoom;
        else
            drawZoom = Camera.main.orthographicSize;

        Gizmos.color = Color.cyan;
        Vector2 center = new Vector2((leftDown.x + (rightUp.x - leftDown.x) / 2), (leftDown.y + (rightUp.y - leftDown.y) / 2));
        Vector2 size = new Vector2((rightUp.x - leftDown.x), (rightUp.y - leftDown.y)) + new Vector2(2 * Camera.main.aspect * drawZoom, 2 * drawZoom);
        Gizmos.DrawWireCube(center, size);
    }
}
