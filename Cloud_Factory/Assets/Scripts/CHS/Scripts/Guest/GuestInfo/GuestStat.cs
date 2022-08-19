using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestStat : MonoBehaviour
{
    public string mName;                                            // 손님의 이름
    public int[] mSeed;                                             // 손님이 심고 갈 수 있는 재료의 인덱스 값
    public int[] mEmotion = new int[20];                            // 손님의 감정 값. 총 20가지

    public int mSatatisfaction;                                     // 손님의 만족도
    public EState EState;                                           // 손님의 현재 행동 상태
    public Sprite[] sImg;                                           // 손님의 이미지 -> 날씨의 공간에서의 상태     

    public SSatEmotion[] mSatEmotions = new SSatEmotion[5];         // 손님의 만족도 범위 5종류
    public SLimitEmotion[] mLimitEmotions = new SLimitEmotion[2];

    public bool isDisSat;                                           // 불만 뭉티인지 확인
    public bool isCure;                                             // 손님이 만족도 5를 채워 모두 치유하였는지 확인 
    public int mVisitCount;                                         // 남은 방문 횟수
    public int mNotVisitCount;                                      // 방문하지 않는 횟수
    public bool isChosen;                                           // 선택되었는지 확인하는 변수

    public int[] mUsedCloud = new int[10];                          // 사용한 구름 리스트를 저장한다. 최대 10개
}
