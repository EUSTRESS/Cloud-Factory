using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 벡터 위치 상수값 혹시 모를 수정에 용이하게하기 위해서,, 더 좋은 아이디어가 떠오르지는 않음,,
namespace ConstantsVector
{
    public enum EFrontToBack // 맨 앞에서 맨 뒤로 가는 위치
    {
        X = -625,
        Y = -180
    }
    public enum EBackToFront // 맨 뒤에서 맨 앞으로 가는 위치
    {
        X = -425,
        Y = 20
    }
    public enum EFirstPos    // 제일 앞에서 보이는 위치
    {
        X = -575,
        Y = -130
    }
    public enum ESecondPos   // 중간에서 보이는 위치
    {
        X = -525,
        Y = -80
    }
    public enum EThirdPos    // 마지막으로 보이는 위치
    {
        X = -475,
        Y = -30
    }
}

public class ProfileMoving : MonoBehaviour
{    
    private RecordUIManager mUIManager;   // UI Manager 스크립트

    private RectTransform   rProflieBG;   // UI의 이동은 RectTransform이 담당함

    private Vector2         vOriginalPos; // 현재의 벡터를 담는다
    private Vector2         vNextPos;     // 다음 페이지로 넘어갈 벡터를 담는다
    private Vector2         vPrevPos;     // 이전 페이지로 넘어갈 벡터를 담는다

    private Animator        mPageAnim;    // 페이지 넘기기 애니메이션 담당

    private float           mMoveSpeed;   // 페이지 넘어가는 속도 담당

    [HideInInspector]
    public  bool            mIsMoving;    // 페이지가 움직이는지 판단하는 bool값

    public  bool mIsUpset;                // 불만뭉티를 보는지 전체뭉티를 보는지

    void Awake()
    {
        mMoveSpeed = 250f; // 페이지 넘기는 속도

        mUIManager = GameObject.Find("UIManager").GetComponent<RecordUIManager>();
        mPageAnim  = GetComponent<Animator>();
        rProflieBG = GetComponent<RectTransform>();

        // 애니메이션이 활성화되면 이동이 고정되기 때문에 필요한 경우에만 사용한다
        mPageAnim.enabled = false; 

        InitPosValue();   // 벡터들을 초기화시킨다.
        mIsMoving = true; // 초기값은 움직일 수 있다.
    }

