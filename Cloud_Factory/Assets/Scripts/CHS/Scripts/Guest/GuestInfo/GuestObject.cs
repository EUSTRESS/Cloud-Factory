using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Pathfinding;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GuestObject : MonoBehaviour
{
    // 오브젝트 내에서 필요한 변수
    [Header("[손님 정보]")]
    public float        mLimitTime;         // 손님이 대기한 시간
    public float        mMaxLimitTime;      // 손님이 대기하는 시간의 최대값
    public int          mGuestNum;          // 해당 오브젝트의 손님번호
    private Transform   mTransform;         // 위치값이 변하는지 확인하기 위한 변수
    public Transform    mTargetChair;       // 목표로 하는 의자의 위치
    public int          mTargetChiarIndex;

    [Header("[FSM 관련]")]
    public bool isSit;                      // 자리에 앉아있는가?
    public bool isUsing;                    // 구름 치료를 받는중인가?
    public bool isMove;                     // 이동중인가?   
    public bool isGotoEntrance;             // 출구로 나가는 중인가?
    public bool isEndUsingCloud;            // 구름 사용을 끝마쳤는가?

    [Header("[감정표현 관련]")]
    public int   dialogEmotion;             // 감정 표현시, 말풍선으로 나오는 감정의 번호 
    public int[] faceValue;                 // 감정 표현시, 이펙트로 나오는 감정의 번호       
    public GameObject SpeechBubble;         // 감정 표현시, 말풍선 내용을 채우는 텍스트 칸
    public bool isSpeakEmotion;             // 손님이 감정표현 중인지를 나타내는 변수값      

    [Header("[희귀도 4 재료 제공 대사 관련]")]
    private RLHReader   textReader;
    private bool        isHintTextPrinted;
    public bool         isUseRarity4;
    private bool        isUsingHint;

    [Header("[기타]")]
    public Animator     mGuestAnim;         // 손님의 애니메이션 변수
    private Guest       mGuestManager;
    public SOWManager   mSOWManager;

    // 상수값 저장
    const int MAX_GUEST_NUM = 20;
    const int MAX_EMOTION_NUM = 20;
    const float DELAY_OF_SPEECH_BUBBLE = 5.0f;
    const float CHAR_SIZE = 0.9f;

    List<List<int>> EmotionList = new List<List<int>>
    {
        new List<int> {0,8,15,7}, // JOY
        new List<int> {1,2,16,13}, // SAD
        new List<int> {4,9}, // CALM
        new List<int> {3,6,14,12,11}, // ANGRY
        new List<int> {18,19,5,17,10}  // SURPRISE
    };

    // 감정표현 시, 감정에 대해 표현할 말풍선 내용 리스트 -> 감정 번호를 이용하여 내용을 가져와서 채운다.
    string[] EmotionDialogList = new string[]
    {
        "기쁨",
        "불안",
        "슬픔",
        "짜증",
        "수용",
        "놀람&혼란",
        "혐오",
        "관심&기대",
        "사랑",
        "순종",
        "경외심",
        "반대",
        "자책",
        "경멸",
        "공격성",
        "낙천",
        "씁쓸함",
        "애증",
        "얼어붙음",
        "혼란스러움",
    };

    // 손님과 상호작용을 위해 필요한 콜라이더 
    private Collider2D sitCollider;
    private Collider2D walkCollider;

    // 입장과 퇴장시의 만족도 저장
    private int enterSat;
    private int outSat;

    // 각 손님의 번호에 따라 애니메이터를 만들어서 저장한다.
    public RuntimeAnimatorController[] animators = new RuntimeAnimatorController[MAX_GUEST_NUM];

    // 각 감정별 이펙트를 저장해두고 해당 상황에 따라 변경해주어 출력한다.
    public Animation[] EffectAnimations = new Animation[MAX_EMOTION_NUM];

    // 감정표현 이펙트를 Front/Back으로 나누어서 관리한다.
    public Animator FrontEffect;
    public Animator BackEffect;

    // 손님 번호를 저장해준다.
    public void setGuestNum(int guestNum = 0)
    {
        mGuestNum = guestNum;      
    }


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void init()
    {
        // 대기시간 초기화
        mLimitTime = 0.0f;
        //mMaxLimitTime = 50.0f;

        isSit = false;
        isUsing = false;
        isMove = false;
        isGotoEntrance = false;
        isEndUsingCloud = false;
        mTransform = this.transform;
        mTargetChiarIndex = -1;
        mTargetChair = null;
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        mGuestAnim = GetComponent<Animator>();

        sitCollider = this.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<CircleCollider2D>();
        walkCollider = this.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<CircleCollider2D>();

        enterSat = mGuestManager.mGuestInfo[mGuestNum].mSatatisfaction;

        faceValue = mGuestManager.SpeakEmotionEffect(mGuestNum);
        dialogEmotion = mGuestManager.SpeakEmotionDialog(mGuestNum);
        SpeechBubble = this.transform.GetChild(1).gameObject;
        isSpeakEmotion = false;

        BackEffect = this.transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<Animator>();

        textReader = this.gameObject.GetComponent<RLHReader>();
        isHintTextPrinted = false;
        isUseRarity4 = true; // Test를 위해서 true로 임시 변경
        isUsingHint = false;
    }

// 걷는 애니메이션 출력
// 걷는 애니메이션을 디폴트 애니메이션으로 설정

private void Update()
    {
        // 할당받는 의자 설정
        if (mTargetChiarIndex != -1 && isGotoEntrance == false)
        {
            mTargetChair = mSOWManager.mChairPos[mTargetChiarIndex].transform;
            mSOWManager.mCheckChairEmpty[mTargetChiarIndex] = false;
            this.GetComponent<AIDestinationSetter>().enabled = true;
            this.GetComponent<AIDestinationSetter>().target = mTargetChair.transform;

            // 의자에 도착하지 않았다면 AIPATH를 활성화한다.
            if (this.transform != mTargetChair.transform)
            {
                this.GetComponent<WayPoint>().isMove = false;
                this.GetComponent<AIPath>().enabled = true;
            }
            else
            {
                this.GetComponent<AIPath>().enabled = false;
            }

            mGuestAnim.SetBool("isStand", false);
        }

        // 구름을 제공받는 상태가 아니라면 대기시간을 갱신시킨다.
        if (isUsing == false)
        {
            TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
            if (SceneManager.GetActiveScene().name != "Lobby"
                && SceneManager.GetActiveScene().name != "Cloud Storage"
                && SceneManager.GetActiveScene().name != "Give Cloud"
                && SceneManager.GetActiveScene().name != "Give Cloud"
                && mTutorialManager.isTutorial == false)
            {
                mLimitTime += Time.deltaTime;
            }
        }

        // 대기시간이 지나거나 불만뭉티가 된 경우에 (치료를 마치고 가능 경우는 제외)
        if ((mLimitTime > mMaxLimitTime || mGuestManager.mGuestInfo[mGuestNum].isDisSat == true) && !isGotoEntrance)
        {
            // 사용자 리스트에서 없애고, 해당 의자를 다시 true로 바꿔주어야 한다.
            mSOWManager.mCheckChairEmpty[mTargetChiarIndex] = true;
            mTargetChair = null;
            isSit = false;


            // 구름 사용가능 리스트에서 삭제
            int count = mSOWManager.mUsingGuestList.Count;
            for (int i = 0; i < count; i++)
            {
                if (mSOWManager.mUsingGuestList[i] == mGuestNum)
                {
                    mSOWManager.mUsingGuestList.RemoveAt(i);
                    mSOWManager.mUsingGuestObjectList.RemoveAt(i);
                }
            }

            // 불만 손님으로 변환 후, 귀가
            mGuestManager.mGuestInfo[mGuestNum].isDisSat = true;

            Debug.Log("Time");

            MoveToEntrance();
        }

        // 입구에 도달한 경우
        if (isGotoEntrance == true && transform.position.x - mSOWManager.mWayPoint[0].transform.position.x <= 0.2f)
        {
            Debug.Log("Destroy");
            Destroy(this.gameObject);
           
        }

        // 의자에 도달한 경우
        if (mTargetChiarIndex != -1)
        {
            if (isGotoEntrance == false && Mathf.Abs(transform.position.x - mTargetChair.transform.position.x) 
                <= 0.1f && Mathf.Abs(transform.position.y - mTargetChair.transform.position.y) <= 0.1f)
            {
                // 의자 위치로 이동 , 방향에 따라서 LocalScale 조정
                if (mSOWManager.mSitDir[mTargetChiarIndex] == 1)
                {
                    transform.localScale = new Vector3(CHAR_SIZE, CHAR_SIZE, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(-CHAR_SIZE, CHAR_SIZE, 1f);

                    SpeechBubble.transform.GetChild(1).gameObject.transform.localScale = new Vector3(-CHAR_SIZE, CHAR_SIZE, 1f);
                }

                mGuestAnim.SetBool("isSit", true);
                ChangeLayerToSit();

                // TODO : 콜라이더 변경 Walking ->Sitting
                sitCollider.enabled = true;
                walkCollider.enabled = false;

                this.transform.position = mTargetChair.transform.position;
                isSit = true;
            }
        }
        // 상태에 따라서 애니메이션 제공
        if (isSit)
        {
            if(mGuestManager.mGuestInfo[mGuestNum].isUsing == true)
            {
                isUsing = true;      
            }
            // 치료 중인 경우 치료효과에 따라서 주기적으로 애니메이션을 제공
            if (isUsing)
            {
                // 제공 받은 구름의 영향에 따라서 앉아있는 모습이 긍정적/부정적 중 하나가 나온다.
                mGuestAnim.SetBool("isUsing", true);

                // 사용시간이 지나면 구름 오브젝트에서 실행된 코루틴을 통해 isEndUsingCloud가 true가 되어 귀가한다.
                if (isEndUsingCloud)
                {
                    // 희귀도 4재료를 사용했는지 체크
                    if (isUseRarity4)
                    {
                        // 사용하였고 아직 힌트를 출력하지 않았다면 힌트 출력
                        if (!isHintTextPrinted && !isUsingHint)
                        {
                            Hint();
                        }
                        // 힌트 출력을 완료했다면 귀가
                        else if (isHintTextPrinted)
                        {
                            MoveToEntrance();
                        }
                        MoveToEntrance();
                    }
                    else
                    {
                        // 사용하지 않았다면 바로 귀가
                        MoveToEntrance();
                    }
                }
            }
        }
        // 구름 이용이 끝났을 때         TODO: 희귀도 4재료가 들어갔는지 체크해야 함
        if (isEndUsingCloud && !isHintTextPrinted)
        {

            //isHintTextPrinted = true;
			//TextMeshPro Text = SpeechBubble.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
			//Text.text = textReader.PrintHintText();
			//SpeechBubble.transform.GetChild(0).gameObject.SetActive(true);                                  // 말풍선 활성화
			//SpeechBubble.transform.GetChild(1).gameObject.SetActive(true);                                  // 텍스트 활성화
			//Invoke("EndBubble", 5.0f);
		}

        // 걷는 방향에 따라 애니메이션의 방향을 다르게 지정한다.
        if (GetComponent<AIPath>().desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(CHAR_SIZE, CHAR_SIZE, 1f);
        }
        else if (GetComponent<AIPath>().desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-CHAR_SIZE, CHAR_SIZE, 1f);

            SpeechBubble.transform.GetChild(1).gameObject.transform.localScale = new Vector3(-CHAR_SIZE, CHAR_SIZE, 1f);
        }
        // 현재 위치를 저장한다.
        mTransform = transform;
    }

    public void SpeakEmotion()
    { 
        // 앉아있는 경우에만 클릭 시 상호작용을 통해 감정을 표현한다.
        if (!mGuestAnim.GetBool("isSit")) return;

        // 이미 상호작용 중인 경우에는 클릭할 수 없게 제한한다.
        if (isSpeakEmotion)
        {
            Debug.Log("Already Speaking");
            return;         
        }


        // 힌트를 출력중인 경우에도 감정표현을 할 수 없다.
        // TODO : 힌트를 출력중인 경우 return하게끔 구현

        Debug.Log("감정 모션을 출력합니다");
        isSpeakEmotion = true;

        // 감정 상한, 하한 범위에 가장 가까운 감정에 대한 힌트(이펙트)
        for (int i = 0; i< faceValue.Length; i++)
        {
            StartCoroutine(Emotion(3.0f * (i), faceValue[i]));
        }
        // 만족도 반영 범위에서 가장 먼 감정을 알려주는 말풍선  -> 손님의 위치값에 따라 좌/우 측에 생성
        StartCoroutine("DialogEmotion");

        // 상호작용 중임을 나타내는 bool값을 상호작용이 끝난 이후에 false로 갱신한다.
        if (faceValue.Length > 1) Invoke("EndSpeakEmotion", faceValue.Length * 3.0f + 1.0f);
        else Invoke("EndSpeakEmotion", 6.0f);
    }

    // Hint를 출력해야 하는 경우 
    public void Hint()
    {
        // 앉아있는 경우에만 클릭 시 상호작용을 통해 감정을 표현한다.
        if (!mGuestAnim.GetBool("isSit")) return;

        // 이미 상호작용 중인 경우에는 클릭할 수 없게 제한한다.
        if (isSpeakEmotion)
        {
            Debug.Log("Already Speaking");
            return;
        }

        isUsingHint = true;
        isSpeakEmotion = true;

        // 말풍선에 사용할 내용 불러오기 -> 리스트에서 감정값에 따라서 불러오기
        textReader.LoadHintInfo(mGuestNum);
        string temp = textReader.PrintHintText();
        TextMeshPro Text = SpeechBubble.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
        Animator Anim = SpeechBubble.transform.GetChild(0).gameObject.GetComponent<Animator>();
        Text.text = temp;

        SpeechBubble.transform.GetChild(1).gameObject.transform.localScale = this.transform.localScale;

        // 말풍선 띄우기
        SpeechBubble.SetActive(true);
        SpeechBubble.transform.GetChild(0).gameObject.SetActive(true);
        Anim.SetTrigger("Start");
        mGuestAnim.SetTrigger("Hint");

        // 일정시간 이후 말풍선 제거
        Invoke("EndBubble", 5.0f);
    }
    private void EndBubble()
    {
        Animator Anim = SpeechBubble.transform.GetChild(0).gameObject.GetComponent<Animator>();
        Anim.SetTrigger("EndBubble");
        mGuestAnim.SetTrigger("EndHint");
        isHintTextPrinted = true;
        EndSpeakEmotion();
    }


    private void EndSpeakEmotion()
    {
        isSpeakEmotion = false;
    }

    IEnumerator DialogEmotion()
    {
        // 말풍선 내용 채우기
        string temp = EmotionDialogList[dialogEmotion];
        TextMeshPro Text = SpeechBubble.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
        Animator Anim = SpeechBubble.transform.GetChild(0).gameObject.GetComponent<Animator>();

        Debug.Log("DialogEmotion : " + dialogEmotion);

        // Text가 NULL이 아닌경우 내용 채워넣기
        if (Text != null)
            Text.text = temp;

        SpeechBubble.transform.GetChild(1).gameObject.transform.localScale = this.transform.localScale;

        // 말풍선 띄우기
        SpeechBubble.SetActive(true);
        SpeechBubble.transform.GetChild(0).gameObject.SetActive(true);
        Anim.SetTrigger("Start");

        // 딜레이
        yield return new WaitForSeconds(DELAY_OF_SPEECH_BUBBLE);

        // 말풍선 지우기
        Anim.SetTrigger("EndBubble");
    }

    IEnumerator Emotion(float delay, int emotionNum)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log(emotionNum + "Emotion 출력");

        // Interaction 트리거 발동 -> emotionNum에 따라서 FaceValue값을 변동시킨다.
        mGuestAnim.SetInteger("FaceValue", ChangeFaceValue(emotionNum));

        // 해당 emotionNum에 해당하는 이펙트를 재생시킨다.
        BackEffect.SetInteger("EmotionValue", emotionNum);
        Debug.Log("Emotion Value : " + emotionNum);

        mGuestAnim.SetTrigger("Interaction");
        Invoke("EndInteraction", 2.9f);
    }

    int ChangeFaceValue(int emotionNum)
    {
        for(int i = 0; i< EmotionList.Count; i++)
        {
            foreach(int index in EmotionList[i])
            {
                if (index == emotionNum)
                    return i;
            }
        }
        return -1;
    }

    void EndInteraction()
    {
        BackEffect.SetInteger("EmotionValue", -1);
        mGuestAnim.SetTrigger("InteractionEnd");
        Debug.Log("Emotion 출력 마무리");
    }

    void EndHint()
    {
        mGuestAnim.SetTrigger("EndHint");
    }


    // 애니메이션 클립들을 손님에 맞게 초기화한다.
    public void initAnimator()
    {
        GetComponent<Animator>().runtimeAnimatorController = animators[mGuestNum];
        Debug.Log("init Guest Anim");
    }

    // 입구로 퇴장하는 함수이다.
    private void MoveToEntrance()
    {
        //대기 시간이 지났거나, 구름을 제공받았을 때, 만족도 증감도 계산
        outSat = mGuestManager.mGuestInfo[mGuestNum].mSatatisfaction;
        CalcSatVariation(enterSat, outSat);
        // Demo Version
        if(mGuestManager.mGuestInfo[mGuestNum].mSatatisfaction <= 0) { mGuestManager.mGuestInfo[mGuestNum].mSatatisfaction = 1; }

        mSOWManager.mCheckChairEmpty[mTargetChiarIndex] = true;
        mTargetChair = null;
        isSit = false;
        isUsing = false;
        mGuestManager.mGuestInfo[mGuestNum].isUsing = false;
        mGuestAnim.SetBool("isUsing", false);

        isGotoEntrance = true;
        mGuestAnim.SetBool("isSit", false);

        if (mGuestManager.mGuestInfo[mGuestNum].isDisSat == true)
        {
            mGuestAnim.SetBool("isDisSat", true);
            Invoke("ChangeTarget", 2.5f);
        }
        else if(mGuestManager.mGuestInfo[mGuestNum].mSatatisfaction >= 5)
        {
            mGuestAnim.SetBool("isFullSat", true);
            GameObject.Find("GuestManager").GetComponent<LetterController>().SetSatGuestList(mGuestNum);
            Invoke("ChangeTarget", 3.0f);
        }
        else
        {
            Invoke("ChangeTarget", 1.0f);
        }
        ChangeLayerToDefault();

        // TODO : 콜라이더 변경 Sitting -> Walking
        sitCollider.enabled = false;
        walkCollider.enabled = true;

        // 부여받은 의자 인덱스값 초기화
        mGuestManager.mGuestInfo[mGuestNum].mSitChairIndex = -1;
    }
    private void CalcSatVariation(int enterSat, int outSat)
    {
        if (enterSat > outSat) { mGuestManager.mGuestInfo[mGuestNum].mSatVariation = -1; }           // 만족도 감소
        else if (enterSat == outSat) { mGuestManager.mGuestInfo[mGuestNum].mSatVariation = 0; }     // 만족도 유지
        else { mGuestManager.mGuestInfo[mGuestNum].mSatVariation = 1; }                             // 만족도 증가
    }


    private void ChangeTarget()
    {
        this.GetComponent<WayPoint>().isMove = false;
        this.GetComponent<AIPath>().enabled = true;
        this.GetComponent<AIDestinationSetter>().enabled = true;

        this.GetComponent<AIDestinationSetter>().target = mSOWManager.mWayPoint[0].transform;
    }

    public void ChangeLayerToSit()
    {
        this.GetComponent<SortingGroup>().sortingLayerName = "SittingGuest";
    }

    public void ChangeLayerToDefault()
    {
        this.GetComponent<SortingGroup>().sortingLayerName = "Guest";
    }
}
