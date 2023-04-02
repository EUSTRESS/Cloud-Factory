using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 씬에서 사용되는 임시 데이터 저장
// 씬 이동 시 들고가야할 데이터
// 예) 볼륨 크기, 씬 인덱스
public class SceneData : MonoBehaviour
{
    // SceneData 인스턴스를 담는 전역 변수
    private static SceneData instance = null;
    public  static SceneData Instance
    {
        get
        {
            if (null == instance) return null;

            return instance;
        }
    }

    [SerializeField]
    private int mCurrentSceneIndex;    // 현재 씬 인덱스
    public int currentSceneIndex
    {
        get { return mCurrentSceneIndex; }
        set { mCurrentSceneIndex = value; }
    }

    [SerializeField]
    private int     mPrevSceneIndex;    // 이전 씬 인덱스
    public  int     prevSceneIndex
    {
        get { return mPrevSceneIndex; }
        set { mPrevSceneIndex = value; }
    }

    private float   mBGMValue;          // BGM 소리 크기 
    public  float   BGMValue
    {
        get { return mBGMValue; }
        set { mBGMValue = value; }
    }
    private float   mSFxValue;          // SFx 소리 크기 
    public  float   SFxValue
    {
        get { return mSFxValue; }
        set { mSFxValue = value; }
    }

    public bool mContinueGmae = false;

    void Awake()
    {
        // 인스턴스 할당
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // 이미 존재하면 이전부터 사용하던 것을 사용함
            Destroy(this.gameObject);
        }

        // 초기값 할당
        mBGMValue = 0.5f;
        mSFxValue = 1.0f;
    }

    void Start()
    {
        
    }
}