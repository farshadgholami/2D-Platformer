using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private bool pickedUp;
    private bool savedStatus;
    private SpriteRenderer sprite;

    private void Awake()
    {
        GameManager.SaveSceneAction += Save;
        GameManager.LoadSceneAction += Load;
    }
    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickUpFuntion(collision.gameObject.GetComponent<PlayerStats>());
            GraphicsCheck();
        }
    }
    protected virtual void GraphicsCheck()
    {
        sprite.enabled = !pickedUp;
        GetComponent<BoxCollider2D>().enabled = !pickedUp;
    }

    protected virtual void PickUpFuntion(PlayerStats stats)
    {
        pickedUp = true;
    }

    protected virtual void Save()
    {
        savedStatus = pickedUp;
    }
    protected virtual void Load()
    {
        pickedUp = savedStatus;
        GraphicsCheck();
    }
}
