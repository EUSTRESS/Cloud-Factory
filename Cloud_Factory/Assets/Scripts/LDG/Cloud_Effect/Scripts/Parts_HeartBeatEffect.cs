using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts_HeartBeatEffect : MonoBehaviour
{
    // 하트비트 시키는 부분
    public GameObject[] Parts = new GameObject[3];

    private float[] Max_Scale = new float[3];
    private float[] Min_Scale = new float[3];
    private float[] ScaleSpeed = new float[3];

    private bool[] ScalingUp = new bool[3];

    private float[] HideTime = new float[3];

    private float StartEffect = 0.0f;

    void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            Max_Scale[i] = 0.4f;
            Min_Scale[i] = 0.3f;
            ScalingUp[i] = true;
            HideTime[i] = 0.0f;
        }

        ScaleSpeed[0] = 10.0f;
        ScaleSpeed[1] = 20.0f;
        ScaleSpeed[2] = 30.0f;
    }

    void Update()
    {
        StartEffect += Time.deltaTime;
        HeartBeat_Part1();
        if (StartEffect >= 0.5f)
        {
            HeartBeat_Part3();
        }
        if (StartEffect >= 1.0f)
        {
            HeartBeat_Part2();
        }
    }

    void HeartBeat_Part1()
    {
        HideTime[0] += Time.deltaTime;
        float targetScale = ScalingUp[0] ? Max_Scale[0] : Min_Scale[0];
        Vector3 Change_Scale = Vector3.Lerp(Parts[0].transform.localScale, new Vector3(targetScale, targetScale, 0.0f), ScaleSpeed[0] * Time.deltaTime/0.5f);
        Parts[0].transform.localScale = Change_Scale;

        if (Mathf.Approximately(Change_Scale.x, targetScale))
        {
            ScalingUp[0] = !(ScalingUp[0]);
        }
        if (HideTime[0] >= 0.8f)
        {
            Parts[0].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.0f);
            HideTime[0] = 0.0f;
        }
    }

    void HeartBeat_Part2()
    {
        HideTime[1] += Time.deltaTime;
        float targetScale2 = ScalingUp[1] ? Max_Scale[1] : Min_Scale[1];
        Vector3 Change_Scale = Vector3.Lerp(Parts[1].transform.localScale, new Vector3(targetScale2, targetScale2, 0.0f), ScaleSpeed[1] * Time.deltaTime/0.5f);
        Parts[1].transform.localScale = Change_Scale;

        if (Mathf.Approximately(Change_Scale.x, targetScale2))
        {
            ScalingUp[1] = !(ScalingUp[1]);
        }
        if (HideTime[1] >= 0.6f)
        {
            Parts[1].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.0f);
            HideTime[1] = 0.0f;
        }
    }

    void HeartBeat_Part3()
    {
        HideTime[2] += Time.deltaTime;
        float targetScale3 = ScalingUp[2] ? Max_Scale[2] : Min_Scale[2];
        Vector3 Change_Scale = Vector3.Lerp(Parts[2].transform.localScale, new Vector3(targetScale3, targetScale3, 0.0f), ScaleSpeed[2] * Time.deltaTime*1.5f);
        Parts[2].transform.localScale = Change_Scale;

        if (Mathf.Approximately(Change_Scale.x, targetScale3))
        {
            ScalingUp[2] = !ScalingUp[2];
        }
        if(HideTime[2]>= 0.8f)
        {
            Parts[2].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.0f);
            HideTime[2] = 0.0f;
        }
    }
}
