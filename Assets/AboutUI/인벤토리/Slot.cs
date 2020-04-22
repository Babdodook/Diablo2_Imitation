using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject getItem;
    public Vector2 slotPos;
    public bool occupied;
    public bool isOccupied;
    public bool ItemOnSlot;
    public bool Overlap;
    public bool ItemOccupied;
    public int index;
    public Image img;

    void Start()
    {
        //occupied = true;
        img = this.transform.GetComponent<Image>();
    }

    private void Update()
    {
        Color();
    }

    public void Color()
    {
        if (occupied)
        {
            img.color = new Color(0, 0, 255, 0.15f);        //파랑슬롯
        }
        if (!occupied)
        {
            img.color = new Color(0, 0, 0);                 //색깔없음
        }
        if(ItemOnSlot)
        {
            img.color = new Color(0, 255, 0, 0.15f);        //초록슬롯
        }
        if(Overlap)
        {
            img.color = new Color(255, 255, 255, 0.15f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Occupied")
        {
            isOccupied = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isOccupied = false;
    }

    public void ClickSlot()
    {
        Debug.Log("isOccupied: " + isOccupied);
        Debug.Log("occupied: " + occupied);
        Debug.Log("ItemOnSlot: " + ItemOnSlot);
        Debug.Log("Overlap: " + Overlap);
    }
}
