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
    public GameObject   gOption; // 옵션 게임 오브젝트

    [Header("TEXT")]
    public Text     tBgmValue;   // BGM 볼륨 텍스트
    public Text     tSfxValue;   // SFx 볼륨 텍스트

    [Header("SLIDER")]
    public Slider   sBGM;        // BGM 슬라이더
    public Slider   sSFx;        // SFx 슬라이더

    private AudioSource mSFx;    // 효과음 오디오 소스

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

        // 데이터를 초기화 시키는 함수 호출하기.
        // 현재는 씬 이동만 저장하기 때문에 그냥 날씨의 공간으로 이동하면 된다.
    }
    public void ContinueGame()
    {
        mSFx.Play();

        // 로드하는 함수 호출 후에 그 씬 인덱스로 이동
        FileStream fSceneBuildIndexStream 
            // 해당 경로에 있는 json 파일을 연다
            = new FileStream(Application.dataPath + "/Data/SceneBuildIndex.json", FileMode.Open);
        // 열려있는 json 값들을 byte배열에 넣는다
        byte[] bData = new byte[fSceneBuildIndexStream.Length];
        // 끝까지 읽는다
        fSceneBuildIndexStream.Read(bData, 0, bData.Length);
        fSceneBuildIndexStream.Close();
        // 문자열로 변환한다
        string sData = Encoding.UTF8.GetString(bData);

        // 문자열을 int형으로 파싱해서 빌드 인덱스로 활용한다
        SceneManager.LoadScene(int.Parse(sData));
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
    }
}

