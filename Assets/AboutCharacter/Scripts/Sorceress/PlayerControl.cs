using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public GameObject m_Shadow;
    public AudioSource DieSound;
    private PlayerMotion Player_m;
    private PlayerState Player_s;
    private SkillManager SkillM;
    public GameObject TextB;
    public GameObject Text;
    DiabloControl Diablo_c;
    SkillDatabase SkillDB;
    public static bool DontMove = false;
    public bool ManaShield = false;
    public IzualAI IzualScript;

    public static bool HitSound = false;
    public bool izualdeath = false;

    public bool[] isDelay;
    bool soundDelay = false;
    bool once = false;
    bool once2 = false;

    bool skillmotion = false;

    IEnumerator NoWork, MapName;

    bool isHit = false;

    bool npcev = false;

    float speed;
    int num;

    public bool wait = true;
    public bool die = false;
    public static int tel = 0;

    GameObject roof, rooftile;
    GameObject obj, obj2, obj3;

    SpriteRenderer[] obj_spr, obj2_spr, obj3_spr;
    Rigidbody2D rd;

    float x, y;

    public GameObject loading;

    GameObject EnterMap;
    Image[] Map;

    Vector2 mousePos;

    void Start()
    {
        IzualScript = GetComponent<IzualAI>();

        SkillDB = GameObject.Find("SkillDatabase").GetComponent<SkillDatabase>();

        Diablo_c = GameObject.Find("Diablo").GetComponent<DiabloControl>();
        isDelay = new bool[10];
        System.Array.Clear(isDelay, 0, isDelay.Length);

        roof = GameObject.Find("FortressRoof");
        rooftile = GameObject.Find("Fortress Roofs");

        obj = GameObject.Find("FortressObject2");
        obj_spr = obj.gameObject.GetComponentsInChildren<SpriteRenderer>();

        obj2 = GameObject.Find("alpha01");
        obj2_spr = obj2.gameObject.GetComponentsInChildren<SpriteRenderer>();

        obj3 = GameObject.Find("alpha02");
        obj3_spr = obj3.gameObject.GetComponentsInChildren<SpriteRenderer>();

        EnterMap = GameObject.Find("EnterMap");
        Map = EnterMap.GetComponentsInChildren<Image>();

        rd = GetComponent<Rigidbody2D>();
        GameObject.Find("TownBGM").GetComponent<AudioSource>().Play();
        StartCoroutine(MapFadeIn());

        gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true;

        Player_m = GetComponent<PlayerMotion>();
        Player_s = GetComponent<PlayerState>();
        SkillM = GetComponent<SkillManager>();
        Player_m.animator.SetBool("isEquiped", false);

        wait = true;
        x = y = 0f;
    }

    void LoadingWait()
    {
        wait = false;
        Player_m.animator.SetBool("Walk", false);
        Player_m.animator.SetBool("Run", false);
        loading.SetActive(true);
        GameObject.Find("Cursor").GetComponent<Curser>().Waiting = true;
        StartCoroutine("LoadingWaitTime");
    }

    IEnumerator MapFadeIn()
    {
        float animTime = 3f;
        float start = 1f;
        float time = 0f;
        float end = 0f;

        Color c = Map[tel].color;
        c.a = Mathf.Lerp(start, end, time);

        for (int i = 0; i < Map.Length; i++)
        {
            if (Map[tel] != Map[i])
            {
                Map[i].color = new Color(1, 1, 1, 0);
            }
        }

        while (c.a > 0f)
        {
            time += Time.deltaTime / animTime;
            c.a = Mathf.Lerp(start, end, time);
            Map[tel].color = c;

            yield return null;
        }
    }

    IEnumerator LoadingWaitTime()
    {
        num = 0;

        while (!wait)
        {
            if (num > 3)
            {
                loading.SetActive(false);
                GameObject.Find("Cursor").GetComponent<Curser>().Waiting = false;
                wait = true;

                switch (tel)
                {
                    case 0:
                        break;
                    case 1:
                        break;

                    case 2:
                        break;
                }
            }
            num++;
            yield return new WaitForSeconds(0.50f);
        }
    }

    AudioSource RandomSound(string type1, string type2, string type3)
    {
        int soundNum;

        GameObject obj = null;
        AudioSource sound;

        soundNum = Random.Range(0, 3);
        switch (soundNum)
        {
            case 0:
                obj = GameObject.Find(type1);
                break;
            case 1:
                obj = GameObject.Find(type2);
                break;
            case 2:
                obj = GameObject.Find(type3);
                break;
        }
        sound = obj.GetComponent<AudioSource>();

        return sound;
    }

    IEnumerator HitDelay()
    {
        AudioSource sound = RandomSound("hit1", "hit2", "hit3");
        sound.Play();
        yield return new WaitForSeconds(1f);
        isHit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 대사 출력
        if (collision.gameObject.name == "EntryTrigger")
        {
            if (!once)
            {
                once = true;
                StartCoroutine(ShowText(140f, 20f, "enterfield", "사악한 자들은 각오해라!", 3f));
            }
        }

        //  스킬 히트
        if (collision.gameObject.tag == "Dia_Fire")
        {
            if (!isHit)
            {
                isHit = true;
                StartCoroutine(HitDelay());
            }

            if (ManaShield)
                Player_s.mp -= 10;
            else
                Player_s.hp -= 10;
        }
        if (collision.gameObject.tag == "Dia_Lazer")
        {
            if (!isHit)
            {
                isHit = true;
                StartCoroutine(HitDelay());
            }

            if (ManaShield)
                Player_s.mp -= 1;
            else
                Player_s.hp -= 1;
        }

        // 포탈
        //------------------------------------------------------------------------
        {
            if (collision.gameObject.name == "TownGate")
            {
                tel = 0;

                Player_m.isWalk = false;
                Player_m.isRun = false;
                Player_m.animator.SetBool("Walk", false);
                Player_m.animator.SetBool("Run", false);
                PlayerMotion.ray_dis = 0.4f;

                GameObject.Find("EnterSound").GetComponent<AudioSource>().Play();

                GameObject.Find("TownBGM").GetComponent<AudioSource>().Stop();
                GameObject.Find("HellBGM").GetComponent<AudioSource>().Stop();

                GameObject.Find("TownBGM").GetComponent<AudioSource>().Play();

                x = GameObject.Find("zone01").transform.position.x;
                y = GameObject.Find("zone01").transform.position.y;

                transform.position = new Vector2(x, y);

                if (MapName != null)
                    StopCoroutine(MapName);

                MapName = MapFadeIn();
                StartCoroutine(MapName);
            }

            if (collision.gameObject.name == "DiabloEnter")
            {
                if (tel != 1)
                {
                    tel = 1;

                    GameObject.Find("HellBGM").GetComponent<AudioSource>().Stop();
                    GameObject.Find("DiabloBGM").GetComponent<AudioSource>().Play();

                    gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "DiabloMap";
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5;

                    m_Shadow.GetComponent<SpriteRenderer>().sortingLayerName = "DiabloMap";
                    m_Shadow.GetComponent<SpriteRenderer>().sortingOrder = 4;

                    if (MapName != null)
                        StopCoroutine(MapName);

                    MapName = MapFadeIn();
                    StartCoroutine(MapName);
                }
            }

            if (collision.gameObject.name == "RiverEnter")
            {
                if (tel != 2)
                {
                    tel = 2;

                    GameObject.Find("DiabloBGM").GetComponent<AudioSource>().Stop();
                    GameObject.Find("HellBGM").GetComponent<AudioSource>().Play();

                    gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

                    m_Shadow.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                    m_Shadow.GetComponent<SpriteRenderer>().sortingOrder = -1;

                    if (MapName != null)
                        StopCoroutine(MapName);

                    MapName = MapFadeIn();
                    StartCoroutine(MapName);
                }
            }

            if (collision.gameObject.name == "RiverGate")
            {
                tel = 2;

                Player_m.isWalk = false;
                Player_m.isRun = false;
                Player_m.animator.SetBool("Walk", false);
                Player_m.animator.SetBool("Run", false);
                PlayerMotion.ray_dis = 0.3f;

                GameObject.Find("EnterSound").GetComponent<AudioSource>().Play();

                GameObject.Find("TownBGM").GetComponent<AudioSource>().Stop();
                GameObject.Find("HellBGM").GetComponent<AudioSource>().Play();

                x = GameObject.Find("zone03").transform.position.x;
                y = GameObject.Find("zone03").transform.position.y;

                transform.position = new Vector2(x, y);

                if (MapName != null)
                    StopCoroutine(MapName);

                MapName = MapFadeIn();
                StartCoroutine(MapName);
            }
        }
        //------------------------------------------------------------------------

        // town 건물 투명화
        //------------------------------------------------------------------------
        {
            if (collision.gameObject.name == "RoofIn")
            {
                if (roof != null)
                    roof.SetActive(false);
                if (rooftile != null)
                    rooftile.SetActive(false);

                Color s = new Color(1, 1, 1, 0.3f);

                if (obj != null)
                {
                    for (int i = 0; i < obj_spr.Length; i++)
                    {
                        obj_spr[i].color = s;
                    }
                }
            }

            if (collision.gameObject.name == "roofin2")
            {
                Color s = new Color(1, 1, 1, 0.3f);

                if (obj2 != null)
                {
                    for (int i = 0; i < obj2_spr.Length; i++)
                    {
                        obj2_spr[i].color = s;
                    }
                }
            }

            if (collision.gameObject.name == "roofin3")
            {
                Color s = new Color(1, 1, 1, 0.3f);

                if (obj3 != null)
                {
                    for (int i = 0; i < obj3_spr.Length; i++)
                    {
                        obj3_spr[i].color = s;
                    }
                }
            }

            if (collision.gameObject.name == "RoofOut")
            {
                if (roof != null)
                    roof.SetActive(true);
                if (rooftile != null)
                    rooftile.SetActive(true);

                Color s = new Color(1, 1, 1, 1f);

                if (obj != null)
                {
                    for (int i = 0; i < obj_spr.Length; i++)
                    {
                        obj_spr[i].color = s;
                    }
                }
            }

            if (collision.gameObject.name == "roofout2")
            {
                Color s = new Color(1, 1, 1, 1f);

                if (obj2 != null)
                {
                    for (int i = 0; i < obj2_spr.Length; i++)
                    {
                        obj2_spr[i].color = s;
                    }
                }
            }

            if (collision.gameObject.name == "roofout3")
            {
                Color s = new Color(1, 1, 1, 1f);

                if (obj3 != null)
                {
                    for (int i = 0; i < obj3_spr.Length; i++)
                    {
                        obj3_spr[i].color = s;
                    }
                }
            }
        }
        //------------------------------------------------------------------------
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 캐릭터 이동 관련
        //------------------------------------------------------------------------
        //if (collision.gameObject.tag == "npc")
        //{
        //    GameObject temp = GameObject.Find("NPC");
        //    if (temp.GetComponent<NPC>().MouseState())
        //    {
        //        npcev = true;
        //    }
        //}
        //------------------------------------------------------------------------

        // 레이어 관련
        //------------------------------------------------------------------------
        if (collision.gameObject.tag == "Layer_0")
        {
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            m_Shadow.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
        if (collision.gameObject.tag == "Layer_1")
        {
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            m_Shadow.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        if (collision.gameObject.tag == "Layer_2")
        {
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
            m_Shadow.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        if (collision.gameObject.tag == "Layer_3")
        {
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
            m_Shadow.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 레이어 관련
        //------------------------------------------------------------------------
        if (collision.gameObject.tag == "Layer_0" || collision.gameObject.tag == "Layer_1" ||
            collision.gameObject.tag == "Layer_2" || collision.gameObject.tag == "Layer_3" ||
            collision.gameObject.tag == "Layer_4")
        {
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
            m_Shadow.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "NoWork" || collision.gameObject.tag == "Alpha")
        {
            StopCoroutine(NoWork);
            npcev = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "NoWork" || collision.gameObject.tag == "Alpha")
        {
            NoWork = NoWorkWait();
            StartCoroutine(NoWork);
        }
    }

    IEnumerator NoWorkWait()
    {
        yield return new WaitForSeconds(0.7f);
        npcev = true;
    }

    public void Wait()
    {
        wait = false;

        Player_m.animator.SetBool("Walk", false);
        Player_m.animator.SetBool("Run", false);
        Player_m.isWalk = false;
        Player_m.isRun = false;
    }

    public void Play()
    {
        wait = true;
    }

    public void Stop()
    {
        npcev = true;
    }

    void Update()
    {
        if (Player_m == null)
            Player_m = GetComponent<PlayerMotion>();
        if (Player_s == null)
            Player_s = GetComponent<PlayerState>();
        if (SkillM == null)
            SkillM = GetComponent<SkillManager>();
        if (IzualScript == null)
            IzualScript = GetComponent<IzualAI>();

        // --------------------------------- 대사
        if (HitSound)
        {
            if (!isHit)
            {
                isHit = true;
                StartCoroutine(HitSoundDelay());
            }
        }

        if (izualdeath && IzualSoul.SoulExit)
        {
            izualdeath = false;
            StartCoroutine(ShowText(150f, 20f, "freeIzual", "그는 마음속 깊히 타락해버렸어. 난 그를 동정해", 3f));
        }

        if (wait && !die)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (tel == 0)
            {
                gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

                m_Shadow.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                m_Shadow.GetComponent<SpriteRenderer>().sortingOrder = -1;

                gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
                gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
            }

            if (tel == 2)
            {
                gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
                gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
            }

            //멈춤
            if (Input.GetKey(KeyCode.LeftShift) || npcev)
            {
                Player_m.isShift = true;
                Player_m.animator.SetBool("Walk", false);
                Player_m.animator.SetBool("Run", false);
                Player_m.isWalk = false;
                Player_m.isRun = false;
                npcev = false;
            }
            else
            {
                Player_m.isShift = false;
            }

            //걷기
            if (Input.GetMouseButton(0) && !DontMove)
            {
                if (!skillmotion)
                    Player_m.Walk(mousePos);
            }

            //달리기
            if ((Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl)) ||
                (Input.GetMouseButton(0) && UI_StateManager.Rbutton) && !DontMove)
            {
                if (!skillmotion)
                    Player_m.Run(mousePos);
            }

            else
            {
                if (!UI_StateManager.Rbutton)
                {
                    Player_m.isRun = false;
                    Player_m.animator.SetBool("Run", false);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if(skillmotion)
                    skillmotion = false;
            }

            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift) && !DontMove)
            {
                Use_Skill(Player_s.L_Skill);
                if (Player_s.L_Skill != 0 && Player_s.L_Skill != 6 && Player_s.L_Skill != 7 && tel != 0)
                    Player_m.UseSkill(mousePos);
            }

            else if (Input.GetMouseButtonDown(1) && Player_m.isCanUseSkill && !DontMove && !skillmotion)
            {
                Player_m.animator.SetBool("Walk", false);
                Player_m.animator.SetBool("Run", false);
                Player_m.isWalk = false;
                Player_m.isRun = false;

                if (Input.GetMouseButton(0))
                    skillmotion = true;

                Use_Skill(Player_s.R_Skill);
                if (Player_s.R_Skill != 0 && Player_s.R_Skill != 6 && Player_s.R_Skill != 7 && tel != 0)
                    Player_m.UseSkill(mousePos);
            }

            else if (Input.GetMouseButtonDown(1) && !Player_m.isCanUseSkill && !DontMove && !skillmotion)
            {
                Player_m.animator.SetBool("Walk", false);
                Player_m.animator.SetBool("Run", false);
                Player_m.isWalk = false;
                Player_m.isRun = false;

                if (Input.GetMouseButton(0))
                    skillmotion = true;

                Use_Skill(Player_s.R_Skill);
            }

            else
            {
                Player_m.animator.SetBool("useSkill", false);
                Player_m.animator.SetBool("isEquiped", true);
                Player_m.isUseSkill = false;
            }

            if (Player_s.hp <= 0)
            {
                Player_m.isWalk = false;
                Player_m.isRun = false;

                gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            
                gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                gameObject.GetComponent<CircleCollider2D>().enabled = false;

                DieSound.Play();
                Player_m.animator.SetTrigger("Death");
                m_Shadow.SetActive(false);
                die = true;
            }
        }
    }

    //*******************************************************
    //  스킬사용
    //*******************************************************

    public void Use_Skill(int n) // 스킬사용함수
    {
        if (n != 0 && tel != 0)
        {
            switch (n)
            {
                case 1:     //블리자드
                    if (!isDelay[1] && ((Player_s.mp - SkillDB.Blizzard_mp) >= 0))
                    {
                        isDelay[1] = true;
                        Player_m.UseSkill(mousePos);
                        SkillM.UseBlizzard(mousePos, this.transform.position);
                        StartCoroutine(SkillDelay(1, 5));

                        Player_s.mp -= SkillDB.Blizzard_mp;
                    }
                    else if ((Player_s.mp - SkillDB.Blizzard_mp) < 0)
                    {
                        if (!soundDelay)
                        {
                            StartCoroutine(ManasoundDelay());
                        }
                    }
                    else
                    {
                        if (!soundDelay)
                        {
                            StartCoroutine(CantsoundDelay());
                        }
                    }
                    break;
                case 2:     //아이스오브
                    if (!isDelay[2] && ((Player_s.mp - SkillDB.Iceorb_mp) >= 0))
                    {
                        isDelay[2] = true;
                        Player_m.UseSkill(mousePos);
                        SkillM.UseIceBolt(mousePos, this.transform.position);
                        StartCoroutine(SkillDelay(2, 5));

                        Player_s.mp -= SkillDB.Iceorb_mp;
                    }
                    else if ((Player_s.mp - SkillDB.Iceorb_mp) < 0)
                    {
                        if (!soundDelay)
                        {
                            StartCoroutine(ManasoundDelay());
                        }
                    }
                    else
                    {
                        if (!soundDelay)
                        {
                            StartCoroutine(CantsoundDelay());
                        }
                    }
                    break;
                case 3:     //마나쉴드
                    if (!isDelay[3])
                    {
                        isDelay[3] = true;
                        Player_m.UseSkill(mousePos);
                        SkillM.UseManaShield(this.transform.position);

                        StartCoroutine(SkillDelay(3, 70));

                        ManaShield = true;
                    }
                    else
                    {
                        if (!soundDelay)
                        {
                            StartCoroutine(CantsoundDelay());
                        }
                    }
                    break;
                case 4:     //블레이즈
                    if (!isDelay[4] && ((Player_s.mp - SkillDB.Blaze_mp) >= 0))
                    {
                        isDelay[4] = true;
                        Player_m.UseSkill(mousePos);
                        SkillM.UseBlaze();
                        StartCoroutine(SkillDelay(4, 10));

                        Player_s.mp -= SkillDB.Blaze_mp;
                    }
                    else if ((Player_s.mp - SkillDB.Blaze_mp) < 0)
                    {
                        if (!soundDelay)
                        {
                            StartCoroutine(ManasoundDelay());
                        }
                    }
                    else
                    {
                        if (!soundDelay)
                        {
                            StartCoroutine(CantsoundDelay());
                        }
                    }
                    break;
                case 5:     //파이어월
                    if (!isDelay[5] && ((Player_s.mp - SkillDB.Firewall_mp) >= 0))
                    {
                        isDelay[5] = true;
                        Player_m.UseSkill(mousePos);
                        SkillM.UseFireWall(mousePos);
                        StartCoroutine(SkillDelay(5, 10));

                        Player_s.mp -= SkillDB.Firewall_mp;
                    }
                    else if ((Player_s.mp - SkillDB.Firewall_mp) < 0)
                    {
                        if (!soundDelay)
                        {
                            StartCoroutine(ManasoundDelay());
                        }
                    }
                    else
                    {
                        if (!soundDelay)
                        {
                            StartCoroutine(CantsoundDelay());
                        }
                    }
                    break;
                case 6:     //파이어볼트
                    if (!isDelay[6] && ((Player_s.mp - SkillDB.Firebolt_mp) >= 0))
                    {
                        isDelay[6] = true;
                        Player_m.UseSkill(mousePos);
                        SkillM.UseFirebolt();
                        StartCoroutine(SkillDelay(6, 0.3f));

                        Player_s.mp -= SkillDB.Firebolt_mp;
                    }
                    else if ((Player_s.mp - SkillDB.Firebolt_mp) < 0)
                    {
                        if (!soundDelay)
                        {
                            StartCoroutine(ManasoundDelay());
                        }
                    }
                    break;
                case 7:     //파이어볼
                    if (!isDelay[7] && ((Player_s.mp - SkillDB.Fireball_mp) >= 0))
                    {
                        isDelay[7] = true;
                        Player_m.UseSkill(mousePos);
                        SkillM.UseFireball();
                        StartCoroutine(SkillDelay(7, 0.3f));

                        Player_s.mp -= SkillDB.Fireball_mp;
                    }
                    else if ((Player_s.mp - SkillDB.Fireball_mp) < 0)
                    {
                        if (!soundDelay)
                        {
                            StartCoroutine(ManasoundDelay());
                        }
                    }
                    break;
                case 8:     //메테오
                    if (!isDelay[8] && ((Player_s.mp - SkillDB.Meteo_mp) >= 0))
                    {
                        isDelay[8] = true;
                        Player_m.UseSkill(mousePos);
                        SkillM.UseMeteor(mousePos);
                        StartCoroutine(SkillDelay(8, 6));

                        Player_s.mp -= SkillDB.Meteo_mp;
                    }
                    else if ((Player_s.mp - SkillDB.Meteo_mp) < 0)
                    {
                        if (!soundDelay)
                        {
                            StartCoroutine(ManasoundDelay());
                        }
                    }
                    else
                    {
                        if (!soundDelay)
                        {
                            StartCoroutine(CantsoundDelay());
                        }
                    }
                    break;
            }
        }
        else
        {
            if (!soundDelay)
                StartCoroutine(CantsoundDelay());
            return;
        }
        Player_m.isUseSkill = false;
        Player_m.animator.SetBool("useSkill", false);
    }

    IEnumerator HitSoundDelay()
    {
        AudioSource sound = RandomSound("hit1", "hit2", "hit3");
        sound.Play();
        yield return new WaitForSeconds(1f);
        isHit = false;
        HitSound = false;
    }

    IEnumerator SkillDelay(int index, float time)
    {
        yield return new WaitForSeconds(time);
        isDelay[index] = false;
    }

    IEnumerator CantsoundDelay()
    {
        soundDelay = true;
        AudioSource sound = RandomSound("cant1", "cant2", "cant3");
        sound.Play();
        TextB.SetActive(true);
        TextB.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150f);
        TextB.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20f);

        Text.SetActive(true);
        Text.GetComponent<Text>().text = "지금은 사용할 수 없어";
        yield return new WaitForSeconds(1.5f);
        TextB.SetActive(false);
        Text.SetActive(false);
        soundDelay = false;
    }

    IEnumerator ManasoundDelay()
    {
        soundDelay = true;
        AudioSource sound = RandomSound("mana1", "mana2", "mana3");
        sound.Play();
        TextB.SetActive(true);
        TextB.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100f);
        TextB.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20f);

        Text.SetActive(true);
        Text.GetComponent<Text>().text = "마나가 부족해";
        yield return new WaitForSeconds(1.5f);
        TextB.SetActive(false);
        Text.SetActive(false);
        soundDelay = false;
    }

    public IEnumerator ShowText(float width, float height, string name, string text, float time)
    {
        AudioSource sound = GameObject.Find(name).GetComponent<AudioSource>();
        sound.Play();
        TextB.SetActive(true);
        TextB.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        TextB.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        Text.SetActive(true);
        Text.GetComponent<Text>().text = text;
        yield return new WaitForSeconds(time);
        TextB.SetActive(false);
        Text.SetActive(false);
        soundDelay = false;
    }
}
