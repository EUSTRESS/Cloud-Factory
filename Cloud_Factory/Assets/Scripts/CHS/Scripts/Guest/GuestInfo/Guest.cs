using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// GuestInfo�߿��� �����ؾ��ϴ� ���������� ���� Ŭ����\
[System.Serializable]
public class GuestInfoSaveData
{
    public int[] mEmotion = new int[20];
    public int mSatatisfaction;
    public int mSatVariation;
    public bool isDisSat;
    public bool isCure;
    public int mVisitCount;
    public int mNotVisitCount;
    public bool isChosen;
    public int mSitChairIndex;
    public bool isUsing;
}

// GuestManager���� �����ؾ� �ϴ� ���������� ���� Ŭ����
[System.Serializable]
public class GuestManagerSaveData
{
    // ��� ����
    private const int NUM_OF_GUEST = 20;
    private const int NUM_OF_TODAY_GUEST_LIST = 6;

    public GuestInfoSaveData[] GuestInfos = new GuestInfoSaveData[NUM_OF_GUEST];

    public bool isGuestLivingRoom;
    public bool isTimeToTakeGuest;
    public int mGuestIndex;
    public int[] mTodayGuestList = new int[NUM_OF_TODAY_GUEST_LIST];
    public int mGuestCount;
    public float mGuestTime;
}

// GuestObject ���ο��� ����Ǿ�� �� ���������� ���� Ŭ����
[System.Serializable]
public class GuestObjectSaveData
{
    // Transform
    public float xPos;
    public float yPos;
    public float xScale;

    // GuestObject.cs
    public int   mGuestNum;
    public float mLimitTime;

    public int mTargetChairIndex;
    public bool isSit;
    public bool isUsing;
    public bool isMove;
    public bool isGotoEntrance;
    public bool isEndUsingCloud;

    // WayPoint.cs
    public int WayNum;
}

[System.Serializable]
public class SOWSaveData
{
    // �մ� ������Ʈ ���� (���/���� ���� ���� ����)
    public List<GuestObjectSaveData> UsingObjectsData = new List<GuestObjectSaveData>();
    public List<GuestObjectSaveData> WaitObjectsData = new List<GuestObjectSaveData>();

    public int mMaxChairNum;
    public Dictionary<int, bool> mCheckChairEmpty = new Dictionary<int, bool>();
}


public class Guest : MonoBehaviour
{
    // ��� ����
    public const int NUM_OF_GUEST = 20;                                 // �մ��� �� �ο� ��
    private const int NUM_OF_TODAY_GUEST_LIST = 6;                      // �Ϸ翡 �湮�ϴ� �մ��� �� �ο� ��

    [Header ("[�մ� ������ ����Ʈ]")]
    public GuestInfos[] mGuestInfo;                                     // �մԵ��� �ΰ��� ������
    [SerializeField]
    private GuestInfo[] mGuestInitInfos;                                // Scriptable Objects���� ������ ��� �ִ� �迭

    [Header ("[�մ� �湮 ���� ����]")]
    public bool isGuestInLivingRoom;                                    // �����ǿ� �մ��� �湮���ִ°�?
    public bool isTimeToTakeGuest;                                      // ��Ƽ �湮�ֱⰡ �������� Ȯ��

    [Space(10f)]
    public int mGuestIndex;                                             // �̹��� �湮�� ��Ƽ�� ��ȣ
    public int[] mTodayGuestList = new int[NUM_OF_TODAY_GUEST_LIST];    // ���� �湮 ������ ��Ƽ ���
    public int mGuestCount;                                             // �湮�� ��Ƽ�� ����

    [Space (10f)]
    public float mGuestTime;                                            // ��Ƽ�� �湮 �ֱ��� ���� ��
    public float mMaxGuestTime;                                         // ��Ƽ�� �湮 �ֱ�


    [SerializeField]
    private int mGuestMax;                                              // ���� �湮�ϴ� ��Ƽ�� �ִ� ����

