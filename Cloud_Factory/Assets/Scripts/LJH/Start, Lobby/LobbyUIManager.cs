using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using Newtonsoft.Json;

// 로비 씬 UI 담당
// 설정 창, 새로하기, 이어하기
public class LobbyUIManager : MonoBehaviour
{
    [Header("GAME OBJECT")]
    // 오브젝트 Active 관리
    public GameObject   gOption;     // 옵션 게임 오브젝트
    public GameObject   gWarning;    // 새로운 게임 경고창

    [Header("TEXT")]
    public Text         tBgmValue;   // BGM 볼륨 텍스트
    public Text         tSfxValue;   // SFx 볼륨 텍스트

    [Header("SLIDER")]
    public Slider       sBGM;        // BGM 슬라이더
    public Slider       sSFx;        // SFx 슬라이더

    private AudioSource mSFx;        // 효과음 오디오 소스

    void Awake()
    {
        mSFx = GameObject.Find("SFx").GetComponent<AudioSource>();        
    }

    void Update()
    {
        // 현재 음량으로 업데이트
        if (sBGM && sSFx) // null check
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
    }

    /*
     * BUTTON에 할당할 메소드
     */

    public void NewGame()
    {        
        mSFx.Play();

        SceneManager.LoadScene("Space Of Weather");

        if (Directory.Exists(Application.dataPath + "/Data/")) // Data 폴더가 있으면 삭제하기
        {
            Debug.Log("삭제");
            Directory.Delete(Application.dataPath + "/Data/", true);
        }
        // 데이터를 초기화 시키는 함수 호출할 필요 없이
        // 각 클래스 생성자에서 자동 초기화된다.
    }

    public void ContinueGame()
    {
        mSFx.Play();

        /*
         저장된 씬 넘버 로딩         
         */

        // newtonsoft library (모노비헤이비어 상속된 클래스 사용 불가능, 딕셔너리 사용 가능)
        // 로드하는 함수 호출 후에 그 씬 인덱스로 이동
        FileStream fSceneBuildIndexStream 
            // 해당 경로에 있는 json 파일을 연다
            = new FileStream(Application.dataPath + "/Data/SceneBuildIndex.json", FileMode.Open);
        // 열려있는 json 값들을 byte배열에 넣는다
        byte[] bSceneData = new byte[fSceneBuildIndexStream.Length];
        // 끝까지 읽는다
        fSceneBuildIndexStream.Read(bSceneData, 0, bSceneData.Length);
        fSceneBuildIndexStream.Close();
        // 문자열로 변환한다
        string sSceneData = Encoding.UTF8.GetString(bSceneData);

        /*
         저장된 날짜 시간 계절 로딩         
         */

        // jsonUitlity (모노비헤이비어 상속된 클래스 사용 가능, 딕셔너리 사용 불가능)
        string mSeasonDatePath = Path.Combine(Application.dataPath + "/Data/", "SeasonDate.json");

        if (File.Exists(mSeasonDatePath)) // null check
        {
            // 새로운 오브젝트를 생성하고
            GameObject gSeasonDate = new GameObject();
            string sDateData = File.ReadAllText(mSeasonDatePath);

            Debug.Log(sDateData);
            
            // 데이터를 새로운 오브젝트에 덮어씌운다
            JsonUtility.FromJsonOverwrite(sDateData, gSeasonDate.AddComponent<SeasonDateCalc>());

            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!
            SeasonDateCalc.Instance.mSecond = gSeasonDate.GetComponent<SeasonDateCalc>().mSecond;
            SeasonDateCalc.Instance.mDay = gSeasonDate.GetComponent<SeasonDateCalc>().mDay;
            SeasonDateCalc.Instance.mSeason = gSeasonDate.GetComponent<SeasonDateCalc>().mSeason;
            SeasonDateCalc.Instance.mYear = gSeasonDate.GetComponent<SeasonDateCalc>().mYear;
        }
        
        // 문자열을 int형으로 파싱해서 빌드 인덱스로 활용한다
        SceneManager.LoadScene(int.Parse(sSceneData));
    }

    public void ActiveOption()
    {
        mSFx.Play();
        gOption.SetActive(true);
    }
    public void UnActiveOption()
    {
        mSFx.Play();
        gOption.SetActive(false);
    }

    // 게임 종료
    public void QuitGame()
    {
        mSFx.Play();
        Application.Quit();
    }

    public void GoCredit()
    {
        // 크레딧 화면으로 전환
        Debug.Log("크레딧화면으로 전환");
    }
    // 새로하기 경고창
    public void ActiveWarning()
    {
        gWarning.SetActive(true);
    }
    public void UnAcitveWarning()
    {
        gWarning.SetActive(false);
    }
}

