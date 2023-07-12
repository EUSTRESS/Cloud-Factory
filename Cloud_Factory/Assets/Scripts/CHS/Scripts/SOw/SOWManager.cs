using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SOWManagerSaveData
{
    public int[] yardGatherCount = new int[4];
}


public class SOWManager : MonoBehaviour
{
    [Header("[날씨의 공간에 배치된 손님 정보]")]
    public Queue<int> mWaitGuestList;                   // 응접실에서 수락을 받고 넘어온 손님들의 리스트
    public List<int> mUsingGuestList;                   // 날씨의 공간에서 자리에 앉아 구름을 제공받을 준비가 된 손님들의 리스트

    [HideInInspector]
    public Queue<GameObject> mWaitGuestObjectQueue;     // 대기 손님 오브젝트들을 관리할 리스트
    [HideInInspector]
    public List<GameObject> mUsingGuestObjectList;     // 사용 손님 오브젝트들을 관리할 리스트

    public List<GameObject> mCloudObjectList;

    [Header("[의자 & 웨이포인트 관련]")]
    public GameObject[] mChairPos;                      // 손님이 앉아서 구름을 사용할 의자(구름)
    public GameObject[] mWayPoint;                      // 손님이 걸어다니며 산책하는 경로들
    public int[]mSitDir = {1,1,1,1};                    // 손님이 의자에 앉는 방향 ( 좌 : 0 , 우 : 1로 표시)

    [HideInInspector]
    public Dictionary<int, bool> mCheckChairEmpty;      // 의자마다 의자가 비어있는지를 확인하는 딕셔너리 변수
    [HideInInspector]
    public bool isNewGuest;                             // 응접실에서 넘어올때 새로운 손님이 오는가?
    [HideInInspector]
    public int mMaxChairNum;                            // 현재 단계에 따른 의자의 개수

    [Header("[프리팹]")]
    [SerializeField]
    public GameObject mGuestObject;                    // 인스턴스하여 생성할 손님 오브젝트 프리팹

    public int[] yardGatherCount = new int[4];

    private Guest mGuestManager;                        // GuestManager를 가져온다.
    private static SOWManager instance = null;

    [HideInInspector]
    public int mCloudGuestNum;
    [HideInInspector]
    public StoragedCloudData mStorageCloudData;
    [HideInInspector]
    public bool isCloudGet;

    private int mTempGuestNum;                          // 임시 손님 번호값


    // QA용
    int _sat = 0;

    private float[][][] ChairPosList = new float[][][]{ 
        new float[][]{ new float[]{ 1.71f, -2.91f }, new float[] { -3.52f, -0.36f }, new float[] { -0.51f, 0.39f }, new float[] { 1.55f, 0.53f } } ,
        new float[][]{ new float[]{ 1.545f, -2.154f }, new float[] { -3.436f, -0.178f }, new float[] { -0.27f, -2.8f }, new float[] { -1.64f, -2.07f } } ,
        new float[][]{ new float[]{ 3.186f, -2.646f }, new float[] { -0.187f, -2.355f }, new float[] { 1.057f, 0.297f }, new float[] { 1.97f, 0.23f } } ,
        new float[][]{ new float[]{ 1.9f, -3.21f }, new float[] { -1.11f, 0.19f }, new float[] { 2.06f, 0.15f }, new float[] { -0.16f, 0.17f } }
    };

    private float[][][] WayPosList = new float[][][]{
        new float[][]{ new float[] { -8.66f, -2.4f }, new float[]{ -6.82f, -1.43f }, new float[] { -3.91f, -0.63f }, new float[] { -0.95f, 0.13f }, new float[] { 1.97f, -0.97f }, new float[] { 4.64f, -2.1f }, new float[] { -0.91f, 0.08f }, new float[] { -4.14f, -0.51f } } ,
        new float[][]{ new float[] { -8.8f, -2.33f }, new float[]{ -6.84f, -1.56f }, new float[] { -3.81f, -0.62f }, new float[] { -0.76f, -1.1f }, new float[] { 1.36f, -0.39f }, new float[] { 2.25f, 0.2f}, new float[] { -0.64f, -1.14f }, new float[] { -4.14f, -0.64f } } ,
        new float[][]{ new float[] { -8.8f, -2.22f }, new float[]{ -6.73f, -1.39f }, new float[] { 0.49f, 0.51f }, new float[] { 5.64f, -2.16f }, new float[] { 1.57f, -4.22f}, new float[] { -2.11f, -2.13f }, new float[] { 1.97f, -0.36f }, new float[] { 0.35f, 0.59f } } ,
        new float[][]{ new float[] { -8.37f, -2.39f }, new float[]{ -6.26f, -1.58f }, new float[] { -2.6f, -0.87f }, new float[] { 2.04f, -2.49f }, new float[] { 4.25f, -1.96f}, new float[] { 2.19f, -2.47f }, new float[] { -2.53f, -0.79f }, new float[] { -4.08f, -0.92f} } 
    };

