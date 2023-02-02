using UnityEngine;

[System.Serializable]
public struct SSatEmotion
{
    public int emotionNum;                                      // 감정 번호
    public int up;                                              // 만족도 범위 상한선
    public int down;                                            // 만족도 범위 하한선
}

[System.Serializable]
public struct SLimitEmotion
{
    public int upLimitEmotion;                                  // 손님의 감정 상한선
    public int upLimitEmotionValue;                             // 손님의 감정 상한선 값

    public int downLimitEmotion;                                // 손님의 감정 하한선
    public int downLimitEmotionValue;                           // 손님의 감정 하한선 값
}


[CreateAssetMenu(menuName = "Scriptable/Guest_info", fileName = "Guest Info")]
// 손님의 초기 정보값들을 ScriptableObject형식으로 가지고 있는다.
public class GuestInfo : ScriptableObject
{
    [Header(" [손님 정보] ")]
    public string mName;                                            // 손님의 이름
    public int mSeed;                                               // 손님이 심고 갈 수 있는 재료의 인덱스 값
    public int  mAge;                                               // 손님의 나이
    public string  mJob;                                            // 손님의 직업

    [Header ("[감정 관련]")]
    public int[] mEmotion = new int[20];                            // 손님의 감정 값. 총 20가지
    public SSatEmotion[] mSatEmotions = new SSatEmotion[5];         // 손님의 만족도 범위 5종류
    public SLimitEmotion[] mLimitEmotions = new SLimitEmotion[2];   // 손님의 상하한선 감정 각각 2종류

    [Header("[만족도 관련]")]
    public int mSatatisfaction;                                     // 손님의 만족도
    public int mSatVariation;                                       // 손님의 만족도 증감 정도
    public bool isDisSat;                                           // 불만 뭉티인지 확인
    public bool isCure;                                             // 손님이 만족도 5를 채워 모두 치유하였는지 확인 

    [Header("[방문 횟수 관련]")]
    public int  mVisitCount;                                        // 남은 방문 횟수
    public int  mNotVisitCount;                                     // 방문하지 않는 횟수

    [Header("[날씨의 공간 배치 관련]")]
    public bool isChosen;                                           // 선택되었는지 확인하는 변수
    public int   mSitChairIndex;                                    // 손님이 현재 받은 의자 인덱스
    public bool isUsing = false;                                    // 구름을 제공받았는지

    [Header("[기타]")]
    public int[] mUsedCloud = new int[10];                          // 사용한 구름 리스트를 저장한다. 최대 10개
}


// GuestManager에서 관리하는 손님 정보값들
[System.Serializable]
public class GuestInfos
{
    [Header(" [손님 정보] ")]
    public string mName;                                            // 손님의 이름
    public int mSeed;                                             // 손님이 심고 갈 수 있는 재료의 인덱스 값
    public int mAge;                                                // 손님의 나이
    public string mJob;                                             // 손님의 직업

    [Header("[감정 관련]")]
    public int[] mEmotion = new int[20];                            // 손님의 감정 값. 총 20가지
    public SSatEmotion[] mSatEmotions = new SSatEmotion[5];         // 손님의 만족도 범위 5종류
    public SLimitEmotion[] mLimitEmotions = new SLimitEmotion[2];   // 손님의 상하한선 감정 각각 2종류

    [Header("[만족도 관련]")]
    public int mSatatisfaction;                                     // 손님의 만족도
	public int mSatVariation;                                       // 손님의 만족도 증감 정도
	public bool isDisSat;                                           // 불만 뭉티인지 확인
    public bool isCure;                                             // 손님이 만족도 5를 채워 모두 치유하였는지 확인 

    [Header("[방문 횟수 관련]")]
    public int mVisitCount;                                         // 남은 방문 횟수
    public int mNotVisitCount;                                      // 방문하지 않는 횟수

    [Header("[날씨의 공간 배치 관련]")]
    public bool isChosen;                                           // 선택되었는지 확인하는 변수
    public int mSitChairIndex;                                      // 손님이 현재 받은 의자 인덱스
    public bool isUsing = false;                                    // 구름을 제공받았는지

    [Header("[기타]")]
    public int[] mUsedCloud = new int[10];                          // 사용한 구름 리스트를 저장한다. 최대 10개
}

