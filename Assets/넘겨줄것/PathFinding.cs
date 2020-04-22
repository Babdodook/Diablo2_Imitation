using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*  PathFinding Algorithm
 *  1. 노드리스트에 모든 노드의 정보를 저장
 *  2. 스타트 포지션에서 타겟 포지션으로 레이캐스팅
 *  3. 벽에 맞았다면 -> 길 찾기 시작
 *  4. 스타트 포지션에서 노드리스트에 있는 노드(모든 노드)에 레이캐스팅
 *  5. 조건검사 : 스타트 포지션과 노드 사이에 벽이 있는가?
 *     - 위 과정에서 RaycastAll은 순차적으로 히트 스캔을 하지 않는다
 *     
 *       벽이 스캔된 경우
 *       스타트 포지션 - 노드, 스타트 포지션 - 벽의 거리를 비교하여
 *       노드가 더 가까이 있다면 OpenList에 insert한다.
 *       
 *       노드만 스캔된 경우
 *       별다른 과정없이 OpenList에 insert
 *       
 *       
 */

public class PathFinding : MonoBehaviour
{
    List<GameObject> NodeList = new List<GameObject>();

    GameObject NowNode;
    public Vector3 MovePos;
    Vector3 TargetPos;

    int index = 150;
    public bool isWall = false;
    public bool isMove = false;
    public float Angle;

