using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherUIManager : MonoBehaviour
{
    public GameObject mGuideGather; // 채집할건지 안할건지 알려주는 UI
    public GameObject mGathering;   // 채집 중 출력하는 UI
    public GameObject mGatherResult;// 채집 결과를 출력하는 UI

    public Animator mGatheringAnim; // 채집 애니메이션

    public Text tGatheringText;      // 채집 중... 텍스트
    private int mGatheringTextCount; // 채집 중 '.' 재귀 제한

    public RectTransform mGatherImageRect; // 채집 이미지 Rect Transform

    public RectTransform[] mFxShine = new RectTransform[3]; // 3개의 채집 결과 회전 효과

    void Update()
    {
        if (mGatherResult.activeSelf)
        {
            // 채집 결과 효과
            mFxShine[0].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[1].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[2].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
        }
    }
    // 마당 버튼 클릭 시, 채집하시겠씁니까? 오브젝트 활성화    
    public void OpenGuideGather()
    {
        mGuideGather.SetActive(true);
    }
    // 나가기, 채집하시겠씁니까? 오브젝트 비활성화    
    public void CloseGuideGather()
    {
        mGuideGather.SetActive(false);
    }
    // 채집하기
    public void GoingToGather()
    {
        mGuideGather.SetActive(false);
        mGathering.SetActive(true);
        mGatheringTextCount = 0; // 초기화
        tGatheringText.text = "재료 채집 중"; // 초기화

        if (SeasonDateCalc.Instance) // null check
        {                            // 각 해당하는 애니메이션 출력
            Invoke("PrintGatheringText", 0.5f); // 0.5초 딜레이마다 . 추가
            if (SeasonDateCalc.Instance.mSeason == 1) // 봄이라면
            {
                mGatherImageRect.sizeDelta = new Vector2(1090, 590); // 이미지 사이즈 맞추기
                
                mGatheringAnim.SetBool("Spring", true);
                mGatheringAnim.SetBool("Summer", false);
                mGatheringAnim.SetBool("Fall", false);
                mGatheringAnim.SetBool("Winter", false);
            }
            else if (SeasonDateCalc.Instance.mSeason == 2) // 여름이라면
            {
                mGatherImageRect.sizeDelta = new Vector2(1090, 590); // 이미지 사이즈 맞추기

                mGatheringAnim.SetBool("Spring", false);
                mGatheringAnim.SetBool("Summer", true);
                mGatheringAnim.SetBool("Fall", false);
                mGatheringAnim.SetBool("Winter", false);
            }
            else if (SeasonDateCalc.Instance.mSeason == 3) // 가을이라면
            {
                mGatherImageRect.sizeDelta = new Vector2(735, 420); // 이미지 사이즈 맞추기

                mGatheringAnim.SetBool("Spring", false);
                mGatheringAnim.SetBool("Summer", false);
                mGatheringAnim.SetBool("Fall", true);
                mGatheringAnim.SetBool("Winter", false);
            }
            else if (SeasonDateCalc.Instance.mSeason == 4) // 겨울이라면
            {
                mGatherImageRect.sizeDelta = new Vector2(560, 570); // 이미지 사이즈 맞추기

                mGatheringAnim.SetBool("Spring", false);
                mGatheringAnim.SetBool("Summer", false);
                mGatheringAnim.SetBool("Fall", false);
                mGatheringAnim.SetBool("Winter", true);
            }
        }
        // 5초 동안 채집 후 결과 출력
        Invoke("Gathering", 5.0f);
    }
    void Gathering()
    {
        mGathering.SetActive(false);
        mGatherResult.SetActive(true);

        CancelInvoke(); // 인보크 충돌 방지를 위해서 출력 결과가 나오면 모든 인보크 꺼버림
    }
    // 재귀함수로 마침표를 재귀적으로 출력한다
    void PrintGatheringText()
    {
        mGatheringTextCount++;
        tGatheringText.text = tGatheringText.text + ".";

        if (mGatheringTextCount <= 3)
        {
            Invoke("PrintGatheringText", 0.25f); // 0.25초 딜레이마다 . 추가
        }
        else // 초기화
        {
            mGatheringTextCount = 0;
            tGatheringText.text = "재료 채집 중";
            Invoke("PrintGatheringText", 0.25f); // 0.25초 딜레이마다 . 추가
        }
    }
    // 채집 끝!
    public void CloseResultGather()
    {
        mGatherResult.SetActive(false);        
    }
}
