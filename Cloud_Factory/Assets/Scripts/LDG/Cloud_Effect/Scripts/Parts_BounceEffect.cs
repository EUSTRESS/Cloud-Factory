using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts_BounceEffect : MonoBehaviour
{
    // scale값 변화시키는 부분
    public GameObject[] Parts = new GameObject[3];
    // 이동시키는 부분
    public GameObject[] PartBodys = new GameObject[3];

    private Vector3 TargetPos;
    private Vector3 StartPos;
    private bool A = true;
    private float Speed = 1.0f;

    void Start()
    {
        TargetPos = new Vector3(PartBodys[0].transform.localPosition.x, 0.0f, PartBodys[0].transform.localPosition.z);
        StartPos = new Vector3(PartBodys[0].transform.localPosition.x, PartBodys[0].transform.localPosition.y, PartBodys[0].transform.localPosition.z);
    }

    void Update()
    {
        Bounce_Part1();
    }

    void Bounce_Part1()
    {
        if (A)
        {
            Vector3 Change_Pos = Vector3.Lerp(PartBodys[0].transform.localPosition, TargetPos, Speed * Time.deltaTime);
            PartBodys[0].transform.localPosition = Change_Pos;
            if (Mathf.Approximately(Change_Pos.y, 0.0f)) 
            {
                A = false;
            }
        }
        if (!A)
        {
            Vector3 Back_Pos = Vector3.Lerp(PartBodys[0].transform.localPosition, StartPos, Speed * Time.deltaTime);
            PartBodys[0].transform.localPosition = Back_Pos;
        }
        
        
    }
}
