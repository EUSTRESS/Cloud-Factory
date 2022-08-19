using UnityEngine;

// 열거형으로 손님의 현재 행동 상태를 표현 ( 총 4가지)
public enum EState
{
    dialog, use, wait, none // 순서대로 (대화중, 구름 이용 중, 구름 이용 대기중, 가게 밖)
}

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

[ CreateAssetMenu(menuName = "Scriptable/Guest_info", fileName = "Guest Info")]

public class GuestInfo : ScriptableObject
{
    public string       mName;                                      // 손님의 이름
    public int[]        mSeed;                                      // 손님이 심고 갈 수 있는 재료의 인덱스 값
    public int[]        mEmotion = new int[20];                     // 손님의 감정 값. 총 20가지

    public int          mSatatisfaction;                            // 손님의 만족도
    public EState       EState;                                     // 손님의 현재 행동 상태
    public Sprite[]     sImg;                                       // 손님의 이미지 -> 날씨의 공간에서의 상태     

    public SSatEmotion[]   mSatEmotions = new SSatEmotion[5];       // 손님의 만족도 범위 5종류
    public SLimitEmotion[] mLimitEmotions = new SLimitEmotion[2]; 

    public bool         isDisSat;                                   // 불만 뭉티인지 확인
    public bool         isCure;                                     // 손님이 만족도 5를 채워 모두 치유하였는지 확인 
    public int          mVisitCount;                                // 남은 방문 횟수
    public int          mNotVisitCount;                             // 방문하지 않는 횟수
    public bool         isChosen;                                   // 선택되었는지 확인하는 변수

    public int[]        mUsedCloud = new int[10];                   // 사용한 구름 리스트를 저장한다. 최대 10개
}

// 감정
// 감정은 기본 감정(4) + 조합 감정(4) + 최종 감정(8) + 반대 감정(4) 총 20가지로 이루어져 있다.

// 만족 수치 범위
// 뭉티가 뭉티가 완치 상태가 되기 위해 조절해야 하는 특정 5가지 감정들의 적정 범위이다.
// 정해진 감정의 수치가 만족 수치 범위 내에 위치하게 되면 만족도가 올라간다.
// 만족도가 5가 되면 해당 뭉티는 완치 상태가 된다. (모든 감정 수치가 만족 범위에 위치)

// 특징
// 손님마다의 특정 5가지 감정이 설정되어있음.
// 손님마다 각각 두개의 감정 상한 범위와 감정 하한 범위가 존재한다. (손님 3.3.4 참고)
// 상하한 범위를 벗어난 뭉티는 만족도가 0이 되며, 집으로 돌아가 재방문 X (불만 뭉티)   

// 손님들마다 정보를 스크립트 오브젝트로 저장한다.
// 저장한 값들을 필요할 때마다 접근하여 사용하고 변경한다.

// 하나의 뭉티가 방문할 수 있는 횟수는 10회이다.
// 하루에 뭉티는 최대 5명이 방문한다. 

// 최대 방문횟수를 초과하지 않은 뭉티 중에서 5마리를 랜덤으로 뽑는다.
// 랜덤으로 뽑힌 뭉티중에 불만뭉티가 존재한다면 다시 뽑지 않고 해당 뭉티를 제외하고 방문시킨다.
// 완치된 뭉티들은 더이상 Cloud Factory를 방문하지 않는다.

// 결론 
