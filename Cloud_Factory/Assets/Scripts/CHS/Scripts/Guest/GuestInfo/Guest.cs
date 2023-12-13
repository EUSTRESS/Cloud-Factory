using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// GuestInfo중에서 저장해야하는 정보값만을 담은 클래스\
[System.Serializable]
public class GuestInfoSaveData
{
    public int[] mEmotion = new int[20];
    public int mSatatisfaction;
    public int mSatVariation;
    public bool isDisSat;
    public bool isCure;
    public int mVisitCount;
    public int mNotVisitCount;
    public bool isChosen;
    public int mSitChairIndex;
    public bool isUsing;
}

// GuestManager에서 저장해야 하는 정보값들을 담은 클래스
[System.Serializable]
public class GuestManagerSaveData
{
    // 상수 선언
    private const int NUM_OF_GUEST = 20;
    private const int NUM_OF_TODAY_GUEST_LIST = 6;

    public GuestInfoSaveData[] GuestInfos = new GuestInfoSaveData[NUM_OF_GUEST];

    public bool isGuestLivingRoom;
    public bool isTimeToTakeGuest;
    public int mGuestIndex;
    public int[] mTodayGuestList = new int[NUM_OF_TODAY_GUEST_LIST];
    public int mGuestCount;
    public float mGuestTime;
}

// GuestObject 내부에서 저장되어야 할 정보값들을 담은 클래스
[System.Serializable]
public class GuestObjectSaveData
{
    // Transform
    public float xPos;
    public float yPos;
    public float xScale;

    // GuestObject.cs
    public int   mGuestNum;
    public float mLimitTime;

    public int mTargetChairIndex;
    public bool isSit;
    public bool isUsing;
    public bool isMove;
    public bool isGotoEntrance;
    public bool isEndUsingCloud;

    // WayPoint.cs
    public int WayNum;
}

[System.Serializable]
public class SOWSaveData
{
    // 손님 오브젝트 정보 (대기/착석 상태 따로 저장)
    public List<GuestObjectSaveData> UsingObjectsData = new List<GuestObjectSaveData>();
    public List<GuestObjectSaveData> WaitObjectsData = new List<GuestObjectSaveData>();

    public int mMaxChairNum;
    public Dictionary<int, bool> mCheckChairEmpty = new Dictionary<int, bool>();
}


public class Guest : MonoBehaviour
{
    // 상수 선언
    public const int NUM_OF_GUEST = 20;                                 // 손님의 총 인원 수
    private const int NUM_OF_TODAY_GUEST_LIST = 6;                      // 하루에 방문하는 손님의 총 인원 수

    [Header ("[손님 정보값 리스트]")]
    public GuestInfos[] mGuestInfo;                                     // 손님들의 인게임 정보값
    [SerializeField]
    private GuestInfo[] mGuestInitInfos;                                // Scriptable Objects들의 정보를 담고 있는 배열

    [Header ("[손님 방문 관련 정보]")]
    public bool isGuestInLivingRoom;                                    // 응접실에 손님이 방문해있는가?
    public bool isTimeToTakeGuest;                                      // 뭉티 방문주기가 지났는지 확인

    [Space(10f)]
    public int mGuestIndex;                                             // 이번에 방문할 뭉티의 번호
    public int[] mTodayGuestList = new int[NUM_OF_TODAY_GUEST_LIST];    // 오늘 방문 예정인 뭉티 목록
    public int mGuestCount;                                             // 방문한 뭉티의 숫자
    
    private Queue<int> mGuestQueue;                                  // 뭉티 리스트

    [Space (10f)]
    public float mGuestTime;                                            // 뭉티의 방문 주기의 현재 값
    public float mMaxGuestTime;                                         // 뭉티의 방문 주기


    [SerializeField]
    private int mGuestMax;                                              // 오늘 방문하는 뭉티의 최대 숫자

