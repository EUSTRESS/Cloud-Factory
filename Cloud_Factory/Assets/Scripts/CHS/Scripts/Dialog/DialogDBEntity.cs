using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogDBEntity
{
    // 해당 틀에 맞춰서 엑셀파일을 채운다.
    public int      GuestID;            // 손님 번호
    public int      Sat;                // 손님 만족도
    public int      SatVariation;       // 손님 만족도 증감도
    public int      DialogIndex;        // 대화 순서
    public int      DialogImageNumber;  // 해당 대화에 맞는 표정 
    public string   Text;               // 대화 내용
    public int      isGuest;            // 손님/플레이어 중 누가 말하는지 (0/1 로 구분)
    public int      VisitCount;         // 손님의 현재 방문 횟수
    public int      Emotion;            // 손님이 표현하고 싶은 감정         
}
