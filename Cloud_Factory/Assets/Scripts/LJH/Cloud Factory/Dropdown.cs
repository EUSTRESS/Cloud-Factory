using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dropdown : MonoBehaviour
{
    [SerializeField]
    public TMP_Dropdown mDropdown; // 드롭다운

    public int mDropdownIndex;

    // 20가지 감정인데 일단 예람이꺼랑 기획서 보고 했음 틀리면 수정해줘..
    private string[]     mEmotionArrayKR = new string[20] 
    {
        "기쁨",      "불안",   "슬픔",      "짜증",     "수용",
        "놀람,혼란", "혐오",   "관심,기대", "사랑",     "순종,순응",
        "경외심",    "반대",   "비난",      "경멸",     "공격성",
        "낙천",      "씁쓸함", "애증",      "얼어붙음", "혼란스러움"
    };

    private string[] mEmotionArrayEN = new string[20]
    {
        "joy", "apprehension", "sadness", "annoyance", "acceptance",
        "surprise", "disgust", "interest", "love", "submission",
        "awe", "disapproval", "remorse", "contempt", "aggressiveness",
        "optimism", "bitter", "love & hate", "freezing", "confusion"
    };

    void Awake()
    {
        // 초기화
        mDropdown.ClearOptions();

        // 새로운 옵션 설정을 위한 OptionData 생성
        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        // array에 있는 문자열 데이터 저장
        if (LanguageManager.GetInstance().GetCurrentLanguage() == "Korean")
        {
            foreach (string str in mEmotionArrayKR)
            {
                optionList.Add(new TMP_Dropdown.OptionData(str));
            }
        }
        else
        {
            foreach (string str in mEmotionArrayEN)
            {
                optionList.Add(new TMP_Dropdown.OptionData(str));
            }
        }
        

        // 생성한 optionlist를 dropdown의 옵션 값에 추가
        mDropdown.AddOptions(optionList);

        // 현재 dropdown에 선택된 옵션을 0번으로 설정
        mDropdown.value = 0;
    }

}

