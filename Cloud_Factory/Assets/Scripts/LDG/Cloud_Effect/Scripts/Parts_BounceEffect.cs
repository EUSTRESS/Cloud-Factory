using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts_BounceEffect : MonoBehaviour
{
    // scale값 변화시키는 부분
    public GameObject[] Parts = new GameObject[3];
    // 이동시키는 부분
    public GameObject[] PartBodys = new GameObject[3];

    private Vector3[] TargetPos = new Vector3[3];
    private Vector3[] StartPos = new Vector3[3];

    private Vector3[] TargetScale = new Vector3[3];
    private Vector3[] StartScale = new Vector3[3];

    private bool[] DownMove_Pos = new bool[3];
    private float Speed = 3.0f;

    private float StartEffect_Time = 0.0f;

    void Start()
    {
        for(int i = 0; i < 3; i++) 
        {
            DownMove_Pos[i] = true;
            StartPos[i] = PartBodys[i].transform.localPosition;
            StartScale[i] = PartBodys[i].transform.localScale;
        }

        TargetPos[0] = new Vector3(PartBodys[0].transform.localPosition.x, 0.0f, PartBodys[0].transform.localPosition.z);
        TargetPos[1] = new Vector3(PartBodys[1].transform.localPosition.x, -0.7f, PartBodys[1].transform.localPosition.z);
        TargetPos[2] = new Vector3(PartBodys[2].transform.localPosition.x, 0.3f, PartBodys[2].transform.localPosition.z);

        TargetScale[0] = new Vector3(PartBodys[0].transform.localScale.x, 0.6f, PartBodys[0].transform.localScale.z);
        TargetScale[1] = new Vector3(PartBodys[1].transform.localScale.x, 0.6f, PartBodys[1].transform.localScale.z);
        TargetScale[2] = new Vector3(PartBodys[2].transform.localScale.x, 0.6f, PartBodys[2].transform.localScale.z);
    }

    void Update()
    {
        Bounce_Part1();
        StartEffect_Time += Time.deltaTime;
        if (StartEffect_Time > 0.5f) { Bounce_Part2(); }
        if (StartEffect_Time > 1.5f) { Bounce_Part3(); }
    }

    void Bounce_Part3()
    {
        Vector3 Current_Pos = PartBodys[2].transform.localPosition;
        if (DownMove_Pos[2])
        {
            Parts[2].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
            PartBodys[2].transform.localPosition = Vector3.Lerp(PartBodys[2].transform.localPosition, TargetPos[2], 3* Speed * Time.deltaTime);
            PartBodys[2].transform.localScale = Vector3.Lerp(PartBodys[2].transform.localScale, TargetScale[2], 2 * Speed * Time.deltaTime);

            if (Vector3.Distance(Current_Pos, TargetPos[2]) < 0.01f) { DownMove_Pos[2] = !DownMove_Pos[2]; }
        }
        else
        {
            PartBodys[2].transform.localPosition = Vector3.Lerp(PartBodys[2].transform.localPosition, StartPos[2], 3 * Speed * Time.deltaTime);
            PartBodys[2].transform.localScale = Vector3.Lerp(PartBodys[2].transform.localScale, StartScale[2], 1.5f * Speed * Time.deltaTime);
            if(StartEffect_Time > 2.6f) { Parts[2].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f); }
        }
    }

    void Bounce_Part2()
    {
        Vector3 Current_Pos = PartBodys[1].transform.localPosition;
        if (DownMove_Pos[1])
        {
            Parts[1].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
            PartBodys[1].transform.localPosition = Vector3.Lerp(PartBodys[1].transform.localPosition, TargetPos[1], 1.5f * Speed * Time.deltaTime);
            PartBodys[1].transform.localScale = Vector3.Lerp(PartBodys[1].transform.localScale, TargetScale[1], 2 * Speed * Time.deltaTime);

            if (Vector3.Distance(Current_Pos, TargetPos[1]) < 0.01f) { DownMove_Pos[1] = !DownMove_Pos[1]; }
        }
        else
        {
            PartBodys[1].transform.localPosition = Vector3.Lerp(PartBodys[1].transform.localPosition, StartPos[1], Speed * Time.deltaTime);
            PartBodys[1].transform.localScale = Vector3.Lerp(PartBodys[1].transform.localScale, StartScale[1], 2f * Speed * Time.deltaTime);
            if(StartEffect_Time > 2.5f) { Parts[1].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f); }
        }
    }

    void Bounce_Part1()
    {
        Vector3 Current_Pos = PartBodys[0].transform.localPosition;
        if (DownMove_Pos[0])
        {
            Parts[0].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
            PartBodys[0].transform.localPosition = Vector3.Lerp(PartBodys[0].transform.localPosition, TargetPos[0], Speed * Time.deltaTime);
            PartBodys[0].transform.localScale = Vector3.Lerp(PartBodys[0].transform.localScale, TargetScale[0], 2 * Speed * Time.deltaTime);

            if (Vector3.Distance(Current_Pos, TargetPos[0]) < 0.01f) { DownMove_Pos[0] = !DownMove_Pos[0]; }
        }
        else
        {
            PartBodys[0].transform.localPosition = Vector3.Lerp(PartBodys[0].transform.localPosition, StartPos[0], Speed * Time.deltaTime);
            PartBodys[0].transform.localScale = Vector3.Lerp(PartBodys[0].transform.localScale, StartScale[0], 2 * Speed * Time.deltaTime);
            if(StartEffect_Time > 1.3f) { Parts[0].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f); }
        }
    }
}
