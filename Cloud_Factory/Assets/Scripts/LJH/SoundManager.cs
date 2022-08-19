using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �� ������ BGM�� �ν����Ϳ��� �Ҵ�
// �����̴��� ���� ����� �� ���� ���� ���� �� ����
public class SoundManager : MonoBehaviour
{   
    private SeasonDateCalc mSeason; // ����

    // BGM, ȿ���� ����� �ҽ�
    private AudioSource mBGM;
    private AudioSource mSFx;

    public AudioClip[] mSeasonBGMArray = new AudioClip[4]; // 4�������� �޶����� BGM
    public AudioClip[] mBGMArray = new AudioClip[2]; // [0] �׸� [1] ������
    public bool[] isOneTime = new bool[4]; // �뷡 �ѹ��� Ʋ��
    void Awake()
    {        
        // ����� �ҽ� ã��
        mBGM = GameObject.Find("mBGM").GetComponent<AudioSource>();
        mSFx = GameObject.Find("mSFx").GetComponent<AudioSource>();
                
        mSeason = GameObject.Find("Season Date Calc").GetComponent<SeasonDateCalc>();
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

        if ((SceneManager.GetActiveScene().name == "Lobby"))
        {
            mBGM.clip = mBGMArray[0];
            mBGM.Play();
        }
        else if (SceneManager.GetActiveScene().name == "Drawing Room" )
        {
            mBGM.clip = mBGMArray[1];
            mBGM.Play();
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Space Of Weather")
        { // ������ ���� ���� ���� �Ұ����� �ߺ� �÷��� ����
            switch (mSeason.mSeason)
            {
                case 1:
                    if (!isOneTime[0])
                    {
                        mBGM.clip = mSeasonBGMArray[0];
                        mBGM.Play();
                        isOneTime[0] = true;
                    }                    
                    break;
                case 2:
                    if (!isOneTime[1])
                    {
                        mBGM.clip = mSeasonBGMArray[1];
                        mBGM.Play();
                        isOneTime[1] = true;
                    }
                    break;
                case 3:
                    if (!isOneTime[2])
                    {
                        mBGM.clip = mSeasonBGMArray[2];
                        mBGM.Play();
                        isOneTime[2] = true;
                    }
                    break;
                case 4:
                    if (!isOneTime[3])
                    {
                        mBGM.clip = mSeasonBGMArray[3];
                        mBGM.Play();
                        isOneTime[3] = true;
                    }
                    break;
                default:
                    break;
            }           
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
