using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LanguageChanger : MonoBehaviour
{
    // button text
    [Header("버튼내 텍스트")] 
    public Text tOK1;
    public Text tOK2;
    public Text tBack;
    public Text tRecordOfHealing;
    public Text tGuideBook;
    public Text tDrawingRoom;
    public Text tCloudFactory;
    public Text tSpaceOfWeather;
    public Text tGameOver;
    
    // text
    [Header("독립 텍스트")] 
    public Text tSetting;
    public Text tBackGroundSound;
    public Text tEffectSound;
    public Text tGather;
    public Text tGatherWarning;
    public Text tResult;
    
    

    // Start is called before the first frame update
    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Space Of Weather":
                ChangeLanguageInSOW();
                break;
            default:
                break;

        }
    }


    private void ChangeLanguageInSOW()
    {
        // 년, 일은 CommonUIManager에서 관리
        // "채집 중"은 WeatherUIManager에서 관리
        if (LanguageManager.GetInstance() != null 
            && LanguageManager.GetInstance().GetCurrentLanguage() == "Korean")
        {
            tRecordOfHealing.text = "치유의\n기록";
            tGuideBook.text = "도감";
            tDrawingRoom.text = "응접실";
            tCloudFactory.text = "구름공장";
            tSetting.text = "설정";
            tBackGroundSound.text = "배경음";
            tEffectSound.text = "효과음";
            tGameOver.text = "게임 종료";
            tGather.text = "마당에서 재료 채집을 진행하시겠습니까?";
            tGatherWarning.text = "(재료 인벤토리가 꽉 차있는 경우, 별도의 알림 없이 수집 된 재료는 버려집니다.)";
            tOK1.text = "재료 채집";
            tBack.text = "나가기";
            tResult.text = "재료 채집 결과";
            tOK2.text = "확인";
        }
        else
        {
            tRecordOfHealing.text = "Record\nOf\nHealing";
            tGuideBook.text = "Guide\nBook";
            tDrawingRoom.text = "Drawing Room";
            tCloudFactory.text = "Cloud\nFactory";
            tSetting.text = "Setting";
            tBackGroundSound.text = "BGM";
            tEffectSound.text = "SFx";
            tGameOver.text = "Game Over";
            tGather.text = "Continue to gather ingredients from garden?";
            tGatherWarning.text = "(Warning Text.)";
            tOK1.text = "Gather";
            tBack.text = "Exit";
            tResult.text = "Result";
            tOK2.text = "OK";
        }
    }
}
