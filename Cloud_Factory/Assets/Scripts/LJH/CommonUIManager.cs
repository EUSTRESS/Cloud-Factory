using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 공통 UI 담당
// 계절, 날짜 업데이트
// 씬 이동
public class CommonUIManager : MonoBehaviour
{
    [Header("GAME OBJECT")]
    // 오브젝트 Active 관리
    public GameObject   gOption;       // 옵션 게임 오브젝트
    public GameObject   gGuideBook;
    private bool OptionOn; // 옵션창이 켜졌는지 확인하는 변수

    [Header("TEXT")]
    public Text         tDate;         // 요일 텍스트
    public Text         tYear;         // 년도 텍스트
    public Text         tDateNum;   // 요일 숫자 텍스트
    public Text         tYearNum;   // 년도 숫자 텍스트
    [Space (3f)]
    public Text         tBgmValue;     // BGM 볼륨 텍스트
    public Text         tSfxValue;     // SFx 볼륨 텍스트

    [Header("SLIDER")]
    public Slider       sBGM;          // BGM 슬라이더
    public Slider       sSFx;          // SFx 슬라이더

    [Header("SPRITES")]
    public Sprite[]     sSeasons = new Sprite[4]; // 봄 여름 가을 겨울 달력

    [Header("IMAGES")]
    public Image        iSeasons;      // 달력 이미지

    private AudioSource mSFx;          // 효과음 오디오 소스 예시, 기본 효과음
    private AudioSource mBGM;

    private TutorialManager mTutorialManager;
    private Guest mGuestManager;
	void Awake()
    {
        //if (SceneManager.GetActiveScene().name == "Lobby" ||
        //    SceneManager.GetActiveScene().name == "Space Of Weather" ||
        //    SceneManager.GetActiveScene().name == "Drawing Room" ||
        //    SceneManager.GetActiveScene().name == "Cloud Factory")

        // 모든 씬에서 찾는다.
        mSFx = GameObject.Find("mSFx").GetComponent<AudioSource>();
        mBGM = GameObject.Find("mBGM").GetComponent<AudioSource>();

        mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();

        if (SceneData.Instance) // null check
        {
            // 씬이 변경될 때 저장된 값으로 새로 업데이트
            mBGM.volume = SceneData.Instance.BGMValue;
            mSFx.volume = SceneData.Instance.SFxValue;
            //mSubSFx.volume = SceneData.Instance.SFxValue;
        }
    }

    void Update()
    {
        if (tDate && tYear && iSeasons && SeasonDateCalc.Instance) // null check
        {
            if (LanguageManager.GetInstance() != null 
                && LanguageManager.GetInstance().GetCurrentLanguage() == "English")
            {
                tDate.text = "Day" + SeasonDateCalc.Instance.mDay.ToString();
                tYear.text = "Year" + SeasonDateCalc.Instance.mYear.ToString();

            }
            else
            {
                tDate.text = SeasonDateCalc.Instance.mDay.ToString() + "일";
                tYear.text = SeasonDateCalc.Instance.mYear.ToString() + "년차";
            }

            if (SeasonDateCalc.Instance.mSeason == 1)
                iSeasons.sprite = sSeasons[0]; // 봄
            else if (SeasonDateCalc.Instance.mSeason == 2)
                iSeasons.sprite = sSeasons[1]; // 여름
            else if (SeasonDateCalc.Instance.mSeason == 3)
                iSeasons.sprite = sSeasons[2]; // 가을
            else if (SeasonDateCalc.Instance.mSeason == 4)
                iSeasons.sprite = sSeasons[3]; // 겨울
        }

        // 현재 음량으로 업데이트
        if (sBGM && sSFx && SceneData.Instance)           // null check
        {
            sBGM.value = SceneData.Instance.BGMValue;
            sSFx.value = SceneData.Instance.SFxValue;
        }
        if (tBgmValue && tSfxValue) // null check
        {
            // 소수점 -2 자리부터 반올림
            tBgmValue.text = Mathf.Ceil(sBGM.value * 100).ToString();
            tSfxValue.text = Mathf.Ceil(sSFx.value * 100).ToString();
        }

        if (gOption)
        {
            OptionOn = gOption.activeSelf;
            if (OptionOn)
            {
                sSFx.onValueChanged.AddListener(ChangeCheck_SFx);
            }
        }
        
    }

