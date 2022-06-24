using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 각종 UI 담당
public class UIManager : MonoBehaviour
{
    // t_변수 (text)
    public Text t_Season;
    public Text t_date;

    void Update()
    {
        if (t_Season && t_date) // null check
        {
            t_date.text = SeasonDateCalc.Instance.m_day.ToString() + "일차";
            if (SeasonDateCalc.Instance.m_season == 1)
                t_Season.text = "봄";
            else if (SeasonDateCalc.Instance.m_season == 2)
                t_Season.text = "여름";
            else if (SeasonDateCalc.Instance.m_season == 3)
                t_Season.text = "가을";
            else if (SeasonDateCalc.Instance.m_season == 4)
                t_Season.text = "겨울";
        }  
    }

    // 버튼
    public void Go_SpaceOfWeatherBtn()
    {
        SceneManager.LoadScene("Space Of Weather");
    }
    public void Go_CloudFactoryBtn()
    {
        SceneManager.LoadScene("Cloud Factory");
    }
    public void Go_DrawingRoomBtn()
    {
        SceneManager.LoadScene("Drawing Room");
    }
    public void Active_RecordOfHealing()
    {
        // 치유의 기록
    }
}