    private int[][] SitDirList = new int[][] {
        new int[]{ 1,1,1,1 },
        new int[]{ 0,1,1,1 },
        new int[]{ 0,1,0,1 },
        new int[]{ 1,0,0,1 }
    };


    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);

            mGuestManager           = GameObject.Find("GuestManager").GetComponent<Guest>();
            mWaitGuestList          = new Queue<int>();
            mUsingGuestList         = new List<int>();
            mWaitGuestObjectQueue   = new Queue<GameObject>();
            mUsingGuestObjectList   = new List<GameObject>();
            mCloudObjectList        = new List<GameObject>();
            mCheckChairEmpty        = new Dictionary<int, bool>();
            isNewGuest              = false;
            isCloudGet              = false;

            // 현재 단계에 맞는 의자 개수 설정
            mMaxChairNum = 3;
            InitSOW();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        // 새로운 손님이 날씨의 공간으로 넘어온 경우
        if (isNewGuest)
        {
            isNewGuest = false;

            GameObject tempObject;

            // 손님 오브젝트 생성 및 초기화
            tempObject = Instantiate(mGuestObject);

            // 손님 생성 확인을 위한 디버깅
            Debug.Log("손님 생성");

            // 손님 오브젝트에 해당하는 번호를 넣어준다.
            tempObject.GetComponent<GuestObject>().setGuestNum(mTempGuestNum);
			tempObject.GetComponent<RLHReader>().LoadHintInfo(mTempGuestNum);
			tempObject.GetComponent<GuestObject>().initAnimator();
            tempObject.GetComponent<GuestObject>().init();

            // 산책로를 설정한다. 
            tempObject.GetComponent<WayPoint>().WayPos = mWayPoint;

            // 기본 위치값을 선언한다. <- 산책로의 첫번째 값으로 설정
            tempObject.transform.position = mWayPoint[0].transform.position;

            // 대기중인 손님 큐에 해당 손님을 추가한다.
            mWaitGuestObjectQueue.Enqueue(tempObject);
        }

        // 산책로를 걷는 손님이 존재하고, 이용 가능한 의자가 비어있다면 의자로 이동시키기
        if (mWaitGuestList.Count != 0)
        {
            // 남은 의자가 있다면 해당 의자에 대한 인덱스를 반환받고, 이외는 -1를 받아온다.
            int chairNum = GetRandChiarIndex();

            if (chairNum != -1)
            {
                // 사용자 리스트로 해당 손님을 옮긴다.
                MoveToUsingList(chairNum);
            }
        }

        // 날씨의 공간 상의 손님을 선택하기 <- 상호작용
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(vec, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject hitObject = hit.transform.root.gameObject;
                if (hit.transform.gameObject.tag == "Guest")
                {

                    Debug.Log(hitObject.GetComponent<GuestObject>().mGuestNum + "번 손님을 클릭하였습니다.");
                    hit.transform.gameObject.GetComponent<GuestObject>().SpeakEmotion();
                }
                else
                {
                    Debug.Log(hit.transform.gameObject);
                }
            }
        }
        // Test. 희귀도4 재료를 이용한 구름을 제공받은 경우 클리어 힌트를 제공하는 것을 우클릭으로 테스트한다.
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(vec, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject hitObject = hit.transform.root.gameObject;
                if (hit.transform.gameObject.tag == "Guest")
                {
                    Debug.Log(hitObject.GetComponent<GuestObject>().mGuestNum + "번 손님을 클릭하였습니다.");
                    hit.transform.gameObject.GetComponent<GuestObject>().Hint();
                }
                else
                {
                    Debug.Log(hit.transform.gameObject);
                }
            }
        }

        if (isCloudGet)
        {
            Debug.Log("isCloudGet is True");
            if (SceneManager.GetActiveScene().name == "Space Of Weather")
            {
                GetCloudToGuest(mCloudGuestNum, mStorageCloudData, _sat);
            }
            else
            {
                Debug.Log("씬을 찾는 것에 실패");
            }
        }
    }

    // 하루가 지나면 날씨의 공간을 초기화한다.
    public void InitSOW()
    {
        mWaitGuestList.Clear();
        mUsingGuestList.Clear();
        mCheckChairEmpty.Clear();

        // 구름 오브젝트 정리
        DestoryClouds();
        mCloudObjectList.Clear();

        // 업그레이드 단계에 따라 mCheckChairEmpty에서 확인하는 의자의 개수가 줄어든다.
        // 아직 합치지 않았으므로 일괄적으로 3개로 가정하고 개발한다.
        for (int i = 0; i < mMaxChairNum; i++)
        {
            // 모든 의자는 비어있는 상태로 초기화
            mCheckChairEmpty.Add(i, true);
        }

        for (int i = 0; i < 4; i++) { yardGatherCount[i] = 2; }

        int Season = GameObject.Find("Season Date Calc").GetComponent<SeasonDateCalc>().mSeason;
        ChangeWeatherObject(Season-1);
    }

    // 대기 리스트에 손님을 추가시켜주는 함수
    public void InsertGuest(int guestNum)
    {
        mWaitGuestList.Enqueue(guestNum);
        mTempGuestNum = guestNum;
    }


    // 대기 리스트에서 손님을 제공 받는 리스트로 추가시켜주는 함수
    private void MoveToUsingList(int chairNum)
    {
        // 사용자 목록으로 이동할 손님의 번호를 받아온다.
        int guestNum = mWaitGuestList.Dequeue();

        // 받아온 손님의 정보를 사용자 목록으로 넣는다.
        mUsingGuestList.Add(guestNum);

        // tempObject를 통해 의자를 배정한다.    
        GameObject tempObject = mWaitGuestObjectQueue.Dequeue();

        tempObject.GetComponent<GuestObject>().mTargetChiarIndex = chairNum;

        // 오브젝트를 사용자 오브젝트 목록으로 넣는다.
        mUsingGuestObjectList.Add(tempObject);

        // Guest의 부여받은 의자 인덱스를 갱신
        mGuestManager.mGuestInfo[guestNum].mSitChairIndex = chairNum;
    }

    // 하루가 끝날 때 Queue에 남아있는 뭉티들을 불만 뭉티로 만들어준다.
    public void MakeGuestDisSat()
    {
        int waitCount = mWaitGuestList.Count;
        for (int i = 0; i < waitCount; i++)
        {
            int guestNum = mWaitGuestList.Dequeue();
            mGuestManager.mGuestInfo[guestNum].isDisSat = true;
            Debug.Log(guestNum + "번 손님이 대기중 집으로 돌아갑니다"); 
        }
        for (int i = 0; i < mUsingGuestList.Count; i++)
        {
            int guestNum = mUsingGuestList[i];
            mGuestManager.mGuestInfo[guestNum].isDisSat = true;
            mGuestManager.mGuestInfo[guestNum].mSitChairIndex = -1;
            Debug.Log(guestNum + "번 손님이 착석 중 집으로 돌아갑니다");
        }

        //// 존재하는 모든 구름을 삭제한다.
        //GameObject[] Clouds = GameObject.FindGameObjectsWithTag("Cloud");
        //if (Clouds != null)
        //{
        //    foreach(GameObject i in Clouds)
        //    {
        //        CloudObject cloudObject = i.GetComponent<CloudObject>();
        //        if(cloudObject != null)
        //        {
        //            cloudObject.StopToUse();
        //        }
        //    }
        //}

    }

    // 빈 의자에 대한 개수를 검색한다.
    private int GetRandChiarIndex()
    {
        int result = -1;
        bool isSelect = false;

        // 모든 의자가 차 있는지 확인
        int count = 0;
        for (int i = 0; i < mMaxChairNum; i++)
        {
            if (mCheckChairEmpty[i] == false)
            {
                count++;
            }
        }
        if (count == mMaxChairNum)
        {
            return -1;
        }

        // 빈 의자를 선택할 때까지 진행
        while (!isSelect)
        {
            result = -1;
            result = Random.Range(0, mMaxChairNum);
            if (mCheckChairEmpty[result] == true)
            {
                isSelect = true;
                mCheckChairEmpty[result] = false;
            }
        }
        return result;
    }

    // 구름 스포너로 구름정보를 넘긴다.
    public void GetCloudToGuest(int guestNum, StoragedCloudData storagedCloudData /*QA용*/, int sat)
    {
        GameObject CloudSpawner = GameObject.Find("CloudSpawner");
        if(CloudSpawner != null) 
        {
            CloudSpawner.GetComponent<CloudSpawner>().SpawnCloud(guestNum, storagedCloudData, sat);
            Debug.Log("SpanwCloud");
            isCloudGet = false;
        }
        else
        {
            Debug.Log("CloudSpawner is null");
        }
    }

    public void SetCloudData(int guestNum, StoragedCloudData storagedCloudData /* QA용 */ , int sat)
    {
        mCloudGuestNum = guestNum;
        mStorageCloudData = storagedCloudData;
        isCloudGet = true;
        _sat = sat;
    }


    public void ChangeWeatherObject(int weather)
    {
        for(int i = 0; i< 4; i++)
        {
            mChairPos[i].transform.position = new Vector3(ChairPosList[weather][i][0], ChairPosList[weather][i][1],0f);
            mSitDir[i] = SitDirList[weather][i];
        }
        for (int i = 0; i < 8; i++)
        {
            mWayPoint[i].transform.position = new Vector3(WayPosList[weather][i][0], WayPosList[weather][i][1], 0f);
        }
    }

    void DestoryClouds()
    {
        foreach(GameObject obj in mCloudObjectList)
        {
            if(obj != null)
            {
                CloudObject cloudObject = obj.GetComponent<CloudObject>();
                if(cloudObject != null)
                {
                    cloudObject.StopToUse();
                }
            }
        }
    }

    void LoadSaveData()
    {
        // 계절에 맞는 UI불러오기

        // 손님 오브젝트 리스트가 존재했는지 체크
        // -> 있었다면 오브젝트를 생성하여 정보값을 넣는다.

        // yardGatherCount[]의 int값 불러오기
       

    }

}