    // SOWManger�� �����Ͽ� ���� ����
    public SOWSaveData SaveSOWdatas;                                 // �̾��ϱ⸦ ���� �����͵��� �����س��� ����Ʈ
    public bool isLoad = false;

    private static Guest instance = null;                               // �̱��� ����� ���� instance ����
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
            mMaxGuestTime = 5.0f;
            
            mGuestInfo = new GuestInfos[NUM_OF_GUEST];
            for (int i = 0; i< NUM_OF_GUEST; i++)
            {
                // �⺻������ �մԵ��� �����͸� �⺻ �����ͷ� �ʱ�ȭ�Ѵ�.
                // �̾��ϱ⸦ �ϰԵǴ� ��� �մԵ��� ���� �������� �����س��� �մ����� ����Ʈ���� �޾ƿ� �����Ѵ�.
                InitGuestData(i);
            }

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
        // ��Ƽ�� �湮�ֱ⸦ ������. (������ �����ϱ� ��(�κ�)������ �ð��� �帣�� �ʴ´�.
        if (mGuestTime < mMaxGuestTime && SceneManager.GetActiveScene().name != "Lobby")
        {
            mGuestTime += Time.deltaTime;
        }
        else if (mGuestTime >= mMaxGuestTime && isTimeToTakeGuest == false)
        {
            // ��� �ε����� �� ���� �ʴ� �� ��Ƽ �湮�ֱⰡ �ٵȰ�� ���ο� ��Ƽ�� �鿩������.
            if (mGuestCount < mGuestMax - 1) 
            {
				TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();

                if (mTutorialManager.isTutorial == true
                    && mGuestCount >= 0)
                { }
                else
                {
                    isTimeToTakeGuest = true;
                    TakeGuest();
                }
                // ������ �̵��ϴ� ��ư�鿡 ���� ��ȣ�ۿ�
            }
            else
            {
                Debug.Log("��� ��Ƽ�� �湮�Ͽ����ϴ�.");
            }
        }

        //Guest �湮 ����Ʈ Ȯ���� ���� �׽�Ʈ �ڵ�
        if (Input.GetKeyDown(KeyCode.P))
        {
            mGuestInfo[0].mEmotion[0] += 5;
		}

        //
        if (Input.GetKeyDown(KeyCode.O))
        {
            int[] tempList = SpeakEmotionEffect(0);
			for (int i = 0; i < tempList.Length; i++)
			{
				Debug.Log("Danger Emotion" + (i + 1) + " : " + tempList[i]);
			}
		}

        if (Input.GetKeyDown(KeyCode.I))
        {
            int maxDiffValue = SpeakEmotionDialog(0);
            Debug.Log("������ ���̰� ���� ū ������ " + maxDiffValue + "�Դϴ�.");
        }

        // Ʃ�丮�� ��ŵ ��Ű
        if (Input.GetKeyDown(KeyCode.A))
        {
            TutorialManager tuto = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
            for(int i = 0; i< tuto.isFinishedTutorial.Length; i++)
            {
                tuto.isFinishedTutorial[i] = true;
            }
        
        }

        // �����Ѽ� ħ�� Ȯ���� ���� �Լ� �׽�Ʈ (����)
        if (Input.GetKeyDown(KeyCode.D))
        {
            mGuestInfo[0].isDisSat = CheckIsDisSat(0);
            Debug.Log(mGuestInfo[0].isDisSat);
        }

