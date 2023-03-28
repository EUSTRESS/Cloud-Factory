using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] public GameObject[] WayPos;
    [SerializeField] private float speed = 1.0f;
    public int WayNum = 0;
    
    float standTime = 0.0f;
    float randomTime = 0.0f;

    public bool isMove = true;

    private Animator Anim;

    // 이전 위치값을 가지는 변수값
    [SerializeField]
    private float temp_X;

    void Start()
    {
        temp_X = this.transform.position.x;
        randomTime = Random.Range(0.5f, 1.0f);
        Anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        standTime += Time.deltaTime;
        if (isMove)
        {
            if (standTime > 5.0f)
            {
                Anim.SetBool("isStand", false);
                standTime = 0.0f;
                randomTime = Random.Range(0.5f, 1.0f);
            }
            if (standTime > 5.0f - randomTime)
            {
                Anim.SetBool("isStand", true);
                return;
            }

            MovePath();

            if(this.transform.position.x > temp_X)
            {
                transform.localScale = new Vector3(0.9f, 0.9f, 1f);
            }
            else if(this.transform.position.x < temp_X)
            {
                transform.localScale = new Vector3(0.9f, 0.9f, 1f);
            }

            temp_X = this.transform.position.x;
        }
    }
    public void MovePath()
    {
        transform.position = Vector2.MoveTowards
            (transform.position, WayPos[WayNum].transform.position, speed * Time.deltaTime);
        if (transform.position == WayPos[WayNum].transform.position)
        {
            WayNum++;
        }
        if (WayNum == WayPos.Length)
        {
            WayNum = 1;
        }
     }
    
}
