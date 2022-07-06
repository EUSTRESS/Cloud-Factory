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

    // 마당 버튼 클릭 시
    public void OpenGuideGather()
    {
        mGuideGather.SetActive(true);
    }
    // 나가기
    public void CloseGuideGather()
    {
        mGuideGather.SetActive(false);
    }
    // 채집하기
    public void GoingToGather()
    {
        mGuideGather.SetActive(false);
        mGathering.SetActive(true);
        mGatheringTextCount = 0;
        tGatheringText.text = "재료 채집 중";

        if (SeasonDateCalc.Instance) // null check
        {                            // 각 해당하는 애니메이션 출력
            Invoke("PrintGatheringText", 0.5f); // 0.5초 딜레이마다 . 추가
            if (SeasonDateCalc.Instance.mSeason == 1) // 봄이라면
            {                
                mGatheringAnim.SetBool("Spring", true);
                mGatheringAnim.SetBool("Summer", false);
            }
            else if (SeasonDateCalc.Instance.mSeason == 2) // 여름이라면
            {
                mGatheringAnim.SetBool("Spring", false);
                mGatheringAnim.SetBool("Summer", true);
            }
            else if (SeasonDateCalc.Instance.mSeason == 3) // 가을이라면
            {

            }
            else if (SeasonDateCalc.Instance.mSeason == 4) // 겨울이라면
            {

            }
        }
        // 5초 동안 딜레이 후 결과 출력
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
            Invoke("PrintGatheringText", 0.5f); // 0.5초 딜레이마다 . 추가
        }
        else // 초기화
        {
            mGatheringTextCount = 0;
            tGatheringText.text = "재료 채집 중";
            Invoke("PrintGatheringText", 0.5f); // 0.5초 딜레이마다 . 추가
        }
    }
    
    public void CloseResultGather()
    {
        mGatherResult.SetActive(false);        
    }
}
