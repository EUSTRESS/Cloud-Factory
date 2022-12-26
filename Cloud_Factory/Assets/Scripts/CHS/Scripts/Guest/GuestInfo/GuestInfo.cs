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

public class GuestInfo : ScriptableObject
{
    public string mName;                                            // 손님의 이름
    public int[] mSeed;                                             // 손님이 심고 갈 수 있는 재료의 인덱스 값
    public int[] mEmotion = new int[20];                            // 손님의 감정 값. 총 20가지
    public int  mAge;
    public string  mJob;

    public int mSatatisfaction;                                     // 손님의 만족도
    public Sprite[] sImg;                                           // 손님의 이미지 -> 날씨의 공간에서의 상태     

    public SSatEmotion[] mSatEmotions = new SSatEmotion[5];         // 손님의 만족도 범위 5종류
    public SLimitEmotion[] mLimitEmotions = new SLimitEmotion[2];

    public bool isDisSat;                                           // 불만 뭉티인지 확인
    public bool isCure;                                             // 손님이 만족도 5를 채워 모두 치유하였는지 확인 
    public int  mVisitCount;                                        // 남은 방문 횟수
    public int  mNotVisitCount;                                     // 방문하지 않는 횟수
    public bool isChosen;                                           // 선택되었는지 확인하는 변수

    public int[] mUsedCloud = new int[10];                          // 사용한 구름 리스트를 저장한다. 최대 10개

    public int   mSitChairIndex;                                    // 손님이 현재 받은 의자 인덱스
    public bool isUsing = false;
}

