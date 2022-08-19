using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestStat : MonoBehaviour
{
    public string mName;                                            // �մ��� �̸�
    public int[] mSeed;                                             // �մ��� �ɰ� �� �� �ִ� ����� �ε��� ��
    public int[] mEmotion = new int[20];                            // �մ��� ���� ��. �� 20����

    public int mSatatisfaction;                                     // �մ��� ������
    public EState EState;                                           // �մ��� ���� �ൿ ����
    public Sprite[] sImg;                                           // �մ��� �̹��� -> ������ ���������� ����     

    public SSatEmotion[] mSatEmotions = new SSatEmotion[5];         // �մ��� ������ ���� 5����
    public SLimitEmotion[] mLimitEmotions = new SLimitEmotion[2];

    public bool isDisSat;                                           // �Ҹ� ��Ƽ���� Ȯ��
    public bool isCure;                                             // �մ��� ������ 5�� ä�� ��� ġ���Ͽ����� Ȯ�� 
    public int mVisitCount;                                         // ���� �湮 Ƚ��
    public int mNotVisitCount;                                      // �湮���� �ʴ� Ƚ��
    public bool isChosen;                                           // ���õǾ����� Ȯ���ϴ� ����

    public int[] mUsedCloud = new int[10];                          // ����� ���� ����Ʈ�� �����Ѵ�. �ִ� 10��
}
