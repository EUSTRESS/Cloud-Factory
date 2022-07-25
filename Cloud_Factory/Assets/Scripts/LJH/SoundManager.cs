using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 각 씬별로 BGM은 인스펙터에서 할당
// 슬라이더의 값이 변경될 때 마다 볼륨 조절 및 저장
public class SoundManager : MonoBehaviour
{
    // BGM, 효과음 오디오 소스
    private AudioSource mBGM;
    private AudioSource mSFx;

    void Awake()
    {        
        // 오디오 소스 찾기
        mBGM = GameObject.Find("BGM").GetComponent<AudioSource>();
        mSFx = GameObject.Find("SFx").GetComponent<AudioSource>();
    }
    // 유니티 생명주기 활용
    void Start()
    {
        if (SceneData.Instance) // null check
        {
            // 씬이 변경될 때 저장된 값으로 새로 업데이트
            mBGM.volume = SceneData.Instance.BGMValue;
            mSFx.volume = SceneData.Instance.SFxValue;
        }
    }

    // BGM 볼륨 조정
    public void SetBGMVolume(float volume)
    {        
        mBGM.volume = volume;
        // 값을 변경할 때 마다 저장
        SceneData.Instance.BGMValue = volume;
    }

    // SFx 볼륨 조정
    public void SetSFxVolume(float volume)
    {
        mSFx.volume = volume;
        // 값을 변경할 때 마다 저장
        SceneData.Instance.SFxValue = volume;
    }
}
