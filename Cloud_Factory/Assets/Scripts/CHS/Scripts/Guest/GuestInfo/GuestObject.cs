using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Pathfinding;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
public class GuestObject : MonoBehaviour
{
    // 오브젝트 내에서 필요한 변수
    [Header("[손님 정보]")]
    public float        mLimitTime;         // 손님이 대기한 시간
    public float        mMaxLimitTime;      // 손님이 대기하는 시간의 최대값
    public int          mGuestNum;          // 해당 오브젝트의 손님번호
    private Transform   mTransform;         // 위치값이 변하는지 확인하기 위한 변수
    public GameObject   mTargetChair;       // 목표로 하는 의자의 위치
    public int          mTargetChiarIndex;

    [Header("[FSM 관련]")]
    public bool isSit;                      // 자리에 앉아있는가?
    public bool isUsing;                    // 구름 치료를 받는중인가?
    public bool isMove;                     // 이동중인가?   
    public bool isGotoEntrance;             // 출구로 나가는 중인가?
    public bool isEndUsingCloud;            // 구름 사용을 끝마쳤는가?

    [Header("[기타]")]
    public Animator     mGuestAnim;         // 손님의 애니메이션 변수
    private Guest       mGuestManager;
    public SOWManager   mSOWManager;

    const int MAX_GUEST_NUM = 20;


    // 손님과 상호작용을 위해 필요한 콜라이더 
    private Collider2D sitCollider;
    private Collider2D walkCollider;

    // 각 손님의 번호에 따라 애니메이터를 만들어서 저장한다.
    public RuntimeAnimatorController[] animators = new RuntimeAnimatorController[MAX_GUEST_NUM];

    // 손님 번호를 저장해준다.
    public void setGuestNum(int guestNum = 0)
    {
        mGuestNum = guestNum;

        
    }


    private void Awake()
    {

        DontDestroyOnLoad(this.gameObject);

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
    }

    // 걷는 애니메이션 출력
    // 걷는 애니메이션을 디폴트 애니메이션으로 설정

    private void Update()
    {
        // 할당받는 의자 설정
        if (mTargetChiarIndex != -1 && isGotoEntrance == false)
        {
            mTargetChair = mSOWManager.mChairPos[mTargetChiarIndex];
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
        if (isUsing != true)
        {
            if (SceneManager.GetActiveScene().name != "Lobby"
                && SceneManager.GetActiveScene().name != "Cloud Storage"
                && SceneManager.GetActiveScene().name != "Give Cloud")
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
                    mSOWManager.mUsingGuestList.RemoveAt(i);
            }

            // 불만 손님으로 변환 후, 귀가
            mGuestManager.mGuestInfo[mGuestNum].isDisSat = true;
            MoveToEntrance();
        }

        // 입구에 도달한 경우
        if (isGotoEntrance == true && transform.position.x - mSOWManager.mWayPoint[0].transform.position.x <= 0.2f)
        {
            Destroy(this.gameObject);
        }

        // 의자에 도달한 경우
        if (mTargetChiarIndex != -1)
        {
            if (isGotoEntrance == false && Mathf.Abs(transform.position.x - mTargetChair.transform.position.x) 
                <= 0.1f && Mathf.Abs(transform.position.y - mTargetChair.transform.position.y) <= 0.1f)
            {
                // 의자 위치로 이동 , 방향에 따라서 LocalScale 조정
                if(mSOWManager.mSitDir[mTargetChiarIndex] == 1)
                    transform.localScale = new Vector3(1f, 1f, 1f);
                else
                    transform.localScale = new Vector3(-1f, 1f, 1f);

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
                Debug.Log("isUsing : true");
                mGuestManager.mGuestInfo[mGuestNum].isUsing = false;
            }
            // 치료 중인 경우 치료효과에 따라서 주기적으로 애니메이션을 제공
            if (isUsing)
            {
                // 제공 받은 구름의 영향에 따라서 앉아있는 모습이 긍정적/부정적 중 하나가 나온다.
                // 테스트를 위해 일단은 웃는 모습으로 진행한다.
                mGuestAnim.SetBool("isUsing", true);

                // 사용시간이 지나면 구름 오브젝트에서 실행된 코루틴을 통해 isEndUsingCloud가 true가 되어 귀가한다.
                if (isEndUsingCloud)
                    MoveToEntrance();
            }
        }

        // 걷는 방향에 따라 애니메이션의 방향을 다르게 지정한다.
        if (GetComponent<AIPath>().desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (GetComponent<AIPath>().desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        // 현재 위치를 저장한다.
        mTransform = transform;

    }

    public void SpeakEmotion()
    {
        Debug.Log("감정 모션을 출력합니다");

        // 앉아있는 경우에만 클릭 시 상호작용을 통해 감정을 표현한다.
        if (!mGuestAnim.GetBool("isSit")) return;

        // 감정 상한, 하한 범위에 가장 가까운 감정에 대한 힌트(이펙트)
        

        // 만족도 반영 범위에서 가장 먼 감정을 알려주는 말풍선  -> 손님의 위치값에 따라 좌/우 측에 생성
    
  
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
        isSit = false;
        isUsing = false;
        mGuestAnim.SetBool("isUsing", false);

        isGotoEntrance = true;
        mGuestAnim.SetBool("isSit", false);
        ChangeLayerToDefault();

        // TODO : 콜라이더 변경 Sitting -> Walking
        sitCollider.enabled = false;
        walkCollider.enabled = true;

        // 부여받은 의자 인덱스값 초기화
        mGuestManager.mGuestInfo[mGuestNum].mSitChairIndex = -1;

        Invoke("ChangeTarget", 3.0f);
    }

    private void ChangeTarget()
    {
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
