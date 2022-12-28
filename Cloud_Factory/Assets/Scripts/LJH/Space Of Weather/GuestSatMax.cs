using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestSatMax : MonoBehaviour
{
    private Guest mGuestManager;
    private SeasonDateCalc mSeason;

    public bool[] isMaxSat = new bool[6]; // 손님 번호에 해당하는 bool 관리
    public  int mPrevDate; // 이전 날짜

    public GameObject mLetter;

    void Awake()
    {
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mSeason = GameObject.Find("Season Date Calc").GetComponent<SeasonDateCalc>();
    }

    void Update()
    {
        for (int i = 0; i < 6; i++)
        {
            /// 만족도 5인 뭉티를 찾는 작업
            if (!isMaxSat[i] &&
                5 == mGuestManager.mGuestInfo[mGuestManager.mTodayGuestList[i]].mSatatisfaction)
            {
                // 만족도 5 팝업 Bool 켜놓고
                isMaxSat[i] = true;
                // 현재 날짜 받아오기
                mPrevDate = mSeason.mDay;
            }
        }

        for (int i = 0; i < 6; i++)
        {
            // 만족도 5 이면서 날짜가 변경되면
            if (isMaxSat[i] && (mPrevDate != mSeason.mDay))
            {
                // 다음 날이 될 때 팝업 띄우기
                Debug.Log("편지 팝업");
                mLetter.SetActive(true);
                // 팝업 띄웠으면 초기화
                isMaxSat[i] = false;

                // 만족도 5인 뭉티가 집에가면 위의 for문 돌지 않을 것임
                // 현재는 계속 뭉티가 있어서 isMaxSat배열을 false로 바꾸고 바로 위에서 True로 바꾸고있음
                // 만족도 5였던 뭉티 만족도를 1로 내리거나 집에 갔다는 표시하면 될 듯?
            }
        }
    }

    public void CloseLetter()
    {
        mLetter.SetActive(false);
    }
}
