using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] public GameObject[] WayPos;
    [SerializeField] private float speed = 1.0f;
    int WayNum = 0;
    public bool isMove = true;

    void Start()
    {
        transform.position = WayPos[WayNum].transform.position;
    }

    void Update()
    {
        if (isMove)
        {
            Invoke("MovePath", 2.0f);
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
            WayNum = 0;
        }
     }
    
}
