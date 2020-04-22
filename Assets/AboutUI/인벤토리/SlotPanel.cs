using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotPanel : MonoBehaviour
{
    public GameObject player;
    public GameObject shadow;
    public GameObject WeaponSlot;
    public GameObject Helmet;
    public GameObject Armor;
    public GameObject hp_Potion;
    public GameObject mp_Potion;
    public GameObject Weapon;

    //장착 확인 인스턴스
    private GameObject HelmetCheck;
    private GameObject ChestCheck;
    private GameObject WeaponCheck;

    public GameObject startPos;
    public GameObject Slot;

    [HideInInspector] public GameObject[] Items;
    int index;
    
    float startSlotX;   //슬롯 시작 좌표
    float startSlotY;   //슬롯 시작 좌표
    Vector2 mousePos;

    private void Start()
    {
        Items = new GameObject[30];
        System.Array.Clear(Items, 0, Items.Length);
        index = 0;

        //슬롯 시작 좌표
        startSlotX = startPos.transform.localPosition.x;
        startSlotY = startPos.transform.localPosition.y;

        //슬롯셋팅
        SetSlot();
        //아이템 배치
        CreateItems();

        HelmetCheck = GameObject.Find("Helmet(Clone)");
        ChestCheck = GameObject.Find("Chest(Clone)");
        WeaponCheck = GameObject.Find("Weapon(Clone)");
    }

    private void FixedUpdate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CheckEmptySlot();
        IsAllEquiped();
    }

    //초기 슬롯 생성
    private void SetSlot()
    {
        float yindex = 0;
        int index = 0;

        float width = startPos.GetComponent<RectTransform>().rect.width;
        float height = startPos.GetComponent<RectTransform>().rect.height;
        for (int y = 0; y < 4; y++) 
        {
            for (int x = 0; x < 10; x++) 
            {
                GameObject obj = Instantiate(Slot) as GameObject;

                obj.transform.name = "slot[" + y + "," + x + "]";
                obj.transform.SetParent(this.transform, false);
                obj.transform.localPosition = new Vector2(startSlotX + x * 30 + x, startSlotY - y * 25 - y*3.8f);

                obj.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                obj.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height+1);

                obj.transform.GetComponent<Slot>().index = index;

                obj.transform.GetComponent<Slot>().slotPos = new Vector2(y, x);

                index++;
            }
            yindex += 0.6f;
        }
    }

    //초기 아이템 생성
    // index by index, Name of Object, Type of Object
    public void SetItem(int itemX, int itemY, GameObject obj, string ItemType)
    { 
        int isOccupied = -1;

        int start_spaceX;
        int start_spaceY;
        int final_spaceX;
        int final_spaceY;

        int checkx;
        int checky;
        
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                isOccupied = -1;
                GameObject slotObject = GameObject.Find("slot[" + y + "," + x + "]");
                Slot slot = slotObject.GetComponent<Slot>();
                start_spaceX = x;
                start_spaceY = y;

                checkx = x;
                checky = y;

                //슬롯이 사용중인지
                if (!slot.occupied)
                {
                    //슬롯이 비었을 경우 아이템에 맞는 itemX x itemY칸 체크
                    for (int slotY = checky; slotY < itemY+checky; slotY++)
                    {
                        for (int slotX = checkx; slotX < itemX+checkx; slotX++)
                        {
                            GameObject tempslot = GameObject.Find("slot[" + slotY + "," + slotX + "]");
                            Slot checkslot = tempslot.GetComponent<Slot>();
                            //슬롯이 사용중, 브레이크
                            if (checkslot.occupied)
                            {
                                isOccupied = 1; //사용중
                                break;
                            }
                        }
                    }

                    if(isOccupied != 1)
                        isOccupied = 0;
                }
                else
                {
                    continue;
                }

                //체크 결과 칸이 사용중이 아닐때
                //아이템 생성
                if (isOccupied==0)
                {
                    //Item ItemInfo = null;
                    //switch (ItemType)
                    //{
                    //    case "Armor":
                    //        ItemInfo = obj.GetComponent<Armor>();
                    //        break;
                    //    case "Helmet":
                    //        break;
                    //    case "Weapon":
                    //        break;
                    //    case "Potion":
                    //        break;
                    //}

                    //ItemInfo.XbyY = new Vector2(itemX, itemY);

                    final_spaceX = itemX - 1 + start_spaceX;
                    final_spaceY = itemY - 1 + start_spaceY;

                    //아이템 인스턴스
                    GameObject item = Instantiate(obj) as GameObject;
                    GameObject Occupied = GameObject.Find("Occupied");

                    item.GetComponent<Item>().ItemX = itemX;
                    item.GetComponent<Item>().ItemY = itemY;

                    //아이템 중심 좌표 계산
                    GameObject finalslotX = GameObject.Find("slot[" + start_spaceY + "," + final_spaceX + "]");
                    GameObject finalslotY = GameObject.Find("slot[" + final_spaceY + "," + start_spaceX + "]");

                    float xpos = slotObject.transform.position.x - (slotObject.transform.position.x - finalslotX.transform.position.x) / 2;
                    float ypos = slotObject.transform.position.y - (slotObject.transform.position.y - finalslotY.transform.position.y) / 2;

                    //ItemX x ItemY 칸 중앙에 배치
                    item.transform.SetParent(Occupied.transform, false);
                    item.transform.position = new Vector3(xpos, ypos, 0f);
                    

                    int tempX = start_spaceX;
                    for (int iy = 0; iy < itemY; iy++)
                    {
                        for (int ix = 0; ix <itemX; ix++)
                        {
                            GameObject slotObj = GameObject.Find("slot[" + start_spaceY + "," + start_spaceX + "]");
                            slotObj.GetComponent<Slot>().occupied = true;
                            //ItemInfo.slotPos[iy,ix]=
                            //Debug.Log("[" + start_spaceY + "," + start_spaceX + "]");
                            start_spaceX++;
                        }
                        start_spaceY++;
                        start_spaceX = tempX;
                    }

                    //아이템 배열에 추가
                    Items[index++] = item;

                    break;
                }
            }
            if (isOccupied==0)
                break;
        }
    }

    void CheckEmptySlot()
    {
        Slot slotScript;
        Color Gray = new Color(255, 255, 255, 0.15f);
        Color Blue = new Color(0, 0, 255, 0.15f);

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 10; x++) 
            {
                GameObject slot = GameObject.Find("slot[" + y + "," + x + "]");
                slotScript = slot.GetComponent<Slot>();

                if (slotScript.isOccupied == false && slotScript.img.color == Blue)
                {
                    slotScript.occupied = false;
                }
                if (slotScript.occupied == true && slotScript.img.color == Gray)
                {
                    slotScript.occupied = false;
                    slotScript.ItemOnSlot = false;
                    slotScript.Overlap = false;
                }
                if (slotScript.occupied == false && slotScript.img.color == Gray)
                {
                    slotScript.occupied = false;
                    slotScript.ItemOnSlot = false;
                    slotScript.Overlap = false;
                }
            }
        }
    }

    void IsAllEquiped()
    {
        if (HelmetCheck.GetComponent<Helmet>().HelmetEquiped &&
            ChestCheck.GetComponent<Armor>().ChestEquiped &&
            WeaponCheck.GetComponent<Weapon>().WeaponEquiped)
        {
            player.GetComponent<PlayerMotion>().ArmorEquiped(true);
            shadow.GetComponent<ShadowMotion>().ArmorEquiped(true);
        }
        else
        {
            player.GetComponent<PlayerMotion>().ArmorEquiped(false);
            shadow.GetComponent<ShadowMotion>().ArmorEquiped(false);
        }

        if (WeaponCheck.GetComponent<Weapon>().ActiveSlot)
        {
            WeaponSlot.SetActive(true);
        }
        else
            WeaponSlot.SetActive(false);
    }

    void CreateItems()
    {
        SetItem(2, 3, Armor, "Armor");
        SetItem(2, 2, Helmet, "Helmet");
        SetItem(1, 4, Weapon, "Weapon");
        SetItem(1, 1, hp_Potion, "Potion");
        SetItem(1, 1, hp_Potion, "Potion");
        SetItem(1, 1, hp_Potion, "Potion");
        SetItem(1, 1, hp_Potion, "Potion");
        SetItem(1, 1, mp_Potion, "Potion");
        SetItem(1, 1, mp_Potion, "Potion");
        SetItem(1, 1, mp_Potion, "Potion");
        SetItem(1, 1, mp_Potion, "Potion");
    }
}
