using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SOWManager : MonoBehaviour
{
    public Queue<int>               mWaitGuestList;         // �����ǿ��� ������ �ް� �Ѿ�� �մԵ��� ����Ʈ
    public Queue<int>               mUsingGuestList;        // ������ �������� �ڸ��� �ɾ� ������ �������� �غ� �� �մԵ��� ����Ʈ
    int                             mMaxNumOfUsingGuest;    // mUsingGuestList�� ���� �� �ִ� �ִ��� ũ��
    int                             mTempGuestNum;           // �ӽ� �մ� ��ȣ��

    private Queue<GameObject>       mWaitGuestObjectList;   // ��� �մ� ������Ʈ���� ������ ����Ʈ
    private Queue<GameObject>       mUsingGuestObjectList;  // ��� �մ� ������Ʈ���� ������ ����Ʈ

    [SerializeField]
    private GameObject              mGuestObject;           // �ν��Ͻ��Ͽ� ������ �մ� ������Ʈ
    public GameObject[]             mChairPos;              // �մ��� �ɾƼ� ������ ����� ����(����)
    public GameObject[]             mWayPoint;              // �մ��� �ɾ�ٴϸ� ��å�ϴ� ��ε�
    public Dictionary<int, bool>    mCheckChairEmpty;       // ���ڸ��� ���ڰ� ����ִ����� Ȯ���ϴ� ��ųʸ� ����
    public bool                     isNewGuest;             // �����ǿ��� �Ѿ�ö� ���ο� �մ��� ���°�?
    public int                      mMaxChairNum;           // ���� �ܰ迡 ���� ������ ����

    private Guest                   mGuestManager;          // GuestManager�� �����´�.
    private static  SOWManager      instance = null;


    void Start()
    {

    }
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);

            mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
            mWaitGuestList = new Queue<int>();
            mUsingGuestList = new Queue<int>();
            mWaitGuestObjectList = new Queue<GameObject>();
            mUsingGuestObjectList = new Queue<GameObject>();
            mCheckChairEmpty = new Dictionary<int, bool>();
            isNewGuest = false;
            
            // ���� �ܰ迡 �´� ���� ���� ����
            mMaxChairNum = 3;
            InitSOW();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ���ο� �մ��� ������ �������� �Ѿ�� ���
        if (isNewGuest)
        {
            isNewGuest = false;

            GameObject tempObject;

            // �մ� ������Ʈ ���� �� �ʱ�ȭ
            //mWaitGuestObjectList.Enqueue(tempObject);
            tempObject = Instantiate(mGuestObject);

            // �մ� ���� Ȯ���� ���� �����
            Debug.Log("�մ� ����");

            // �մ� ������Ʈ�� �ش��ϴ� ��ȣ�� �־��ش�.
            tempObject.GetComponent<GuestObject>().setGuestNum(mTempGuestNum);

            // ��å�θ� �����Ѵ�. <- �������� �޶����� ������ �Ѵ�.
            tempObject.GetComponent<WayPoint>().WayPos = mWayPoint;

            // �⺻ ��ġ���� �����Ѵ�. <- ��å���� ù��° ������ ����
            tempObject.transform.position = mWayPoint[0].transform.position;

            // ������� �մ� ť�� �ش� �մ��� �߰��Ѵ�.
            mWaitGuestObjectList.Enqueue(tempObject);
        }

        // ��å�θ� �ȴ� �մ��� �����ϰ�, �̿� ������ ���ڰ� ����ִٸ� ���ڷ� �̵���Ű��
        if (mWaitGuestList.Count != 0)
        {
            // ���� ���ڰ� �ִٸ� �ش� ���ڿ� ���� �ε����� ��ȯ�ް�, �ܴ̿� -1�� �޾ƿ´�.
            int chairNum = GetRandChiarIndex();

            if (chairNum != -1)
            {
                // ����� ����Ʈ�� �ش� �մ��� �ű��.
                MoveToUsingList(chairNum);
            }
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            MakeGuestDisSat();
        }

        // ������ ���� ���� �մ��� �����ϱ�
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(vec, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hit.transform.gameObject.tag == "Guest")
                {
                    Debug.Log(hitObject.GetComponent<GuestObject>().mGuestNum + "�� �մ��� Ŭ���Ͽ����ϴ�.");
                    // Ŭ�� �� �մ��� ���¿� ���� �ٸ� ��ȣ�ۿ�
                    if (hitObject.GetComponent<GuestObject>().isSit)
                    {
                        hit.transform.gameObject.GetComponent<GuestObject>().OpenCloudWindow();
                    }
                    else
                    {
                        hit.transform.gameObject.GetComponent<GuestObject>().SpeakEmotion();
                    }
                    Debug.Log(hit.transform.gameObject.GetComponent<GuestObject>().mTargetChiarIndex);
                }
                else
                {
                    Debug.Log(hit.transform.gameObject);
                }
            }
        }
    }

    // �Ϸ簡 ������ ������ ������ �ʱ�ȭ�Ѵ�.
    private void InitSOW()
    {
        mWaitGuestList.Clear();
        mUsingGuestList.Clear();
        mCheckChairEmpty.Clear();

        // ���׷��̵� �ܰ迡 ���� mCheckChairEmpty���� Ȯ���ϴ� ������ ������ �پ���.
        // ���� ��ġ�� �ʾ����Ƿ� �ϰ������� 3���� �����ϰ� �����Ѵ�.
        for (int i = 0; i< mMaxChairNum; i++)
        {
            // ��� ���ڴ� ����ִ� ���·� �ʱ�ȭ
            mCheckChairEmpty.Add(i, true);
        }
    }

    // ��� ����Ʈ�� �մ��� �߰������ִ� �Լ�
    public void InsertGuest(int guestNum)
    {
        mWaitGuestList.Enqueue(guestNum);

        Debug.Log(guestNum + "�� �մ��� ��� ����Ʈ�� �߰��Ǿ����ϴ�.");

        mTempGuestNum = guestNum; // �׽�Ʈ

    }


    // ��� ����Ʈ���� �մ��� ���� �޴� ����Ʈ�� �߰������ִ� �Լ�
    private void MoveToUsingList(int chairNum)
    {
            // ����� ������� �̵��� �մ��� ��ȣ�� �޾ƿ´�.
            int guestNum = mWaitGuestList.Dequeue();

            // �޾ƿ� �մ��� ������ ����� ������� �ִ´�.
            mUsingGuestList.Enqueue(guestNum);

            // tempObject�� ���� ���ڸ� �����Ѵ�.    
            GameObject tempObject = mWaitGuestObjectList.Dequeue();
            
            tempObject.GetComponent<GuestObject>().mTargetChiarIndex = chairNum;

            Debug.Log(chairNum + "�� ���ڸ� �����޾ҽ��ϴ�.");
            // ������Ʈ�� ����� ������Ʈ ������� �ִ´�.
            mUsingGuestObjectList.Enqueue(tempObject);

            // Ȯ���� ���� �����
            Debug.Log(guestNum + "�� �մ��� ��� ����Ʈ���� ����� ����Ʈ�� �̵��Ͽ����ϴ�.");

            // �ش� ��ȣ�� ���� ������Ʈ�� ���¸� �����Ѵ�.


            // �ش� guestNum�� ������ �ִ� �մ��� ����ִ� ������ ��ġ�� Ÿ������ ��ġ�̵� ��Ų��.

    }

    // �Ϸ簡 ���� �� Queue�� �����ִ� ��Ƽ���� �Ҹ� ��Ƽ�� ������ش�.
    private void MakeGuestDisSat()
    {
        for(int i = 0; i< mWaitGuestList.Count; i++)
        {
            mGuestManager.mGuestInfos[mWaitGuestList.Dequeue()].isDisSat = true;
        }
        for (int i = 0; i < mUsingGuestList.Count; i++)
        {
            mGuestManager.mGuestInfos[mUsingGuestList.Dequeue()].isDisSat = true;
        }
    }

    // �� ���ڿ� ���� ������ �˻��Ѵ�.
    private int GetRandChiarIndex()
    {
        int result = -1;
        bool isSelect = false;

        // ��� ���ڰ� �� �ִ��� Ȯ��
        int count = 0;
        for(int i = 0; i< mMaxChairNum; i++)
        {
            if(mCheckChairEmpty[i] == false)
            {
                count++;
            }
        }
        if(count == mMaxChairNum)
        {
            //Debug.Log("���ڸ� �������� ���Ͽ����ϴ�");
            return -1;
        }

        // �� ���ڸ� ������ ������ ����
        while(!isSelect)
        {
            result = -1;
            result = Random.Range(0, mMaxChairNum);
            if(mCheckChairEmpty[result] == true)
            {
                isSelect = true;
                mCheckChairEmpty[result] = false;
            }
        }
        return result;
    }
}
