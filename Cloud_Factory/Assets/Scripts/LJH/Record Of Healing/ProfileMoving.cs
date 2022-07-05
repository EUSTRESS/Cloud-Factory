using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 프로필 BG 원래 위치
// 1 : -575 -130 0
// 2 : -525 -80  0 
// 3 : -475 -30  0
// TEMP --> 뒤 : -425 20 0
// TEMP --> 앞 : -625 -180 0

// 1,2,3 위치에 있는 놈들 색깔 변경
// 템프로 위치 변경

// 초기 인덱스
// 0부터 4까지 순서대로 앞에서 뒤로

// 인스턴스화해서 캔버스에 넣기
// 현재 위치에서 x -50 y -60 까지(if문으로 rect~걸어서) 일정한 속도로 Translate (다음)
// 맨 뒤에 인스턴스하나 만들고 맨 앞에 있는 놈은 그 위치까지 도달하면 삭제

// 현재 위치에서 x +50 y +60 까지(if문으로 rect~걸어서) 일정한 속도로 Translate (이전)
// 맨 앞에 인스턴스하나 만들고 맨 뒤에 있는 놈은 그 위치까지 도달하면 삭제

// 모든 놈들은 위치값으로 색깔 변경하기


// 예외처리할 것
// 다음페이지 눌렀다가 바로 이전페이지누르면 중간에 있는 놈이 나와야댐

// 시블링인덱스
// 2 (제일 앞) 1 (중간) 0 (제일 뒤)

public class ProfileMoving : MonoBehaviour
{
    private CommonUIManager mUIManager;

    // UI의 이동은 RectTransform이다
    private RectTransform rProflieBG;
    public RectTransform rParentProflie;
    public Vector2 vOriginalPos;
    public Vector2 vNextPos;
    public Vector2 vPrevPos;

    private Image iProfile;

    private float mMoveSpeed;

    [HideInInspector]
    public bool isMoving;

    void Awake()
    {
        Debug.Log(gameObject.name + " : "+ transform.GetSiblingIndex());
        mMoveSpeed = 0.25f;
        mUIManager = GameObject.Find("UIManager").GetComponent<CommonUIManager>();
        iProfile = GetComponent<Image>();

        // rectTransform 가져옴
        rProflieBG = GetComponent<RectTransform>();
        rParentProflie = this.gameObject.transform.parent.GetComponent<RectTransform>();
        InitPosValue();
        isMoving = true;
    }

    void Update()
    {
        if (mUIManager) // null check
        {
            if (isMoving)
            {
                // 다음 페이지 버튼을 누르면
                // 현재 rect 앵커 포지션이 다음 위치까지 도달하도록
                if (mUIManager.mIsNext && (rProflieBG.anchoredPosition.x > vNextPos.x
                                       && rProflieBG.anchoredPosition.y > vNextPos.y))
                {
                    this.gameObject.transform.
                        Translate(new Vector2(-mMoveSpeed * Time.timeScale, -mMoveSpeed * Time.timeScale));
                }
                else if (mUIManager.mIsNext && rProflieBG.anchoredPosition.x <= vNextPos.x
                      && rProflieBG.anchoredPosition.y <= vNextPos.y)
                {
                    InitPosValue();
                }

                // 반대로
                if (mUIManager.mIsPrev && (rProflieBG.anchoredPosition.x < vPrevPos.x
                                           && rProflieBG.anchoredPosition.y < vPrevPos.y))
                {
                    this.gameObject.transform.
                        Translate(new Vector2(mMoveSpeed * Time.timeScale, mMoveSpeed * Time.timeScale));
                }
                else if (mUIManager.mIsPrev && rProflieBG.anchoredPosition.x >= vPrevPos.x
                     && rProflieBG.anchoredPosition.y >= vPrevPos.y)
                {
                    InitPosValue();
                }

                // TEMP로 앞에 있을 때
                if (rProflieBG.anchoredPosition.x <= -625
                                       && rProflieBG.anchoredPosition.y <= -180)
                {
                    rProflieBG.anchoredPosition = new Vector2(-475, -30);
                    //if (this.gameObject.transform.parent.GetSiblingIndex() == 2)
                    //{
                    //    this.gameObject.transform.parent.GetComponent<Animator>().SetTrigger("isLastPage");
                    //    //Invoke("Delay", 0.5f);
                    //}

                    InitPosValue();
                }
                // TMEP로 뒤에 있을 때
                else if (rProflieBG.anchoredPosition.x >= -425
                                          && rProflieBG.anchoredPosition.y >= 20)
                {
                    rProflieBG.anchoredPosition = new Vector2(-575, -130);
                    InitPosValue();
                }
            }

            // 제일 앞에 있을 때
            if (rProflieBG.anchoredPosition.x == -575 && rProflieBG.anchoredPosition.y == -130)
            {
                this.gameObject.transform.parent.SetSiblingIndex(2);
            }
            // 중간
            else if (rProflieBG.anchoredPosition.x == -525 && rProflieBG.anchoredPosition.y == -80)
            {
                this.gameObject.transform.parent.SetSiblingIndex(1);
            }
            // 제일 뒤에 있을 때
            else if (rProflieBG.anchoredPosition.x == -475 && rProflieBG.anchoredPosition.y == -30)
            {
                this.gameObject.transform.parent.SetSiblingIndex(0);
            }            
        }
    }

    void Delay()
    {
        rProflieBG.anchoredPosition = new Vector2(-475, -30);
        rParentProflie.anchoredPosition = new Vector2(0, 0);
    }

    void InitPosValue()
    {
        vOriginalPos = rProflieBG.anchoredPosition;
        vNextPos = new Vector2(vOriginalPos.x - 50, vOriginalPos.y - 50);
        vPrevPos = new Vector2(vOriginalPos.x + 50, vOriginalPos.y + 50);

        isMoving = false;
    }
}
