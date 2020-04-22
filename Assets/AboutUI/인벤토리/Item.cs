using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ItemX;
    public int ItemY;
    int checkSlot;
    int overlapCheck;

    Vector2[] slotObj;
    public bool isDrag;
    public bool ActiveSlot;
    bool noneMove;
    bool Equip;
    GameObject EquipArea;
    GameObject PotionMove;

    // 착용 모션 인스턴스 값
    public bool HelmetEquiped;
    public bool ChestEquiped;
    public bool WeaponEquiped;

    private void Start()
    {
        slotObj = new Vector2[ItemY*ItemX];
        checkSlot = 0;
        isDrag = false;
        noneMove = false;
        overlapCheck = 0;
        PotionMove = GameObject.Find("PotionMove");
    }

    private void FixedUpdate()
    {
        if (isDrag)
        {
            this.transform.position = Input.mousePosition;
        }
    }

    public void ClickItem()
    {
        if (Equip)
        {
            isDrag = false;
            EquipItem();
            Equip = false;
        }
        else
        {
            if (overlapCheck < 2)
                isDrag = !isDrag;

            if (CheckSlot() == ItemX * ItemY && noneMove == false)
                GetSlotObject();
            else
                isDrag = true;

            if (isDrag == true)
            {
                DragItem();
            }
            if (isDrag == false && noneMove == false && overlapCheck < 2)
            {
                DropItem();
            }
        }
    }

    //아이템 드래그
    private void DragItem()
    {
        if (this.gameObject.name == "Hp_Potion(Clone)" || this.gameObject.name == "Mp_Potion(Clone)")
        {
            this.transform.SetParent(PotionMove.transform);
        }
        else
        {
            GameObject Parent = GameObject.Find("UnOccupied");
            this.transform.SetParent(Parent.transform);
        }
        this.gameObject.tag = "UnOccupied";
        ActiveSlot = false;

        if (this.gameObject.name == "Helmet(Clone)")
        {
            HelmetEquiped = false;
        }
        if (this.gameObject.name == "Chest(Clone)")
        {
            ChestEquiped = false;
        }
        if (this.gameObject.name == "Weapon(Clone)")
        {
            WeaponEquiped = false;
        }
    }
    
    //아이템 드랍(슬롯)
    private void DropItem()
    {
        GameObject Parent = GameObject.Find("Occupied");
        this.transform.SetParent(Parent.transform);
        this.transform.position = GetCenter();
        this.gameObject.tag = "Occupied";
    }

    //아이템 장착
    public void EquipItem()
    {
        if (this.gameObject.name == "Hp_Potion(Clone)" || this.gameObject.name == "Mp_Potion(Clone)")
        {
            Debug.Log("포션무브에 놓음");
            this.transform.SetParent(PotionMove.transform);
        }
        else
        {
            GameObject Parent = GameObject.Find("Occupied");
            this.transform.SetParent(Parent.transform);
        }

        this.transform.position = EquipArea.transform.position;
        this.gameObject.tag = "Occupied";
        ActiveSlot = true;

        if(this.gameObject.name == "Helmet(Clone)")
        {
            HelmetEquiped = true;
        }
        if(this.gameObject.name == "Chest(Clone)")
        {
            ChestEquiped = true;
        }
        if(this.gameObject.name == "Weapon(Clone)")
        {
            WeaponEquiped = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //체스트 장착 슬롯
        if (this.gameObject.name == "Chest(Clone)" && collision.gameObject.tag == "ChestES")
        {
            EquipArea = collision.gameObject;
            Equip = true;
        }

        //헬멧 장착 슬롯
        if (this.gameObject.name == "Helmet(Clone)" && collision.gameObject.tag == "HelmetES")
        {
            EquipArea = collision.gameObject;
            Equip = true;
        }

        //무기 장착 슬롯
        if (this.gameObject.name == "Weapon(Clone)" && collision.gameObject.tag == "WeaponES")
        {
            EquipArea = collision.gameObject;
            Equip = true;
        }

        //포션 장착 슬롯
        if ((this.gameObject.name == "Hp_Potion(Clone)" || this.gameObject.name == "Mp_Potion(Clone)")
            && collision.gameObject.tag == "PotionES")
        {
            EquipArea = collision.gameObject;
            Equip = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slot")
        {
            Slot slotScript;
            slotScript = collision.GetComponent<Slot>();
            ItemTrigger ChildScript = transform.GetComponentInChildren<ItemTrigger>();

            if (this.gameObject.tag == "Overlap")
            {
                slotScript.Overlap = true;
            }

            if(this.gameObject.tag == "Occupied")
            {
                slotScript.occupied = true;
                slotScript.ItemOnSlot = false;
                slotScript.Overlap = false;
            }
            else if(this.gameObject.tag == "UnOccupied")
            {
                slotScript.occupied = false;
                slotScript.ItemOnSlot = true;
                slotScript.Overlap = false;
            }
        }

        if(collision.gameObject.tag == "None-Move")
        {
            noneMove = true;
        }

        if(collision.gameObject.tag == "Occupied" && this.gameObject.tag == "Overlap")
        {
            isDrag = true;
            DragItem();
        }

        if (collision.gameObject.tag == "Overlap" && collision.gameObject.name != "ItemTrigger")
        {
            CheckOverlap();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slot")
        {
            Slot slotScript;
            slotScript = collision.GetComponent<Slot>();

            if (slotScript.ItemOnSlot)
            {
                slotScript.ItemOnSlot = false;
            }

            System.Array.Clear(slotObj, 0, slotObj.Length);
            checkSlot = 0;
        }

        if (collision.gameObject.tag == "None-Move")
        {
            noneMove = false;
        }

        if (collision.gameObject.tag == "Overlap")
        {
            overlapCheck = 0;
        }

        //체스트 장착 슬롯
            if (this.gameObject.name == "Chest(Clone)" && collision.gameObject.tag == "ChestES")
        {
            Equip = false;
        }

        //헬멧 장착 슬롯
        if (this.gameObject.name == "Helmet(Clone)" && collision.gameObject.tag == "HelmetES")
        {
            Equip = false;
        }

        //무기 장착 슬롯
        if (this.gameObject.name == "Weapon(Clone)" && collision.gameObject.tag == "WeaponES")
        {
            Equip = false;
        }

        //포션 장착 슬롯
        if ((this.gameObject.name == "Hp_Potion(Clone)" || this.gameObject.name == "Mp_Potion(Clone)")
            && collision.gameObject.tag == "PotionES")
        {
            Equip = false;
        }
    }

    //현재 아이템이 올라가있는 슬롯의 개수가 ItemX*ItemY와 일치하는지 확인하기 위해 checkSlot 반환
    int CheckSlot()
    {
        for (int y = 0; y < 4; y++) 
        {
            for (int x = 0; x < 10; x++) 
            {
                GameObject obj = GameObject.Find("slot[" + y + "," + x + "]");
                Slot slotScript;
                slotScript = obj.GetComponent<Slot>();

                if (slotScript.ItemOnSlot == true)
                {
                    checkSlot++;
                }
            }
        }
        Debug.Log(checkSlot);

        return checkSlot;
    }

    //현재 아이템이 올라가 있는 슬롯을 구함
    void GetSlotObject()
    {
        int index=0;

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                GameObject obj = GameObject.Find("slot[" + y + "," + x + "]");
                Slot slotScript;
                slotScript = obj.GetComponent<Slot>();

                if (slotScript.ItemOnSlot == true)
                {
                    slotObj[index] = obj.transform.position;
                    if (index < ItemX * ItemY)
                        index++;
                }
            }
        }
    }

    //ItemX by ItemY의 중앙에 아이템 배치
    Vector2 GetCenter()
    {
        float startX;
        float startY;
        float finalX;
        float finalY;
        Vector2 centerPos;

        startX = slotObj[0].x;
        startY = slotObj[0].y;

        finalX = slotObj[ItemX-1].x;
        finalY = slotObj[ItemX*ItemY-1].y;

        centerPos.x = startX - (startX - finalX) / 2;
        centerPos.y = startY - (startY - finalY) / 2;

        return centerPos;
    }

    void CheckOverlap()
    {
        overlapCheck = 0;
        GameObject SlotPanel = GameObject.Find("SlotPanel");
        GameObject obj;
        for (int i=0;i<11;i++)
        {
            obj = SlotPanel.GetComponent<SlotPanel>().Items[i];
            if (obj.tag == "Overlap")
            {
                overlapCheck++;
            }
        }
    }
}