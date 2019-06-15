using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool used;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!used && collision.CompareTag("Player"))
        {
            used = true;
            GetComponent<Animator>().Play("Activated");
            GameManager.SaveScene();            
        }
    }
}
