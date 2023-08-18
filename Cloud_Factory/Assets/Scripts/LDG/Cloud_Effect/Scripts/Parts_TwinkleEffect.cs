using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts_TwinkleEffect : MonoBehaviour
{
    // 페이드인,아웃과 scale값 변화시키는 부분
    public GameObject[] Parts = new GameObject[3];
    // 이동시키는 부분
    public GameObject[] PartBodys = new GameObject[3];

    private float StartFade = 0.0f;
    private float[] Fade_State = new float[3];
    private bool[] FadeOutStart = new bool[3];

    private Vector3 Start_Scale;
    private Vector3 Target_Scale;
    private float Speed = 1.0f;

    void Start()
    {
        for(int i = 0; i < 3; i++) 
        { 
            Fade_State[i] = 0.0f;
            FadeOutStart[i] = false;
        }
        Start_Scale = Parts[2].transform.localScale;
        Target_Scale = new Vector3(0.3f, 0.3f, Parts[2].transform.localScale.z);
    }

    void Update()
    {
        if (!FadeOutStart[0]) { TwinkleStart_Part(0); } // part1 시작
        if(StartFade >= 1.7f) 
        { 
            FadeOutStart[0] = true;     // part1상태변화
            TwinkleStart_Part(1);           // part2 시작
        }
        if (FadeOutStart[0]) { TwinkleEnd_Part(0); }        // part1 끝

        if(StartFade >= 3.0f) { FadeOutStart[1] = true; }   // part2 상태변화
        if (FadeOutStart[1]) { TwinkleEnd_Part(1); }            // part2 끝


        if(StartFade >= 2.5f) { TwinkleStart_Part3(); }     // part3 시작
        if(StartFade>= 3.5f) { FadeOutStart[2] = true; }    // part3 상태변화
        if (FadeOutStart[2]) { TwinkleEnd_Part3(); }        // part3 끝

    }

    void TwinkleStart_Part(int index)
    {
        StartFade += Time.deltaTime;
        if (!FadeOutStart[index])
        {
            Fade_State[index] += 0.003f;
            Parts[index].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, Fade_State[index]);
        } 
    }

    void TwinkleEnd_Part(int index)
    {
        if (FadeOutStart[index])
        {
            Fade_State[index] -= 0.003f;
            Parts[index].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, Fade_State[index]);
        }
    }

    void TwinkleStart_Part3()
    {
        if (!FadeOutStart[2])
        {
            Fade_State[2] += 0.003f;
            Parts[2].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, Fade_State[2]);
            Parts[2].transform.localScale = Vector3.Lerp(Parts[2].transform.localScale, Target_Scale, Speed * Time.deltaTime);
        }
    }

    void TwinkleEnd_Part3()
    {
        if (FadeOutStart[2])
        {
            Fade_State[2] -= 0.003f;
            Parts[2].transform.localScale = Vector3.Lerp(Parts[2].transform.localScale, Start_Scale, Speed * Time.deltaTime);
            Parts[2].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, Fade_State[2]);
        }
    }
}
