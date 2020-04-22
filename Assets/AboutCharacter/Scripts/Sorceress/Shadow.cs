using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    private ShadowMotion Shadow_m;

    Vector2 mousePos;

    void Start()
    {
        Shadow_m = GetComponent<ShadowMotion>();
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //멈춤
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Shadow_m.Stop(mousePos);
        }

        //걷기
        if (Input.GetMouseButton(0))
        {
            Shadow_m.Walk(mousePos);
        }

        //달리기
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
        {
            Shadow_m.Run(mousePos);
        }

        ////스킬사용
        //if (Input.GetKeyDown(KeyCode.Q) ||  //블리자드
        //    Input.GetKeyDown(KeyCode.W) ||  //아이스오브
        //    Input.GetKeyDown(KeyCode.E) ||  //마나쉴드
        //    Input.GetKeyDown(KeyCode.A) ||  //블레이즈
        //    Input.GetKeyDown(KeyCode.S) ||  //파이어월
        //    Input.GetKeyDown(KeyCode.D) ||  //파이어볼트
        //    Input.GetKeyDown(KeyCode.F) ||  //파이어볼
        //    Input.GetKeyDown(KeyCode.R))    //메테오
        //{
        //    Shadow_m.UseSkill(mousePos);
        //}
        //else
        //{
        //    Shadow_m.animator.SetBool("useSkill", false);
        //}
    }
}