    // 움직이는 거라서 물리기반 업데이트 사용해야 랜덤으로 프레임호출 되는 Update에 비해서 버그 안생김
    void FixedUpdate() 
    {
        if (mUIManager) // null check
        {
            if (mIsMoving) // 움직인다면
            {
                // 다음 페이지 버튼을 누르면
                // 현재 rect 앵커 포지션이 다음 위치까지 도달하도록
                if (mUIManager.mIsNext && rProflieBG.anchoredPosition.x > vNextPos.x
                                       && rProflieBG.anchoredPosition.y > vNextPos.y)
                {
                    // mMoveSpeed 만큼 움직인다.
                    this.gameObject.transform.
                        Translate(new Vector2(-1, -1) * mMoveSpeed * Time.deltaTime);
                }
                // 다음 위치에 도달하거나 그 이상 넘어갈 경우 바로 현재 위치값으로 갱신
                else if (mUIManager.mIsNext && rProflieBG.anchoredPosition.x <= vNextPos.x
                                            && rProflieBG.anchoredPosition.y <= vNextPos.y)
                {
                    InitPosValue();
                }

                // 이전 페이지 버튼을 누르면
                if (mUIManager.mIsPrev && rProflieBG.anchoredPosition.x < vPrevPos.x
                                       && rProflieBG.anchoredPosition.y < vPrevPos.y)
                {
                    this.gameObject.transform.
                        Translate(new Vector2(1, 1) * mMoveSpeed * Time.deltaTime);
                }
                else if (mUIManager.mIsPrev && rProflieBG.anchoredPosition.x >= vPrevPos.x
                                            && rProflieBG.anchoredPosition.y >= vPrevPos.y)
                {
                    InitPosValue();
                }

                // 페이지가 넘어가는 애니메이션을 실행할 위치
                if (rProflieBG.anchoredPosition.x <= (int)ConstantsVector.EFrontToBack.X
                 && rProflieBG.anchoredPosition.y <= (int)ConstantsVector.EFrontToBack.Y)
                {
                    // 불만 뭉티랑 전체 보기랑 페이지 넘기는 방식이 다름
                    // 불만 뭉티만 보기
                    if (mIsUpset == true)
                    {
                        // 위치 이동
                        rProflieBG.anchoredPosition = new Vector2((int)ConstantsVector.EThirdPos.X,
                                                                  (int)ConstantsVector.EThirdPos.Y);
                        InitPosValue();                // 벡터 갱신
                    }
                    // 전체보기
                    else if (mIsUpset == false)
                    {
                        this.gameObject.transform.SetSiblingIndex(3);
                        Invoke("DelaySibling", 0.12f);

                        mPageAnim.enabled = true;           // 애니메이터 엑티브하여 사용할 수 있게 한다                    
                        mPageAnim.SetTrigger("isLastPage"); // 애니메이션 실행
                        Invoke("DelayMoveProfile", 0.5f);
                    }
                }
                // 제일 뒈에 보이는 것에서 앞으로 이동할 때
                else if (rProflieBG.anchoredPosition.x >= (int)ConstantsVector.EBackToFront.X
                      && rProflieBG.anchoredPosition.y >= (int)ConstantsVector.EBackToFront.Y)
                {
                    // 맨 앞으로 이동
                    rProflieBG.anchoredPosition = new Vector2((int)ConstantsVector.EFirstPos.X, 
                                                              (int)ConstantsVector.EFirstPos.Y);
                    InitPosValue();                     // 벡터 갱신
                }
            }

            // 하이어라키 시블링 조작하여 UI 우선순위 관리
            // 제일 앞에 있을 때
            if (rProflieBG.anchoredPosition.x == (int)ConstantsVector.EFirstPos.X
             && rProflieBG.anchoredPosition.y == (int)ConstantsVector.EFirstPos.Y)
            {
                this.gameObject.transform.SetSiblingIndex(2); 
            }
            // 중간
            else if (rProflieBG.anchoredPosition.x == (int)ConstantsVector.ESecondPos.X
                  && rProflieBG.anchoredPosition.y == (int)ConstantsVector.ESecondPos.Y)
            {
                this.gameObject.transform.SetSiblingIndex(1);
            }
            // 제일 뒤에 있을 때
            else if (rProflieBG.anchoredPosition.x == (int)ConstantsVector.EThirdPos.X
                  && rProflieBG.anchoredPosition.y == (int)ConstantsVector.EThirdPos.Y)
            {
                this.gameObject.transform.SetSiblingIndex(0);
            }            
        }
    }
    // 0번 시블링 인덱스 살짝 딜레이 연출
    void DelaySibling()
    {
        this.gameObject.transform.SetSiblingIndex(0);
    }
    // 애니메이션이 조작되는 동안 시간 딜레이
    void DelayMoveProfile()
    {
        // 위치 이동
        rProflieBG.anchoredPosition = new Vector2((int)ConstantsVector.EThirdPos.X,
                                                  (int)ConstantsVector.EThirdPos.Y);
        mPageAnim.enabled = false; // 애니메이션 끔   
        InitPosValue();                // 벡터 갱신
    }
    // 벡터 갱신
    void InitPosValue()
    {   
        vOriginalPos = rProflieBG.anchoredPosition;                       // 현재 위치
        vNextPos = new Vector2(vOriginalPos.x - 50, vOriginalPos.y - 50); // 다음페이지 위치
        vPrevPos = new Vector2(vOriginalPos.x + 50, vOriginalPos.y + 50); // 이전페이지 위치
        mIsMoving = false;                                                // 이동을 멈춘다
    }    
}