    private void ChangeCheck_SFx(float change_value)
    {
        sSFx.value = change_value;
        mSFx.Play();
    }

    // 씬 이동 버튼들
    public void GoSpaceOfWeather()
    {
		// 응접실 튜토리얼 중 구름공장으로 이동하지 못하도록 제한
		if (mTutorialManager.isFinishedTutorial[1] == false) { return; }

        LoadingSceneController.Instance.LoadScene("Space Of Weather");
        mSFx.Play();
    }
    public void GoCloudFactory()
    {
        // 응접실 튜토리얼 중 구름공장으로 이동하지 못하도록 제한
        if (mTutorialManager.isFinishedTutorial[1] == false) { return; }

        if (SceneManager.GetActiveScene().name == "DecoCloud"
            && !mTutorialManager.isFinishedTutorial[6]) { return; }

        // 재료 채집 튜토리얼 후 구름공장으로 이동할 때, 튜토리얼 진행도를 저장
		if (!mTutorialManager.isFinishedTutorial[2]) { mTutorialManager.isFinishedTutorial[2] = true; }
		LoadingSceneController.Instance.LoadScene("Cloud Factory");
        mSFx.Play();
    }
    public void GoDrawingRoom()
    {
        if(mGuestManager.isGuestInLivingRoom == false) { return; }
        if (mTutorialManager.isFinishedTutorial[0] == false) { mTutorialManager.isFinishedTutorial[0] = true; }

        LoadingSceneController.Instance.LoadScene("Drawing Room");
        mSFx.Play();
    }
    public void GoRecordOfHealing()
    {
        // 치유의 기록 이전 씬 인덱스 저장
        SceneData.Instance.prevSceneIndex = SceneManager.GetActiveScene().buildIndex;

        Debug.Log("수락후 치유의기록이동!!!");

        LoadingSceneController.Instance.LoadScene("Record Of Healing");
        mSFx.Play();
    }
    public void GoPrevScene()
    {
        // 치유의 기록을 들어가기전 씬으로 이동      
        LoadingSceneController.Instance.LoadScene(SceneData.Instance.prevSceneIndex);
        mSFx.Play();        
    }
    public void GoGiveCloud()
    {
        if (mTutorialManager.isFinishedTutorial[3] == false) { mTutorialManager.isFinishedTutorial[3] = true; }

        // 구름 만들기가 끝난 뒤, 구름 데코 튜토리얼로 이동하기 전, 구름 제작 씬으로의 이동을 막는다.
        if (mTutorialManager.isFinishedTutorial[4] == true
            && mTutorialManager.isFinishedTutorial[5] == false) 
        { return; }

        LoadingSceneController.Instance.LoadScene("Give Cloud");
        GameObject.Find("InventoryManager").GetComponent<InventoryManager>().go2CloudFacBtn();
        mSFx.Play();
    }
    public void GoCloudStorage()
    {
        LoadingSceneController.Instance.LoadScene("Cloud Storage");
        mSFx.Play();
    }

    public void GoDecoCloud()
    {
        // 구름 공장에서 구름 데코까지 이동하는 튜토리얼
		if (mTutorialManager.isFinishedTutorial[5] == false) { mTutorialManager.isFinishedTutorial[5] = true; }
		LoadingSceneController.Instance.LoadScene("DecoCloud");
        mSFx.Play();
    }

    // 옵션창 활성화, 비활성화
    public void ActiveOption()
    {
        mSFx.Play();
        gOption.SetActive(true);
    }
    public void UnActiveOption()
    {
        mSFx.Play();
        gOption.SetActive(false);

        // 옵션창 끌 때 소리 변경내역 저장함.
        SaveUnitManager mSaveUnitData = GameObject.Find("SaveUnitManager").GetComponent<SaveUnitManager>();
        if (null == mSaveUnitData)
            return;
        mSaveUnitData.Save_SoundData();
    }
    public void ActiveGuideBook()
    {
        gGuideBook.SetActive(true);
        mSFx.Play();
    }
    public void UnActiveGuideBook()
    {
        gGuideBook.SetActive(false);
        mSFx.Play();
    }

    // 게임 종료
    public void QuitGame()
    {
        mSFx.Play();
        Application.Quit();
    }

    public void SFx_Play()
    {
        mSFx.Play();
    }
}
