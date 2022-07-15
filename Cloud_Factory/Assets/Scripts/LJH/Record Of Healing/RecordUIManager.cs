using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordUIManager : MonoBehaviour
{
    [Header("GAME OBJECT")]
    // 치유의 기록
    public GameObject gShowUpset;
    public GameObject gShowAll;
    public GameObject[] gStampF = new GameObject[4]; // 불만 뭉티 스탬프
    // 치유의기록
    public Image[] iProfileBG = new Image[4]; // 프로필 배경

    [Header("SPRITES")]
    public Sprite[] sBasicProfile = new Sprite[4]; // 기본 프로필
    public Sprite[] sUpsetProfile = new Sprite[4]; // 화난 프로필

    [Header("BUTTON")]

    //치유의 기록     
    public Button btNextBtn;            // 다음페이지 버튼
    public Button btPrevBtn;            // 이전페이지 버튼

    [HideInInspector]
    public bool mIsNext;               // 치유의 기록 다음 페이지   
    [HideInInspector]
    public bool mIsPrev;               // 치유의 기록 이전 페이지

    private ProfileMoving mProfile1;   // 프로필 움직임 담당 스크립트
    private ProfileMoving mProfile2;   // 프로필 움직임 담당 스크립트
    private ProfileMoving mProfile3;   // 프로필 움직임 담당 스크립트

    // **********임시로써 지워야 함***********
    public Camera MainCam;
    // **********임시로써 지워야 함***********

    // Start is called before the first frame update
    void Awake()
    {
        mProfile1 = GameObject.Find("I_ProfileBG1").GetComponent<ProfileMoving>();
        mProfile2 = GameObject.Find("I_ProfileBG2").GetComponent<ProfileMoving>();
        mProfile3 = GameObject.Find("I_ProfileBG3").GetComponent<ProfileMoving>();
    }

    public void ShowNextProfile()
    {
        // 다음 페이지
        mIsNext = true;
        mIsPrev = false;

        mProfile1.mIsMoving = true;
        mProfile2.mIsMoving = true;
        mProfile3.mIsMoving = true;

        // 버그 방지를 위한 버튼 비활성화
        btNextBtn.interactable = false;
        btPrevBtn.interactable = false;
        // 불만 뭉티만 볼 때
        if (mProfile1.mIsUpset && mProfile2.mIsUpset && mProfile3.mIsUpset)
            Invoke("DelayActiveBtn", 0.5f);  // 활성화 딜레이
        // 전체 뭉티 볼 때
        else if (!mProfile1.mIsUpset && !mProfile2.mIsUpset && !mProfile3.mIsUpset)
            Invoke("DelayActiveBtn", 1.0f);  // 활성화 딜레이        

        // 뭉티 정보 불러오는 메소드 호출하는 부분
    }
    public void ShowPrevProfile()
    {
        // 이전 페이지
        mIsNext = false;
        mIsPrev = true;

        mProfile1.mIsMoving = true;
        mProfile2.mIsMoving = true;
        mProfile3.mIsMoving = true;

        btNextBtn.interactable = false;
        btPrevBtn.interactable = false;
        Invoke("DelayActiveBtn", 0.5f);

        // 뭉티 정보 불러오는 메소드 호출하는 부분
    }
    void DelayActiveBtn()
    {
        btNextBtn.interactable = true;
        btPrevBtn.interactable = true;
    }
    public void ShowUpsetMoongti()
    {
        // 화난배경으로 스프라이트 교체
        for (int index = 0; index < iProfileBG.Length; index++)
        {
            iProfileBG[index].sprite = sUpsetProfile[index];
        }
        // 색 원색
        iProfileBG[0].color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        iProfileBG[1].color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        iProfileBG[2].color = new Color(255f / 255f, 255f / 255f, 255f / 255f);

        mProfile1.mIsUpset = true;
        mProfile2.mIsUpset = true;
        mProfile3.mIsUpset = true;

        gShowAll.SetActive(true);
        gShowUpset.SetActive(false);

        MainCam.backgroundColor = new Color(222f / 255f, 219f / 255f, 217f / 255f);

        for (int index = 0; index < gStampF.Length; index++)
        { // stamp 활성화
            gStampF[index].SetActive(true);
        }

        // 뭉티 정보 불러오는 메소드 호출하는 부분
    }

    public void ShowAllMoongti()
    {
        // 기본배경으로 스프라이트 교체
        for (int index = 0; index < iProfileBG.Length; index++)
        {
            iProfileBG[index].sprite = sBasicProfile[index];
        }
        // 색 정해진 색
        iProfileBG[0].color = new Color(235f / 255f, 246f / 255f, 255f / 255f);
        iProfileBG[1].color = new Color(255f / 255f, 237f / 255f, 253f / 255f);
        iProfileBG[2].color = new Color(255f / 255f, 255f / 255f, 239f / 255f);

        mProfile1.mIsUpset = false;
        mProfile2.mIsUpset = false;
        mProfile3.mIsUpset = false;

        gShowAll.SetActive(false);
        gShowUpset.SetActive(true);

        MainCam.backgroundColor = new Color(194f / 255f, 216f / 255f, 233f / 255f);

        for (int index = 0; index < gStampF.Length; index++)
        { // stamp 비활성화
            gStampF[index].SetActive(false);
        }

        // 뭉티 정보 불러오는 메소드 호출하는 부분
    }

    public void GiveCloud()
    {
        // 구름 제공하는 메소드 호출
        Debug.Log("구름제공");
    }
}