    // SOWManger와 연동하여 값을 저장
    public SOWSaveData SaveSOWdatas;                                 // 이어하기를 위한 데이터들을 저장해놓는 리스트
    public bool isLoad = false;

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
                // 기본적으로 손님들의 데이터를 기본 데이터로 초기화한다.
                // 이어하기를 하게되는 경우 손님들의 각각 정보들을 저장해놓은 손님정보 리스트에서 받아와 갱신한다.
                InitGuestData(i);
            }

			// Demo Version
			InitGuestQueue(1);

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
        // 뭉티의 방문주기를 돌린다. (게임을 시작하기 전(로비)에서는 시간이 흐르지 않는다.
        if (mGuestTime < mMaxGuestTime && SceneManager.GetActiveScene().name != "Lobby")
        {
            mGuestTime += Time.deltaTime;
        }
        else if (mGuestTime >= mMaxGuestTime && isTimeToTakeGuest == false)
        {
            // 모든 인덱스가 다 되지 않는 한 뭉티 방문주기가 다된경우 새로운 뭉티를 들여보낸다.
            if (mGuestCount < mGuestMax - 1) 
            {
				TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();

                if (mTutorialManager.isTutorial == true
                    && mGuestCount >= 0)
                { }
                else
                {
                    isTimeToTakeGuest = true;
                    TakeGuest();
                }
                // 응접실 이동하는 버튼들에 대한 상호작용
            }
            else
            {

            }
        }

        // 이어하기인 경우 Load해서 받아온 데이터를 SOWManager로 넘겨준다.
        if (isLoad && SceneManager.GetActiveScene().name == "Space Of Weather")
        {
            SOWManager sowManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

            if (sowManager != null)
            {
                isLoad = false;

                // 의자가 비어있는지에 대한 정보를 넘겨준다.
                sowManager.mCheckChairEmpty = SaveSOWdatas.mCheckChairEmpty;

#if UNITY_EDITOR
                Debug.Log(sowManager.mCheckChairEmpty);
#endif

                sowManager.mMaxChairNum = 3;

                // 대기상태 오브젝트에 대하여 넘겨준다.
                foreach (GuestObjectSaveData data in SaveSOWdatas.WaitObjectsData)
                {
                    sowManager.mWaitGuestList.Enqueue(data.mGuestNum);
                    sowManager.mWaitGuestObjectQueue.Enqueue(SetLoadGuest(data, sowManager));
                }

                // 착석상태 오브젝트에 대하여 넘겨준다.
                foreach (GuestObjectSaveData data in SaveSOWdatas.UsingObjectsData)
                {
                    sowManager.mUsingGuestList.Add(data.mGuestNum);
                    sowManager.mUsingGuestObjectList.Add(SetLoadGuest(data, sowManager));
                }

            }
        }
    }

    public GameObject SetLoadGuest(GuestObjectSaveData data, SOWManager sow)
    {
        GameObject tempObject;

        // Instance 생성
        tempObject = Instantiate(sow.mGuestObject);

        // etc.
        tempObject.GetComponent<RLHReader>().LoadHintInfo(data.mGuestNum);
        tempObject.GetComponent<WayPoint>().WayPos = sow.mWayPoint;
        tempObject.GetComponent<WayPoint>().WayNum = data.WayNum;

        // GuestObject.cs
        GuestObject Info = tempObject.GetComponent<GuestObject>();
        Info.setGuestNum(data.mGuestNum);
        Info.initAnimator();
        Info.init();

        Info.mLimitTime = data.mLimitTime;
        Info.mTargetChiarIndex = data.mTargetChairIndex;
        Info.isSit = data.isSit;
        Info.isUsing = data.isUsing;
        Info.isMove = data.isMove;
        Info.isGotoEntrance = data.isGotoEntrance;
        Info.isEndUsingCloud = data.isEndUsingCloud;

        // transform
        tempObject.transform.position = new Vector3(data.xPos,data.yPos,0.0f);
        tempObject.transform.localScale = new Vector3(data.xScale, 1.0f, 1.0f);

        if(Info.mTargetChiarIndex != -1 && Info.isSit == true)
        {
            Info.mGuestAnim.SetTrigger("AlreadySit");
#if UNITY_EDITOR
            Debug.Log("AlreadySit");
#endif
        }

        return tempObject;
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
#if UNITY_EDITOR
                Debug.Log("상하한선을 침범하였습니다");
#endif
                return limitEmotion[i].upLimitEmotion;
            }
            else if (mGuestInfo[guestNum].mEmotion[limitEmotion[i].downLimitEmotion] <= limitEmotion[i].downLimitEmotionValue)
            {
#if UNITY_EDITOR
                Debug.Log("상하한선을 침범하였습니다");
#endif
                return limitEmotion[i].downLimitEmotion;
            }
        }

        // 상하한선 모두 침범하지 않는 경우
