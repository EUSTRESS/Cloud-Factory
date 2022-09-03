using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guest : MonoBehaviour
{
    public GuestInfo[]      mGuestInfos;                        // Scriptable Objects들의 정보를 담고 있는 배열

    public float            mGuestTime;                         // 뭉티의 방문 주기

    public int              mGuestIndex;                        // 이번에 방문할 뭉티의 번호

    [SerializeField]
    public int[]           mTodayGuestList = new int[6];       // 오늘 방문 예정인 뭉티 목록
    [SerializeField]
    public bool             isGuestInLivingRoom;                // 응접실에 손님이 방문해있는가?

    public bool             isTimeToTakeGuest;                  // 뭉티 방문주기가 지났는지 확인
    [SerializeField]
    private int              mGuestCount;                       // 이번에 방문할 뭉티의 순서
    [SerializeField]
    private int              mGuestMax;                         // 오늘 방문하는 뭉티의 최대 숫자

    private static Guest    instance = null;                    // 싱글톤 기법을 위함 instance 생성

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
        if (mGuestTime < 5.0f)
        {
            mGuestTime += Time.deltaTime;
        }
        else if(mGuestTime >= 5.0f && isTimeToTakeGuest == false)
        {
            // 모든 인덱스가 다 되지 않는 한 뭉티 방문주기가 다된경우 새로운 뭉티를 들여보낸다.
            if (mGuestCount < mGuestMax - 1) // 0 1 2 3 4 5 
            {
                Debug.Log("뭉티 방문시간이 되었습니다");
                isTimeToTakeGuest = true;
                TakeGuest();
                // 응접실 이동하는 버튼들에 대한 상호작용
            }
            else
            {
                Debug.Log("모든 뭉티가 방문하였습니다.");
            }
        }

        // 구름을 수령 받고 이용을 마친 경우에만 해당 뭉티의 감정의 변화값을 부여함


        // 싱글톤 기법 확인을 위한 테스트코드
        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeGuest();
        }
        // 감정값 변환을 위한 함수 테스트 (성공)
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetEmotion(0, 0, 1, 5, 10);
        }
        // 만족도 갱신을 위한 함수 테스트 (성공)
        if (Input.GetKeyDown(KeyCode.C))
        {
            RenewakSat(0);
        }
        // 상하한선 침범 확인을 위한 함수 테스트 (성공)
        if (Input.GetKeyDown(KeyCode.D))
        {
            mGuestInfos[0].isDisSat = CheckIsDisSat(0);
            Debug.Log(mGuestInfos[0].isDisSat);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            InitGuestTime();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            NewChoiceGuest();
            for (int i = 0; i < 6; i++)
            {
                Debug.Log(mTodayGuestList[i] + "번 뭉티가 추가되었습니다.");
            }
        }
    }

    public void TakeGuest()
    {
        if (isTimeToTakeGuest == true && isGuestInLivingRoom == false)
        {
            mGuestCount++;
            Debug.Log("mGuestCount가 증가합니다.");
            mGuestIndex = mTodayGuestList[mGuestCount];
            isGuestInLivingRoom = true;
        }
    }

    // 뭉티의 정보값들을 받아오는 API
    public string GetName(int gusetNum) 
    { 
        return mGuestInfos[gusetNum].mName; 
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    // 구름 제공 순서 (구름이 화면상에서 뭉티까지 도착하여지는 작업은 해당 순서에서 생략)
    // 1. 구름을 선택하여 뭉티에게 제공 (뭉티가 앉아있는 상태가 아니라면 제공 불가능) 
    // 2. 구름의 이용시간만큼을 대기 (대기도중 날이 바뀌면 제공 실패)
    // 3. 구름의 감정값만큼을 뭉티의 감정에 더하기 - 함수 생성
    // 4. 구름을 제공받은 뭉티의 감정선들을 확인. (만족도와 감정 상하한선 침범 여부) - 함수 생성
    // 5. 만약 감정 상하한선을 침범했을 경우 뭉티를 불만 뭉티로 설정 (불만 뭉티에 대한 관리는 스크립트 추가 작성) - 함수 생성
    // 6. 만약 만족도가 변경되었을 시에 만족도 값 갱신 (해당 뭉티의 대표감정 갱신) - 함수 생성

    // 7. 만족도가 올라갔을 경우 마당에 뿌릴 수 있는 씨앗(재료)에 관련된 값을 받아서 심기
    // 8. 구름 제공에 관한 결과를 화면에 띄워주고 뭉티를 날씨의 공간에서 내보내기
    //------------------------------------------------------------------------------------------------------------------------------------------
    
    public bool CheckIsDisSat(int guestNum)
    {
        int temp = IsExcessLine(guestNum);                      // 침범하는 경우에 감정값을 임의로 저장할 변수

        // 상하한 선을 침범한 경우를 확인
        if (temp != -1) 
        {
            mGuestInfos[guestNum].isDisSat = true;              // 불만 뭉티로 변환
            mGuestInfos[guestNum].mSatatisfaction = 0;          // 만족도 0 으로 갱신
            mGuestInfos[guestNum].mVisitCount = 0;              // 남은 방문횟수 0으로 갱신
            
            // 치유의 기록으로 불만 뭉티가 된 상태와 손님 번호, 어떤 감정 변화로 인한 것인지 전달해주기


            return true;
        }
        return false;
    }

    // 뭉티의 정보값 변경에 필요한 API 
    // Event Handler를 이용하여 만족도 범위안에 들지 못하거나 감정 상하한선을 침범하여 불만 뭉티가 되는경우 이벤트를 발동시켜 관리
    public void SetEmotion(int guestNum, int emotionNum0, int emotionNum1, int value0, int value1) 
    { 
        mGuestInfos[guestNum].mEmotion[emotionNum0] += value0; 
        mGuestInfos[guestNum].mEmotion[emotionNum1] += value1; 
    }

    public int IsExcessLine(int guestNum) // 감정 상하한선을 침범했는지 확인하는 함수. -> 구름 제공 순서 4번에서 진행
    {

        SLimitEmotion[] limitEmotion = mGuestInfos[guestNum].mLimitEmotions;

        for (int i = 0; i < 2; i++)
        {
            if (mGuestInfos[guestNum].mEmotion[limitEmotion[i].upLimitEmotion] >= limitEmotion[i].upLimitEmotionValue) // 상하한선을 침범한 경우
            {
                Debug.Log("상하한선을 침범하였습니다");
                return limitEmotion[i].upLimitEmotion;
            }
            else if (mGuestInfos[guestNum].mEmotion[limitEmotion[i].downLimitEmotion] <= limitEmotion[i].downLimitEmotionValue)
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

        for(int i = 0; i< 5; i++)
        {
            // 만족도 범위 내에 들어가는지 확인
            if(mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum] <= mGuestInfos[guestNum].mSatEmotions[i].up &&
             mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum] >= mGuestInfos[guestNum].mSatEmotions[i].down)
            {
                temp++;
            }
        }
        mGuestInfos[guestNum].mSatatisfaction = temp;
        Debug.Log(temp);
    }

    // 구름 제공으로 인해 뭉티의 감정이 변하는 경우 일정한 조건이 필요하다.
    // 1. 구름을 제공받아 이용하는 동안 날이 바뀌어 집으로 돌아가야 하는 경우 무효
    // -> 일정 시간 (구름을 제공받아 이용하는 시간)동안 대기하다가 시간이 지난 후 감정값을 변경하는 방법 사용 예정)
    //
    // 2. 구름을 제공받아 변화된 감정이 뭉티의 감정 상한선과 하한선을 침범하면 안된다.
    // -> 구름을 제공받고 나서 감정 상한선과 하한선이 침범당하는 경우 불만 뭉티로 변환된다.

    // 불만 뭉티의 패널티
    // Cloud Factory 방문에 제한
    // 치유의 기록에 불만뭉티 표시
    // 만족도 0으로 변환

    public int CheckDisSat(int[] guestList, int Index)
    {
        int result = 0;

        for(int i = 0; i<= Index; i++)
        {
            if (mGuestInfos[i].isDisSat == true || mGuestInfos[i].mNotVisitCount != 0)
            {
                result++;
            }
        }

        return result;
    }
    // 뭉티 리스트를 새로 생성하는 함수
    public int[] NewChoiceGuest()
    {
        int[]       guestList           = new int[6];       // 반환시킬 뭉티의 리스트
        int         possibleToTake      = 6;                // 받을 수 있는 총 뭉티의 수

        int         totalGuestNum       = 20;               // 총 뭉티의 수
        int         possibleGuestNum    = 0;                // 방문이 가능한 뭉티의 수

        List<int>   VisitedGuestNum     = new List<int>();  // 방문 이력이 있는 뭉티의 리스트
        List<int>   NotVisitedGuestNum  = new List<int>();  // 방문 이력이 없는 뭉티의 리스트

        // 방문 횟수가 끝난 뭉티와 만족도가 5가 된 뭉티는 제외되어야 하므로 먼저 리스트에서 빼낸다.
        for (int i = 0; i < totalGuestNum; i++)
        {
            if(mGuestInfos[i].mVisitCount != 10 && mGuestInfos[i].isCure == false)
            {
                if (mGuestInfos[i].mVisitCount == 0)
                {
                    NotVisitedGuestNum.Add(i);
                }
                else
                {
                    VisitedGuestNum.Add(i);
                }
            }
            if(mGuestInfos[i].isDisSat == false && mGuestInfos[i].mNotVisitCount == 0 && mGuestInfos[i].mVisitCount != 10 && mGuestInfos[i].isCure == false)
            {
                possibleGuestNum++;
            }
        }

        int     GuestIndex = 0;
        bool    isOverLap = true;

        // 방문 이력이 있는 뭉티가 5명 이상이 없는 경우
        // 모든 방문 이력이 있는 뭉티를 뽑고 나머지를 방문 이력이 없는 뭉티로 채운다.
        if (VisitedGuestNum.Count < possibleToTake-1)
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
                        if (mGuestInfos[guestList[j]].isDisSat == true || mGuestInfos[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    //Debug.Log("reject Count : " + rejectCount);
                    if ((mGuestInfos[VisitedGuestNum[temp]].isDisSat == true || mGuestInfos[VisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake-2)
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
                        if (mGuestInfos[guestList[j]].isDisSat == true || mGuestInfos[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfos[NotVisitedGuestNum[temp]].isDisSat == true || mGuestInfos[NotVisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake-2)
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
                        if (mGuestInfos[guestList[j]].isDisSat == true || mGuestInfos[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfos[VisitedGuestNum[temp]].isDisSat == true || mGuestInfos[VisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake-2)
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
            for (int i = 0; i < possibleToTake-1; i++)
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
                        if (mGuestInfos[guestList[j]].isDisSat == true || mGuestInfos[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfos[VisitedGuestNum[temp]].isDisSat == true || mGuestInfos[VisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake-2)
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
                        if (mGuestInfos[guestList[j]].isDisSat == true || mGuestInfos[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfos[NotVisitedGuestNum[temp]].isDisSat == true || mGuestInfos[NotVisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake-2)
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
        for(int i=0; i< possibleToTake; i++)
        {
            tempList[i] = -1;
        }
        for(int i = 0; i< possibleToTake; i++)
        {
            if(mGuestInfos[guestList[i]].isDisSat == false && mGuestInfos[guestList[i]].mNotVisitCount == 0)
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
    public void InitGuestData() // 추후에 개발
    {
        // 스크립터블 오브젝트를 하나 더 만들어서 그 값을 받아오는 식으로 설계 예정

    }

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
        mTodayGuestList = NewChoiceGuest();

        // 방문 주기를 초기화한다.
        InitGuestTime();

        // 채집물들이 다시 갱신된다.
    }

    public int SpeakEmotionEffect(int guestNum)
    {
        int result = -1;            // 상하한선에 가장 근접한 감정 번호
        int temp = -1;              // result의 상하한선과의 차이값

        // 상한선 값보다 높거나 하한선보다 낮다면 불만 뭉티이므로 표현할 일이 없기 때문에 고려하지 않는다.

        // 첫번째 상하한선 값 중에서 더 상하한선에 근접한 값을 초기 결과값으로 놓는다. 
        if(mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[0].downLimitEmotion]
            - mGuestInfos[guestNum].mLimitEmotions[0].downLimitEmotionValue  >
            mGuestInfos[guestNum].mLimitEmotions[0].upLimitEmotionValue
            - mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[0].upLimitEmotion])
        {
            result = mGuestInfos[guestNum].mLimitEmotions[0].upLimitEmotion;
            temp = mGuestInfos[guestNum].mLimitEmotions[0].upLimitEmotionValue
            - mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[0].upLimitEmotion];
        }
        else
        {
            result = mGuestInfos[guestNum].mLimitEmotions[0].downLimitEmotion;
            temp = mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[0].downLimitEmotion]
            - mGuestInfos[guestNum].mLimitEmotions[0].downLimitEmotionValue;
        }
        if (temp > mGuestInfos[guestNum].mLimitEmotions[1].upLimitEmotionValue
             - mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[1].upLimitEmotion])
        {
            result = mGuestInfos[guestNum].mLimitEmotions[1].upLimitEmotion;
            temp = mGuestInfos[guestNum].mLimitEmotions[1].upLimitEmotionValue
             - mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[1].upLimitEmotion];
        }
        if (temp > mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[1].downLimitEmotion]
            - mGuestInfos[guestNum].mLimitEmotions[1].downLimitEmotionValue)
        {
            result = mGuestInfos[guestNum].mLimitEmotions[1].downLimitEmotion;
            temp = mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[1].downLimitEmotion]
            - mGuestInfos[guestNum].mLimitEmotions[1].downLimitEmotionValue;
        }

        return result;
    }

    public int SpeakEmotionDialog(int guestNum)
    {
        int result = -1;         // 반환할 감정 번호
        int temp = -1;           // 임시로 저장할 만족도 범위와의 차이값
        int maxValue = -1;       // 차이값 중에서 가장 큰 값을 저장하는 것

        for(int i = 0; i<5; i++)
        {
            // 만족도 범위보다 현재 값이 높다면
            if(mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum] 
                > mGuestInfos[guestNum].mSatEmotions[i].up)
            {
                temp = mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum] 
                    - mGuestInfos[guestNum].mSatEmotions[i].up;
            }
            // 만족도 범위보다 현재 값이 낮다면
            else if(mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum]
                < mGuestInfos[guestNum].mSatEmotions[i].down)
            {
                temp = mGuestInfos[guestNum].mSatEmotions[i].down
                    - mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum];
            }
            // 이외의 경우는 만족범위안에 있는 것이므로 무시한다.

            // temp값이 기존 저장된 값보다 만족도 범위와 멀다면 갱신한다.
            if(maxValue < temp)
            {
                maxValue = temp;
                result = mGuestInfos[guestNum].mSatEmotions[i].emotionNum;
            }            
        }
        return result;
    }
}
