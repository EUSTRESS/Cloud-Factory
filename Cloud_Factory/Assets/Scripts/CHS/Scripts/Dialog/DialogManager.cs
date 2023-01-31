using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class DialogManager : MonoBehaviour
{
    // 불러올 값들 선언
    private Guest mGuestManager;
    private SOWManager mSOWManager;

    public int mGuestNum;                       // 손님의 번호를 넘겨받는다.
    private int mGuestSat;                      // 손님의 현재 만족도
    private int mGuestVisitCount;               // 손님의 현재 방문 횟수
    private int mGuestSatVariation;             // 손님의 현재 만족도 증감도
    string mTestName;                           // 테스트를 위한 임시 이름 ( 손님의 이름을 가져왔다고 가정)

    [SerializeField]
    private DialogDB mDialogDB;                 // 대화 내용을 저장해 놓은 DB
    [SerializeField]
    private string[] mTextList;                 // 대화 내용을 불러와서 저장해둘 리스트
    private int[] mGuestImageList;              // 대화 내용에 맞는 표정을 저장해둘 리스트
    private int[] mIsGuset;                     // 누가 이야기하는 내용인지 처리하기

    // 씬 화면에 나올 텍스트에 들어갈 내용 
    private string mDialogGuestName;            // 화면에 출력시킬 손님 이름
    private string mDialogText;                 // 실제로 화면에 출력시킬 내용

    // 씬 화면에 들어가는 텍스트 오브젝트 선언
    public GameObject gTextPanel;               // 대화 창
    public GameObject gTakeGuestPanel;          // 손님 받기/ 거절 버튼

    public Text tText;                          // 대화가 진행 될 텍스트
    public Text tGuestName;                     // 대화중이 손님의 이름이 표시될 텍스트
    public Text tPlayerText;                    // 대화중에 플레이어의 대화가 진행 될 텍스트
    public Text tGuestText;                     // 대화중에 플레이어의 대화가 진행 될 텍스트

    // 손님의 이미지를 띄우는데 필요한 변수들 선언
    public Sprite[] sGuestImageArr;             // 이미지 인덱스들
    public GameObject gGuestSprite;             // 실제 화면에 출력되는 이미지 오브젝트
    private SpriteRenderer sGuestSpriteRender;  // 오브젝트의 Sprite 컴포넌트를 읽어올 SpriteRenderer
    public Animator mGuestAnimator;

    // 대화 구현에 필요한 변수값 선언
    private int mDialogIndex;                   // 해당 만족도에 속하는 지문의 인덱스s
    private int mDialogCharIndex;               // 실제로 화면에 출력시키는 내용의 인덱스
    private int mDialogImageIndex;              // 실제로 화면에 출력시키는 이미지의 인덱스
    private bool isReading;                     // 현재 대화창에서 대화를 출력하는 중인가?
    private bool isLastDialog;                  // 마지막 대화를 불러왔는가?

    // 수락/거절 패널에 필요한 텍스트 오브젝트 받기
    [SerializeField]
    private Text tPanelName;                    // 방문 손님의 이름
    [SerializeField]
    private Text tPanelAge;                     // 방문 손님의 나이
    [SerializeField]
    private Text tPanelJob;                     // 방문 손님의 직업
    [SerializeField]
    private Image iPanelPortrait;               // 방문 손님의 초상화


    // 테스트 함수
    // 대화창에서 다른 캐릭터 혹은 다른 만족도의 텍스트를 받아오는 경우 오류가 있는지 확인하기 위한 함수

    void Awake()
    {

        initDialogManager();

        // 방문주기가 되지 않으면 손님이 나오지 않는다.
        if (mGuestManager.isTimeToTakeGuest)
        {
            LoadDialogInfo();
            ReadDialog();

            initAnimator();
            initTakeGuestPanel();

            // 대화 패널을 활성화
            gTextPanel.SetActive(true);

            // 손님 이미지를 활성화
            gGuestSprite.SetActive(true);
        }
    }

    void initDialogManager()
    {
        mDialogIndex = 0;
        mDialogIndex = GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex;
        mDialogCharIndex = 0;
        mDialogImageIndex = 0;
        tText.text = "";

        mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        sGuestSpriteRender = gGuestSprite.GetComponent<SpriteRenderer>();
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();

        mGuestAnimator = gGuestSprite.GetComponent<Animator>();

        mGuestNum = mGuestManager.mGuestIndex;
        mGuestSat = mGuestManager.mGuestInfo[mGuestNum].mSatatisfaction;
        mGuestVisitCount = mGuestManager.mGuestInfo[mGuestNum].mVisitCount;
        mGuestSatVariation = mGuestManager.mGuestInfo[mGuestNum].mSatVariation;
        tGuestName.text = mGuestManager.mGuestInfo[mGuestNum].mName;

        mGuestImageList = new int[30];
        mTextList = new string[30];
        mIsGuset = new int[30];
        isReading = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void initAnimator()
    {
        // 초상화의 애니메이션 클립을 초기화한다.
        mGuestAnimator.runtimeAnimatorController = GetComponent<DrawAniClip>().animators[mGuestNum];
    }

    public void MoveSceneToWeatherSpace()
    {
        SceneManager.LoadScene("Space Of Weather");
    }

    // 해당 손님에 대한 대화값 정보를 불러오는 함수
    private void LoadDialogInfo()
    {
        // 게임 내에 GameManager 한개를 생성하고, 그 곳에서 하루마다 6명의 손님을 지정하여 응접실에 플레이어가 없는 시간에 한하여 랜덤하게 한명씩 방문시킨다.
        // GameManager에서 지정한 손님의 번호를 받아오고, 손님의 번호에 맞는 손님의 정보를 가져온다.

        int i;
        int j = 0;

        List<DialogDBEntity> Dialog;
        Dialog = mDialogDB.SetDialogByGuestNum(mGuestNum);
        int[] speakEmotionEffect = mGuestManager.SpeakEmotionEffect(mGuestNum);
        int tempVisitCount = 0;                                     // 시트에 방문 횟수가 정수가 아닌 범위로 되어있는 관계로 설정하는 임시 정수
                                                                    // 해당 정수가 2일 때, 1<x<10 범위로 판단

        // Dialog Null 반환시 오류 출력
        if (Dialog == null)
            Debug.Log("대화를 불러오는데에 오류가 발생하였습니다.");

		if (mGuestVisitCount <= 1 || mGuestVisitCount >= 10) { tempVisitCount = mGuestVisitCount; }
		else { tempVisitCount = 2; }

		// 손님 번호 -> 방문 횟수 -> 만족도 순으로 엑셀 텍스트 파일을 체크한다.
		for (i = 0; i < Dialog.Count; ++i)
        {
            if (Dialog[i].GuestID == mGuestNum + 1                  // 게스트 번호
                && Dialog[i].VisitCount == tempVisitCount           // 방문 횟수
                && Dialog[i].Sat == mGuestSat)                      // 만족도
            {
                if (tempVisitCount >= 10 || (tempVisitCount < 10 && Dialog[i].SatVariation == mGuestSatVariation)) // 방문 횟수가 10일 때, 만족도 증감도에 관계없이
                {
                    //Text가 Hint이면 xls에서 상하한선에 가장 가까운 감정의 대사 동적으로 할당
                    if (Dialog[i].Text == "Hint")
                    {
                        for (int count = 0; count < speakEmotionEffect.Length; count++)
                        {
                            for (int num = 0; num < Dialog.Count; num++)
                            {
                                if (Dialog[num].GuestID == mGuestNum + 1
                                    && Dialog[num].VisitCount == 0                          // 대사 파일 받고 수정할 가능성 O
                                    && Dialog[num].Emotion == speakEmotionEffect[count])    // TODO: 추후 텍스트 엑셀 파일 보고 조건 수정 필요
                                {
                                    mTextList[j] += Dialog[num].Text;
                                    mGuestImageList[j] = Dialog[num].DialogImageNumber;
                                    mIsGuset[j] = Dialog[num].isGuest;
                                    j++;
                                    continue;
                                }
                            }
                        }
                    }
                    else
                    {
                        mTextList[j] = Dialog[i].Text;
                        mGuestImageList[j] = Dialog[i].DialogImageNumber;
                        mIsGuset[j] = Dialog[i].isGuest;
                        Debug.Log(j + " " + mIsGuset[j]);
                        j++;
                    }
                }
            }
        }
        //for (i = 0; i < mDialogDB.DialogText1.Count; ++i)
        //{
        //    if (mDialogDB.DialogText1[i].GuestID == mGuestNum + 1)
        //    {
        //        if (mDialogDB.DialogText1[i].VisitCount == mGuestVisitCount)
        //        {
        //            if (mDialogDB.DialogText1[i].Sat == mGuestSat)
        //            {
        //                mTextList[j] = mDialogDB.DialogText1[i].Text;
        //                mGuestImageList[j] = mDialogDB.DialogText1[i].DialogImageNumber;
        //                mIsGuset[j] = mDialogDB.DialogText1[i].isGuest;
        //                Debug.Log(j + " " + mIsGuset[j]);
        //                j++;
        //            }
        //        }
        //    }
        //}

        mTextList[j] = "End";
    }

    private void InitDialog()
    {
        mDialogCharIndex = 0;
        tGuestText.text = "";
        tPlayerText.text = "";

        // 손님의 대사라면
        if (mIsGuset[GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex] == 1)
        {
            tText = tGuestText;
        }
        else
        {
            tText = tPlayerText;
        }
        Debug.Log(mDialogIndex + " " + mIsGuset[mDialogIndex]);
    }
    public string GetDialog(int dialogindex) // 만족도 , 대화 내용 순번
    {
        return mTextList[dialogindex];
    }

    private void ReadDialogAtAll()
    {
        tText.text += GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex);
        isReading = false;
    }

    private void ReadDialogAtOne()
    {
        isReading = true;
        if (tText.text == GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex))
        {
            // 텍스트가 모두 출력이 된 경우에 클릭 시, 다음 문장이 출력된다.
            if (GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex) != "End")
            {
                GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex += 1;
                mDialogImageIndex++;
                isReading = false;
            }
            return;
        }
        tText.text += GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex)[mDialogCharIndex];
        mDialogCharIndex++;

        Invoke("ReadDialogAtOne", 0.05f);
    }

    // 손님과의 대화를 실행시켜주는 함수
    public void ReadDialog()
    {
        InitDialog();

        mGuestAnimator.SetInteger("index", mGuestImageList[GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex]);

        // 마지막 End 문자열이 나오는 경우 ( 대화를 모두 불러온 경우)
        if (GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex) == "End")
        {
            isLastDialog = true;
            // 대화 내용을 모두 출력하고 나면 손님 응대에 관한 여부를 플레이어에게 묻는다. (받는다/ 받지 않는다)
            TakeGuest();
            return;
        }
        // 대화가 출력중인 도중에 클릭한 경우, 문장이 한번에 출력이 된다.
        if (isReading == true)
        {
            ReadDialogAtAll();
            return;
        }
        // 기본적으로 빈 텍스트에서 대화 내용을 한 글자씩 추가하여 출력하고 딜레이 하기를 반복한다.
        ReadDialogAtOne();
        return;
    }

    private void TakeGuest()
    {
        gTakeGuestPanel.SetActive(true);
    }

    // 손님 수락하기
    public void AcceptGuest()
    {
        gTakeGuestPanel.SetActive(false);

		mSOWManager.InsertGuest(mGuestNum);
		mSOWManager.isNewGuest = true;

		mGuestManager.InitGuestTime();

		// 손님이 이동했으므로 응접실에 있는 것들을 초기화 시켜준다.
		ClearGuest();
		MoveSceneToWeatherSpace();
	}

    // 손님 거절하기
    public void RejectGuest()
    {
        Debug.Log("손님을 받지 않습니다.");

        // 방문하지 않는 횟수를 3으로 지정한다. (3일간 방문 X)
        mGuestManager.mGuestInfo[mGuestNum].mNotVisitCount = 3;
        mGuestManager.InitGuestTime();

        // 손님이 이동했으므로 응접실에 있는 것들을 초기화 시켜준다.
        ClearGuest();
        MoveSceneToWeatherSpace();
    }

    // 응접실을 초기화 시켜준다.
    private void ClearGuest()
    {
        // 방문횟수 1회 증가
        mGuestManager.mGuestInfo[mGuestNum].mVisitCount++;

        // 손님이 응접실에 없다고 표시
        mGuestManager.isGuestInLivingRoom = false;

        // 대화 인덱스를 0으로 초기화
        GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex = 0;

        // 대화 패널을 비활성화
        gTextPanel.SetActive(false);

        // 손님 이미지를 비활성화
    }

    // 수락/거절 하는 패널을 방문한 손님의 정보로 초기화 한다.
    private void initTakeGuestPanel()
    {
        GuestInfos guest = mGuestManager.mGuestInfo[mGuestNum];

        tPanelName.text = "이름: " + guest.mName;
        tPanelAge.text = "나이: " + guest.mAge;
        tPanelJob.text = "직업: " + guest.mJob;
        iPanelPortrait.sprite = sGuestImageArr[mGuestNum];
    }

}