#if UNITY_EDITOR
        Debug.Log("상하한선을 침범하지 않았습니다");
#endif
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

#if UNITY_EDITOR
        Debug.Log("기존 만족도 : " + mGuestInfo[guestNum].mSatatisfaction + "현재 만족도 : " + temp);
#endif
        mGuestInfo[guestNum].mSatatisfaction = temp;

        if(mGuestInfo[guestNum].mSatatisfaction >= 5)
        {
            mGuestInfo[guestNum].isCure = true;
        }
    }


    // 뭉티 리스트를 새로 생성하는 함수
    public int[] NewChoiceGuest()
    {
        // Demo Version
        int temp = -1;
        List<int> guestList = new List<int>();
        for(int i = mGuestQueue.Count; i > 0; i--)
        {
            temp = mGuestQueue.Dequeue();
            if (mGuestInfo[temp].isDisSat == false && 
                mGuestInfo[temp].isCure == false &&
                mGuestInfo[temp].mVisitCount <= 10)
            { guestList.Add(temp); mGuestQueue.Enqueue(temp); }
        }

        int idx = 0;
        int[] GuestArray = new int[guestList.Count];
        int guestListCount = guestList.Count;
        for (int i = 0; i < guestListCount; i++)
        {
            int num = guestList[Random.Range(0, guestList.Count)];
            GuestArray[idx++] = num;
            guestList.Remove(num);
        }
        
        TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        if (mTutorialManager.isTutorial == true)
        {
            for (int i = 0; i < guestListCount; i++)
            {
                if (GuestArray[i] == 0)
                {
                    int tempGuestNum = GuestArray[0];
                    GuestArray[0] = GuestArray[i];
                    GuestArray[i] = tempGuestNum;
                    break;
                }
            }
        }
        
        return GuestArray;
        /*
        List<int> guestList = new List<int>();          // 저장할 뭉티의 리스트
        int[] returnValueList = new int[6];             // 반환할 뭉티의 리스트, size는 초기화만
        int possibleToTake = 6;                         // 받을 수 있는 총 뭉티의 수

        int totalGuestNum = 20;                         // 총 뭉티의 수
        int possibleGuestNum = 0;                       // 방문이 가능한 뭉티의 수

        List<int> VisitedGuestNum = new List<int>();    // 방문 이력이 있는 뭉티의 리스트
        List<int> NotVisitedGuestNum = new List<int>(); // 방문 이력이 없는 뭉티의 리스트

        int loopCount = 0;                              // 무한 루프가 몇번 돌아갔는지 체크하는 변수

        // 방문 횟수가 끝난 뭉티와 만족도가 5가 된 뭉티는 제외되어야 하므로 먼저 리스트에서 빼낸다.
        for (int i = 0; i < totalGuestNum; i++)
        {
            if (mGuestInfo[i].mVisitCount < 10 && mGuestInfo[i].isCure == false)
            {
                if (mGuestInfo[i].mVisitCount == 0)
                {
                    NotVisitedGuestNum.Add(i);
                }
                else
                {
                    VisitedGuestNum.Add(i);
                }
				possibleGuestNum++;             //방문 가능한 뭉티의 수를 총합한다
			}
        }
		if (possibleGuestNum <= possibleToTake) { possibleToTake = possibleGuestNum; }       // 방문 가능한 뭉티의 수가 6마리 이하일 때, 받을 수 있는 뭉티의 수를 변경해준다
        bool isFinishedChoice = false;          //리스트 선정 완료 여부 확인

        while (!isFinishedChoice)                 
        {
            guestList.Clear();                  // 손님 리스트를 재작성할 때마다 비워준다.
            loopCount++;                        // 루프 반복 횟수를 계산한다.

            int currentNum  = -1;       // 랜덤 변수
            int newGuest    = -1;       // 새로 뽑은 손님의 번호 저장

            if(loopCount >= 10)         // 리스트 선정이 10회이상 반복해도 결정이 안되었을 때, 불만/방문 불가 뭉티를 제외하고 리스트를 작성한다.
            {
                possibleGuestNum =  0;         // 방문 가능한 손님의 수를 초기화 해준다.
                possibleToTake =    3;         // 손님의 수를 최대 3명 뽑는다. 

                NotVisitedGuestNum.Clear();    // 리스트를 빈 상태로 만들어준다.
                VisitedGuestNum.Clear();       // //

                for (int i = 0; i < totalGuestNum; i++)
                {
                    if (mGuestInfo[i].mVisitCount < 10 && mGuestInfo[i].isCure == false && mGuestInfo[i].isDisSat == false && mGuestInfo[i].mNotVisitCount <= 0)
                    {
                        if (mGuestInfo[i].mVisitCount == 0)
                        {
                            NotVisitedGuestNum.Add(i);
                        }
                        else
                        {
                            VisitedGuestNum.Add(i);
                        }
                        possibleGuestNum++;             //방문 가능한 뭉티의 수를 총합한다
                    }
                }
				if (possibleGuestNum <= possibleToTake) { possibleToTake = possibleGuestNum; }
			}

            //방문 이력이 없는 뭉티의 자리를 최소 한 자리 비우고, 방문 이력이 있는 뭉티를 최대로 뽑는다
            //방문 이력이 있는 뭉티의 수가 possibleToTake - 1 이하일 때
            if (VisitedGuestNum.Count <= possibleToTake - 1){ guestList = AddToGuestList(guestList, VisitedGuestNum, VisitedGuestNum.Count); }
            //방문 이력이 있는 뭉티의 수가 possibleToTake 이상일 때
            else                                            { guestList = AddToGuestList(guestList, VisitedGuestNum, possibleToTake - 1); }

			//방문 이력이 없는 뭉티가 없을 때, 방문 이력이 있는 뭉티로 남은 자리를 채운다.
			if (NotVisitedGuestNum.Count <= 0)              { guestList = AddToGuestList(guestList, VisitedGuestNum, possibleToTake); }
            //방문 이력이 없는 뭉티가 있을 때, 남은 자리를 모두 방문 이력이 없는 뭉티로 채운다
            else
            {
                if (NotVisitedGuestNum.Count > 0) { currentNum = Random.Range(0, NotVisitedGuestNum.Count); }
                for (int num = 0; num < NotVisitedGuestNum.Count;)    // 방문 이력이 없는 뭉티가 남아있으면
                {
                    if (guestList.Count >= possibleToTake) { break; }        // 자리가 모두 찼을 때 반복문 탈출, 자리가 남아있어도 뭉티가 없는 경우는 위의 for문에서 검거

                    if (guestList.Contains(NotVisitedGuestNum[currentNum])) { currentNum = Random.Range(0, NotVisitedGuestNum.Count); }
                    else
                    {
                        guestList.Add(NotVisitedGuestNum[currentNum]);
                        num++;
                    }
                }
            }

			//리스트에서 불만 뭉티의 수를 저장하는 변수
			int rejectCount = 0;
            foreach (var num in guestList)
            {
                if (mGuestInfo[num].isDisSat == true || mGuestInfo[num].mNotVisitCount > 0) { rejectCount++; }
            }

            //뽑을 수 있는 뭉티가 4마리 이상이고, 다시 뽑게되는 불만/방문 불가 뭉티 수가 possibleToTake - 2 이상이면 다시 뽑기
            if (possibleToTake >= 4 && rejectCount >= possibleToTake - 2) { continue; }
            //guest list 작성 while문 종료 및 불만/방문 불가 뭉티 guestList에서 제외
            else
            {
                isFinishedChoice = true;    // 리스트 작성 종료

                List<int> mixList = new List<int>();    // 리스트 내 손님의 순서를 섞어 저장할 새로운 리스트

                int tempNum = 0;            // 반환하는 array의 index
                int listSize = 0;           // 반환하는 array의 size 

                // 불만/방문 불가 뭉티를 제외한 Guest의 수를 다시 계산
                foreach (var num in guestList)
                {
                    if (mGuestInfo[num].isDisSat == false && mGuestInfo[num].mNotVisitCount <= 0) { listSize++; }
                }

                //guestList에 있는 손님 번호를 방문 경험이 있는지 없는지에 상관없이 섞어준다.
                List<int> tempList = new List<int>();
                tempList = AddToGuestList(tempList, guestList, guestList.Count);

                //섞인 guestList의 정보를 저장한 tempList의 손님 번호 중, 불만 뭉티가 아니고, 방문 불가 상태가 아닌 뭉티만 mixList에 추가해준다.
                foreach(var num in tempList)
                {
					if (mGuestInfo[num].isDisSat == false && mGuestInfo[num].mNotVisitCount <= 0) { mixList.Add(num); }
				}

                // 튜토리얼이 진행 중인 경우, 첫 번째 손님을 0번 손님으로 고정한다.
				TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
				if (mTutorialManager.isTutorial == true)
                {
                    if (mixList.Contains(0))
                    {
                        int pos = 0;
                        foreach(int num in mixList)
                        {
                            if(num == 0) { break; }
                            pos++;
                        }
                        if (pos != 0)
                        {
                            int temp = mixList[pos];
                            mixList[pos] = mixList[0];
                            mixList[0] = temp;
                        }
                    }
                    else
                    {
                        mixList[0] = 0;
                    }
                }

                returnValueList = new int[listSize];
                foreach (var num in mixList)
                {
                    if (mGuestInfo[num].isDisSat == false && mGuestInfo[num].mNotVisitCount <= 0) { returnValueList[tempNum++] = num; }
                }
			}
        }
        return returnValueList;
    }

    private List<int> AddToGuestList(List<int> guest_list, List<int> visit_guest_list, int max_list)    //AddToGuestList(반환 리스트, 방문 가능 손님 리스트, 함수에서 뽑을 최대 손님 수);
    {
        List<int> temp_list = guest_list;
        int currentNum = -1;
		if (visit_guest_list.Count > 0) { currentNum = Random.Range(0, visit_guest_list.Count); }
		for (; temp_list.Count < max_list;)
		{
			if (temp_list.Count > visit_guest_list.Count) { break; } //자리를 모두 채우기 전에 남아있는 방문 이력이 있는 뭉티가 없으면 반복문을 빠져나온다
			if (temp_list.Contains(visit_guest_list[currentNum])) { currentNum = Random.Range(0, visit_guest_list.Count); }
			else
			{
				temp_list.Add(visit_guest_list[currentNum]);
			}
		}
        return temp_list;
        */
	}


    // 해당 뭉티를 초기화 시켜주는 함수
    public void InitGuestData(int guestNum) 
    {
        // 스크립터블 오브젝트로 만들어 놓은 초기 데이터값을 받아와서 초기화를 시킨다.

        GuestInfos temp         = new GuestInfos();
        temp.mName              = mGuestInitInfos[guestNum].mName.Clone() as string;
        temp.mNameEN            = mGuestInitInfos[guestNum].mNameEN.Clone() as string;
        temp.mSeed              = mGuestInitInfos[guestNum].mSeed;
        temp.mEmotion           = mGuestInitInfos[guestNum].mEmotion.Clone() as int[];
        temp.mAge               = mGuestInitInfos[guestNum].mAge;
        temp.mJob               = mGuestInitInfos[guestNum].mJob;
        temp.mJobEN             = mGuestInitInfos[guestNum].mJobEN;
        temp.mSatatisfaction    = mGuestInitInfos[guestNum].mSatatisfaction;
        temp.mSatVariation      = mGuestInitInfos[guestNum].mSatVariation;
        temp.mSatEmotions       = mGuestInitInfos[guestNum].mSatEmotions;
        temp.mLimitEmotions     = mGuestInitInfos[guestNum].mLimitEmotions;
        temp.mSentence          = mGuestInitInfos[guestNum].mSentence;
        temp.isDisSat           = false;
        temp.isCure             = false;
        temp.mVisitCount        = 1;
        temp.mNotVisitCount     = 0;
        temp.isChosen           = false;
        temp.mUsedCloud         = new List<StoragedCloudData>();
        temp.mSitChairIndex     = -1;
        temp.isUsing            = false;
        // Demo Version
        temp.rejectCount        = 0;

        mGuestInfo[guestNum] = temp;
    }

    // TODO : 함수 개편
    // 방문주기를 초기화 해주는 함수
    public void InitGuestTime()
    {
        mGuestTime = 0.0f;
        isTimeToTakeGuest = false;
        isGuestInLivingRoom = false;
    }

    // 하루가 지나면서 초기화가 필요한 정보들을 변환해준다.
    public void InitDay()
    {
        // 새로운 방문 뭉티 리스트를 뽑는다.
		mGuestCount = -1;

		// 새로운 리스트를 뽑는 함수를 호출 (테스트를 위해서 잠시 주석처리)

		mGuestMax = NUM_OF_TODAY_GUEST_LIST;
        int[] list = new int[mGuestMax];
        mTodayGuestList = list;

        mTodayGuestList = NewChoiceGuest();
        mGuestMax = mTodayGuestList.Length;
        //mGuestIndex = mTodayGuestList[0];

        //mTodayGuestList = new int[] {0,3,6,9,12,13};

        // 방문 주기를 초기화한다.
        InitGuestTime();

        GameObject DialogIndex = GameObject.Find("DialogIndex");
        if(DialogIndex != null)
        {
            DialogIndex.GetComponent<DialogIndex>().mDialogIndex = 0;
        }
        // 채집물들이 다시 갱신된다.

    }

    //감정 상하한선에 근접한 감정들 배열 형식으로 return
    public int[] SpeakEmotionEffect(int guestNum)
    {
		List<int> emotionList = new List<int>();    // 상하한선 근접한 감정을 저장하는 list
		int[] returnEmotionList = new int[4];       // 위의 List 정보를 반환하는 배열
        int nearValue = 100;                        // 가장 가깝다의 기준이 되는 값, 감정 수치 차이의 최대치인 100으로 초기화

        int upDiffValue =   100;                    // 상한선의 차이 값 저장 변수
        int downDiffValue = 100;                    // 하한선의 차이 값 저장 변수

		GuestInfos targetGuest = mGuestInfo[guestNum];  // 대상 guest
        
        // 상하한선에 가장 가까운 값 탐색
        for(int num = 0; num < 2; num++){
            upDiffValue = targetGuest.mLimitEmotions[num].upLimitEmotionValue                   
                - targetGuest.mEmotion[targetGuest.mLimitEmotions[num].upLimitEmotion];
            downDiffValue = targetGuest.mEmotion[targetGuest.mLimitEmotions[num].downLimitEmotion]
            - targetGuest.mLimitEmotions[num].downLimitEmotionValue;

			if (upDiffValue <= nearValue) { nearValue = upDiffValue; }
            if (downDiffValue <= nearValue) { nearValue = downDiffValue; }
        }

		//상하한선에 가장 근접한 감정인지를 체크 후 리스트에 추가
        // UpLimit보다 높거나 DownLimit보다 낮으면 불만 뭉티이므로 확인 할 필요 X
        for(int num = 0; num < 2; num++) {
			upDiffValue = targetGuest.mLimitEmotions[num].upLimitEmotionValue                     
				- targetGuest.mEmotion[targetGuest.mLimitEmotions[num].upLimitEmotion];
			downDiffValue = targetGuest.mEmotion[targetGuest.mLimitEmotions[num].downLimitEmotion] 
			- targetGuest.mLimitEmotions[num].downLimitEmotionValue;

            if (upDiffValue == nearValue) { emotionList.Add(targetGuest.mLimitEmotions[num].upLimitEmotion); }
            if (downDiffValue == nearValue) { emotionList.Add(targetGuest.mLimitEmotions[num].downLimitEmotion); }
		}

		int listSize = emotionList.Count;

		// 리스트의 크기에 맞게 return할 배열 사이즈 재 할당
		returnEmotionList = new int[listSize];

		//순서대로 dialog 출력하도록 오름차순으로 배열에 정렬
		for (int num = 0; num < listSize; num++)
		{
			returnEmotionList[num] = ListMinValue(emotionList);
			emotionList.Remove(ListMinValue(emotionList));
		}

		return returnEmotionList;
	}

    //List의 최솟값 반환 함수
    private int ListMinValue(List<int> list)
    {
        if (list.Count <= 0) return -1; // 리스트가 비어있으면 '-1'반환
        int returnValue = 20;          // 감정 수치의 개수로 초기화

        foreach(var num in list) { if (num <= returnValue) returnValue = num; }   // list 내의 최솟값 탐색

        return returnValue;
    }


    //만족도가 가장 많이 차이나는 감정 번호 return
    public int SpeakEmotionDialog(int guestNum)
    {
		int returnEmotionNum = -1;      // 반환할 감정 번호
		int diffValue = -1;             // 임시로 저장할 만족도 범위와의 차이값
		int maxDiffValue = -1;          // 차이값 중에서 가장 큰 값을 저장하는 것


		GuestInfos targetGuest = mGuestInfo[guestNum];

		for (int i = 0; i < 5; i++)
		{
			// 만족도 범위보다 현재 값이 높다면
			if (targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum]
				> targetGuest.mSatEmotions[i].up)
			{
				diffValue = targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum]
					- targetGuest.mSatEmotions[i].up;
			}
			// 만족도 범위보다 현재 값이 낮다면
			else if (targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum]
				< targetGuest.mSatEmotions[i].down)
			{
				diffValue = targetGuest.mSatEmotions[i].down
					- targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum];
			}
			// 이외의 경우는 만족범위안에 있는 것이므로 무시한다.
			// temp값이 기존 저장된 값보다 만족도 범위와 멀다면 갱신한다.
			if (maxDiffValue < diffValue)
			{
				maxDiffValue = diffValue;
				returnEmotionNum = targetGuest.mSatEmotions[i].emotionNum;
			}
		}
		return returnEmotionNum;
	}

    public int SpeakLeastDiffEmotion(int guestNum)
    {
        int returnEmotionNum = -1;      // 반환할 감정 번호
        int diffValue = 100;             // 임시로 저장할 만족도 범위와의 차이값
        int minDiffValue = 100;         // 차이값 중에서 가장 큰 값을 저장하는 것

        GuestInfos targetGuest = mGuestInfo[guestNum];

        for (int i = 0; i < 5; i++)
        {
            // 만족도 범위보다 현재 값이 높다면
            if (targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum] 
            >  targetGuest.mSatEmotions[i].up)
            {
                diffValue = targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum]
                    - targetGuest.mSatEmotions[i].up;
            }
            // 만족도 범위보다 현재 값이 낮다면
            else if (targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum]
                < targetGuest.mSatEmotions[i].down)
            {
                diffValue = targetGuest.mSatEmotions[i].down
                    - targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum];
            }
            // 이외의 경우는 만족범위안에 있는 것이므로 무시한다.
            // temp값이 기존 저장된 값보다 만족도 범위와 멀다면 갱신한다.
            if (minDiffValue >= diffValue)
            {
                minDiffValue = diffValue;
                returnEmotionNum = targetGuest.mSatEmotions[i].emotionNum;
            }
        }
        return returnEmotionNum;
    }

    // 만족도 5인 뭉티 정보를 넘겨주는 Array
    public int[] GetSatGuestList()
    {
		int[] temp_list;
		int temp_idx = 0;

		int list_size = 0;

		for (int num = 0; num < 20; num++) { if (mGuestInfo[num].isCure == true) list_size++; }

		temp_list = new int[list_size];

		for (int num = 0; num < 20; num++) { if (mGuestInfo[num].isCure == true) temp_list[temp_idx++] = num; }

		return temp_list;
	}

    // 불만 뭉티 정보를 넘겨주는 List
    public int[] DisSatGuestList() {
            int[] temp_list;
		    int temp_idx = 0;

            int list_size = 0;

		    for (int num = 0; num < 20; num++) { if (mGuestInfo[num].isDisSat == true) list_size++; }

            temp_list = new int[list_size];

            for(int num = 0; num < 20; num++) { if (mGuestInfo[num].isDisSat == true) temp_list[temp_idx++] = num; }

            return temp_list;
    }
    
    public void InitGuestQueue(int season)
    {
        mGuestQueue = new Queue<int>();

        int[] seasonGuestList = new int[0];
        switch (season)
        {
            case 1:
                seasonGuestList = new int[] { 0, 1, 3, 4, 6 }; // + 22
                break;
            case 2:
                seasonGuestList = new int[] { 12, 13, 14, 18, 19 }; // + 20
                break;
            case 3:
                seasonGuestList = new int[] { 2, 8, 9, 10, 16 };
                break;
            case 4:
                seasonGuestList = new int[] { 5, 7, 11, 15, 17 }; // + 21
                break;
            default:
                break;
        }

        for (int i = 0; i < seasonGuestList.Length; i++)
        {
            mGuestQueue.Enqueue(seasonGuestList[i]);
        }
	}
}
