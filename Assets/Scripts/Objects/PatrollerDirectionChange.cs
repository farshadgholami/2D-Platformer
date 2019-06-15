using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollerDirectionChange : MonoBehaviour
{
    [SerializeField]
    private Side side;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Patroller"))
        {
            collision.GetComponent<PatrollerBrain>().SetTarget(transform.position, Toolkit.SideToVector(side));
        }
    }
}
