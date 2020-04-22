using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerState : MonoBehaviour {

    //레벨, 경험치
    public int level;
    public int exp;
    public int nextexp;

    //스킬포인트, 스탯포인트
    public int skillpoint;
    public int statepoint;

    //힘, 민첩성, 생명력, 에너지
    public int str;
    public int dex;
    public int life;
    public int energy;

    //스태미나, 라이프, 마나 , 자연 HP회복 , 자연 MP 회복
    public float stamina;
    public int hp;
    public int mp;
    public static int mpp;
    public int Recover_hp;
    public int Recover_mp;

    //최대 수치
    public int Max_stamina;
    public int Max_hp;
    public int Max_mp;

    //파이어 , 콜드, 라이트닝, 포이즌 저항력 , 가드
    public int fire_resistance;
    public int cold_resistance;
    public int lightning_resistance;
    public int poison_resistance;
    public int Guard;

    //골드
    public int gold;

    // 뛰기
    public bool run;

    //마을에서 스킬사용 금지
    public bool DoNotSkill = true;

    //스킬단축키입력
    public int A = -1;
    public int S = -1;
    public int D = -1;
    public int F = -1;
    public int G = -1;
    public int H = -1;
    public int L_Skill;
    public int R_Skill;

    public GameObject LB;
    public GameObject RB;

    public AudioSource LevelUpSFX;

    //초기값 설정
    private void Init()
    {
        level = 1;
        exp = 0;
        nextexp = 300;
        Guard = 0;

        skillpoint = 9;

        str = 10;
        dex = 12;
        life = 10;
        energy = 15;

        stamina = life * 10 + 100;
        hp = life * 15;
        mp = energy * 20;

        Max_stamina = life * 10 + 100;
        Max_hp = life * 15;
        Max_mp = energy * 20;

        fire_resistance = 10;
        cold_resistance = 10;
        lightning_resistance = 10;
        poison_resistance = 10;

        statepoint = 0;
        gold = 513854;
        Recover_hp = 1;
        Recover_mp = 1;
        run = false;
    }

    private void Awake()
    {
        Init();
    }
    public void Use_Potion(int n)
    {
        switch(n)
        {
            case 1:
                if (Max_hp > hp)
                {
                    hp += Max_hp / 3;
                    if (hp > Max_hp)
                        hp = Max_hp;
                }
                break;
            case 2:
                if (Max_mp > mp)
                {
                    mp += Max_mp / 3;
                    if (mp > Max_mp)
                        mp = Max_mp;
                }
                break;
        }
    }
    private void FixedUpdate()
    {
        mpp = Max_mp;
        if (exp >= nextexp)
        {
            LevelUp();
        }

        if(statepoint == 0)
           LB.SetActive(false);
        else
            LB.SetActive(true);

        if (skillpoint == 0)
            RB.SetActive(false);
        else
            RB.SetActive(true);

        //-------------------------

        if (hp < Max_hp)
        {
            Recover_hp += 1;
            if (Recover_hp >= 200)
            {
                hp += 5;
                Recover_hp = 1;
            }
        }
        if (mp < Max_mp)
        {
            Recover_mp += 1;
            if (Recover_mp >= 200)
            {
                mp += 20;
                Recover_mp = 1;
            }
        }
    }

    //레벨업 했을 시
    public void LevelUp()
    {
        LevelUpSFX.Play();

        exp -= nextexp;
        level += 1;
        nextexp = level * 300;  // 최대 경험치 공식
        skillpoint += 3;
        statepoint += 3;

        stamina = Max_stamina;
        hp = Max_hp;
        mp = Max_mp;
    }

    //힘 증가
    public void plusStr()
    {
        if (statepoint > 0)
        {
            GameObject.Find("Vol").GetComponent<AudioSource>().Play();

            str += 1;
            statepoint -= 1;

        }


    }

    //민첩성 증가
    public void plusDex()
    {

        if (statepoint > 0)
        {
            GameObject.Find("Vol").GetComponent<AudioSource>().Play();

            dex += 1;
            statepoint -= 1;

        }
    }

    //생명력 증가
    public void plusLife()
    {
        if (statepoint > 0)
        {
            GameObject.Find("Vol").GetComponent<AudioSource>().Play();

            life += 1;
            Max_stamina += 10;
            stamina = Max_stamina;
            Max_hp += 15;
            hp = Max_hp;
            Debug.Log("최대 hp" + Max_hp);
            statepoint -= 1;
        }
    }

    //에너지 증가
    public void plusEnergy()
    {
        if (statepoint > 0)
        {
            GameObject.Find("Vol").GetComponent<AudioSource>().Play();

            energy += 1;
            Max_mp += 20;
            mp = Max_mp;
            statepoint -= 1;
        }
    }
    public void Run()
    {
        run = true;
    }
    public void Walk()
    {
        run = false;
    }










}
