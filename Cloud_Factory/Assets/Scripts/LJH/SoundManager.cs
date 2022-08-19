using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �� ������ BGM�� �ν����Ϳ��� �Ҵ�
// �����̴��� ���� ����� �� ���� ���� ���� �� ����
public class SoundManager : MonoBehaviour
{
    // BGM, ȿ���� ����� �ҽ�
    private AudioSource mBGM;
    private AudioSource mSFx;

    void Awake()
    {        
        // ����� �ҽ� ã��
        mBGM = GameObject.Find("BGM").GetComponent<AudioSource>();
        mSFx = GameObject.Find("SFx").GetComponent<AudioSource>();
    }
    // ����Ƽ �����ֱ� Ȱ��
    void Start()
    {
        if (SceneData.Instance) // null check
        {
            // ���� ����� �� ����� ������ ���� ������Ʈ
            mBGM.volume = SceneData.Instance.BGMValue;
            mSFx.volume = SceneData.Instance.SFxValue;
        }
    }

    // BGM ���� ����
    public void SetBGMVolume(float volume)
    {        
        mBGM.volume = volume;
        // ���� ������ �� ���� ����
        SceneData.Instance.BGMValue = volume;
    }

    // SFx ���� ����
    public void SetSFxVolume(float volume)
    {
        mSFx.volume = volume;
        // ���� ������ �� ���� ����
        SceneData.Instance.SFxValue = volume;
    }
}
