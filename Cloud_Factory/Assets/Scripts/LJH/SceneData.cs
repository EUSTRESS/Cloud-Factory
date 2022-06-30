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
}

//// 직렬화 하기 위해서
//[System.Serializable]
//// 날짜 계절 데이터를 저장하는 박스
//public class SeasonDateDataBox
//{
//    // SceneData 인스턴스를 담는 전역 변수
//    private static SeasonDateDataBox instance = null;
//    public static SeasonDateDataBox Instance
//    {
//        get
//        {
//            if (null == instance) return null;

//            return instance;
//        }
//    }

//    public float mSecond
//    {
//        get { return mSecond; }
//        set { mSecond = value; }
//    }
//    public int mDay
//    {
//        get { return mDay; }
//        set { mDay = value; }
//    }

//    public int mWeek
//    {
//        get { return mWeek; }
//        set { mWeek = value; }
//    }
//    public int mSeason
//    {
//        get { return mSeason; }
//        set { mSeason = value; }
//    }
//    public int mYear
//    {
//        get { return mYear; }
//        set { mYear = value; }
//    }

//    // 생성자
//    public SeasonDateDataBox()
//    {
//        // 초기화
//        mSecond = 0;
//        mDay = 1;
//        mWeek = 0;
//        mSeason = 1;
//        mYear = 1;
//    }
//}

