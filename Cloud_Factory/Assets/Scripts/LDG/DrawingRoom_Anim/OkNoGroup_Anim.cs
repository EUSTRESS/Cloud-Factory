using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OkNoGroup_Anim : MonoBehaviour
{
    // 이동시킬 프로필배경 오브젝트
    public GameObject ProfileBG;
    private RectTransform TargetRect;

    // 이동시킬 OK버튼 오브젝트
    public GameObject B_OK;
    private RectTransform B_OK_TargetRect;

    // 이동시킬 NO버튼 오브젝트
    public GameObject B_NO;
    private RectTransform B_NO_TargetRect;

    // ProfileBG회전값
    private Quaternion StartRot;
    private Quaternion MiddleRot;
    private Quaternion EndRot;

    // 이동시킬 좌표 배열
    // 0 : ProfileBG, 1 : B_OK, 2 : B_NO
    private Vector3[] StartPos = new Vector3[3];
    private Vector3[] MiddlePos = new Vector3[3];
    private Vector3[] EndPos = new Vector3[3];

    private float[] Profile_rotationDuration = new float[3]; //0.5f; // 애니메이션 지속 시간
    private float[] Profile_rotationTimer = new float[3]; //0f;       // 지속시간내 체크하는 타이머
    private float[] Profile_rotationTimer_2 = new float[3]; //0f;     // 지속시간내 체크하는 타이머2

    private bool[] Last_Move = new bool[3];     // 마지막움직임을 제어하는 변수

    void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            Last_Move[i] = false;
            Profile_rotationDuration[i] = 0.5f;
            Profile_rotationTimer[i] = 0.0f;
            Profile_rotationTimer_2[i] = 0.0f;
        }

        TargetRect = ProfileBG.GetComponent<RectTransform>();
        B_OK_TargetRect = B_OK.GetComponent<RectTransform>();
        B_NO_TargetRect = B_NO.GetComponent<RectTransform>();

        // ProfileBG를 회전시킬 Rot
        StartRot = Quaternion.Euler(0f, 0f, 45.0f);
        MiddleRot = Quaternion.Euler(0f, 0f, -10.0f);
        EndRot = Quaternion.Euler(0f, 0f, 0f);

        // ProfileBG를 이동시킬 Pos
        StartPos[0] = TargetRect.localPosition;
        MiddlePos[0] = new Vector3(TargetRect.localPosition.x, -115.0f, TargetRect.localPosition.z);
        EndPos[0] = new Vector3(TargetRect.localPosition.x, -50.0f, TargetRect.localPosition.z);

        // B_OK를 이동시킬 Pos
        StartPos[1] = B_OK_TargetRect.localPosition;
        MiddlePos[1] = new Vector3(100.0f, B_OK_TargetRect.localPosition.y, B_OK_TargetRect.localPosition.z);
        EndPos[1] = new Vector3(310.0f, B_OK_TargetRect.localPosition.y, B_OK_TargetRect.localPosition.z);

        // B_NO를 이동시킬 Pos
        StartPos[2] = B_NO_TargetRect.localPosition;
        MiddlePos[2] = new Vector3(180.0f, B_NO_TargetRect.localPosition.y, B_NO_TargetRect.localPosition.z);
        EndPos[2] = new Vector3(350.0f, B_NO_TargetRect.localPosition.y, B_NO_TargetRect.localPosition.z);
    }

    void Update()
    {
        Move_ProfileBG();
        Move_B_OK();
        Move_B_NO();
    }

    void Move_B_OK()
    {
        if(Profile_rotationTimer[1] < Profile_rotationDuration[1])
        {
            Profile_rotationTimer[1] += Time.deltaTime;
            float Speed = Profile_rotationTimer[1] / Profile_rotationDuration[1];
            B_OK_TargetRect.localPosition = Vector3.Lerp(StartPos[1], MiddlePos[1], Speed);

            if (Speed >= 1.0f) { Last_Move[1] = !Last_Move[1]; }
        }
        if (Last_Move[1])
        {
            Profile_rotationTimer_2[1] += Time.deltaTime;
            float Speed = Profile_rotationTimer_2[1] / 0.2f;
            B_OK_TargetRect.localPosition = Vector3.Lerp(B_OK_TargetRect.localPosition , EndPos[1], Speed);
        }
    }

    void Move_B_NO()
    {
        if(Profile_rotationTimer[2] < Profile_rotationDuration[2])
        {
            Profile_rotationTimer[2] += Time.deltaTime;
            float Speed = Profile_rotationTimer[2] / Profile_rotationDuration[2];
            B_NO_TargetRect.localPosition = Vector3.Lerp(StartPos[2], MiddlePos[2], Speed);

            if(Speed >= 1.0f) { Last_Move[2] = !Last_Move[2]; }
        }
        if (Last_Move[2])
        {
            Profile_rotationTimer_2[2] += Time.deltaTime;
            float Speed = Profile_rotationTimer_2[2] / 0.2f;
            B_NO_TargetRect.localPosition = Vector3.Lerp(B_NO_TargetRect.localPosition, EndPos[2], Speed);
        }
    }

    void Move_ProfileBG()
    {
        if (Profile_rotationTimer[0] < Profile_rotationDuration[0])
        {
            Profile_rotationTimer[0] += Time.deltaTime;
            float speed = Profile_rotationTimer[0] / Profile_rotationDuration[0];
            ProfileBG.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
            TargetRect.rotation = Quaternion.Lerp(StartRot, MiddleRot, speed);
            TargetRect.localPosition = Vector3.Lerp(StartPos[0], MiddlePos[0], speed);

            if(speed >= 1.0f) { Last_Move[0] = !Last_Move[0]; }
        }
        if (Last_Move[0])
        {
            Profile_rotationTimer_2[0] += Time.deltaTime;
            float speed = Profile_rotationTimer_2[0] / 0.2f;
            TargetRect.rotation = Quaternion.Lerp(TargetRect.rotation, EndRot, speed);
            TargetRect.localPosition = Vector3.Lerp(TargetRect.localPosition, EndPos[0], speed);
        }
    }
}
