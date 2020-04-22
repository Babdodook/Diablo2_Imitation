using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{

    public GameObject distance;
    private GameObject player;
    public int Quick_data;
    private GameObject PSlot1;
    private GameObject PSlot2;
    private GameObject PSlot3;
    private GameObject PSlot4;
    private void Awake()
    {
        player = GameObject.Find("Sorceress");
        PSlot1 = GameObject.Find("PotionSlot1");
        PSlot2 = GameObject.Find("PotionSlot2");
        PSlot3 = GameObject.Find("PotionSlot3");
        PSlot4 = GameObject.Find("PotionSlot4");
    }
    public void OnME()
    {
        distance.SetActive(true);
    }
    public void OnMX()
    {
        distance.SetActive(false);
    }
    public void OnC()
    {
        distance.SetActive(false);
    }

    public void Sound()
    {
        AudioSource sound = GetComponent<AudioSource>();
        sound.Play();
    }

    private void Update()
    {
        if (ActiveSlot == true)
        {
            if (gameObject.transform.parent.name == "PotionMove")
            {
                if(gameObject.transform.position == PSlot1.transform.position)
                {
                   
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        Debug.Log("q1");
                        player.GetComponent<PlayerState>().Use_Potion(Quick_data);
                        Destroy(gameObject);
                    }
                }
                if (gameObject.transform.position == PSlot2.transform.position)
                {

                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        Debug.Log("q2");
                        player.GetComponent<PlayerState>().Use_Potion(Quick_data);
                        Destroy(gameObject);
                    }
                }
                if (gameObject.transform.position == PSlot3.transform.position)
                {

                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        Debug.Log("q3");
                        player.GetComponent<PlayerState>().Use_Potion(Quick_data);
                        Destroy(gameObject);
                    }
                }
                if (gameObject.transform.position == PSlot4.transform.position)
                {

                    if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        Debug.Log("q4");
                        player.GetComponent<PlayerState>().Use_Potion(Quick_data);
                        Destroy(gameObject);
                    }
                }
            }





            //    if (gameObject.transform.parent.name == "PotionSlot1")
            //{
            //    Debug.Log("d");
            //    if (Input.GetKeyDown(KeyCode.Alpha1))
            //    {
            //        player.GetComponent<PlayerState>().Use_Potion(Quick_data);
            //        Destroy(this);
            //    }
            //}
            //if (gameObject.transform.parent.name == "PotionSlot2")
            //{
            //    if (Input.GetKeyDown(KeyCode.Alpha2))
            //    {
            //        player.GetComponent<PlayerState>().Use_Potion(Quick_data);
            //        Destroy(this);
            //    }
            //}
            //if (gameObject.transform.parent.name == "PotionSlot3")
            //{
            //    if (Input.GetKeyDown(KeyCode.Alpha3))
            //    {
            //        player.GetComponent<PlayerState>().Use_Potion(Quick_data);
            //        Destroy(this);
            //    }
            //}
            //if (gameObject.transform.parent.name == "PotionSlot4")
            //{
            //    if (Input.GetKeyDown(KeyCode.Alpha4))
            //    {
            //        player.GetComponent<PlayerState>().Use_Potion(Quick_data);
            //        Destroy(this);
            //    }
            //}

        }
    }
}
