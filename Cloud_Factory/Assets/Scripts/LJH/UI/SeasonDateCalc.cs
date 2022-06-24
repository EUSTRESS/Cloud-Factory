using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 날짜 및 계절 계산 스크립트
public class SeasonDateCalc : MonoBehaviour
{
    // SeasonDateCalc의 인스턴스를 담는 전역 변수
    private static SeasonDateCalc instance = null;

    // m = 클래스 멤버 변수 표시
    public float m_second; // 초, 시간, 600초=10분=하루
    public int   m_day; // 일 (1~20일)
    public int   m_week; // 주 (5일마다 1주, 4주가 최대)
    public int   m_season; // 달, 계절 (4주마다 1달, 봄,여름,가을,겨울 순으로 4달)
    public int   m_year; // 년 (~)

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

    // SeasonDateCalc Instance에 접근할 수 있는 프로퍼티, 다른 클래스에서 사용가능
    public static SeasonDateCalc Instance
    {
        get
        {
            if (null == instance) return null;

            return instance;
        }
    }

    void Update()
    {
        // 초 계산
        m_second += Time.deltaTime;
        // 일 계산
        // 20일 제한
        if (m_day > 20) m_day = 1;
        else m_day += dayCalc(ref m_second);
        // 주 계산        
        m_week = weekCalc(ref m_day);
        // 달, 계절 계산
        m_season += seasonCalc(ref m_week);
        // 년 계산
        m_year += yearCalc(ref m_season);
    }

    // ref를 선언해서 변수의 주소 값 접근
    int dayCalc(ref float second)
    {
        int temp = 0;
        // 10분당 1일, 600초당 1일 추가
        if (second >= 600.0f)
        {
            temp += 1;
            second = 0;
        }
        return temp;
    }
    int weekCalc(ref int day)
    {        
        // 0~4까지 1주가 나오려면
        // ex) day는 1부터 시작, 5일이라면, 5-1 / 5 = 0 >> +1 >> 1주차
        // 6-1 / 5 = 1 >> +1 >> 2주차
        return ((day - 1) / 5) + 1;
    }
    int seasonCalc(ref int week)
    {
        int temp = 0;
        // 4주가 최대, 5주차부터는 없음
        if (week > 4)
        {
            temp += 1;
            week = 1;
        }
        return temp;
    }
    int yearCalc(ref int month)
    {
        int temp = 0;
        if (month > 4)
        {
            temp += 1;
            month = 1;
        }
        return temp;
    }
}
