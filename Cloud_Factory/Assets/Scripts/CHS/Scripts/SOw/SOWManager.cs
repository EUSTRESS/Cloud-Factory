using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SOWManager : MonoBehaviour
{
    [Header("[날씨의 공간에 배치된 손님 정보]")]
    public Queue<int> mWaitGuestList;                   // 응접실에서 수락을 받고 넘어온 손님들의 리스트
    public List<int> mUsingGuestList;                   // 날씨의 공간에서 자리에 앉아 구름을 제공받을 준비가 된 손님들의 리스트

    [HideInInspector]
    public Queue<GameObject> mWaitGuestObjectQueue;     // 대기 손님 오브젝트들을 관리할 리스트
    [HideInInspector]
    public Queue<GameObject> mUsingGuestObjectList;     // 사용 손님 오브젝트들을 관리할 리스트
    
    [Header("[의자 & 웨이포인트 관련]")]
    public GameObject[] mChairPos;                      // 손님이 앉아서 구름을 사용할 의자(구름)
    public GameObject[] mWayPoint;                      // 손님이 걸어다니며 산책하는 경로들

    [HideInInspector]
    public Dictionary<int, bool> mCheckChairEmpty;      // 의자마다 의자가 비어있는지를 확인하는 딕셔너리 변수
    [HideInInspector]
    public bool isNewGuest;                             // 응접실에서 넘어올때 새로운 손님이 오는가?
    [HideInInspector]
    public int mMaxChairNum;                            // 현재 단계에 따른 의자의 개수

    [Header("[프리팹]")]
    [SerializeField]
    private GameObject mGuestObject;                    // 인스턴스하여 생성할 손님 오브젝트 프리팹

    private Guest mGuestManager;                        // GuestManager를 가져온다.
    private static SOWManager instance = null;

    [HideInInspector]
    public int mCloudGuestNum;
    [HideInInspector]
    public StoragedCloudData mStorageCloudData;
    [HideInInspector]
    public bool isCloudGet;

    private int mTempGuestNum;                          // 임시 손님 번호값

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
            mUsingGuestObjectList   = new Queue<GameObject>();
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
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(mUsingGuestList.Count);
        }

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
            tempObject.GetComponent<GuestObject>().initAnimator();

            // 산책로를 설정한다. <- 계절별로 달라지게 만들어야 한다.
            // 계절별로 만들어서 봄 여름 가을 겨울의 배열값을 받게끔 하는것을 고려중.
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

        if(isCloudGet)
        {
            if (SceneManager.GetActiveScene().name == "Space Of Weather")
            {
                GetCloudToGuest(mCloudGuestNum, mStorageCloudData);
                isCloudGet = false;
            }
        }
    }

    // 하루가 지나면 날씨의 공간을 초기화한다.
    private void InitSOW()
    {
        mWaitGuestList.Clear();
        mUsingGuestList.Clear();
        mCheckChairEmpty.Clear();

        // 업그레이드 단계에 따라 mCheckChairEmpty에서 확인하는 의자의 개수가 줄어든다.
        // 아직 합치지 않았으므로 일괄적으로 3개로 가정하고 개발한다.
        for (int i = 0; i < mMaxChairNum; i++)
        {
            // 모든 의자는 비어있는 상태로 초기화
            mCheckChairEmpty.Add(i, true);
        }
        // TODO : 방문할 뭉티, 끝나는 시점에 날씨의 공간에 있는 뭉티들 불만 뭉티로 전환



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
        mUsingGuestObjectList.Enqueue(tempObject);

        // Guest의 부여받은 의자 인덱스를 갱신
        mGuestManager.mGuestInfo[guestNum].mSitChairIndex = chairNum;
    }

    // 하루가 끝날 때 Queue에 남아있는 뭉티들을 불만 뭉티로 만들어준다.
    public void MakeGuestDisSat()
    {
        for (int i = 0; i < mWaitGuestList.Count; i++)
        {
            mGuestManager.mGuestInfo[mWaitGuestList.Dequeue()].isDisSat = true;
        }
        for (int i = 0; i < mUsingGuestList.Count; i++)
        {
            int guestNum = mUsingGuestList[i];
            mGuestManager.mGuestInfo[guestNum].isDisSat = true;
            mGuestManager.mGuestInfo[guestNum].mSitChairIndex = -1;
        }
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
    public void GetCloudToGuest(int guestNum, StoragedCloudData storagedCloudData)
    {
        GameObject.Find("CloudSpawner").GetComponent<CloudSpawner>().SpawnCloud(guestNum, storagedCloudData);
    }

    public void SetCloudData(int guestNum, StoragedCloudData storagedCloudData)
    {
        mCloudGuestNum = guestNum;
        mStorageCloudData = storagedCloudData;
        isCloudGet = true;
    }
}

