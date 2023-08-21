using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts_FogEffect : MonoBehaviour
{
    // 페이드인,아웃과 scale값 변화시키는 부분
    public GameObject[] Parts = new GameObject[3];
    // 이동시키는 부분
    public GameObject[] PartBodys = new GameObject[3];

    private float Speed = 2.0f;
    private float length = 0.3f;
    private float[] moveTime = new float[3];
    private float[] yPos = new float[3];
    private float[] xPos = new float[3];

    void Start()
    {
        for(int i = 0; i < 3; i++) { moveTime[i] = 0.0f; }

        // part1 위치
        xPos[0] = -1.4f;
        yPos[0] = 0.2f;

        // part2 위치
        xPos[1] = -0.9f;
        yPos[1] = -0.2f;

        // part3 위치
        xPos[2] = 0.3f;
        yPos[2] = 0.5f;
    }

    void Update()
    {
        Move_Part3();
    }

    void Move_Part3()
    {
        moveTime[2] += Time.deltaTime * Speed;
        yPos[2] = 0.5f + Mathf.Sin(moveTime[2]) * length;
        xPos[2] += 0.25f * Time.deltaTime;
        PartBodys[2].transform.localPosition = new Vector3(xPos[2], yPos[2], 0f);
    }
}
