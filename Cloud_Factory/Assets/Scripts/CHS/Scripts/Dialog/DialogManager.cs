using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    // �ҷ��� ���� ����
    private Guest                   mGuestManager;
    private SOWManager              mSOWManager;

    public int                      mGuestNum;           // �մ��� ��ȣ�� �Ѱܹ޴´�.
    private int                     mGuestSat;           // �մ��� ���� ������
    string                          mTestName;           // �׽�Ʈ�� ���� �ӽ� �̸� ( �մ��� �̸��� �����Դٰ� ����)

    [SerializeField]
    private DialogDB                mDialogDB;           // ��ȭ ������ ������ ���� DB
    private string[]                mTextList;           // ��ȭ ������ �ҷ��ͼ� �����ص� ����Ʈ
    private int[]                   mGuestImageList;     // ��ȭ ���뿡 �´� ǥ���� �����ص� ����Ʈ

    // �� ȭ�鿡 ���� �ؽ�Ʈ�� �� ���� 
    private string                  mDialogGuestName;    // ȭ�鿡 ��½�ų �մ� �̸�
    private string                  mDialogText;         // ������ ȭ�鿡 ��½�ų ����

    // �� ȭ�鿡 ���� �ؽ�Ʈ ������Ʈ ����
    public GameObject               gTextPanel;          // ��ȭ â
    public GameObject               gTakeGuestPanel;     // �մ� �ޱ�/ ���� ��ư
    public Text                     tGuestText;          // ��ȭ�� ���� �� �ؽ�Ʈ
    public Text                     tGuestName;          // ��ȭ���� �մ��� �̸��� ǥ�õ� �ؽ�Ʈ

    // �մ��� �̹����� ���µ� �ʿ��� ������ ����
    public Sprite[]                 sGuestImageArr;      // �̹��� �ε�����
    public GameObject               gGuestSprite;        // ���� ȭ�鿡 ��µǴ� �̹��� ������Ʈ
    private SpriteRenderer          sGuestSpriteRender;  // ������Ʈ�� Sprite ������Ʈ�� �о�� SpriteRenderer

    // ��ȭ ������ �ʿ��� ������ ����
    private int                     mDialogIndex;        // �ش� �������� ���ϴ� ������ �ε���s
    private int                     mDialogCharIndex;    // ������ ȭ�鿡 ��½�Ű�� ������ �ε���
    private int                     mDialogImageIndex;   // ������ ȭ�鿡 ��½�Ű�� �̹����� �ε���
    private bool                    isReading;           // ���� ��ȭâ���� ��ȭ�� ����ϴ� ���ΰ�?
    private bool                    isLastDialog;        // ������ ��ȭ�� �ҷ��Դ°�?

    // �׽�Ʈ �Լ�
    // ��ȭâ���� �ٸ� ĳ���� Ȥ�� �ٸ� �������� �ؽ�Ʈ�� �޾ƿ��� ��� ������ �ִ��� Ȯ���ϱ� ���� �Լ�
    void A() 
    {
        tGuestName.text = mGuestManager.GetName(0);  // ������ �������� �����Ƿ� ���� ������ �޾Ƽ� �Ѿ�Բ� �����ϸ� �� �� ����. 
        mGuestSat =  mGuestManager.mGuestInfos[0].mSatatisfaction;

        mDialogIndex = 0;
        mDialogCharIndex = 0;
        mDialogImageIndex = 0;
        tGuestText.text = "";

        LoadDialogInfo();
    }

    void Awake()
    {
        mDialogIndex = 0;
        mDialogIndex = GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex;
        mDialogCharIndex = 0;
        mDialogImageIndex = 0;
        tGuestText.text = "";
        tGuestName.text = "�ǿ�";

        mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        sGuestSpriteRender = gGuestSprite.GetComponent<SpriteRenderer>();
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
   
        mGuestNum = mGuestManager.mGuestIndex;
        mGuestSat = mGuestManager.mGuestInfos[mGuestNum].mSatatisfaction;

        mGuestImageList = new int[20];
        mTextList = new string[20];

        isReading = false;

        // �湮�ֱⰡ ���� ������ �մ��� ������ �ʴ´�.
        if (mGuestManager.isTimeToTakeGuest)
        {
            LoadDialogInfo();
            ReadDialog();

            // ��ȭ �г��� Ȱ��ȭ
            gTextPanel.SetActive(true);

            // �մ� �̹����� Ȱ��ȭ
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �̱��� ��� Ȯ���� ���� �׽�Ʈ�ڵ�
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveScenetoWeatherSpace();
            GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex--;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            A();
        }
    }

    public void MoveScenetoWeatherSpace()
    {
        SceneManager.LoadScene("Space Of Weather");
    }

    // �ش� �մԿ� ���� ��ȭ�� ������ �ҷ����� �Լ�
    private void LoadDialogInfo()
    {
        // ���� ���� GameManager �Ѱ��� �����ϰ�, �� ������ �Ϸ縶�� 5���� �մ��� �����Ͽ� �����ǿ� �÷��̾ ���� �ð��� ���Ͽ� �����ϰ� �Ѹ� �湮��Ų��.
        // GameManager���� ������ �մ��� ��ȣ�� �޾ƿ���, �մ��� ��ȣ�� �´� �մ��� ������ �����´�.

        int i;
        int j = 0;

        for (i = 0; i < mDialogDB.DialogText.Count; ++i)
        {
            if (mDialogDB.DialogText[i].GuestID == mGuestNum)
            {
                if (mDialogDB.DialogText[i].Sat == mGuestSat) 
                {
                    mTextList[j] = mDialogDB.DialogText[i].Text;
                    mGuestImageList[j] = mDialogDB.DialogText[i].DialogImageNumber;
                    j++;
                }
            }
        }
        mTextList[j] = "End";
    }

    private void InitDialog()
    {
        mDialogCharIndex = 0;
        tGuestText.text = "";
    }
    public string GetDialog(int dialogindex) // ������ , ��ȭ ���� ����
    {
        return mTextList[dialogindex];
    }

    private void ReadDialogAtAll()
    {
        tGuestText.text += GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex);
        isReading = false;
    }

    private void ReadDialogAtOne()
    {
        isReading = true;
        //GuestName.text = testName;
        if (tGuestText.text == GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex))
        {
            // �ؽ�Ʈ�� ��� ����� �� ��쿡 Ŭ�� ��, ���� ������ ��µȴ�.
            if (GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex) != "End")
            {
                GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex += 1;
                mDialogImageIndex++;
                sGuestSpriteRender.sprite = sGuestImageArr[mDialogImageIndex];
                isReading = false;
            }
            return;
        }
        tGuestText.text += GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex)[mDialogCharIndex];
        mDialogCharIndex++;

        Invoke("ReadDialogAtOne", 0.05f);
    }

    // �մ԰��� ��ȭ�� ��������ִ� �Լ�
    public void ReadDialog()
    {
        InitDialog();
        Debug.Log(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex);
        // ������ End ���ڿ��� ������ ��� ( ��ȭ�� ��� �ҷ��� ���)
        if (GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex) == "End")
        {
            isLastDialog = true;
            // ��ȭ ������ ��� ����ϰ� ���� �մ� ���뿡 ���� ���θ� �÷��̾�� ���´�. (�޴´�/ ���� �ʴ´�)
            TakeGuest();
            return;
        }
        // ��ȭ�� ������� ���߿� Ŭ���� ���, ������ �ѹ��� ����� �ȴ�.
        if (isReading == true)
        {
            ReadDialogAtAll();
            return;
        }
        // �⺻������ �� �ؽ�Ʈ���� ��ȭ ������ �� ���ھ� �߰��Ͽ� ����ϰ� ������ �ϱ⸦ �ݺ��Ѵ�.
        ReadDialogAtOne();
        return;
  
        // ��� ������� ����Ͽ����� �����Ѵ�.
        // DialogIndex �� �ʱ�ȭ ���� �ʴ� �̻�, ��ȭ�� ���� Ȥ�� ���ķ� �Ѿ�� �ʱ� ������ �켱�� �����ϴ� ������ ���� ��.
    }

    private void TakeGuest()
    {
        gTakeGuestPanel.SetActive(true);
    }

    // �մ� �����ϱ�
    public void AcceptGuest()
    {
        Debug.Log("�մ��� �޽��ϴ�.");
        mSOWManager.InsertGuest(mGuestNum);
        mSOWManager.isNewGuest = true;

        mGuestManager.InitGuestTime();
        // �մ��� �̵������Ƿ� �����ǿ� �ִ� �͵��� �ʱ�ȭ �����ش�.
        ClearGuest();
        MoveScenetoWeatherSpace();
    }

    // �մ� �����ϱ�
    public void RejectGuest()
    {
        Debug.Log("�մ��� ���� �ʽ��ϴ�.");

        // �湮���� �ʴ� Ƚ���� 3���� �����Ѵ�. (3�ϰ� �湮 X)
        mGuestManager.mGuestInfos[mGuestNum].mNotVisitCount = 3;
        mGuestManager.InitGuestTime();

        // �մ��� �̵������Ƿ� �����ǿ� �ִ� �͵��� �ʱ�ȭ �����ش�.
        ClearGuest();
        MoveScenetoWeatherSpace();
    }

    // �������� �ʱ�ȭ �����ش�.
    private void ClearGuest()
    {
        // �湮Ƚ�� 1ȸ ����
        mGuestManager.mGuestInfos[mGuestNum].mVisitCount++;

        // �մ��� �����ǿ� ���ٰ� ǥ��
        mGuestManager.isGuestInLivingRoom = false;

        // ��ȭ �ε����� 0���� �ʱ�ȭ
        GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex = 0;

        // ��ȭ �г��� ��Ȱ��ȭ
        gTextPanel.SetActive(false);

        // �մ� �̹����� ��Ȱ��ȭ
    }
}
// �߰��� ��� �������

// ��ȭ �����̳� ��ȭ ���� ���� �Ϸ簡 ����Ǵ� ��� �ش� ��Ƽ�� �湮 �̷��� ���� ������ ó��
// -> ����, ���� ��ư ������ �湮�� ������ �߰���Ű�� ������� ���� 

// ����, ���� ��ư�� �������� ���ȭ���� ���̵�ƿ� �Ǹ鼭 ��ư�� ���̶���Ʈ �Ǿ�� ��.