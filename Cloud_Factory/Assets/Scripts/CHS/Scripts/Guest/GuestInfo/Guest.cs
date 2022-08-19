using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guest : MonoBehaviour
{
    public GuestInfo[]      mGuestInfos;                        // Scriptable Objects���� ������ ��� �ִ� �迭

    public float            mGuestTime;                         // ��Ƽ�� �湮 �ֱ�

    public int              mGuestIndex;                        // �̹��� �湮�� ��Ƽ�� ��ȣ

    [SerializeField]
    private int[]           mTodayGuestList = new int[6];       // ���� �湮 ������ ��Ƽ ���
    [SerializeField]
    public bool             isGuestInLivingRoom;                // �����ǿ� �մ��� �湮���ִ°�?

    public bool             isTimeToTakeGuest;                  // ��Ƽ �湮�ֱⰡ �������� Ȯ��
    [SerializeField]
    private int              mGuestCount;                       // �̹��� �湮�� ��Ƽ�� ����
    [SerializeField]
    private int              mGuestMax;                         // ���� �湮�ϴ� ��Ƽ�� �ִ� ����

    private static Guest    instance = null;                    // �̱��� ����� ���� instance ����

    private void Awake()
    {
        // �̱��� ��� ���
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            mGuestTime = 0;
            mGuestCount = -1;
            mGuestMax = 0;

            InitDay();

            isTimeToTakeGuest = false;
            isGuestInLivingRoom = false;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ��Ƽ�� �湮�ֱ⸦ ������.
        if (mGuestTime < 5.0f)
        {
            mGuestTime += Time.deltaTime;
        }
        else if(mGuestTime >= 5.0f && isTimeToTakeGuest == false)
        {
            // ��� �ε����� �� ���� �ʴ� �� ��Ƽ �湮�ֱⰡ �ٵȰ�� ���ο� ��Ƽ�� �鿩������.
            if (mGuestCount < mGuestMax - 1) // 0 1 2 3 4 5 
            {
                Debug.Log("��Ƽ �湮�ð��� �Ǿ����ϴ�");
                isTimeToTakeGuest = true;
                TakeGuest();
                // ������ �̵��ϴ� ��ư�鿡 ���� ��ȣ�ۿ�
            }
            else
            {
                Debug.Log("��� ��Ƽ�� �湮�Ͽ����ϴ�.");
            }
        }

        // ������ ���� �ް� �̿��� ��ģ ��쿡�� �ش� ��Ƽ�� ������ ��ȭ���� �ο���


        // �̱��� ��� Ȯ���� ���� �׽�Ʈ�ڵ�
        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeGuest();
        }
        // ������ ��ȯ�� ���� �Լ� �׽�Ʈ (����)
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetEmotion(0, 0, 1, 5, 10);
        }
        // ������ ������ ���� �Լ� �׽�Ʈ (����)
        if (Input.GetKeyDown(KeyCode.C))
        {
            RenewakSat(0);
        }
        // �����Ѽ� ħ�� Ȯ���� ���� �Լ� �׽�Ʈ (����)
        if (Input.GetKeyDown(KeyCode.D))
        {
            mGuestInfos[0].isDisSat = CheckIsDisSat(0);
            Debug.Log(mGuestInfos[0].isDisSat);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            InitGuestTime();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            NewChoiceGuest();
            for (int i = 0; i < 6; i++)
            {
                Debug.Log(mTodayGuestList[i] + "�� ��Ƽ�� �߰��Ǿ����ϴ�.");
            }
        }
    }

    public void TakeGuest()
    {
        if (isTimeToTakeGuest == true && isGuestInLivingRoom == false)
        {
            mGuestCount++;
            Debug.Log("mGuestCount�� �����մϴ�.");
            mGuestIndex = mTodayGuestList[mGuestCount];
            isGuestInLivingRoom = true;
        }
    }

    // ��Ƽ�� ���������� �޾ƿ��� API
    public string GetName(int gusetNum) 
    { 
        return mGuestInfos[gusetNum].mName; 
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    // ���� ���� ���� (������ ȭ��󿡼� ��Ƽ���� �����Ͽ����� �۾��� �ش� �������� ����)
    // 1. ������ �����Ͽ� ��Ƽ���� ���� (��Ƽ�� �ɾ��ִ� ���°� �ƴ϶�� ���� �Ұ���) 
    // 2. ������ �̿�ð���ŭ�� ��� (��⵵�� ���� �ٲ�� ���� ����)
    // 3. ������ ��������ŭ�� ��Ƽ�� ������ ���ϱ� - �Լ� ����
    // 4. ������ �������� ��Ƽ�� ���������� Ȯ��. (�������� ���� �����Ѽ� ħ�� ����) - �Լ� ����
    // 5. ���� ���� �����Ѽ��� ħ������ ��� ��Ƽ�� �Ҹ� ��Ƽ�� ���� (�Ҹ� ��Ƽ�� ���� ������ ��ũ��Ʈ �߰� �ۼ�) - �Լ� ����
    // 6. ���� �������� ����Ǿ��� �ÿ� ������ �� ���� (�ش� ��Ƽ�� ��ǥ���� ����) - �Լ� ����

    // 7. �������� �ö��� ��� ���翡 �Ѹ� �� �ִ� ����(���)�� ���õ� ���� �޾Ƽ� �ɱ�
    // 8. ���� ������ ���� ����� ȭ�鿡 ����ְ� ��Ƽ�� ������ �������� ��������
    //------------------------------------------------------------------------------------------------------------------------------------------
    
    public bool CheckIsDisSat(int guestNum)
    {
        int temp = IsExcessLine(guestNum);                      // ħ���ϴ� ��쿡 �������� ���Ƿ� ������ ����

        // ������ ���� ħ���� ��츦 Ȯ��
        if (temp != -1) 
        {
            mGuestInfos[guestNum].isDisSat = true;              // �Ҹ� ��Ƽ�� ��ȯ
            mGuestInfos[guestNum].mSatatisfaction = 0;          // ������ 0 ���� ����
            mGuestInfos[guestNum].mVisitCount = 0;              // ���� �湮Ƚ�� 0���� ����
            
            // ġ���� ������� �Ҹ� ��Ƽ�� �� ���¿� �մ� ��ȣ, � ���� ��ȭ�� ���� ������ �������ֱ�


            return true;
        }
        return false;
    }

    // ��Ƽ�� ������ ���濡 �ʿ��� API 
    // Event Handler�� �̿��Ͽ� ������ �����ȿ� ���� ���ϰų� ���� �����Ѽ��� ħ���Ͽ� �Ҹ� ��Ƽ�� �Ǵ°�� �̺�Ʈ�� �ߵ����� ����
    public void SetEmotion(int guestNum, int emotionNum0, int emotionNum1, int value0, int value1) 
    { 
        mGuestInfos[guestNum].mEmotion[emotionNum0] += value0; 
        mGuestInfos[guestNum].mEmotion[emotionNum1] += value1; 
    }

    public int IsExcessLine(int guestNum) // ���� �����Ѽ��� ħ���ߴ��� Ȯ���ϴ� �Լ�. -> ���� ���� ���� 4������ ����
    {

        SLimitEmotion[] limitEmotion = mGuestInfos[guestNum].mLimitEmotions;

        for (int i = 0; i < 2; i++)
        {
            if (mGuestInfos[guestNum].mEmotion[limitEmotion[i].upLimitEmotion] >= limitEmotion[i].upLimitEmotionValue) // �����Ѽ��� ħ���� ���
            {
                Debug.Log("�����Ѽ��� ħ���Ͽ����ϴ�");
                return limitEmotion[i].upLimitEmotion;
            }
            else if (mGuestInfos[guestNum].mEmotion[limitEmotion[i].downLimitEmotion] <= limitEmotion[i].downLimitEmotionValue)
            {
                Debug.Log("�����Ѽ��� ħ���Ͽ����ϴ�");
                return limitEmotion[i].downLimitEmotion;
            }
        }

        // �����Ѽ� ��� ħ������ �ʴ� ���
        Debug.Log("�����Ѽ��� ħ������ �ʾҽ��ϴ�");
        return -1;
    }

    public void RenewakSat(int guestNum)     // �������� �����ϴ� �Լ�. -> ���� ���� ���� 4������ ����
    {
        int temp = 0;

        for(int i = 0; i< 5; i++)
        {
            // ������ ���� ���� ������ Ȯ��
            if(mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum] <= mGuestInfos[guestNum].mSatEmotions[i].up &&
             mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum] >= mGuestInfos[guestNum].mSatEmotions[i].down)
            {
                temp++;
            }
        }
        mGuestInfos[guestNum].mSatatisfaction = temp;
        Debug.Log(temp);
    }

    // ���� �������� ���� ��Ƽ�� ������ ���ϴ� ��� ������ ������ �ʿ��ϴ�.
    // 1. ������ �����޾� �̿��ϴ� ���� ���� �ٲ�� ������ ���ư��� �ϴ� ��� ��ȿ
    // -> ���� �ð� (������ �����޾� �̿��ϴ� �ð�)���� ����ϴٰ� �ð��� ���� �� �������� �����ϴ� ��� ��� ����)
    //
    // 2. ������ �����޾� ��ȭ�� ������ ��Ƽ�� ���� ���Ѽ��� ���Ѽ��� ħ���ϸ� �ȵȴ�.
    // -> ������ �����ް� ���� ���� ���Ѽ��� ���Ѽ��� ħ�����ϴ� ��� �Ҹ� ��Ƽ�� ��ȯ�ȴ�.

    // �Ҹ� ��Ƽ�� �г�Ƽ
    // Cloud Factory �湮�� ����
    // ġ���� ��Ͽ� �Ҹ���Ƽ ǥ��
    // ������ 0���� ��ȯ

    public int CheckDisSat(int[] guestList, int Index)
    {
        int result = 0;

        for(int i = 0; i<= Index; i++)
        {
            if (mGuestInfos[i].isDisSat == true || mGuestInfos[i].mNotVisitCount != 0)
            {
                result++;
            }
        }

        return result;
    }
    // ��Ƽ ����Ʈ�� ���� �����ϴ� �Լ�
    public int[] NewChoiceGuest()
    {
        int[]       guestList           = new int[6];       // ��ȯ��ų ��Ƽ�� ����Ʈ
        int         possibleToTake      = 6;                // ���� �� �ִ� �� ��Ƽ�� ��

        int         totalGuestNum       = 20;               // �� ��Ƽ�� ��
        int         possibleGuestNum    = 0;                // �湮�� ������ ��Ƽ�� ��

        List<int>   VisitedGuestNum     = new List<int>();  // �湮 �̷��� �ִ� ��Ƽ�� ����Ʈ
        List<int>   NotVisitedGuestNum  = new List<int>();  // �湮 �̷��� ���� ��Ƽ�� ����Ʈ

        // �湮 Ƚ���� ���� ��Ƽ�� �������� 5�� �� ��Ƽ�� ���ܵǾ�� �ϹǷ� ���� ����Ʈ���� ������.
        for (int i = 0; i < totalGuestNum; i++)
        {
            if(mGuestInfos[i].mVisitCount != 10 && mGuestInfos[i].isCure == false)
            {
                if (mGuestInfos[i].mVisitCount == 0)
                {
                    NotVisitedGuestNum.Add(i);
                }
                else
                {
                    VisitedGuestNum.Add(i);
                }
            }
            if(mGuestInfos[i].isDisSat == false && mGuestInfos[i].mNotVisitCount == 0 && mGuestInfos[i].mVisitCount != 10 && mGuestInfos[i].isCure == false)
            {
                possibleGuestNum++;
            }
        }

        int     GuestIndex = 0;
        bool    isOverLap = true;

        // �湮 �̷��� �ִ� ��Ƽ�� 5�� �̻��� ���� ���
        // ��� �湮 �̷��� �ִ� ��Ƽ�� �̰� �������� �湮 �̷��� ���� ��Ƽ�� ä���.
        if (VisitedGuestNum.Count < possibleToTake-1)
        {
            Debug.Log("�湮 �̷��� �ִ� ��Ƽ�� 5���̻��� ���� �ʽ��ϴ�");
            for (int i = 0; i < VisitedGuestNum.Count; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                    }
                    int rejectCount = 0;
                    // �Ҹ� ��Ƽ�̰ų� �湮 �Ұ� ���� ��Ƽ�� ���� ���ؾ� �Ѵ�.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfos[guestList[j]].isDisSat == true || mGuestInfos[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    //Debug.Log("reject Count : " + rejectCount);
                    if ((mGuestInfos[VisitedGuestNum[temp]].isDisSat == true || mGuestInfos[VisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake-2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }
                    
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = VisitedGuestNum[temp];
                GuestIndex++;
                isOverLap = true;

            }
            for (int i = 0; i < possibleToTake - VisitedGuestNum.Count; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, NotVisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (NotVisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                    }
                    int rejectCount = 0;
                    // �Ҹ� ��Ƽ�̰ų� �湮 �Ұ� ���� ��Ƽ�� ���� ���ؾ� �Ѵ�.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfos[guestList[j]].isDisSat == true || mGuestInfos[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfos[NotVisitedGuestNum[temp]].isDisSat == true || mGuestInfos[NotVisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake-2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }            
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = NotVisitedGuestNum[temp];
                GuestIndex++;
                isOverLap = true;

            }
        }
        // �湮 �̷��� ���� ��Ƽ�� ���� ���
        // ��� ��Ƽ�� �湮 �̷��� �ִ� ��Ƽ�߿��� �̴´�.
        else if (NotVisitedGuestNum.Count == 0)
        {
            Debug.Log("�湮 �̷��� ���� ��Ƽ�� �����ϴ�");
            for (int i = 0; i < possibleToTake; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                            //Debug.Log("�� �ߺ�.");
                        }
                    }
                    int rejectCount = 0;
                    // �Ҹ� ��Ƽ�̰ų� �湮 �Ұ� ���� ��Ƽ�� ���� ���ؾ� �Ѵ�.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfos[guestList[j]].isDisSat == true || mGuestInfos[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfos[VisitedGuestNum[temp]].isDisSat == true || mGuestInfos[VisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake-2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }
                    
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = VisitedGuestNum[temp];

                GuestIndex++;
                isOverLap = true;

            }
        }
        // �� ���� ��쿡�� �湮 �̷��� �ִ� ��Ƽ 5��, �湮 �̷��� ���� ��Ƽ 1���� �̴´�.
        else
        {
            Debug.Log("�湮�̷� ��Ƽ 5��, �湮 �̷��� ���� ��Ƽ 1���� �̽��ϴ�.");
            for (int i = 0; i < possibleToTake-1; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                    }
                    int rejectCount = 0;
                    // �Ҹ� ��Ƽ�̰ų� �湮 �Ұ� ���� ��Ƽ�� ���� ���ؾ� �Ѵ�.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfos[guestList[j]].isDisSat == true || mGuestInfos[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfos[VisitedGuestNum[temp]].isDisSat == true || mGuestInfos[VisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake-2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }
                    
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = VisitedGuestNum[temp];
                GuestIndex++;
                isOverLap = true;

            }
            for (int i = 0; i < 1; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, NotVisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (NotVisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                    }
                    int rejectCount = 0;
                    // �Ҹ� ��Ƽ�̰ų� �湮 �Ұ� ���� ��Ƽ�� ���� ���ؾ� �Ѵ�.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfos[guestList[j]].isDisSat == true || mGuestInfos[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfos[NotVisitedGuestNum[temp]].isDisSat == true || mGuestInfos[NotVisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake-2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }           
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = NotVisitedGuestNum[temp];

                GuestIndex++;
                isOverLap = true;
            }
        }
        // �Ҹ� ��Ƽ��� ��������.
        int[] tempList = new int[6];
        int a = 0;
        for(int i=0; i< possibleToTake; i++)
        {
            tempList[i] = -1;
        }
        for(int i = 0; i< possibleToTake; i++)
        {
            if(mGuestInfos[guestList[i]].isDisSat == false && mGuestInfos[guestList[i]].mNotVisitCount == 0)
            {
                tempList[a] = guestList[i];
                a++;
                mGuestMax++;
            }
        }
        Debug.Log(mGuestMax);
        guestList = tempList;

        return guestList;
    }

    // �ش� ��Ƽ�� �ʱ�ȭ �����ִ� �Լ�
    public void InitGuestData() // ���Ŀ� ����
    {
        // ��ũ���ͺ� ������Ʈ�� �ϳ� �� ���� �� ���� �޾ƿ��� ������ ���� ����

    }

    // �湮�ֱ⸦ �ʱ�ȭ ���ִ� �Լ�
    public void InitGuestTime()
    {
        mGuestTime = 0.0f;
        isTimeToTakeGuest = false;
        Debug.Log("�湮�ֱ� �ʱ�ȭ");
    }
    
    // �Ϸ簡 �����鼭 �ʱ�ȭ�� �ʿ��� �������� ��ȯ���ش�.
    public void InitDay() 
    {
        // ������ ������ ���� �����ִ� ��Ƽ���� �Ҹ� ��Ƽ�� �����.


        // ���ο� �湮 ��Ƽ ����Ʈ�� �̴´�.
        mGuestIndex = 0;
        mGuestCount = -1;
        mGuestMax = 0;
        mTodayGuestList = NewChoiceGuest();

        // �湮 �ֱ⸦ �ʱ�ȭ�Ѵ�.
        InitGuestTime();

        // ä�������� �ٽ� ���ŵȴ�.
    }

    public int SpeakEmotionEffect(int guestNum)
    {
        int result = -1;            // �����Ѽ��� ���� ������ ���� ��ȣ
        int temp = -1;              // result�� �����Ѽ����� ���̰�

        // ���Ѽ� ������ ���ų� ���Ѽ����� ���ٸ� �Ҹ� ��Ƽ�̹Ƿ� ǥ���� ���� ���� ������ ������� �ʴ´�.

        // ù��° �����Ѽ� �� �߿��� �� �����Ѽ��� ������ ���� �ʱ� ��������� ���´�. 
        if(mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[0].downLimitEmotion]
            - mGuestInfos[guestNum].mLimitEmotions[0].downLimitEmotionValue  >
            mGuestInfos[guestNum].mLimitEmotions[0].upLimitEmotionValue
            - mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[0].upLimitEmotion])
        {
            result = mGuestInfos[guestNum].mLimitEmotions[0].upLimitEmotion;
            temp = mGuestInfos[guestNum].mLimitEmotions[0].upLimitEmotionValue
            - mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[0].upLimitEmotion];
        }
        else
        {
            result = mGuestInfos[guestNum].mLimitEmotions[0].downLimitEmotion;
            temp = mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[0].downLimitEmotion]
            - mGuestInfos[guestNum].mLimitEmotions[0].downLimitEmotionValue;
        }
        if (temp > mGuestInfos[guestNum].mLimitEmotions[1].upLimitEmotionValue
             - mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[1].upLimitEmotion])
        {
            result = mGuestInfos[guestNum].mLimitEmotions[1].upLimitEmotion;
            temp = mGuestInfos[guestNum].mLimitEmotions[1].upLimitEmotionValue
             - mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[1].upLimitEmotion];
        }
        if (temp > mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[1].downLimitEmotion]
            - mGuestInfos[guestNum].mLimitEmotions[1].downLimitEmotionValue)
        {
            result = mGuestInfos[guestNum].mLimitEmotions[1].downLimitEmotion;
            temp = mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mLimitEmotions[1].downLimitEmotion]
            - mGuestInfos[guestNum].mLimitEmotions[1].downLimitEmotionValue;
        }

        return result;
    }

    public int SpeakEmotionDialog(int guestNum)
    {
        int result = -1;         // ��ȯ�� ���� ��ȣ
        int temp = -1;           // �ӽ÷� ������ ������ �������� ���̰�
        int maxValue = -1;       // ���̰� �߿��� ���� ū ���� �����ϴ� ��

        for(int i = 0; i<5; i++)
        {
            // ������ �������� ���� ���� ���ٸ�
            if(mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum] 
                > mGuestInfos[guestNum].mSatEmotions[i].up)
            {
                temp = mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum] 
                    - mGuestInfos[guestNum].mSatEmotions[i].up;
            }
            // ������ �������� ���� ���� ���ٸ�
            else if(mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum]
                < mGuestInfos[guestNum].mSatEmotions[i].down)
            {
                temp = mGuestInfos[guestNum].mSatEmotions[i].down
                    - mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum];
            }
            // �̿��� ���� ���������ȿ� �ִ� ���̹Ƿ� �����Ѵ�.

            // temp���� ���� ����� ������ ������ ������ �ִٸ� �����Ѵ�.
            if(maxValue < temp)
            {
                maxValue = temp;
                result = mGuestInfos[guestNum].mSatEmotions[i].emotionNum;
            }            
        }
        return result;
    }
}
