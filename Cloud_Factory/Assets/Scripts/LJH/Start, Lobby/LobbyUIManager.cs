using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("Space Of Weather");
        mSFx.Play();
    }
    public void ContinueGame()
    {
        // json 데이터 저장 파일 불러와서 씬 이름 할당하기
        // SceneManager.LoadScene("");
        mSFx.Play();
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

