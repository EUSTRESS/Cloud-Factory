using UnityEngine;

// ���������� �մ��� ���� �ൿ ���¸� ǥ�� ( �� 4����)
public enum EState
{
    dialog, use, wait, none // ������� (��ȭ��, ���� �̿� ��, ���� �̿� �����, ���� ��)
}

[System.Serializable]
public struct SSatEmotion
{
    public int emotionNum;                                      // ���� ��ȣ
    public int up;                                              // ������ ���� ���Ѽ�
    public int down;                                            // ������ ���� ���Ѽ�
}

[System.Serializable]
public struct SLimitEmotion
{
    public int upLimitEmotion;                                  // �մ��� ���� ���Ѽ�
    public int upLimitEmotionValue;                             // �մ��� ���� ���Ѽ� ��

    public int downLimitEmotion;                                // �մ��� ���� ���Ѽ�
    public int downLimitEmotionValue;                           // �մ��� ���� ���Ѽ� ��
}

[ CreateAssetMenu(menuName = "Scriptable/Guest_info", fileName = "Guest Info")]

public class GuestInfo : ScriptableObject
{
    public string       mName;                                      // �մ��� �̸�
    public int[]        mSeed;                                      // �մ��� �ɰ� �� �� �ִ� ����� �ε��� ��
    public int[]        mEmotion = new int[20];                     // �մ��� ���� ��. �� 20����

    public int          mSatatisfaction;                            // �մ��� ������
    public EState       EState;                                     // �մ��� ���� �ൿ ����
    public Sprite[]     sImg;                                       // �մ��� �̹��� -> ������ ���������� ����     

    public SSatEmotion[]   mSatEmotions = new SSatEmotion[5];       // �մ��� ������ ���� 5����
    public SLimitEmotion[] mLimitEmotions = new SLimitEmotion[2]; 

    public bool         isDisSat;                                   // �Ҹ� ��Ƽ���� Ȯ��
    public bool         isCure;                                     // �մ��� ������ 5�� ä�� ��� ġ���Ͽ����� Ȯ�� 
    public int          mVisitCount;                                // ���� �湮 Ƚ��
    public int          mNotVisitCount;                             // �湮���� �ʴ� Ƚ��
    public bool         isChosen;                                   // ���õǾ����� Ȯ���ϴ� ����

    public int[]        mUsedCloud = new int[10];                   // ����� ���� ����Ʈ�� �����Ѵ�. �ִ� 10��
}

// ����
// ������ �⺻ ����(4) + ���� ����(4) + ���� ����(8) + �ݴ� ����(4) �� 20������ �̷���� �ִ�.

// ���� ��ġ ����
// ��Ƽ�� ��Ƽ�� ��ġ ���°� �Ǳ� ���� �����ؾ� �ϴ� Ư�� 5���� �������� ���� �����̴�.
// ������ ������ ��ġ�� ���� ��ġ ���� ���� ��ġ�ϰ� �Ǹ� �������� �ö󰣴�.
// �������� 5�� �Ǹ� �ش� ��Ƽ�� ��ġ ���°� �ȴ�. (��� ���� ��ġ�� ���� ������ ��ġ)

// Ư¡
// �մԸ����� Ư�� 5���� ������ �����Ǿ�����.
// �մԸ��� ���� �ΰ��� ���� ���� ������ ���� ���� ������ �����Ѵ�. (�մ� 3.3.4 ����)
// ������ ������ ��� ��Ƽ�� �������� 0�� �Ǹ�, ������ ���ư� ��湮 X (�Ҹ� ��Ƽ)   

// �մԵ鸶�� ������ ��ũ��Ʈ ������Ʈ�� �����Ѵ�.
// ������ ������ �ʿ��� ������ �����Ͽ� ����ϰ� �����Ѵ�.

// �ϳ��� ��Ƽ�� �湮�� �� �ִ� Ƚ���� 10ȸ�̴�.
// �Ϸ翡 ��Ƽ�� �ִ� 5���� �湮�Ѵ�. 

// �ִ� �湮Ƚ���� �ʰ����� ���� ��Ƽ �߿��� 5������ �������� �̴´�.
// �������� ���� ��Ƽ�߿� �Ҹ���Ƽ�� �����Ѵٸ� �ٽ� ���� �ʰ� �ش� ��Ƽ�� �����ϰ� �湮��Ų��.
// ��ġ�� ��Ƽ���� ���̻� Cloud Factory�� �湮���� �ʴ´�.

// ��� 
