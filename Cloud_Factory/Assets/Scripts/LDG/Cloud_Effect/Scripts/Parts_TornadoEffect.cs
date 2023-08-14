using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts_TornadoEffect : MonoBehaviour
{
    // 페이드아웃시키는 부분
    public GameObject[] Parts = new GameObject[3];
    // 이동시키는 부분
    public GameObject[] PartBodys = new GameObject[3];

    // 파츠1의 움직임방향벡터
    private Vector3[] MoveDir_Part1 = new Vector3[2];

    // 파츠2의 움직임방향벡터
    private Vector3[] MoveDir_Part2 = new Vector3[2];

    // 파츠3의 움직임방향벡터
    private Vector3[] MoveDir_Part3 = new Vector3[2];

    // 파츠들의 움직임방향하는 변환시간
    private float[] ChangeDir_Times = new float[3];

    // 움직임변환할때의 상태체크하는 bool변수
    private bool[] ChangeDir_Check = new bool[3];

    // 페이드아웃효과줄 상태를 변화하기 위한 시간변수
    private float[] FadeOut_StartTime = new float[3];

    // 페이드아웃에서 투명도 조정하는 변수
    private float[] transparency = new float[3];

    // 1,2,3파츠가 순서대로 나오는 시간변수
    private float StartEffect;

    private float speed = 0.5f;
    private float frequency = 0.1f;
    private float amplitude = 0.1f;

    void Start()
    {
        MoveDir_Part1[0] = new Vector3(-1.2f, 1.0f, 0f);    // 파츠1의 왼쪽방향
        MoveDir_Part1[1] = new Vector3(1.2f, 1.0f, 0f);     // 파츠1의 오른쪽방향
        MoveDir_Part2[0] = new Vector3(-1.2f, 1.1f, 0f);    // 파츠2의 왼쪽방향
        MoveDir_Part2[1] = new Vector3(1.2f, 1.1f, 0f);     // 파츠2의 오른쪽방향
        MoveDir_Part3[0] = new Vector3(-1.5f, 1.0f, 0f);    // 파츠3의 왼쪽방향
        MoveDir_Part3[1] = new Vector3(1.5f, 1.0f, 0f);     // 파츠3의 오른쪽방향

        for (int i = 0; i < 3; i++) { ChangeDir_Times[i] = 0.0f; }

        // true면은 왼쪽, false면은 오른쪽
        ChangeDir_Check[0] = true;
        ChangeDir_Check[1] = true;
        ChangeDir_Check[2] = false;

        for(int i = 0; i < 3; i++) { FadeOut_StartTime[i] = 0.0f; }
        for(int i = 0; i < 3; i++) { transparency[i] = 1.0f; }

        StartEffect = 0.0f;
    }

    void Update()
    {
        StartEffect += Time.deltaTime;        
        if(StartEffect > 0.1f) { ChangeDir_Part1(ChangeDir_Check[0]); }
        if(StartEffect > 0.3f) { ChangeDir_Part2(ChangeDir_Check[1]); }
        if(StartEffect > 0.7f) { ChangeDir_Part3(ChangeDir_Check[2]); }
    }

    //파츠1를 실질적으로 움직이게하는 함수
    void MovingPartBody1_LR(Vector3 dir1)
    {
        ChangeDir_Times[0] += Time.deltaTime;
        FadeOut_StartTime[0] += Time.deltaTime;
        PartBodys[0].transform.Translate(dir1 * (Time.deltaTime / 2));
        if (ChangeDir_Times[0] >= 0.5f)
        {
            ChangeDir_Check[0] = !(ChangeDir_Check[0]);
            ChangeDir_Times[0] = 0.0f;
            ChangeDir_Part1(ChangeDir_Check[0]);
        }
        if(FadeOut_StartTime[0] >= 1.5f)
        {
            transparency[0] -= 0.02f;
            Parts[0].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, transparency[0]);
        }
    }

    // 파츠2를 실질적으로 움직이게하는 함수
    void MovingPartBody2_LR(Vector3 dir2)
    {
        ChangeDir_Times[1] += Time.deltaTime;
        FadeOut_StartTime[1] += Time.deltaTime;
        PartBodys[1].transform.Translate(dir2 * (Time.deltaTime / 2));
        if (ChangeDir_Times[1] >= 0.5f)
        {
            ChangeDir_Check[1] = !(ChangeDir_Check[1]);
            ChangeDir_Times[1] = 0.0f;
            ChangeDir_Part2(ChangeDir_Check[1]);
        }
        if (FadeOut_StartTime[1] >= 1.5f)
        {
            transparency[1] -= 0.02f;
            Parts[1].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, transparency[1]);
        }
    }

    // 파츠3 을 실질적으로 움직이게 하는 함수
    void MovingPartBody3_LR(Vector3 dir3)
    {
        ChangeDir_Times[2] += Time.deltaTime;
        FadeOut_StartTime[2] += Time.deltaTime;
        PartBodys[2].transform.Translate(dir3 * (Time.deltaTime / 1.5f));
        if (ChangeDir_Times[2] >= 0.5f)
        {
            ChangeDir_Check[2] = !(ChangeDir_Check[2]);
            ChangeDir_Times[2] = 0.0f;
            ChangeDir_Part3(ChangeDir_Check[2]);
        }
        if (FadeOut_StartTime[2] >= 1.5f)
        {
            transparency[2] -= 0.02f;
            Parts[2].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, transparency[2]);
        }
    }

    // 파츠별 방향전환 함수
    void ChangeDir_Part1(bool checkDir1)
    {
        if (checkDir1) { MovingPartBody1_LR(MoveDir_Part1[0]); }
        else { MovingPartBody1_LR(MoveDir_Part1[1]); }
    }

    void ChangeDir_Part2(bool checkDir2)
    {
        if (checkDir2) { MovingPartBody2_LR(MoveDir_Part2[0]); }
        else { MovingPartBody2_LR(MoveDir_Part2[1]); }
    }

    void ChangeDir_Part3(bool checkDir3)
    {
        if (checkDir3) { MovingPartBody3_LR(MoveDir_Part3[0]); }
        else { MovingPartBody3_LR(MoveDir_Part3[1]); }
    }
}
