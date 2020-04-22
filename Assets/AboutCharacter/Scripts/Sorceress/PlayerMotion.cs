using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour {
    public Animator animator;
    PlayerState state;

    public float walk_speed = 0.5f;
    public float run_speed = 1.0f;
    
    Vector2 oldMousePos;

    public bool isWalk;
    public bool isRun;
    public bool isUseSkill;
    public bool isShift;
    public bool isCanUseSkill;
    public bool Move = false;

    public static float ray_dis;

    // Use this for initialization
    void Start () {
        isWalk = false;
        isRun = false;
        isUseSkill = false;
        isShift = false;
        isCanUseSkill = true;
        animator = GetComponent<Animator>();
        state = GetComponent<PlayerState>();

        ray_dis = 0.4f;
    }

    void FixedUpdate()
    {
        if (isWalk)
        {
            transform.position = Vector2.MoveTowards(transform.position, oldMousePos, walk_speed * Time.deltaTime);

            float dis = Vector2.Distance(oldMousePos, transform.position);
            Vector2 dir = (oldMousePos - (Vector2)transform.position) * 180 / Mathf.PI;

            Ray2D ray = new Ray2D(transform.position, dir);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, ray_dis, 1 << LayerMask.NameToLayer("PathFinding"));
            Debug.DrawRay(ray.origin, ray.direction * ray_dis, Color.red);

            if (hit)
            {
                if (!isRun || UI_StateManager.Rbutton)
                {
                    if (hit.collider.gameObject.tag == "Wall" || hit.collider.gameObject.tag == "Alpha")
                    {
                        isWalk = false;
                        isRun = false;
                        animator.SetBool("Walk", false);
                        animator.SetBool("Run", false);
                    }
                }
            }

            if (transform.position.x == oldMousePos.x && transform.position.y == oldMousePos.y)
            {
                animator.SetBool("Walk", false);
                isWalk = false;
            }
        }

        if (isRun && !isShift && (state.stamina > 0))
        {
            transform.position = Vector2.MoveTowards(transform.position, oldMousePos, run_speed * Time.deltaTime);
            if (transform.position.x == oldMousePos.x && transform.position.y == oldMousePos.y)
            {
                animator.SetBool("Run", false);
                isRun = false;
            }
        }

        if (state.stamina <= 0)
        {
            isRun = false;
            animator.SetBool("Run", false);
        }
    }
    
    //걷기 메소드
    public void Walk(Vector2 mousePos)
    {
        animator.SetFloat("Angle", GetDegree(transform.position, mousePos));

        if (!isRun && !isUseSkill)      //달리기, 스킬사용 중 아닐때
        {
            if (!isShift)               //멈춤상태 아닐때
            {
                animator.SetBool("Walk", true);
                oldMousePos = mousePos;
                isWalk = true;
            }
        }
    }

    //달리기 메소드
    public void Run(Vector2 mousePos)
    {
        if (!isUseSkill && !isShift && (state.stamina > 0))
        {
            animator.SetFloat("Angle", GetDegree(transform.position, mousePos));

            oldMousePos = mousePos;
            animator.SetBool("Run", true);
            isRun = true;
        }
    }

    public void UseSkill(Vector2 mousePos)
    {        
            animator.SetFloat("Angle", GetDegree(transform.position, mousePos));

            animator.SetBool("useSkill", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);

            isUseSkill = true;
            isWalk = false;
            isRun = false;
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
