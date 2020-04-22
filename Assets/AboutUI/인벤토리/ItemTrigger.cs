using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "UnOccupied")
        {
            transform.parent.tag = "Overlap";
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "UnOccupied")
        {
            transform.parent.tag = "Overlap";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "UnOccupied")
        {
            transform.parent.tag = "Occupied";
        }
    }
}
