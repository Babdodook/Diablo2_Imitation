using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : Item
{
    public GameObject distance;
    public string Name;
    public int str;
    public int dex;
    public int life;
    public int energy;
    public int defense;
    private GameObject player;
    private PlayerState Player_s;
    public bool active = false;

    private void Awake()
    {
        player = GameObject.Find("Sorceress");
        Player_s = player.GetComponent<PlayerState>();
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
        if (ChestEquiped && active == false)
        {
            Player_s.energy += 30;

            active = true;
        }
        else if (ChestEquiped == false && active == true)
        {
            Player_s.energy -= 30;

            active = false;
        }

    }
}