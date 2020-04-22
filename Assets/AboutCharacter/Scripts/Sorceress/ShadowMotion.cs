using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMotion : MonoBehaviour
{
    private GameObject Player;
    public Animator animator;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Sorceress");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Player.GetComponent<PlayerMotion>().isWalk == false)
        {
            animator.SetBool("Walk", false);
        }

        if(Player.GetComponent<PlayerMotion>().isRun == false)
        {
            animator.SetBool("Run", false);
        }
    }

    public void Stop(Vector2 mousePos)
    {
        animator.SetFloat("Angle", GetDegree(transform.position, mousePos));
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
    }

    //걷기 메소드
    public void Walk(Vector2 mousePos)
    {
        if (Player.GetComponent<PlayerMotion>().isWalk)      //달리기, 스킬사용 중 아닐때
        {
            animator.SetFloat("Angle", GetDegree(transform.position, mousePos));

            animator.SetBool("Walk", true);
        }
    }

    //달리기 메소드
    public void Run(Vector2 mousePos)
    {
        if (Player.GetComponent<PlayerMotion>().isRun)
        {
            animator.SetFloat("Angle", GetDegree(transform.position, mousePos));

            animator.SetBool("Run", true);
        }
    }

    public void UseSkill(Vector2 mousePos)
    {
        animator.SetFloat("Angle", GetDegree(transform.position, mousePos));

        animator.SetBool("useSkill", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
    }

    public void ArmorEquiped(bool type)
    {
        animator.SetBool("ArmorEquiped",type);
    }

    private float GetDegree(Vector2 playerPos, Vector2 mousePos)
    {
        float angle = Mathf.Atan2(mousePos.y - playerPos.y, mousePos.x - playerPos.x) * 180 / Mathf.PI;
        if (angle < 0) angle += 360;

        return angle;
    }
}
