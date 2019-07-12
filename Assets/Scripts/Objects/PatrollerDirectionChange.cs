using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollerDirectionChange : MonoBehaviour
{
    [SerializeField]
    private Side side;

    private Collider2D collider_;

    private void Start()
    {
        collider_ = GetComponent<Collider2D>();
        GameManager.LoadSceneAction += Reload;   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Patroller"))
        {
            collision.GetComponent<PatrollerBrain>().SetTarget(transform.position, Toolkit.SideToVector(side));
        }
    }

    private void Reload()
    {
        collider_.enabled = false;
        collider_.enabled = true;
    }
}
