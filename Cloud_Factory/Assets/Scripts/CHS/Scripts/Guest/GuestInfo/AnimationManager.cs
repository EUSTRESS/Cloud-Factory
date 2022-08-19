using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AnimationManager : MonoBehaviour
{
    // 왼쪽으로 걷는 애니메이션 클립 모음
    [SerializeField] private AnimationClip[] leftWalkAniClip;

    // 오른쪽으로 걷는 애니메이션 클립 모음
    [SerializeField] private AnimationClip[] rightWalkAniClip;

    // 서있는 애니메이션 클립 모음
    [SerializeField] private AnimationClip[] standAniClip;

    // 앉아있는 애니메이션 클립 모음
    [SerializeField] private AnimationClip[] sitAniClip;

    // 감정 표현(표정) 애니메이션 클립 모음
    [SerializeField] private AnimationClip[] expressionAniClip;

    // 감정 표현(머리 위 이펙트) 애니메이션 클립 모음
    [SerializeField] private AnimationClip[] effectAniClip;

    // 돌아가기(행복) 애니메이션 클립 모음
    [SerializeField] private AnimationClip[] backHappyAniClip;

    // 돌아가기(우울) 애니메이션 클립 모음
    [SerializeField] private AnimationClip[] backSadAniClip;

    // 만족도 5 달성 애니메이션 클립 모음
    [SerializeField] private AnimationClip[] satAniClip;

    // 불만 뭉티 애니메이션 클립 모음
    [SerializeField] private AnimationClip[] disSatAniClip;

    // 기타 등등
}
