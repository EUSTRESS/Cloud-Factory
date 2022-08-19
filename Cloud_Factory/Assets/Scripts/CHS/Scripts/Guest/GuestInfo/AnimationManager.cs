using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AnimationManager : MonoBehaviour
{
    // �������� �ȴ� �ִϸ��̼� Ŭ�� ����
    [SerializeField] private AnimationClip[] leftWalkAniClip;

    // ���������� �ȴ� �ִϸ��̼� Ŭ�� ����
    [SerializeField] private AnimationClip[] rightWalkAniClip;

    // ���ִ� �ִϸ��̼� Ŭ�� ����
    [SerializeField] private AnimationClip[] standAniClip;

    // �ɾ��ִ� �ִϸ��̼� Ŭ�� ����
    [SerializeField] private AnimationClip[] sitAniClip;

    // ���� ǥ��(ǥ��) �ִϸ��̼� Ŭ�� ����
    [SerializeField] private AnimationClip[] expressionAniClip;

    // ���� ǥ��(�Ӹ� �� ����Ʈ) �ִϸ��̼� Ŭ�� ����
    [SerializeField] private AnimationClip[] effectAniClip;

    // ���ư���(�ູ) �ִϸ��̼� Ŭ�� ����
    [SerializeField] private AnimationClip[] backHappyAniClip;

    // ���ư���(���) �ִϸ��̼� Ŭ�� ����
    [SerializeField] private AnimationClip[] backSadAniClip;

    // ������ 5 �޼� �ִϸ��̼� Ŭ�� ����
    [SerializeField] private AnimationClip[] satAniClip;

    // �Ҹ� ��Ƽ �ִϸ��̼� Ŭ�� ����
    [SerializeField] private AnimationClip[] disSatAniClip;

    // ��Ÿ ���
}
