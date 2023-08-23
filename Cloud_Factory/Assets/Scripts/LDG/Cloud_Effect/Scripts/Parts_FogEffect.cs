using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts_FogEffect : MonoBehaviour
{
    // 페이드인,아웃과 scale값 변화시키는 부분
    public GameObject[] Parts = new GameObject[3];
    // 이동시키는 부분
    public GameObject[] PartBodys = new GameObject[3];

    private float Speed = 1.0f;
    private float length = 0.3f;
    private float length_2 = 0.2f;
    private float[] moveTime = new float[3];
    private float[] yPos = new float[3];
    private float[] xPos = new float[3];
    private float[] transparency = new float[3];
    private bool[] Stoptransparency = new bool[3];

    private float StartEffect = 0.0f;

    private bool[] StopEffect_Status = new bool[3];

    void Start()
    {
        for(int i = 0; i < 3; i++) 
        { 
            moveTime[i] = 0.0f;
            transparency[i] = 1.0f;
            Stoptransparency[i] = false;
            StopEffect_Status[i] = false;
        }

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
        StartEffect += Time.deltaTime;
        if(StartEffect>= 0.1f)
        {
            Parts[0].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
            Move_Part1();
            Parts[2].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
            Move_Part3();
        }
        if (StartEffect >= 0.6f) 
        {
            Parts[1].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
            Move_Part2(); 
        }

        if (StartEffect >= 5.5f) 
        { 
            StopEffect_Status[0] = true; 
            StopEffect_Status[1] = true;
            StopEffect_Status[2] = true;
        }
    }

    void Move_Part1()
    {
        if (StopEffect_Status[0] == false)
        {
            moveTime[0] += Time.deltaTime * (2 * Speed);
            xPos[0] += 0.25f * Time.deltaTime;
            yPos[0] = 0.2f + Mathf.Sin(moveTime[0]) * length;
            PartBodys[0].transform.localPosition = new Vector3(xPos[0], yPos[0], 0f);

            // 투명도 바꾸는 부분
            if (yPos[0] < 0.2f && yPos[0] > -0.06f && Stoptransparency[0] == false)
            {
                transparency[0] -= 0.005f;
                Parts[0].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, transparency[0]);
                if (yPos[0] <= -0.05f) { Stoptransparency[0] = true; }
            }
            if (yPos[0] > -0.04f && Stoptransparency[0] == true)
            {
                transparency[0] += 0.005f;
                Parts[0].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, transparency[0]);
                if (yPos[0] >= 0.2f) { Stoptransparency[0] = false; }
            }
        }
        else { Parts[0].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f); }
    }

    void Move_Part2()
    {
        if (StopEffect_Status[1] == false)
        {
            moveTime[1] += Time.deltaTime * (2 * Speed);
            xPos[1] += 0.25f * Time.deltaTime;
            yPos[1] = -0.2f + Mathf.Sin(moveTime[1]) * length_2;
            PartBodys[1].transform.localPosition = new Vector3(xPos[1], yPos[1], 0f);

            // 투명도 바꾸는 부분
            if (yPos[1] < -0.2f && yPos[1] > -0.36f && Stoptransparency[1] == false)
            {
                transparency[1] -= 0.005f;
                Parts[1].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, transparency[1]);
                if (yPos[1] <= -0.35f) { Stoptransparency[1] = true; }
            }
            if (yPos[1] > -0.34f && Stoptransparency[1] == true)
            {
                transparency[1] += 0.005f;
                Parts[1].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, transparency[1]);
                if (yPos[1] >= -0.2f) { Stoptransparency[1] = false; }
            }
        }
        else { Parts[1].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f); }
    }

    void Move_Part3()
    {
        if (StopEffect_Status[2] == false)
        {
            moveTime[2] += Time.deltaTime * (2 * Speed);
            xPos[2] += 0.25f * Time.deltaTime;
            yPos[2] = 0.5f + Mathf.Sin(moveTime[2]) * length_2;
            PartBodys[2].transform.localPosition = new Vector3(xPos[2], yPos[2], 0f);

            // 투명도 바꾸는 부분
            if (yPos[2] < 0.5f && yPos[2] > 0.34f && Stoptransparency[2] == false)
            {
                transparency[2] -= 0.005f;
                Parts[2].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, transparency[2]);
                if (yPos[2] <= 0.35f) { Stoptransparency[2] = true; }
            }
            if (yPos[2] > 0.34f && Stoptransparency[2] == true)
            {
                transparency[2] += 0.005f;
                Parts[2].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, transparency[2]);
                if (yPos[2] >= 0.5f) { Stoptransparency[2] = false; }
            }
        }
        else { Parts[2].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f); }
    }
}
