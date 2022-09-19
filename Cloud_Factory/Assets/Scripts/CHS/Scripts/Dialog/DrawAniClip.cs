using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAniClip : MonoBehaviour
{
    const int MAX_GUEST_NUM = 20;

    // 각 손님의 번호에 따라 애니메이터를 만들어서 저장한다.
    public RuntimeAnimatorController[] animators = new RuntimeAnimatorController[MAX_GUEST_NUM];

    
}
