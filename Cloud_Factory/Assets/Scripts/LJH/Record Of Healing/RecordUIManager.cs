using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ġ���� ��� UI
public class RecordUIManager : MonoBehaviour
{
    [Header("GAME OBJECT")]
    // ġ���� ���
    public GameObject gShowUpset; // �Ҹ� ��Ƽ����
    public GameObject gShowAll;   // ��ü ���� 
    public GameObject[] gStampF = new GameObject[4]; // �Ҹ� ��Ƽ ������
    // ġ���Ǳ��
    public Image[] iProfileBG = new Image[4]; // ������ ���

    [Header("SPRITES")]
    public Sprite[] sBasicProfile = new Sprite[4]; // �⺻ ������
    public Sprite[] sUpsetProfile = new Sprite[4]; // ȭ�� ������

    [Header("BUTTON")]

    //ġ���� ���     
    public Button btNextBtn;           // ���������� ��ư
    public Button btPrevBtn;           // ���������� ��ư

    [HideInInspector]
    public bool mIsNext;               // ġ���� ��� ���� ������   
    [HideInInspector]
    public bool mIsPrev;               // ġ���� ��� ���� ������

    private ProfileMoving mProfile1;   // ������ ������ ��� ��ũ��Ʈ
    private ProfileMoving mProfile2;   // ������ ������ ��� ��ũ��Ʈ
    private ProfileMoving mProfile3;   // ������ ������ ��� ��ũ��Ʈ

    // **********�ӽ÷ν� ������ ��***********
    public Camera MainCam; // ī�޶�� ��� ���� ������
    // **********�ӽ÷ν� ������ ��***********

    void Awake()
    {
        mProfile1 = GameObject.Find("I_ProfileBG1").GetComponent<ProfileMoving>();
        mProfile2 = GameObject.Find("I_ProfileBG2").GetComponent<ProfileMoving>();
        mProfile3 = GameObject.Find("I_ProfileBG3").GetComponent<ProfileMoving>();
    }

    public void ShowNextProfile()
    {
        // ���� ������
        mIsNext = true;
        mIsPrev = false;

        mProfile1.mIsMoving = true;
        mProfile2.mIsMoving = true;
        mProfile3.mIsMoving = true;

        // ���� ������ ���� ��ư ��Ȱ��ȭ
        btNextBtn.interactable = false;
        btPrevBtn.interactable = false;
        // �Ҹ� ��Ƽ�� �� ��
        if (mProfile1.mIsUpset && mProfile2.mIsUpset && mProfile3.mIsUpset)
            Invoke("DelayActiveBtn", 0.5f);  // Ȱ��ȭ ������
        // ��ü ��Ƽ �� ��
        else if (!mProfile1.mIsUpset && !mProfile2.mIsUpset && !mProfile3.mIsUpset)
            Invoke("DelayActiveBtn", 1.0f);  // Ȱ��ȭ ������        

        // ���� ��Ƽ ���� �ҷ����� �޼ҵ� ȣ���ϴ� �κ�
        Debug.Log("���� ��Ƽ ���� ȣ��");
    }
    public void ShowPrevProfile()
    {
        // ���� ������
        mIsNext = false;
        mIsPrev = true;

        mProfile1.mIsMoving = true;
        mProfile2.mIsMoving = true;
        mProfile3.mIsMoving = true;

        // ���� ������ ���� ��ư ��Ȱ��ȭ
        btNextBtn.interactable = false;
        btPrevBtn.interactable = false;
        Invoke("DelayActiveBtn", 0.5f);

        // ���� ��Ƽ ���� �ҷ����� �޼ҵ� ȣ���ϴ� �κ�
        Debug.Log("���� ��Ƽ ���� ȣ��");
    }
    void DelayActiveBtn()
    {
        btNextBtn.interactable = true;
        btPrevBtn.interactable = true;
    }
    // �Ҹ� ��Ƽ �����ִ� �Լ�
    public void ShowUpsetMoongti()
    {
        // ȭ��������� ��������Ʈ ��ü
        for (int index = 0; index < iProfileBG.Length; index++)
        {
            iProfileBG[index].sprite = sUpsetProfile[index];
        }
        // �� ����
        iProfileBG[0].color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        iProfileBG[1].color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        iProfileBG[2].color = new Color(255f / 255f, 255f / 255f, 255f / 255f);

        mProfile1.mIsUpset = true;
        mProfile2.mIsUpset = true;
        mProfile3.mIsUpset = true;

        gShowAll.SetActive(true);
        gShowUpset.SetActive(false);

        MainCam.backgroundColor = new Color(222f / 255f, 219f / 255f, 217f / 255f);

        for (int index = 0; index < gStampF.Length; index++)
        { // stamp Ȱ��ȭ
            gStampF[index].SetActive(true);
        }

        // ��Ƽ ���� �ҷ����� �޼ҵ� ȣ���ϴ� �κ�
        Debug.Log("�Ҹ� ��Ƽ ���� �ҷ����� �޼ҵ� ȣ��");
    }
    // ��ü ����
    public void ShowAllMoongti()
    {
        // �⺻������� ��������Ʈ ��ü
        for (int index = 0; index < iProfileBG.Length; index++)
        {
            iProfileBG[index].sprite = sBasicProfile[index];
        }
        // �� ������ ��
        iProfileBG[0].color = new Color(235f / 255f, 246f / 255f, 255f / 255f);
        iProfileBG[1].color = new Color(255f / 255f, 237f / 255f, 253f / 255f);
        iProfileBG[2].color = new Color(255f / 255f, 255f / 255f, 239f / 255f);

        mProfile1.mIsUpset = false;
        mProfile2.mIsUpset = false;
        mProfile3.mIsUpset = false;

        gShowAll.SetActive(false);
        gShowUpset.SetActive(true);

        MainCam.backgroundColor = new Color(194f / 255f, 216f / 255f, 233f / 255f);

        for (int index = 0; index < gStampF.Length; index++)
        { // stamp ��Ȱ��ȭ
            gStampF[index].SetActive(false);
        }

        // ��Ƽ ���� �ҷ����� �޼ҵ� ȣ���ϴ� �κ�
        Debug.Log("��ü ��Ƽ ���� �ҷ����� �޼ҵ� ȣ��");
    }

    public void GiveCloud()
    {
        // ���� �����ϴ� �޼ҵ� ȣ��
        Debug.Log("�������� �޼ҵ� ȣ��");
    }
}