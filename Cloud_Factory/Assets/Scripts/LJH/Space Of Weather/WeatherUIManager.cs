using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class WeatherUIManager : MonoBehaviour
{
    private SeasonDateCalc mSeason; // ���� ��� ��ũ��Ʈ

    [Header("Gather")]
    public GameObject mGuideGather; // ä���Ұ��� ���Ұ��� �˷��ִ� UI
    public GameObject mGathering;   // ä�� �� ����ϴ� UI
    public GameObject mGatherResult;// ä�� ����� ����ϴ� UI

    public GameObject[] mSeasonObj = new GameObject[4]; // 4���� ������Ʈ

    public Animator mGatheringAnim; // ä�� �ִϸ��̼�

    public Text tGatheringText;      // ä�� ��... �ؽ�Ʈ
    private int mGatheringTextCount; // ä�� �� '.' ��� ����

    public RectTransform mGatherImageRect; // ä�� �̹��� Rect Transform

    public RectTransform[] mFxShine = new RectTransform[5]; // 5���� ä�� ��� ȸ�� ȿ��
    public RectTransform[] mGatherRect = new RectTransform[5]; // 5���� ä�� ��� UI �̵�
    public GameObject[] mGatherObj = new GameObject[5]; // 5���� ä�� ���� ������Ʈ

    public int mRandomGather; // ��� ä�� ���� ����

    [Header("BackGround")]
    public Image iMainBG; // ���� ��� �̹��� 
    public Sprite[] mBackground = new Sprite[4]; // �������� �޶����� ���

    //����
    private GameObject selectedYard;
    private void Awake()
    {
        mSeason = GameObject.Find("Season Date Calc").GetComponent<SeasonDateCalc>();
    }

    void Update()
    {
        if (mGatherResult.activeSelf)
        {
            // ä�� ��� ȿ��
            mFxShine[0].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[1].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[2].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[3].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[4].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
        }

        switch (mSeason.mSeason)
        {
            case 1:
                UpdateSeasonBg(0, 3);// ��
                break;
            case 2:
                UpdateSeasonBg(1, 0);// ����
                break;
            case 3:
                UpdateSeasonBg(2, 1);// ����
                break;
            case 4:
                UpdateSeasonBg(3, 2); // �ܿ�
                break;
            default:
                break;
        }
    }

    void UpdateSeasonBg(int _iCurrent, int _iPrev)
    {
        iMainBG.sprite = mBackground[_iCurrent];
        mSeasonObj[_iPrev].SetActive(false);
        mSeasonObj[_iCurrent].SetActive(true);
    }

    // ���� ��ư Ŭ�� ��, ä���Ͻðھ��ϱ�? ������Ʈ Ȱ��ȭ    
    public void OpenGuideGather()
    {
        selectedYard = EventSystem.current.currentSelectedGameObject;
        mGuideGather.SetActive(true);
    }
    // ������, ä���Ͻðھ��ϱ�? ������Ʈ ��Ȱ��ȭ    
    public void CloseGuideGather()
    {
        mGuideGather.SetActive(false);
    }
    // ä���ϱ�
    public void GoingToGather()
    {
        mGuideGather.SetActive(false);
        mGathering.SetActive(true);
        mGatheringTextCount = 0; // �ʱ�ȭ
        tGatheringText.text = "��� ä�� ��"; // �ʱ�ȭ

        if (SeasonDateCalc.Instance) // null check
        {                            // �� �ش��ϴ� �ִϸ��̼� ���
            Invoke("PrintGatheringText", 0.5f); // 0.5�� �����̸��� . �߰�
            if (SeasonDateCalc.Instance.mSeason == 1) // ���̶��
                UpdateGatherAnim(1090, 590, true, false, false, false);
            else if (SeasonDateCalc.Instance.mSeason == 2) // �����̶��
                UpdateGatherAnim(1090, 590, false, true, false, false);
            else if (SeasonDateCalc.Instance.mSeason == 3) // �����̶��
                UpdateGatherAnim(735, 420, false, false, true, false);
            else if (SeasonDateCalc.Instance.mSeason == 4) // �ܿ��̶��
                UpdateGatherAnim(560, 570, false, false, false, true);
        }
        // 5�� ���� ä�� �� ��� ���
        Invoke("Gathering", 5.0f);
    }
    
    void UpdateGatherAnim(int _iX, int _iY, bool _bSpring, bool _bSummer, bool _bFall, bool _bWinter)
    {
        mGatherImageRect.sizeDelta = new Vector2(_iX, _iY); // �̹��� ������ ���߱�
        mGatheringAnim.SetBool("Spring", _bSpring);
        mGatheringAnim.SetBool("Summer", _bSummer);
        mGatheringAnim.SetBool("Fall", _bFall);
        mGatheringAnim.SetBool("Winter", _bWinter);
    }

    void Gathering()
    {
        YardHandleSystem system = selectedYard.GetComponentInParent<YardHandleSystem>();

        mRandomGather = Random.Range(0, 5); // 0~4

        system.Gathered(selectedYard, mRandomGather);
        // ���� �۾�


        if (mRandomGather % 2 == 1) // Ȧ��
        {
            mGatherRect[0].anchoredPosition = new Vector3(125.0f, 0.0f, 0.0f);
            mGatherRect[1].anchoredPosition = new Vector3(-125.0f, 0.0f, 0.0f);
            mGatherRect[2].anchoredPosition = new Vector3(375.0f, 0.0f, 0.0f);
            mGatherRect[3].anchoredPosition = new Vector3(-375.0f, 0.0f, 0.0f);
        }
        else
        {
            mGatherRect[0].anchoredPosition = new Vector3(0, 0.0f, 0.0f);
            mGatherRect[1].anchoredPosition = new Vector3(-225.0f, 0.0f, 0.0f);
            mGatherRect[2].anchoredPosition = new Vector3(225.0f, 0.0f, 0.0f);
            mGatherRect[3].anchoredPosition = new Vector3(-450.0f, 0.0f, 0.0f);
        }

        switch (mRandomGather) // active ����
        {
            case 0:
                ActiveRandGather(true, false, false, false, false);
                break;
            case 1:
                ActiveRandGather(true, true, false, false, false);
                break;
            case 2:
                ActiveRandGather(true, true, true, false, false);
                break;
            case 3:
                ActiveRandGather(true, true, true, true, false);
                break;
            case 4:
                ActiveRandGather(true, true, true, true, true);
                break;
            default:
                break;
        }


        mGathering.SetActive(false);
        mGatherResult.SetActive(true);

        CancelInvoke(); // �κ�ũ �浹 ������ ���ؼ� ��� ����� ������ ��� �κ�ũ ������
    }

    void ActiveRandGather(bool _bOne, bool _bTwo, bool _bThree, bool _bFour, bool _bFive)
    {
        mGatherObj[0].SetActive(_bOne);
        mGatherObj[1].SetActive(_bTwo);
        mGatherObj[2].SetActive(_bThree);
        mGatherObj[3].SetActive(_bFour);
        mGatherObj[4].SetActive(_bFive);
    }

    // ����Լ��� ��ħǥ�� ��������� ����Ѵ�
    void PrintGatheringText()
    {
        mGatheringTextCount++;
        tGatheringText.text = tGatheringText.text + ".";

        if (mGatheringTextCount <= 3)
        {
            Invoke("PrintGatheringText", 0.25f); // 0.25�� �����̸��� . �߰�
        }
        else // �ʱ�ȭ
        {
            mGatheringTextCount = 0;
            tGatheringText.text = "��� ä�� ��";
            Invoke("PrintGatheringText", 0.25f); // 0.25�� �����̸��� . �߰�
        }
    }
    // ä�� ��!
    public void CloseResultGather()
    {
        mGatherResult.SetActive(false);        
    }
}
