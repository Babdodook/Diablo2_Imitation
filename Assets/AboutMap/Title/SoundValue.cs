using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundValue : MonoBehaviour
{
    public AudioSource[] sound = new AudioSource[2];
    float bgmvol, soundvol;
    // Start is called before the first frame update
    void Start()
    {
        bgmvol = PlayerPrefs.GetFloat("backvol", 0.8f);
        sound[0].volume = bgmvol;

        soundvol = PlayerPrefs.GetFloat("soundvol", 0.8f);
        sound[1].volume = soundvol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
