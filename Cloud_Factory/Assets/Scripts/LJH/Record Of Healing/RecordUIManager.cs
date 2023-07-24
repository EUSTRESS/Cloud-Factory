using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 치유의 기록 UI
public class RecordUIManager : MonoBehaviour
{
    [Header("GAME OBJECT")]
    // 치유의 기록
    public GameObject gShowUpset; // 불만 뭉티보기
    public GameObject gShowAll;   // 전체 보기 
    public GameObject[] gStampF = new GameObject[4]; // 불만 뭉티 스탬프
    public GameObject gUpsetStory; // 불만 뭉티는 스토리 없어짐
    // 치유의기록
    public Image[] iProfileBG = new Image[4]; // 프로필 배경
    public Image iMainBg;

    [Header("SPRITES")]
    public Sprite[] sBasicProfile = new Sprite[4]; // 기본 프로필
    public Sprite[] sUpsetProfile = new Sprite[4]; // 화난 프로필

    [Header("BUTTON")]

    //치유의 기록     
    public Button btNextBtn;           // 다음페이지 버튼
    public Button btPrevBtn;           // 이전페이지 버튼


    [HideInInspector]
    public bool mIsNext;               // 치유의 기록 다음 페이지   
    [HideInInspector]
    public bool mIsPrev;               // 치유의 기록 이전 페이지

    private ProfileMoving mProfile1;   // 프로필 움직임 담당 스크립트
    private ProfileMoving mProfile2;   // 프로필 움직임 담당 스크립트
    private ProfileMoving mProfile3;   // 프로필 움직임 담당 스크립트

    // 구름 제공을 위한 참조 오브젝트
    public GameObject mGetCloudContainer;
    SOWManager SOWManager;

    void Awake()
    {
        mProfile1 = GameObject.Find("I_ProfileBG1").GetComponent<ProfileMoving>();
        mProfile2 = GameObject.Find("I_ProfileBG2").GetComponent<ProfileMoving>();
        mProfile3 = GameObject.Find("I_ProfileBG3").GetComponent<ProfileMoving>();

        SOWManager SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

        // 구름 이용이 가능한 뭉티들의 리스트를 초기화 하는 함수 호출
        initProfileList();

    }
    private void Update()
    {


    }
    void initProfileList()
    {


    }

    void updateProfileList()
    {
        // 보여지는 리스트를 기준으로 1번은 현재 프로필이 보이는 뭉티, 2번은 다음 뭉티, 3번은 현재 이전의 뭉티를 업데이트한다.
        // 만약 1,2,3번 중 존재하지 않는 경우에는 따로 업데이트 하지 않는다.


    }


    public void ShowNextProfile()
    {
        if (TutorialManager.GetInstance().isTutorial) return;
        
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

        // 다음 뭉티 정보 불러오는 메소드 호출하는 부분
        Debug.Log("다음 뭉티 정보 호출");
    }
    public void ShowPrevProfile()
    {
        if (TutorialManager.GetInstance().isTutorial) return;

        // 이전 페이지
        mIsNext = false;
        mIsPrev = true;

        mProfile1.mIsMoving = true;
        mProfile2.mIsMoving = true;
        mProfile3.mIsMoving = true;

        // 버그 방지를 위한 버튼 비활성화
        btNextBtn.interactable = false;
        btPrevBtn.interactable = false;
        Invoke("DelayActiveBtn", 0.5f);

        // 다음 뭉티 정보 불러오는 메소드 호출하는 부분
        


        Debug.Log("이전 뭉티 정보 호출");
    }
    void DelayActiveBtn()
    {
        btNextBtn.interactable = true;
        btPrevBtn.interactable = true;
    }
    // 불만 뭉티 보여주는 함수
    public void ShowUpsetMoongti()
    {
        ControlMoongtiUI(sUpsetProfile, true);

        // 색 원색
        iProfileBG[0].color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        iProfileBG[1].color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        iProfileBG[2].color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        iMainBg.color = new Color(222f / 255f, 219f / 255f, 217f / 255f);

        // 뭉티 정보 불러오는 메소드 호출하는 부분
        Debug.Log("불만 뭉티 정보 불러오는 메소드 호출");
    }
    // 전체 보기
    public void ShowAllMoongti()
    {
        ControlMoongtiUI(sBasicProfile, false);

        // 색 정해진 색
        iProfileBG[0].color = new Color(235f / 255f, 246f / 255f, 255f / 255f);
        iProfileBG[1].color = new Color(255f / 255f, 237f / 255f, 253f / 255f);
        iProfileBG[2].color = new Color(255f / 255f, 255f / 255f, 239f / 255f);
        iMainBg.color = new Color(194f / 255f, 216f / 255f, 233f / 255f);

        // 뭉티 정보 불러오는 메소드 호출하는 부분
        Debug.Log("전체 뭉티 정보 불러오는 메소드 호출");
    }

    void ControlMoongtiUI(Sprite[] _szProfile, bool _bisUpset)
    {
        // 기본배경으로 스프라이트 교체
        for (int index = 0; index < iProfileBG.Length; index++)
        {
            iProfileBG[index].sprite = _szProfile[index];
        }

        mProfile1.mIsUpset = _bisUpset;
        mProfile2.mIsUpset = _bisUpset;
        mProfile3.mIsUpset = _bisUpset;

        gShowAll.SetActive(_bisUpset);
        gShowUpset.SetActive(!_bisUpset);

        for (int index = 0; index < gStampF.Length; index++)
        { // stamp 비활성화
            gStampF[index].SetActive(_bisUpset);
        }

        gUpsetStory.SetActive(!_bisUpset);
    }

}