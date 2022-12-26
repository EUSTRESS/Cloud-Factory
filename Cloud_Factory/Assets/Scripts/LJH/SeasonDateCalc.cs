using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 날짜 및 계절 계산 스크립트
[System.Serializable]
public class SeasonDateCalc : MonoBehaviour
{
    // SeasonDateCalc의 인스턴스를 담는 전역 변수
    private static SeasonDateCalc instance = null;    
    // SeasonDateCalc Instance에 접근할 수 있는 프로퍼티, 다른 클래스에서 사용가능
    public  static SeasonDateCalc Instance
    {
        get
        {
            if (null == instance) return null;

            return instance;
        }
    }

    public float    mSecond; // 초, 시간, 600초=10분=하루
    public int      mDay;    // 일 (1~20일)
    public int      mWeek;   // 주 (5일마다 1주, 4주가 최대)
    public int      mSeason; // 달, 계절 (4주마다 1달, 봄,여름,가을,겨울 순으로 4달)
    public int      mYear;   // 년 (~)    

    void Awake()
    {
        // 인스턴스 할당
        if (null == instance)
        {
            instance = this;
            // 모든 씬에서 날짜 계산해야하므로
            // 단, title씬에서는 제외한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // 이미 존재하면 이전부터 사용하던 것을 사용함
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        // 로비, 구름제작, 구름제공 화면에서는 제한
        if (SceneManager.GetActiveScene().name != "Lobby"
         || SceneManager.GetActiveScene().name != "Cloud Storage"
         || SceneManager.GetActiveScene().name != "Give Cloud")
        {
            // 초 계산
            mSecond += Time.deltaTime;
            // 일 계산
            // 20일 제한
            if (mDay > 20) mDay = 1;
            else mDay += CalcDay(ref mSecond);
            // 주 계산        
            mWeek = CalcWeek(ref mDay);
            // 달, 계절 계산
            mSeason += CalcSeason(ref mWeek);
            // 년 계산
            mYear += CalcYear(ref mSeason);
        }
    }

    // ref를 선언해서 변수의 주소 값 접근
    int CalcDay(ref float second)
    {
        int temp = 0;
        // 10분당 1일, 600초당 1일 추가
        if (second >= 600.0f)
        {
            // 날짜 변하는 부분 -> 날짜단위 변환내용은 여기에 작성
            // 날마다 초기화 해야하는 사항 : 방문할 손님 리스트 
            if(!GameObject.FindWithTag("Guest"))
            {
                Debug.Log("모든 손님이 퇴장하였기 때문에 하루가 넘어갑니다");
                temp += 1;
                second = 0;
            }
            else
            {
                Debug.Log("하루의 시간이 지났지만 모든 손님이 퇴장하지 않아서 다음날이 되지 않습니다.");
            }
        }
        return temp;
    }
    int CalcWeek(ref int day)
    {        
        // 0~4까지 1주가 나오려면
        // ex) day는 1부터 시작, 5일이라면, 5-1 / 5 = 0 >> +1 >> 1주차
        // 6-1 / 5 = 1 >> +1 >> 2주차
        return ((day - 1) / 5) + 1;
    }
    int CalcSeason(ref int week)
    {
        int temp = 0;
        // 4주가 최대, 5주차부터는 없음
        if (week > 4)
        {
            // 달 변하는 부분 -> 달 단위 변환내용은 여기에 작성
            // 계절마다 변해야 하는 사항 :  

            temp += 1;
            week = 1;
        }
        return temp;
    }
    int CalcYear(ref int month)
    {
        int temp = 0;
        if (month > 4)
        { 
            // 년 변하는 부분 -> 년 단위 변환내용은 여기에 작성

            temp += 1;
            month = 1;
        }
        return temp;
    }
}