    private void Start()
    {
        NowNode = null;

        for (int i = 0; i < index; i++)
        {
            if (GameObject.Find("Node[" + i + "]") != null)
            {
                GameObject Node = GameObject.Find("Node[" + i + "]");
                NodeList.Add(Node);
            }
        }
    }
    public void Raycasting(Vector3 TargetPos)
    {
        this.TargetPos = TargetPos;

        if (!isWall)
        {
            float distance = (TargetPos - this.transform.position).magnitude;         //타겟과 스타트 포지션 사이 거리
            Vector3 direction = (TargetPos - this.transform.position) / distance;     //방향 계산

            Ray2D ray = new Ray2D(this.transform.position, direction);                //타겟으로 향하는 레이
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, 2f, 1 << LayerMask.NameToLayer("PathFinding"));
            Debug.DrawRay(ray.origin, ray.direction, Color.red);

            List<GameObject> wallList = new List<GameObject>();
            List<float> wallDistance = new List<float>();

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit)
                {
                    if (hit.collider.gameObject.tag == "Wall")   //레이가 벽에 맞았을 시
                    {
                        wallList.Add(hit.collider.gameObject);
                        wallDistance.Add((hit.collider.gameObject.transform.position - this.transform.position).magnitude);

                        // Debug.Log("벽에 맞음");

                        //isWall = true;
                        //StartPathFinding();      //길 찾기 시작
                        //break;
                    }
                }
            }

            wallDistance.Sort();

            GameObject wall = null;

            foreach (GameObject _wall in wallList)
            {
                if ((_wall.transform.position - this.transform.position).magnitude == wallDistance[0])
                {
                    wall = _wall;
                }
            }

            if (wall != null)                           // 벽이 존재할 경우
                                                        // 스타트 포지션에서 타겟과 벽의 거리를 비교한다
            {
                if ((TargetPos - this.transform.position).magnitude
                    > (wall.transform.position - this.transform.position).magnitude)        //벽이 앞에 있는 경우
                {
                    isWall = true;
                    StartPathFinding();     //길 찾기 시작
                }
            }


        }
    }

    public void StartPathFinding()
    {
        List<float> Distance = new List<float>();
        List<GameObject> OpenList = new List<GameObject>();

        //노드 레이 체크
        foreach (GameObject item in NodeList)
        {
            bool insert = false;

            float distance = (item.transform.position - this.transform.position).magnitude;         //노드와 스타트 포지션 사이 거리
            Vector3 direction = (item.transform.position - this.transform.position) / distance;     //방향 계산

            Ray2D ray = new Ray2D(this.transform.position, direction);                              //노드로 향하는 레이
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, 1 << LayerMask.NameToLayer("PathFinding"));
            Debug.DrawRay(ray.origin, ray.direction * 8f, Color.red);

            Debug.Log("------------레이 검사--------------");

            List<GameObject> wallList = new List<GameObject>();
            List<float> wallDistance = new List<float>();

            for (int i = 0; i < hits.Length; i++)              //히트 스캔 중 벽이 존재하는가?
            {
                RaycastHit2D hit = hits[i];
                if (hit.collider.gameObject.tag == "Wall")
                {
                    wallList.Add(hit.collider.gameObject);
                    wallDistance.Add((hit.collider.gameObject.transform.position - this.transform.position).magnitude);
                }
            }

            wallDistance.Sort();

            GameObject wall = null;

            foreach (GameObject _wall in wallList)
            {
                if ((_wall.transform.position - this.transform.position).magnitude == wallDistance[0])
                {
                    wall = _wall;
                }
            }

            if (wall != null)                           // 벽이 존재할 경우
                                                        // 스타트 포지션에서 노드와 벽의 거리를 비교한다
            {
                if ((item.transform.position - this.transform.position).magnitude
                    < (wall.transform.position - this.transform.position).magnitude)
                {
                    insert = true;                      // 노드가 벽보다 스타트 포지션에 가깝다면, 이동 가능한 노드
                }
            }

            if (wall == null)  // 노드만 존재할 경우, 스타트 포지션과 노드의 중간에 벽이 없다는 뜻
            {
                insert = true;
            }

            //for (int i = 0; i < hits.Length; i++)
            //{
            //    RaycastHit2D hit = hits[i];
            //    Debug.Log(hit.collider.gameObject.tag);
            //    if (hit.collider.gameObject.tag == "Wall")
            //    {
            //        break;
            //    }
            //    if (hit.collider.gameObject.tag == "Node")
            //    {
            //        insert = true;
            //        break;
            //    }
            //}

            Debug.Log("------------검사 끝--------------");

            if (NowNode != item && insert) //검사 후 , 오픈리스트에 넣어줌
            {
                OpenList.Add(item);
            }
        }

        foreach (GameObject item in OpenList)
        {
            Debug.Log("In OpenList: " + item.name);
            float distance = (TargetPos - item.transform.position).magnitude;       //오픈리스트에 있는 노드와 타겟과의 거리를 디스턴스 리스트에 넣는다
            Distance.Add(distance);
        }

        //노드 디스턴스 정렬
        Distance.Sort();

        //노드 검색
        foreach (GameObject item in OpenList)
        {
            if ((TargetPos - item.transform.position).magnitude == Distance[0]) //타겟과 오픈리스트의 노드간의 제일 짧은 거리에 있는 노드를 찾는다
            {
                NowNode = item;     //현재 이동해야할 노드

                MovePos = item.transform.position;  //노드의 포지션
                isMove = true;
                Debug.Log("현재 목표 지점: " + NowNode.gameObject.name);
                break;
            }
        }
    }

    public void MoveToNode(float speed, Animator Anim, float _angle, int _dis)
    {

        if (_angle != 0f)
        {
            Angle = GetDregree2(this.transform.position, MovePos);
            Anim.SetFloat("Angle", Angle);
        }
        else
        {
            Angle = GetDegree(this.transform.position, MovePos);
            Anim.SetFloat("Angle", Angle);
        }

        this.transform.position = Vector2.MoveTowards(transform.position, MovePos, speed * Time.deltaTime);

        Vector2 StartPos = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 MovePos2 = new Vector2(MovePos.x, MovePos.y);

        float dir = Vector2.Distance(StartPos, MovePos);

        if (_dis == 0)
        {
            if (StartPos == MovePos2)
            {
                isMove = false;
                isWall = false;
            }
        }

        else
        {
            if (dir < 0.4f)
            {
                isMove = false;
                isWall = false;
            }
        }
    }

    protected float GetDegree(Vector2 playerPos, Vector2 mousePos)
    {
        float angle = Mathf.Atan2(mousePos.y - playerPos.y, mousePos.x - playerPos.x) * 180 / Mathf.PI;
        if (angle < 0) angle += 360;

        return angle;
    }

    protected float GetDregree2(Vector2 playerPos, Vector2 mousePos)
    {
        Vector2 dir = mousePos - playerPos;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        return angle;
    }
}