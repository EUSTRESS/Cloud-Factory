using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guest : MonoBehaviour
{
    // 상수 선언
    private const int NUM_OF_GUEST = 20;                                // 손님의 총 인원 수
    private const int NUM_OF_TODAY_GUEST_LIST = 6;                      // 하루에 방문하는 손님의 총 인원 수

    [Header ("[손님 정보값 리스트]")]
    public GuestInfos[] mGuestInfo;                                     // 손님들의 정보값
    [SerializeField]
    private GuestInfo[] mGuestInitInfos;                                // Scriptable Objects들의 정보를 담고 있는 배열

    [Header ("[손님 방문 관련 정보]")]
    public bool isGuestInLivingRoom;                                    // 응접실에 손님이 방문해있는가?
    public bool isTimeToTakeGuest;                                      // 뭉티 방문주기가 지났는지 확인

    [Space(10f)]
    public int mGuestIndex;                                             // 이번에 방문할 뭉티의 번호
    public int[] mTodayGuestList = new int[NUM_OF_TODAY_GUEST_LIST];    // 오늘 방문 예정인 뭉티 목록
    [SerializeField]
    private int mGuestCount;                                            // 이번에 방문할 뭉티의 순서

    [Space (10f)]
    public float mGuestTime;                                            // 뭉티의 방문 주기의 현재 값
    public float mMaxGuestTime;                                         // 뭉티의 방문 주기


    [SerializeField]
    private int mGuestMax;                                              // 오늘 방문하는 뭉티의 최대 숫자


    private static Guest instance = null;                               // 싱글톤 기법을 위함 instance 생성
    private void Awake()
    {
        // 싱글톤 기법 사용
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            mGuestTime = 0;
            mGuestCount = -1;
            mGuestMax = 0;
            mMaxGuestTime = 5.0f;
            
            mGuestInfo = new GuestInfos[NUM_OF_GUEST];
            for (int i = 0; i< NUM_OF_GUEST; i++)
            {
                InitGuestData(i);
            }

            InitDay();

            isTimeToTakeGuest = false;
            isGuestInLivingRoom = false;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 뭉티의 방문주기를 돌린다.
        if (mGuestTime < mMaxGuestTime)
        {
            mGuestTime += Time.deltaTime;
        }
        else if (mGuestTime >= mMaxGuestTime && isTimeToTakeGuest == false)
        {
            // 모든 인덱스가 다 되지 않는 한 뭉티 방문주기가 다된경우 새로운 뭉티를 들여보낸다.
            if (mGuestCount < mGuestMax - 1) 
            {
                isTimeToTakeGuest = true;
                TakeGuest();
                // 응접실 이동하는 버튼들에 대한 상호작용
            }
            else
            {
                Debug.Log("모든 뭉티가 방문하였습니다.");
            }
        }
        // 싱글톤 기법 확인을 위한 테스트코드
        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeGuest();
        }
        // 만족도 갱신을 위한 함수 테스트 (성공)
        if (Input.GetKeyDown(KeyCode.C))
        {
            RenewakSat(0);
        }
        // 상하한선 침범 확인을 위한 함수 테스트 (성공)
        if (Input.GetKeyDown(KeyCode.D))
        {
            mGuestInfo[0].isDisSat = CheckIsDisSat(0);
            Debug.Log(mGuestInfo[0].isDisSat);
        }
    }

    public void TakeGuest()
    {
        if (isTimeToTakeGuest == true && isGuestInLivingRoom == false)
        {
            mGuestCount++;
            mGuestIndex = mTodayGuestList[mGuestCount];
            isGuestInLivingRoom = true;
        }
    }

    // 뭉티의 정보값들을 받아오는 API
    public string GetName(int gusetNum)
    {
        return mGuestInfo[gusetNum].mName;
    }

    public bool CheckIsDisSat(int guestNum)
    {
        int temp = IsExcessLine(guestNum);                      // 침범하는 경우에 감정값을 임의로 저장할 변수

        // 상하한 선을 침범한 경우를 확인
        if (temp != -1)
        {
            mGuestInfo[guestNum].isDisSat = true;              // 불만 뭉티로 변환
            mGuestInfo[guestNum].mSatatisfaction = 0;          // 만족도 0 으로 갱신
            mGuestInfo[guestNum].mVisitCount = 0;              // 남은 방문횟수 0으로 갱신

            // TODO : 치유의 기록으로 불만 뭉티가 된 상태와 손님 번호, 어떤 감정 변화로 인한 것인지 전달해주기


            return true;
        }
        return false;
    }

    // 뭉티의 정보값 변경에 필요한 API 
    public void SetEmotion(int guestNum, int emotionNum, int value)
    {
        mGuestInfo[guestNum].mEmotion[emotionNum] += value;
    }

    public int IsExcessLine(int guestNum) // 감정 상하한선을 침범했는지 확인하는 함수. 
    {
        SLimitEmotion[] limitEmotion = mGuestInfo[guestNum].mLimitEmotions;

        for (int i = 0; i < 2; i++)
        {
            if (mGuestInfo[guestNum].mEmotion[limitEmotion[i].upLimitEmotion] >= limitEmotion[i].upLimitEmotionValue) // 상하한선을 침범한 경우
            {
                Debug.Log("상하한선을 침범하였습니다");
                return limitEmotion[i].upLimitEmotion;
            }
            else if (mGuestInfo[guestNum].mEmotion[limitEmotion[i].downLimitEmotion] <= limitEmotion[i].downLimitEmotionValue)
            {
                Debug.Log("상하한선을 침범하였습니다");
                return limitEmotion[i].downLimitEmotion;
            }
        }

        // 상하한선 모두 침범하지 않는 경우
        Debug.Log("상하한선을 침범하지 않았습니다");
        return -1;
    }

    public void RenewakSat(int guestNum)     // 만족도를 갱신하는 함수. -> 구름 제공 순서 4번에서 진행
    {
        int temp = 0;

        for (int i = 0; i < 5; i++)
        {
            // 만족도 범위 내에 들어가는지 확인
            if (mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mSatEmotions[i].emotionNum] <= mGuestInfo[guestNum].mSatEmotions[i].up &&
             mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mSatEmotions[i].emotionNum] >= mGuestInfo[guestNum].mSatEmotions[i].down)
            {
                temp++;
            }
        }
        mGuestInfo[guestNum].mSatatisfaction = temp;
        Debug.Log(temp);
    }

    // TODO : 함수 개편
    // 뭉티 리스트를 새로 생성하는 함수
    public int[] NewChoiceGuest()
    {
        int[] guestList = new int[6];                   // 반환시킬 뭉티의 리스트
        int possibleToTake = 6;                         // 받을 수 있는 총 뭉티의 수

        int totalGuestNum = 20;                         // 총 뭉티의 수
        int possibleGuestNum = 0;                       // 방문이 가능한 뭉티의 수

        List<int> VisitedGuestNum = new List<int>();    // 방문 이력이 있는 뭉티의 리스트
        List<int> NotVisitedGuestNum = new List<int>(); // 방문 이력이 없는 뭉티의 리스트

        // 방문 횟수가 끝난 뭉티와 만족도가 5가 된 뭉티는 제외되어야 하므로 먼저 리스트에서 빼낸다.
        for (int i = 0; i < totalGuestNum; i++)
        {
            if (mGuestInfo[i].mVisitCount != 10 && mGuestInfo[i].isCure == false)
            {
                if (mGuestInfo[i].mVisitCount == 0)
                {
                    NotVisitedGuestNum.Add(i);
                }
                else
                {
                    VisitedGuestNum.Add(i);
                }
            }
            if (mGuestInfo[i].isDisSat == false && mGuestInfo[i].mNotVisitCount == 0 && mGuestInfo[i].mVisitCount != 10 && mGuestInfo[i].isCure == false)
            {
                possibleGuestNum++;
            }
        }

        int GuestIndex = 0;
        bool isOverLap = true;

        // 방문 이력이 있는 뭉티가 5명 이상이 없는 경우
        // 모든 방문 이력이 있는 뭉티를 뽑고 나머지를 방문 이력이 없는 뭉티로 채운다.
        if (VisitedGuestNum.Count < possibleToTake - 1)
        {
            Debug.Log("방문 이력이 있는 뭉티가 5명이상이 되지 않습니다");
            for (int i = 0; i < VisitedGuestNum.Count; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // 난수 생성
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // 이미 값이 들어있어 중복되는 경우
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                    }
                    int rejectCount = 0;
                    // 불만 뭉티이거나 방문 불가 상태 뭉티의 수를 구해야 한다.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfo[guestList[j]].isDisSat == true || mGuestInfo[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    //Debug.Log("reject Count : " + rejectCount);
                    if ((mGuestInfo[VisitedGuestNum[temp]].isDisSat == true || mGuestInfo[VisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake - 2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }

                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = VisitedGuestNum[temp];
                GuestIndex++;
                isOverLap = true;

            }
            for (int i = 0; i < possibleToTake - VisitedGuestNum.Count; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // 난수 생성
                    temp = Random.Range(0, NotVisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // 이미 값이 들어있어 중복되는 경우
                        if (NotVisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                    }
                    int rejectCount = 0;
                    // 불만 뭉티이거나 방문 불가 상태 뭉티의 수를 구해야 한다.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfo[guestList[j]].isDisSat == true || mGuestInfo[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfo[NotVisitedGuestNum[temp]].isDisSat == true || mGuestInfo[NotVisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake - 2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = NotVisitedGuestNum[temp];
                GuestIndex++;
                isOverLap = true;

            }
        }
        // 방문 이력이 없는 뭉티가 없는 경우
        // 모든 뭉티를 방문 이력이 있는 뭉티중에서 뽑는다.
        else if (NotVisitedGuestNum.Count == 0)
        {
            Debug.Log("방문 이력이 없는 뭉티가 없습니다");
            for (int i = 0; i < possibleToTake; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // 난수 생성
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // 이미 값이 들어있어 중복되는 경우
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                            //Debug.Log("값 중복.");
                        }
                    }
                    int rejectCount = 0;
                    // 불만 뭉티이거나 방문 불가 상태 뭉티의 수를 구해야 한다.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfo[guestList[j]].isDisSat == true || mGuestInfo[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfo[VisitedGuestNum[temp]].isDisSat == true || mGuestInfo[VisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake - 2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }

                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = VisitedGuestNum[temp];

                GuestIndex++;
                isOverLap = true;

            }
        }
        // 그 외의 경우에는 방문 이력이 있는 뭉티 5명, 방문 이력이 없는 뭉티 1명은 뽑는다.
        else
        {
            Debug.Log("방문이력 뭉티 5명, 방문 이력이 없는 뭉티 1명을 뽑습니다.");
            for (int i = 0; i < possibleToTake - 1; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // 난수 생성
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // 이미 값이 들어있어 중복되는 경우
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                    }
                    int rejectCount = 0;
                    // 불만 뭉티이거나 방문 불가 상태 뭉티의 수를 구해야 한다.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfo[guestList[j]].isDisSat == true || mGuestInfo[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfo[VisitedGuestNum[temp]].isDisSat == true || mGuestInfo[VisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake - 2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }

                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = VisitedGuestNum[temp];
                GuestIndex++;
                isOverLap = true;

            }
            for (int i = 0; i < 1; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // 난수 생성
                    temp = Random.Range(0, NotVisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // 이미 값이 들어있어 중복되는 경우
                        if (NotVisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                    }
                    int rejectCount = 0;
                    // 불만 뭉티이거나 방문 불가 상태 뭉티의 수를 구해야 한다.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfo[guestList[j]].isDisSat == true || mGuestInfo[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfo[NotVisitedGuestNum[temp]].isDisSat == true || mGuestInfo[NotVisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake - 2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = NotVisitedGuestNum[temp];

                GuestIndex++;
                isOverLap = true;
            }
        }
        // 불만 뭉티라면 빼버린다.
        int[] tempList = new int[6];
        int a = 0;
        for (int i = 0; i < possibleToTake; i++)
        {
            tempList[i] = -1;
        }
        for (int i = 0; i < possibleToTake; i++)
        {
            if (mGuestInfo[guestList[i]].isDisSat == false && mGuestInfo[guestList[i]].mNotVisitCount == 0)
            {
                tempList[a] = guestList[i];
                a++;
                mGuestMax++;
            }
        }
        Debug.Log(mGuestMax);
        guestList = tempList;

        return guestList;
    }

    // 해당 뭉티를 초기화 시켜주는 함수
    public void InitGuestData(int guestNum) // 추후에 개발
    {
        // 스크립터블 오브젝트로 만들어 놓은 초기 데이터값을 받아와서 초기화를 시킨다.

        GuestInfos temp         = new GuestInfos();
        temp.mName              = mGuestInitInfos[guestNum].mName;
        temp.mSeed              = mGuestInitInfos[guestNum].mSeed;
        temp.mEmotion           = mGuestInitInfos[guestNum].mEmotion;
        temp.mAge               = mGuestInitInfos[guestNum].mAge;
        temp.mJob               = mGuestInitInfos[guestNum].mJob;
        temp.mSatatisfaction    = mGuestInitInfos[guestNum].mSatatisfaction;
        temp.mSatEmotions       = mGuestInitInfos[guestNum].mSatEmotions;
        temp.mLimitEmotions     = mGuestInitInfos[guestNum].mLimitEmotions;
        temp.isDisSat           = mGuestInitInfos[guestNum].isDisSat;
        temp.isCure             = mGuestInitInfos[guestNum].isCure;
        temp.mVisitCount        = mGuestInitInfos[guestNum].mVisitCount;
        temp.mNotVisitCount     = mGuestInitInfos[guestNum].mNotVisitCount;
        temp.isChosen           = mGuestInitInfos[guestNum].isChosen;
        temp.mUsedCloud         = mGuestInitInfos[guestNum].mUsedCloud;
        temp.mSitChairIndex     = mGuestInitInfos[guestNum].mSitChairIndex;
        temp.isUsing            = mGuestInitInfos[guestNum].isUsing;

        mGuestInfo[guestNum]    = temp;

        Debug.Log(mGuestInitInfos[guestNum].mName);
    }

    // TODO : 함수 개편
    // 방문주기를 초기화 해주는 함수
    public void InitGuestTime()
    {
        mGuestTime = 0.0f;
        isTimeToTakeGuest = false;
        Debug.Log("방문주기 초기화");
    }

    // 하루가 지나면서 초기화가 필요한 정보들을 변환해준다.
    public void InitDay()
    {
        // 날씨의 공간에 아직 남아있는 뭉티들을 불만 뭉티로 만든다.


        // 새로운 방문 뭉티 리스트를 뽑는다.
        mGuestIndex = 0;
        mGuestCount = -1;
        mGuestMax = 0;

        // 새로운 리스트를 뽑는 함수를 호출 (테스트를 위해서 잠시 주석처리)
        int[] list = { 0, 1, 0, 1, 0, 1 };
        mGuestMax = NUM_OF_TODAY_GUEST_LIST;
        mTodayGuestList = list;

        //mTodayGuestList = NewChoiceGuest();

        // 방문 주기를 초기화한다.
        InitGuestTime();

        // 채집물들이 다시 갱신된다.
    }

    // TODO : 함수 개편
    public int SpeakEmotionEffect(int guestNum)
    {
        int result = -1;            // 상하한선에 가장 근접한 감정 번호
        int temp = -1;              // result의 상하한선과의 차이값

        // 상한선 값보다 높거나 하한선보다 낮다면 불만 뭉티이므로 표현할 일이 없기 때문에 고려하지 않는다.
        // 첫번째 상하한선 값 중에서 더 상하한선에 근접한 값을 초기 결과값으로 놓는다. 
        if (mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[0].downLimitEmotion]
            - mGuestInfo[guestNum].mLimitEmotions[0].downLimitEmotionValue >
            mGuestInfo[guestNum].mLimitEmotions[0].upLimitEmotionValue
            - mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[0].upLimitEmotion])
        {
            result = mGuestInfo[guestNum].mLimitEmotions[0].upLimitEmotion;
            temp = mGuestInfo[guestNum].mLimitEmotions[0].upLimitEmotionValue
            - mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[0].upLimitEmotion];
        }
        else
        {
            result = mGuestInfo[guestNum].mLimitEmotions[0].downLimitEmotion;
            temp = mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[0].downLimitEmotion]
            - mGuestInfo[guestNum].mLimitEmotions[0].downLimitEmotionValue;
        }
        if (temp > mGuestInfo[guestNum].mLimitEmotions[1].upLimitEmotionValue
             - mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[1].upLimitEmotion])
        {
            result = mGuestInfo[guestNum].mLimitEmotions[1].upLimitEmotion;
            temp = mGuestInfo[guestNum].mLimitEmotions[1].upLimitEmotionValue
             - mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[1].upLimitEmotion];
        }
        if (temp > mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[1].downLimitEmotion]
            - mGuestInfo[guestNum].mLimitEmotions[1].downLimitEmotionValue)
        {
            result = mGuestInfo[guestNum].mLimitEmotions[1].downLimitEmotion;
            temp = mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[1].downLimitEmotion]
            - mGuestInfo[guestNum].mLimitEmotions[1].downLimitEmotionValue;
        }

        return result;
    }

    public int SpeakEmotionDialog(int guestNum)
    {
        int result = -1;         // 반환할 감정 번호
        int temp = -1;           // 임시로 저장할 만족도 범위와의 차이값
        int maxValue = -1;       // 차이값 중에서 가장 큰 값을 저장하는 것

        for (int i = 0; i < 5; i++)
        {
            // 만족도 범위보다 현재 값이 높다면
            if (mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mSatEmotions[i].emotionNum]
                > mGuestInfo[guestNum].mSatEmotions[i].up)
            {
                temp = mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mSatEmotions[i].emotionNum]
                    - mGuestInfo[guestNum].mSatEmotions[i].up;
            }
            // 만족도 범위보다 현재 값이 낮다면
            else if (mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mSatEmotions[i].emotionNum]
                < mGuestInfo[guestNum].mSatEmotions[i].down)
            {
                temp = mGuestInfo[guestNum].mSatEmotions[i].down
                    - mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mSatEmotions[i].emotionNum];
            }
            // 이외의 경우는 만족범위안에 있는 것이므로 무시한다.
            // temp값이 기존 저장된 값보다 만족도 범위와 멀다면 갱신한다.
            if (maxValue < temp)
            {
                maxValue = temp;
                result = mGuestInfo[guestNum].mSatEmotions[i].emotionNum;
            }
        }
        return result;
    }
}
