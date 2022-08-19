using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDropdown : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown mDropdown; // 드롭다운

    public int mDropdownIndex;

    public InventoryContainer inventoryContainer;
    // 20가지 감정인데 일단 예람이꺼랑 기획서 보고 했음 틀리면 수정해줘..
    private string[]     mEmotionArray = new string[20] 
    {
        "기쁨",      "불안",   "슬픔",      "짜증",     "수용",
        "놀람,혼란", "혐오",   "관심,기대", "사랑",     "순정만화가",
        "경외심",    "반대",   "비난",      "경멸",     "공격성",
        "낙천",      "씁쓸함", "애증",      "얼어붙음", "혼란스러움"
    };

    void Awake()
    {
        // 초기화
        mDropdown.ClearOptions();

        // 새로운 옵션 설정을 위한 OptionData 생성
        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        // array에 있는 문자열 데이터 저장
        foreach (string str in mEmotionArray)
        {
            optionList.Add(new TMP_Dropdown.OptionData(str));
        }

        // 생성한 optionlist를 dropdown의 옵션 값에 추가
        mDropdown.AddOptions(optionList);

        // 현재 dropdown에 선택된 옵션을 0번으로 설정
        mDropdown.value = 0;
    }

    public void OnDropdownEvent(int index)
    {
        mDropdownIndex = index;
        Debug.Log("현재 드롭다운 인덱스 : " + mDropdownIndex);
        Debug.Log("여기서 인덱스에 따라서 변경할 때 감정별로 정렬 메소드 호출");
        inventoryContainer.OnDropdownEvent();

    }
}

