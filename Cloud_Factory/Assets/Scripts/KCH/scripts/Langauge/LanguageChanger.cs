using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

    public Text tAccept;
    public Text tReject;

    public Text[] tGiveCloud = new Text[3];
    public Text tUpsetMoongti;
    public Text tAllMoongti;

    public TMP_Text tAuto;

    // text
    [Header("독립 텍스트")] 
    public Text tSetting;
    public Text tBackGroundSound;
    public Text tEffectSound;
    public Text tGather;
    public Text tGatherWarning;
    public Text tResult;

    public Text[] tName = new Text[3];
    public Text[] tAge = new Text[3];
    public Text[] tJob = new Text[3];
    public Text[] tUsedCloud = new Text[3];
    public Text tReason;
    public Text tSort;

    public Text[] tSat = new Text[3];
    public Text[] tExplanation = new Text[3];
    
    public Text tExpirationDate;

    public Text tResetWarning;
    public Text tContinueWarning;

    private LanguageManager mLanguageManager;

    // Start is called before the first frame update
    void Start()
    {
        mLanguageManager = GameObject.Find("LanguageManager").GetComponent<LanguageManager>();
        switch (SceneManager.GetActiveScene().name)
        {
            case "Space Of Weather":
                ChangeLanguageInSOW();
                break;
            case "Drawing Room":
                ChangeLanguageInDR();
                break;
            case "Record Of Healing":
                ChangeLanguageInROH();
                break;
            case "Cloud Factory":
                ChangeLanguageInCF();
                break;
            case "Give Cloud":
                ChangeLanguageInGC();
                break;
            case "Deco Cloud":
                ChangeLanguageInDC();
                break;
            case "Cloud Storage":
                ChangeLanguageInCS();
                break;
            default:
                break;

        }
    }

	public void ChangeLanguageInLobby()
    {
        if (SceneManager.GetActiveScene().name != "Lobby") return;
        
		if (LanguageManager.GetInstance().GetCurrentLanguage() == "Korean")
        {
            tSetting.text = "설정";
            tBackGroundSound.text = "배경음";
            tEffectSound.text = "효과음";
            tAccept.text = "확인";
            tReject.text = "취소";
            tResetWarning.text = "지금까지의 내용을 초기화 하고\n새로운 게임을 진행 하시겠습니까?\n(한 번 삭제된 데이터는 복구할 수 없습니다.)";
            tContinueWarning.text = "이어할 데이터가 없습니다.";
            tOK1.text = "확인";
        }
        else
        {
            tSetting.text = "SETTINGS";
            tBackGroundSound.text = "BGM";
            tEffectSound.text = "SFX";
            tAccept.text = "ACCEPT";
            tReject.text = "CANCEL";
            tResetWarning.text = "Reset everything and start a new game?\n(Deleted files cannot be restored.)";
            tContinueWarning.text = "There's no data.";
            tOK1.text = "CONFIRM";
        }
	}

    private void ChangeLanguageInSOW()
    {
        // 년, 일은 CommonUIManager에서 관리
        // "채집 중"은 WeatherUIManager에서 관리
        if (mLanguageManager.GetCurrentLanguage() == "Korean")
        {
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
            tDrawingRoom.text = "OFFICE";
            tCloudFactory.text = "CLOUD\nFACTORY";
            tSetting.text = "SETTINGS";
            tBackGroundSound.text = "BGM";
            tEffectSound.text = "SFX";
            tGameOver.text = "Quit";
            tGather.text = "Harvest the ingredients?";
            tGatherWarning.text = "(Some ingredients may be lost if your inventory is full)";
            tOK1.text = "HARVEST";
            tBack.text = "BACK";
            tResult.text = "RESULTS";
            tOK2.text = "CONFIRM";
        }
    }

    private void ChangeLanguageInDR()
    {
        if (mLanguageManager.GetCurrentLanguage() == "Korean")
        {
            tSpaceOfWeather.text = "날씨의 공간";
            tCloudFactory.text = "구름공장";
            tSetting.text = "설정";
            tBackGroundSound.text = "배경음";
            tEffectSound.text = "효과음";
            tGameOver.text = "게임 종료";
            tReason.text = "방문 사유:";
            tAccept.text = "수락";
            tReject.text = "거절";
        }
        else
        {
            tSpaceOfWeather.text = "WEATHER\nROOM";
            tCloudFactory.text = "CLOUD\nFACTORY";
            tSetting.text = "SETTINGS";
            tBackGroundSound.text = "BGM";
            tEffectSound.text = "SFX";
            tGameOver.text = "Quit";
            tReason.text = "REASON OF VISIT:";
            tAccept.text = "ACCEPT";
            tReject.text = "REJECT";
        }
    }
    
    private void ChangeLanguageInROH()
    {
        if (mLanguageManager.GetCurrentLanguage() == "Korean")
        {
            tBack.text = "돌아가기";
            tUpsetMoongti.text = "불만 뭉티만 보기";
            tAllMoongti.text = "전체 보기";
            for (int i = 0; i < 3; i++)
            {
                tGiveCloud[i].text = "구름 제공";
                tName[i].text = "이름: ";
                tAge[i].text = "나이: ";
                tJob[i].text = "직업: ";
                tUsedCloud[i].text = "사용한 구름";
            }
        }
        else
        {
            tBack.text = "BACK";
            tUpsetMoongti.text = "Unhappy Ones";
            tAllMoongti.text = "Everyone";
            for (int i = 0; i < 3; i++)
            {
                tGiveCloud[i].text = "SEND";
                tName[i].text = "NAME: ";
                tAge[i].text = "AGE: ";
                tJob[i].text = "JOB: ";
                tUsedCloud[i].text = "Used Clouds: ";
            }
        }
    }
    
    private void ChangeLanguageInCF()
    {
        if (mLanguageManager.GetCurrentLanguage() == "Korean")
        {
            tSpaceOfWeather.text = "날씨의 공간";
            tDrawingRoom.text = "응접실";
            tSetting.text = "설정";
            tBackGroundSound.text = "배경음";
            tEffectSound.text = "효과음";
            tGameOver.text = "게임 종료";
        }
        else
        {
            tSpaceOfWeather.text = "WEATHER\nROOM";
            tDrawingRoom.text = "OFFICE";
            tSetting.text = "SETTINGS";
            tBackGroundSound.text = "BGM";
            tEffectSound.text = "SFX";
            tGameOver.text = "Quit";
        }
    }

    private void ChangeLanguageInGC()
    {
        if (mLanguageManager.GetCurrentLanguage() == "Korean")
        {
            tBack.text = "돌아가기";
            tSort.text = "감정 별";
        }
        else
        {
            tBack.text = "BACK";
            tSort.text = "EMOTION ⇅";
        }
    }
    
    private void ChangeLanguageInDC()
    {
        if (mLanguageManager.GetCurrentLanguage() == "Korean")
        {
            tAuto.text = "자동";
            tOK1.text = "완성";
            tBack.text = "돌아가기";
        }
        else
        {
            tAuto.text = "AUTO";
            tOK1.text = "FINISH";
            tBack.text = "BACK";
        }
    }
    
    private void ChangeLanguageInCS()
    {
        if (mLanguageManager.GetCurrentLanguage() == "Korean")
        {
            tBack.text = "돌아가기";
            tExpirationDate.text = "보관 기간 별";
            tSort.text = "감정 별";
            for (int i = 0; i < 3; i++)
            {
                tGiveCloud[i].text = "구름 제공";
                tName[i].text = "이름: ";
                tAge[i].text = "나이: ";
                tJob[i].text = "직업: ";
                tSat[i].text = "현재 만족도: ";
                tExplanation[i].text = "한 줄 요약: ";
            }
        }
        else
        {
            tBack.text = "BACK";
            tExpirationDate.text = "TIME LEFT⇅";
            tSort.text = "EMOTION ⇅";
            for (int i = 0; i < 3; i++)
            {
                tGiveCloud[i].text = "SEND";
                tName[i].text = "NAME: ";
                tAge[i].text = "AGE: ";
                tJob[i].text = "JOB: ";
                tSat[i].text = "SATISFACTION: ";
                tExplanation[i].text = "REASON OF VISIT: ";//
            }
        }
    }
}