        // �̾��ϱ��� ��� Load�ؼ� �޾ƿ� �����͸� SOWManager�� �Ѱ��ش�.
        if (isLoad && SceneManager.GetActiveScene().name == "Space Of Weather")
        {
            SOWManager sowManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

            if (sowManager != null)
            {
                isLoad = false;

                // ���ڰ� ����ִ����� ���� ������ �Ѱ��ش�.
                sowManager.mCheckChairEmpty = SaveSOWdatas.mCheckChairEmpty;

                Debug.Log(sowManager.mCheckChairEmpty);

                sowManager.mMaxChairNum = 3;

                // ������ ������Ʈ�� ���Ͽ� �Ѱ��ش�.
                foreach (GuestObjectSaveData data in SaveSOWdatas.WaitObjectsData)
                {
                    sowManager.mWaitGuestList.Enqueue(data.mGuestNum);
                    sowManager.mWaitGuestObjectQueue.Enqueue(SetLoadGuest(data, sowManager));
                }

                // �������� ������Ʈ�� ���Ͽ� �Ѱ��ش�.
                foreach (GuestObjectSaveData data in SaveSOWdatas.UsingObjectsData)
                {
                    sowManager.mUsingGuestList.Add(data.mGuestNum);
                    sowManager.mUsingGuestObjectList.Add(SetLoadGuest(data, sowManager));
                }

            }
        }
    }

    public GameObject SetLoadGuest(GuestObjectSaveData data, SOWManager sow)
    {
        GameObject tempObject;

        // Instance ����
        tempObject = Instantiate(sow.mGuestObject);

        // etc.
        tempObject.GetComponent<RLHReader>().SetGuestNum(data.mGuestNum);
        tempObject.GetComponent<WayPoint>().WayPos = sow.mWayPoint;
        tempObject.GetComponent<WayPoint>().WayNum = data.WayNum;

        // GuestObject.cs
        GuestObject Info = tempObject.GetComponent<GuestObject>();
        Info.setGuestNum(data.mGuestNum);
        Info.initAnimator();
        Info.init();

        Info.mLimitTime = data.mLimitTime;
        Info.mTargetChiarIndex = data.mTargetChairIndex;
        Info.isSit = data.isSit;
        Info.isUsing = data.isUsing;
        Info.isMove = data.isMove;
        Info.isGotoEntrance = data.isGotoEntrance;
        Info.isEndUsingCloud = data.isEndUsingCloud;

        //Info.mTargetChair.position = new Vector3(data.mTargetChairXpos, data.mTargetChairYpos, 0.0f);

        // transform
        tempObject.transform.position = new Vector3(data.xPos,data.yPos,0.0f);
        tempObject.transform.localScale = new Vector3(data.xScale, 1.0f, 1.0f);

        if(Info.mTargetChiarIndex != -1 && Info.isSit == true)
        {
            Info.mGuestAnim.SetTrigger("AlreadySit");
            Debug.Log("AlreadySit");
        }

        return tempObject;
    }


    public void TakeGuest()
    {
        if (isTimeToTakeGuest == true && isGuestInLivingRoom == false)
        {
            mGuestCount++;
            mGuestIndex = mTodayGuestList[mGuestCount];
            isGuestInLivingRoom = true;
        }
    }

    // ��Ƽ�� ���������� �޾ƿ��� API
    public string GetName(int gusetNum)
    {
        return mGuestInfo[gusetNum].mName;
    }

    public bool CheckIsDisSat(int guestNum)
    {
        int temp = IsExcessLine(guestNum);                      // ħ���ϴ� ��쿡 �������� ���Ƿ� ������ ����

        // ������ ���� ħ���� ��츦 Ȯ��
        if (temp != -1)
        {
            mGuestInfo[guestNum].isDisSat = true;              // �Ҹ� ��Ƽ�� ��ȯ
            mGuestInfo[guestNum].mSatatisfaction = 0;          // ������ 0 ���� ����
            mGuestInfo[guestNum].mVisitCount = 0;              // ���� �湮Ƚ�� 0���� ����

            // TODO : ġ���� ������� �Ҹ� ��Ƽ�� �� ���¿� �մ� ��ȣ, � ���� ��ȭ�� ���� ������ �������ֱ�


            return true;
        }
        return false;
    }

    // ��Ƽ�� ������ ���濡 �ʿ��� API 
    public void SetEmotion(int guestNum, int emotionNum, int value)
    {
        mGuestInfo[guestNum].mEmotion[emotionNum] += value;
    }

    public int IsExcessLine(int guestNum) // ���� �����Ѽ��� ħ���ߴ��� Ȯ���ϴ� �Լ�. 
    {
        SLimitEmotion[] limitEmotion = mGuestInfo[guestNum].mLimitEmotions;

        for (int i = 0; i < 2; i++)
        {
            if (mGuestInfo[guestNum].mEmotion[limitEmotion[i].upLimitEmotion] >= limitEmotion[i].upLimitEmotionValue) // �����Ѽ��� ħ���� ���
            {
                Debug.Log("�����Ѽ��� ħ���Ͽ����ϴ�");
                return limitEmotion[i].upLimitEmotion;
            }
            else if (mGuestInfo[guestNum].mEmotion[limitEmotion[i].downLimitEmotion] <= limitEmotion[i].downLimitEmotionValue)
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

        for (int i = 0; i < 5; i++)
        {
            // ������ ���� ���� ������ Ȯ��
            if (mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mSatEmotions[i].emotionNum] <= mGuestInfo[guestNum].mSatEmotions[i].up &&
             mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mSatEmotions[i].emotionNum] >= mGuestInfo[guestNum].mSatEmotions[i].down)
            {
                temp++;
            }
        }
        mGuestInfo[guestNum].mSatatisfaction = temp;
        Debug.Log(temp);
    }

    // TODO : �Լ� ����
    // ��Ƽ ����Ʈ�� ���� �����ϴ� �Լ�
    public int[] NewChoiceGuest()
    {

        List<int> guestList = new List<int>();          // ������ ��Ƽ�� ����Ʈ
        int[] returnValueList = new int[6];             // ��ȯ�� ��Ƽ�� ����Ʈ, size�� �ʱ�ȭ��
        int possibleToTake = 6;                         // ���� �� �ִ� �� ��Ƽ�� ��

        int totalGuestNum = 20;                         // �� ��Ƽ�� ��
        int possibleGuestNum = 0;                       // �湮�� ������ ��Ƽ�� ��

        List<int> VisitedGuestNum = new List<int>();    // �湮 �̷��� �ִ� ��Ƽ�� ����Ʈ
        List<int> NotVisitedGuestNum = new List<int>(); // �湮 �̷��� ���� ��Ƽ�� ����Ʈ

        int loopCount = 0;                              // ���� ������ ��� ���ư����� üũ�ϴ� ����

        // �湮 Ƚ���� ���� ��Ƽ�� �������� 5�� �� ��Ƽ�� ���ܵǾ�� �ϹǷ� ���� ����Ʈ���� ������.
        for (int i = 0; i < totalGuestNum; i++)
        {
            if (mGuestInfo[i].mVisitCount < 10 && mGuestInfo[i].isCure == false)
            {
                if (mGuestInfo[i].mVisitCount == 0)
                {
                    NotVisitedGuestNum.Add(i);
                }
                else
                {
                    VisitedGuestNum.Add(i);
                }
				possibleGuestNum++;             //�湮 ������ ��Ƽ�� ���� �����Ѵ�
			}
        }
		if (possibleGuestNum <= possibleToTake) { possibleToTake = possibleGuestNum; }       // �湮 ������ ��Ƽ�� ���� 6���� ������ ��, ���� �� �ִ� ��Ƽ�� ���� �������ش�
        bool isFinishedChoice = false;          //����Ʈ ���� �Ϸ� ���� Ȯ��

        while (!isFinishedChoice)                 
        {
            guestList.Clear();                  // �մ� ����Ʈ�� ���ۼ��� ������ ����ش�.
            loopCount++;                        // ���� �ݺ� Ƚ���� ����Ѵ�.

            int currentNum  = -1;       // ���� ����
            int newGuest    = -1;       // ���� ���� �մ��� ��ȣ ����

            if(loopCount >= 10)         // ����Ʈ ������ 10ȸ�̻� �ݺ��ص� ������ �ȵǾ��� ��, �Ҹ�/�湮 �Ұ� ��Ƽ�� �����ϰ� ����Ʈ�� �ۼ��Ѵ�.
            {
                possibleGuestNum =  0;         // �湮 ������ �մ��� ���� �ʱ�ȭ ���ش�.
                possibleToTake =    3;         // �մ��� ���� �ִ� 3�� �̴´�. 

                NotVisitedGuestNum.Clear();    // ����Ʈ�� �� ���·� ������ش�.
                VisitedGuestNum.Clear();       // //

                for (int i = 0; i < totalGuestNum; i++)
                {
                    if (mGuestInfo[i].mVisitCount < 10 && mGuestInfo[i].isCure == false && mGuestInfo[i].isDisSat == false && mGuestInfo[i].mNotVisitCount <= 0)
                    {
                        if (mGuestInfo[i].mVisitCount == 0)
                        {
                            NotVisitedGuestNum.Add(i);
                        }
                        else
                        {
                            VisitedGuestNum.Add(i);
                        }
                        possibleGuestNum++;             //�湮 ������ ��Ƽ�� ���� �����Ѵ�
                    }
                }
				if (possibleGuestNum <= possibleToTake) { possibleToTake = possibleGuestNum; }
			}

            //�湮 �̷��� ���� ��Ƽ�� �ڸ��� �ּ� �� �ڸ� ����, �湮 �̷��� �ִ� ��Ƽ�� �ִ�� �̴´�
            //�湮 �̷��� �ִ� ��Ƽ�� ���� possibleToTake - 1 ������ ��
            if (VisitedGuestNum.Count <= possibleToTake - 1){ guestList = AddToGuestList(guestList, VisitedGuestNum, VisitedGuestNum.Count); }
            //�湮 �̷��� �ִ� ��Ƽ�� ���� possibleToTake �̻��� ��
            else                                            { guestList = AddToGuestList(guestList, VisitedGuestNum, possibleToTake - 1); }

			//�湮 �̷��� ���� ��Ƽ�� ���� ��, �湮 �̷��� �ִ� ��Ƽ�� ���� �ڸ��� ä���.
			if (NotVisitedGuestNum.Count <= 0)              { guestList = AddToGuestList(guestList, VisitedGuestNum, possibleToTake); }
            //�湮 �̷��� ���� ��Ƽ�� ���� ��, ���� �ڸ��� ��� �湮 �̷��� ���� ��Ƽ�� ä���
            else
            {
                if (NotVisitedGuestNum.Count > 0) { currentNum = Random.Range(0, NotVisitedGuestNum.Count); }
                for (int num = 0; num < NotVisitedGuestNum.Count;)    // �湮 �̷��� ���� ��Ƽ�� ����������
                {
                    if (guestList.Count >= possibleToTake) { break; }        // �ڸ��� ��� á�� �� �ݺ��� Ż��, �ڸ��� �����־ ��Ƽ�� ���� ���� ���� for������ �˰�

                    if (guestList.Contains(NotVisitedGuestNum[currentNum])) { currentNum = Random.Range(0, NotVisitedGuestNum.Count); }
                    else
                    {
                        guestList.Add(NotVisitedGuestNum[currentNum]);
                        num++;
                    }
                }
            }

			//����Ʈ���� �Ҹ� ��Ƽ�� ���� �����ϴ� ����
			int rejectCount = 0;
            foreach (var num in guestList)
            {
                if (mGuestInfo[num].isDisSat == true || mGuestInfo[num].mNotVisitCount > 0) { rejectCount++; }
            }

            //���� �� �ִ� ��Ƽ�� 4���� �̻��̰�, �ٽ� �̰ԵǴ� �Ҹ�/�湮 �Ұ� ��Ƽ ���� possibleToTake - 2 �̻��̸� �ٽ� �̱�
            if (possibleToTake >= 4 && rejectCount >= possibleToTake - 2) { continue; }
            //guest list �ۼ� while�� ���� �� �Ҹ�/�湮 �Ұ� ��Ƽ guestList���� ����
            else
            {
                isFinishedChoice = true;    // ����Ʈ �ۼ� ����

                List<int> mixList = new List<int>();    // ����Ʈ �� �մ��� ������ ���� ������ ���ο� ����Ʈ

                int tempNum = 0;            // ��ȯ�ϴ� array�� index
                int listSize = 0;           // ��ȯ�ϴ� array�� size 

                // �Ҹ�/�湮 �Ұ� ��Ƽ�� ������ Guest�� ���� �ٽ� ���
                foreach (var num in guestList)
                {
                    if (mGuestInfo[num].isDisSat == false && mGuestInfo[num].mNotVisitCount <= 0) { listSize++; }
                }

                //guestList�� �ִ� �մ� ��ȣ�� �湮 ������ �ִ��� �������� ������� �����ش�.
                List<int> tempList = new List<int>();
                tempList = AddToGuestList(tempList, guestList, guestList.Count);

                //���� guestList�� ������ ������ tempList�� �մ� ��ȣ ��, �Ҹ� ��Ƽ�� �ƴϰ�, �湮 �Ұ� ���°� �ƴ� ��Ƽ�� mixList�� �߰����ش�.
                foreach(var num in tempList)
                {
					if (mGuestInfo[num].isDisSat == false && mGuestInfo[num].mNotVisitCount <= 0) { mixList.Add(num); }
				}

                returnValueList = new int[listSize];
                foreach (var num in mixList)
                {
                    if (mGuestInfo[num].isDisSat == false && mGuestInfo[num].mNotVisitCount <= 0) { returnValueList[tempNum++] = num; }
                }
            }
        }
        return returnValueList;
    }

    private List<int> AddToGuestList(List<int> guest_list, List<int> visit_guest_list, int max_list)    //AddToGuestList(��ȯ ����Ʈ, �湮 ���� �մ� ����Ʈ, �Լ����� ���� �ִ� �մ� ��);
    {
        List<int> temp_list = guest_list;
        int currentNum = -1;
		if (visit_guest_list.Count > 0) { currentNum = Random.Range(0, visit_guest_list.Count); }
		for (; temp_list.Count < max_list;)
		{
			if (temp_list.Count > visit_guest_list.Count) { break; } //�ڸ��� ��� ä��� ���� �����ִ� �湮 �̷��� �ִ� ��Ƽ�� ������ �ݺ����� �������´�
			if (temp_list.Contains(visit_guest_list[currentNum])) { currentNum = Random.Range(0, visit_guest_list.Count); }
			else
			{
				temp_list.Add(visit_guest_list[currentNum]);
			}
		}
        return temp_list;
	}


    // �ش� ��Ƽ�� �ʱ�ȭ �����ִ� �Լ�
    public void InitGuestData(int guestNum) 
    {
        // ��ũ���ͺ� ������Ʈ�� ����� ���� �ʱ� �����Ͱ��� �޾ƿͼ� �ʱ�ȭ�� ��Ų��.

        GuestInfos temp         = new GuestInfos();
        temp.mName              = mGuestInitInfos[guestNum].mName.Clone() as string;
        temp.mSeed              = mGuestInitInfos[guestNum].mSeed;
        temp.mEmotion           = mGuestInitInfos[guestNum].mEmotion.Clone() as int[];
        temp.mAge               = mGuestInitInfos[guestNum].mAge;
        temp.mJob               = mGuestInitInfos[guestNum].mJob;
        temp.mSatatisfaction    = mGuestInitInfos[guestNum].mSatatisfaction;
        temp.mSatVariation      = mGuestInitInfos[guestNum].mSatVariation;
        temp.mSatEmotions       = mGuestInitInfos[guestNum].mSatEmotions;
        temp.mLimitEmotions     = mGuestInitInfos[guestNum].mLimitEmotions;
        temp.isDisSat           = false;
        temp.isCure             = false;
        temp.mVisitCount        = 1;
        temp.mNotVisitCount     = 0;
        temp.isChosen           = false;
        temp.mUsedCloud         = new List<StoragedCloudData>();
        temp.mSitChairIndex     = -1;
        temp.isUsing            = false;

        mGuestInfo[guestNum] = temp;
    }

    // TODO : �Լ� ����
    // �湮�ֱ⸦ �ʱ�ȭ ���ִ� �Լ�
    public void InitGuestTime()
    {
        mGuestTime = 0.0f;
        isTimeToTakeGuest = false;
    }

    // �Ϸ簡 �����鼭 �ʱ�ȭ�� �ʿ��� �������� ��ȯ���ش�.
    public void InitDay()
    {
        // ������ ������ ���� �����ִ� ��Ƽ���� �Ҹ� ��Ƽ�� �����.

        // ���ο� �湮 ��Ƽ ����Ʈ�� �̴´�.
        mGuestIndex = 0;
        mGuestCount = -1;
        mGuestMax = 0;

        // ���ο� ����Ʈ�� �̴� �Լ��� ȣ�� (�׽�Ʈ�� ���ؼ� ��� �ּ�ó��)
        int[] list = { 3, 1, 2, 3, 0, 1 };
        mGuestMax = NUM_OF_TODAY_GUEST_LIST;
        mTodayGuestList = list;

        //mTodayGuestList = NewChoiceGuest();

        // �湮 �ֱ⸦ �ʱ�ȭ�Ѵ�.
        InitGuestTime();

        // ä�������� �ٽ� ���ŵȴ�.

    }

    //���� �����Ѽ��� ������ ������ �迭 �������� return
    public int[] SpeakEmotionEffect(int guestNum)
    {
		List<int> emotionList = new List<int>();    // �����Ѽ� ������ ������ �����ϴ� list
		int[] returnEmotionList = new int[4];       // ���� List ������ ��ȯ�ϴ� �迭
        int nearValue = 100;                        // ���� �������� ������ �Ǵ� ��, ���� ��ġ ������ �ִ�ġ�� 100���� �ʱ�ȭ

        int upDiffValue =   100;                    // ���Ѽ��� ���� �� ���� ����
        int downDiffValue = 100;                    // ���Ѽ��� ���� �� ���� ����

		GuestInfos targetGuest = mGuestInfo[guestNum];  // ��� guest
        
        // �����Ѽ��� ���� ����� �� Ž��
        for(int num = 0; num < 2; num++){
            upDiffValue = targetGuest.mLimitEmotions[num].upLimitEmotionValue                   
                - targetGuest.mEmotion[targetGuest.mLimitEmotions[num].upLimitEmotion];
            downDiffValue = targetGuest.mEmotion[targetGuest.mLimitEmotions[num].downLimitEmotion]
            - targetGuest.mLimitEmotions[num].downLimitEmotionValue;

			if (upDiffValue <= nearValue) { nearValue = upDiffValue; }
            if (downDiffValue <= nearValue) { nearValue = downDiffValue; }
        }

		//�����Ѽ��� ���� ������ ���������� üũ �� ����Ʈ�� �߰�
        // UpLimit���� ���ų� DownLimit���� ������ �Ҹ� ��Ƽ�̹Ƿ� Ȯ�� �� �ʿ� X
        for(int num = 0; num < 2; num++) {
			upDiffValue = targetGuest.mLimitEmotions[num].upLimitEmotionValue                     
				- targetGuest.mEmotion[targetGuest.mLimitEmotions[num].upLimitEmotion];
			downDiffValue = targetGuest.mEmotion[targetGuest.mLimitEmotions[num].downLimitEmotion] 
			- targetGuest.mLimitEmotions[num].downLimitEmotionValue;

            if (upDiffValue == nearValue) { emotionList.Add(targetGuest.mLimitEmotions[num].upLimitEmotion); }
            if (downDiffValue == nearValue) { emotionList.Add(targetGuest.mLimitEmotions[num].downLimitEmotion); }
		}

		int listSize = emotionList.Count;

		// ����Ʈ�� ũ�⿡ �°� return�� �迭 ������ �� �Ҵ�
		returnEmotionList = new int[listSize];

		//������� dialog ����ϵ��� ������������ �迭�� ����
		for (int num = 0; num < listSize; num++)
		{
			returnEmotionList[num] = ListMinValue(emotionList);
			emotionList.Remove(ListMinValue(emotionList));
		}

		return returnEmotionList;
	}

    //List�� �ּڰ� ��ȯ �Լ�
    private int ListMinValue(List<int> list)
    {
        if (list.Count <= 0) return -1; // ����Ʈ�� ��������� '-1'��ȯ
        int returnValue = 20;          // ���� ��ġ�� ������ �ʱ�ȭ

        foreach(var num in list) { if (num <= returnValue) returnValue = num; }   // list ���� �ּڰ� Ž��

        return returnValue;
    }


    //�������� ���� ���� ���̳��� ���� ��ȣ return
    public int SpeakEmotionDialog(int guestNum)
    {
		int returnEmotionNum = -1;      // ��ȯ�� ���� ��ȣ
		int diffValue = -1;             // �ӽ÷� ������ ������ �������� ���̰�
		int maxDiffValue = -1;          // ���̰� �߿��� ���� ū ���� �����ϴ� ��

		GuestInfos targetGuest = mGuestInfo[guestNum];

		for (int i = 0; i < 5; i++)
		{
			// ������ �������� ���� ���� ���ٸ�
			if (targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum]
				> targetGuest.mSatEmotions[i].up)
			{
				diffValue = targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum]
					- targetGuest.mSatEmotions[i].up;
			}
			// ������ �������� ���� ���� ���ٸ�
			else if (targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum]
				< targetGuest.mSatEmotions[i].down)
			{
				diffValue = targetGuest.mSatEmotions[i].down
					- targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum];
			}
			// �̿��� ���� ���������ȿ� �ִ� ���̹Ƿ� �����Ѵ�.
			// temp���� ���� ����� ������ ������ ������ �ִٸ� �����Ѵ�.
			if (maxDiffValue < diffValue)
			{
				maxDiffValue = diffValue;
				returnEmotionNum = targetGuest.mSatEmotions[i].emotionNum;
			}
		}
		return returnEmotionNum;
	}

    public int SpeakLeastDiffEmotion(int guestNum)
    {
        int returnEmotionNum = -1;      // ��ȯ�� ���� ��ȣ
        int diffValue = 100;             // �ӽ÷� ������ ������ �������� ���̰�
        int minDiffValue = 100;         // ���̰� �߿��� ���� ū ���� �����ϴ� ��

        GuestInfos targetGuest = mGuestInfo[guestNum];

        for (int i = 0; i < 5; i++)
        {
            // ������ �������� ���� ���� ���ٸ�
            if (targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum] 
            >  targetGuest.mSatEmotions[i].up)
            {
                diffValue = targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum]
                    - targetGuest.mSatEmotions[i].up;
            }
            // ������ �������� ���� ���� ���ٸ�
            else if (targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum]
                < targetGuest.mSatEmotions[i].down)
            {
                diffValue = targetGuest.mSatEmotions[i].down
                    - targetGuest.mEmotion[targetGuest.mSatEmotions[i].emotionNum];
            }
            // �̿��� ���� ���������ȿ� �ִ� ���̹Ƿ� �����Ѵ�.
            // temp���� ���� ����� ������ ������ ������ �ִٸ� �����Ѵ�.
            if (minDiffValue > diffValue)
            {
                minDiffValue = diffValue;
                returnEmotionNum = targetGuest.mSatEmotions[i].emotionNum;
            }
        }
        return returnEmotionNum;
    }

// �Ҹ� ��Ƽ ������ �Ѱ��ִ� List
public int[] DisSatGuestList() {
        int[] temp_list;
		int temp_idx = 0;

        int list_size = 0;

		for (int num = 0; num < 20; num++) { if (mGuestInfo[num].isDisSat == true) list_size++; }

        temp_list = new int[list_size];

        for(int num = 0; num < 20; num++) { if (mGuestInfo[num].isDisSat == true) temp_list[temp_idx++] = num; }

        return temp_list;
    }
}
