using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rejectbutton_Runout : MonoBehaviour
{
    // NO버튼 관련
    public GameObject OkNoGroup;
    private Transform B_No;
    private RectTransform B_NO_Rect;

    // OK버튼 관련
    private Transform B_Ok;
    private RectTransform B_OK_Rect;

    // ProfileBG관련
    private Transform ProfileBG;
    private RectTransform ProfileBG_Rect;

    // I_Portrait관련
    private Transform I_Portrait;

    private bool NOButton_AnimPlay;

    // ProfileBG 변화시킬 Rotation값
    private Quaternion Profile_StartRot;
    private Quaternion Profile_MiddleRot;
    private Quaternion Profile_EndRot;

    // 이동시킬 좌표 배열
    // 0 : ProfileBG, 1 : B_OK, 2 : B_NO
    private Vector3[] StartPos = new Vector3[3];
    private Vector3[] MiddlePos = new Vector3[3];
    private Vector3[] EndPos = new Vector3[3];

    private float MoveDuration;
    private float[] MoveTimer_1 = new float[2];
    private float[] MoveTimer_2 = new float[2];
    private bool[] LastMove_Toward = new bool[2];

    // 투명도
    private float transparency;
    private float transparency2;

    void Start()
    {
        ProfileBG = OkNoGroup.transform.GetChild(1);
        B_Ok = OkNoGroup.transform.GetChild(2);
        B_No = OkNoGroup.transform.GetChild(3);
        I_Portrait = ProfileBG.transform.GetChild(0);

        B_NO_Rect = B_No.GetComponent<RectTransform>();
        B_OK_Rect = B_Ok.GetComponent<RectTransform>();
        ProfileBG_Rect = ProfileBG.GetComponent<RectTransform>();
        
        NOButton_AnimPlay = false;

        Profile_StartRot = Quaternion.Euler(0f, 0f, 0f);
        Profile_MiddleRot = Quaternion.Euler(0f, 0f, 45.0f);
        Profile_EndRot = Quaternion.Euler(0f, 0f, -10.0f);

        // OK버튼 이동시킬 Pos
        StartPos[1] = new Vector3(300.0f, B_OK_Rect.localPosition.y, B_OK_Rect.localPosition.z);
        MiddlePos[1] = new Vector3(100.0f, B_OK_Rect.localPosition.y, B_OK_Rect.localPosition.z);
        EndPos[1] = new Vector3(1200.0f, B_OK_Rect.localPosition.y, B_OK_Rect.localPosition.z);

        // NO버튼 이동시킬 Pos
        StartPos[2] = new Vector3(350.0f, B_NO_Rect.localPosition.y, B_NO_Rect.localPosition.z);
        MiddlePos[2] = new Vector3(180.0f, B_NO_Rect.localPosition.y, B_NO_Rect.localPosition.z);
        EndPos[2] = new Vector3(1200.0f, B_NO_Rect.localPosition.y, B_NO_Rect.localPosition.z);

        // ProfileBG 이동시킬 Pos
        StartPos[0] = new Vector3(ProfileBG_Rect.localPosition.x, -50f, ProfileBG_Rect.localPosition.z);
        MiddlePos[0] = new Vector3(ProfileBG_Rect.localPosition.x, -20f, ProfileBG_Rect.localPosition.z);
        EndPos[0] = new Vector3(ProfileBG_Rect.localPosition.x, -900f, ProfileBG_Rect.localPosition.z);

        MoveDuration = 0.7f;

        for (int i = 0; i < 2; i++)
        {
            MoveTimer_1[i] = 0f;
            MoveTimer_2[i] = 0f;
            LastMove_Toward[i] = false;
        }

        transparency = 1f;
        transparency2 = 1f;
    }

    void Update()
    {
        if (NOButton_AnimPlay) 
        {
            OkNoGroup.GetComponent<OkNoGroup_Anim>().enabled = false;
            MoveButton();
            MoveProfile();
        }
    }

    public void B_NO_Clicked()
    {
        if (!NOButton_AnimPlay) { NOButton_AnimPlay = !NOButton_AnimPlay; }
    }

    void MoveButton()
    {
        if (MoveTimer_1[1] < MoveDuration) 
        {
            MoveTimer_1[1] += Time.deltaTime;
            float LeftSpeed = MoveTimer_1[1] / MoveDuration;
            transparency -= 0.005f;
            B_Ok.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency);
            B_No.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency);
            B_OK_Rect.localPosition = Vector3.Lerp(StartPos[1], MiddlePos[1], LeftSpeed);
            B_NO_Rect.localPosition = Vector3.Lerp(StartPos[2], MiddlePos[2], LeftSpeed);
            if (LeftSpeed >= 1.0f) { LastMove_Toward[1] = !LastMove_Toward[1]; }
        }
        if (LastMove_Toward[1])
        {
            MoveTimer_2[1] += Time.deltaTime;
            float RightSpeed = MoveTimer_2[1] / MoveDuration;
            transparency -= 0.005f;
            B_Ok.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency);
            B_No.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency);
            B_OK_Rect.localPosition = Vector3.Lerp(B_OK_Rect.localPosition, EndPos[1], RightSpeed);
            B_NO_Rect.localPosition = Vector3.Lerp(B_NO_Rect.localPosition, EndPos[2], RightSpeed);
        }
    }

    void MoveProfile()
    {
        if(MoveTimer_1[0] < MoveDuration)
        {
            MoveTimer_1[0] += Time.deltaTime;
            float UpSpeed = MoveTimer_1[0] / MoveDuration;
            transparency2 -= 0.005f;
            ProfileBG.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency2);
            I_Portrait.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency2);
            ProfileBG_Rect.rotation = Quaternion.Lerp(Profile_StartRot, Profile_MiddleRot, UpSpeed);
            ProfileBG_Rect.localPosition = Vector3.Lerp(StartPos[0], MiddlePos[0], UpSpeed);
            if (UpSpeed >= 1.0f) { LastMove_Toward[0] = !LastMove_Toward[0]; }
        }
        if (LastMove_Toward[0])
        {
            MoveTimer_2[0] += Time.deltaTime;
            float DownSpeed = MoveTimer_2[0] / MoveDuration;
            transparency2 -= 0.005f;
            ProfileBG.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency2);
            I_Portrait.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency2);
            ProfileBG.rotation = Quaternion.Lerp(ProfileBG_Rect.rotation, Profile_EndRot, DownSpeed);
            ProfileBG_Rect.localPosition = Vector3.Lerp(ProfileBG_Rect.localPosition, EndPos[0], DownSpeed);
        }
    }
}
