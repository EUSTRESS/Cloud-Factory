using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Pathfinding;

public class GuestObject : MonoBehaviour
{
    // 오브젝트 내에서 필요한 변수
    public float mLimitTime;            // 손님이 대기하는 시간의 한계값
    public int mGuestNum;               // 해당 오브젝트의 손님번호
    public bool isSit;                  // 자리에 앉아있는가?
    public bool isUsing;                // 구름 치료를 받는중인가?
    private Transform mTransform;       // 위치값이 변하는지 확인하기 위한 변수
    private Guest mGuestManager;
    public GameObject mTargetChair;     // 목표로 하는 의자의 위치
    public int mTargetChiarIndex;
    public bool isMove;                 // 이동중인가?   
    public bool isAlreadyUse;                  // 사용을 완료 했는가?
    public Animator mGuestAnim;         // 손님의 애니메이션 변수
    public SOWManager mSOWManager;


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
        isSit = false;
        isUsing = false;
        isMove = false;
        isAlreadyUse = false;
        mTransform = this.transform;
        mTargetChiarIndex = -1;
        mTargetChair = null;
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        mGuestAnim = GetComponent<Animator>();
        // 손님에 따라서 이미지(애니메이션)을 변경시킨다.
        initAnimator();

    }

    // 걷는 애니메이션 출력
    // 걷는 애니메이션을 디폴트 애니메이션으로 설정

    private void Update()
    {
        // 할당받는 의자 설정
        if (mTargetChiarIndex != -1 && isAlreadyUse == false)
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
        }

        // 구름을 제공받는 상태가 아니라면 대기시간을 갱신시킨다.
        if (isUsing != true)
        {
            mLimitTime += Time.deltaTime;
        }
        

        bool GoHome = false;
        // 대기시간이 지나거나 불만뭉티가 된 경우에
        if ((mLimitTime > 50.0f || mGuestManager.mGuestInfos[mGuestNum].isDisSat == true) && GoHome == false)
        {
            // 사용자 리스트에서 없애고, 해당 의자를 다시 true로 바꿔주어야 한다.
            mSOWManager.mCheckChairEmpty[mTargetChiarIndex] = true;
            mTargetChair = null;
            isSit = false;
            GoHome = true;
            MoveToEntrance();
        }

        // 입구에 도달한 경우
        if (isAlreadyUse == true && transform.position.x - mSOWManager.mWayPoint[0].transform.position.x <= 0.2f)
        {
            Destroy(this.gameObject);
        }

        // 의자에 도달한 경우
        if (mTargetChiarIndex != -1)
        {
            if (isAlreadyUse == false && Mathf.Abs(transform.position.x - mTargetChair.transform.position.x) <= 0.1f)
            {
                // 의자 위치로 이동
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                mGuestAnim.SetBool("isSit", true);
                this.transform.position = mTargetChair.transform.position;
                isSit = true;

            }
        }

        // 상태에 따라서 애니메이션 제공
        if (isSit)
        {
            if(mGuestManager.mGuestInfos[mGuestNum-1].isUsing == true)
            {
                isUsing = true;
                Debug.Log("isUsing : true");
                mGuestManager.mGuestInfos[mGuestNum - 1].isUsing = false;
            }
            // 치료 중인 경우 치료효과에 따라서 주기적으로 애니메이션을 제공
            if (isUsing)
            {

                // 제공 받은 구름의 영향에 따라서 앉아있는 모습이 긍정적/부정적 중 하나가 나온다.
                // 테스트를 위해 일단은 웃는 모습으로 진행한다.
                mGuestAnim.SetBool("isUsing", true);

                // 5초 후에 Invoke 함수를 이용해서 사용중인 상태를 종료하고, 귀가하는 모션을 집어넣는다.
                Invoke("TakeCloud", 5.0f);
                isUsing = false;
            }
        }
        else
        {
            // 산책로를 걸으며 대기중이기 때문에 걷는 모션을 제공한다.

            // 오브젝트의 위치값이 변하지 않았다면 서있는 애니메이션을 출력해준다.
            if (mTransform == transform)
            {
                mGuestAnim.SetBool("isStand", true);
            }
            // 다시 변하는 경우에는 걷는 애니메이션을 출력한다.
            else
            {
                mGuestAnim.SetBool("isStand", false);
            }

        }

        // 걷는 방향에 따라 애니메이션의 방향을 다르게 지정한다.
        if (GetComponent<AIPath>().desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (GetComponent<AIPath>().desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }

        // 현재 위치를 저장한다.
        mTransform = transform;

    }
    
    private void TakeCloud()
    {
        Debug.Log("Take Cloud");
        isUsing = false;
        mGuestAnim.SetBool("isUsing", false);

        // 상하한선 값 및 만족도 갱신


        MoveToEntrance();
    }

    public void SpeakEmotion()
    {
        Debug.Log("감정 모션을 출력합니다");
        // 감정 상한, 하한 범위에 가장 가까운 감정에 대한 힌트(이펙트)
        // 만족도 반영 범위에서 가장 먼 감정을 알려주는 대사
    }

    // 애니메이션 클립들을 손님에 맞게 초기화한다.
    private void initAnimator()
    {

    }

    // 입구로 퇴장하는 함수이다.
    private void MoveToEntrance()
    {
        isAlreadyUse = true;
        mGuestAnim.SetBool("isSit", false);

        // 부여받은 의자 인덱스값 초기화
        mGuestManager.mGuestInfos[mGuestNum].mSitChairIndex = -1;

        Invoke("ChangeTarget", 1.5f);
        //this.GetComponent<AIDestinationSetter>().target = mSOWManager.mWayPoint[0].transform;
    }

    private void ChangeTarget()
    {
        this.GetComponent<AIDestinationSetter>().target = mSOWManager.mWayPoint[0].transform;
    }
}
