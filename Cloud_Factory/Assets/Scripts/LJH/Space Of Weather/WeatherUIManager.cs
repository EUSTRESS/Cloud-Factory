using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherUIManager : MonoBehaviour
{
    public GameObject mGuideGather; // ä���Ұ��� ���Ұ��� �˷��ִ� UI
    public GameObject mGathering;   // ä�� �� ����ϴ� UI
    public GameObject mGatherResult;// ä�� ����� ����ϴ� UI

    public Animator mGatheringAnim; // ä�� �ִϸ��̼�

    public Text tGatheringText;      // ä�� ��... �ؽ�Ʈ
    private int mGatheringTextCount; // ä�� �� '.' ��� ����

    public RectTransform mGatherImageRect; // ä�� �̹��� Rect Transform

    public RectTransform[] mFxShine = new RectTransform[3]; // 3���� ä�� ��� ȸ�� ȿ��
    public List<GameObject> mGatherUI = new List<GameObject>(); // 3���� ä�� ��� ȸ�� ȿ��

    void Update()
    {
        if (mGatherResult.activeSelf)
        {
            // ä�� ��� ȿ��
            mFxShine[0].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[1].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[2].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
        }
    }

    //����: �ܺο��� ä�� ��� ����̹��� �����ϴ� �Լ�
    public void ChangeGatherResultImg(Dictionary<IngredientData, int> results)
    {
        int idx = 0;
        foreach(KeyValuePair<IngredientData,int> result in results)
        {
            if (idx == 3) break;
            mGatherUI[idx].GetComponent<Image>().sprite = result.Key.image;
            mGatherUI[idx].transform.GetChild(0).GetComponent<Text>().text = result.Value.ToString();
            idx++;
        }
    }

    // ���� ��ư Ŭ�� ��, ä���Ͻðھ��ϱ�? ������Ʈ Ȱ��ȭ    
    public void OpenGuideGather()
    {
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
        Debug.Log("ä���� ���� ȣ��");

        mGuideGather.SetActive(false);
        mGathering.SetActive(true);
        mGatheringTextCount = 0; // �ʱ�ȭ
        tGatheringText.text = "��� ä�� ��"; // �ʱ�ȭ

        if (SeasonDateCalc.Instance) // null check
        {                            // �� �ش��ϴ� �ִϸ��̼� ���
            Invoke("PrintGatheringText", 0.5f); // 0.5�� �����̸��� . �߰�
            if (SeasonDateCalc.Instance.mSeason == 1) // ���̶��
            {
                mGatherImageRect.sizeDelta = new Vector2(1090, 590); // �̹��� ������ ���߱�
                
                mGatheringAnim.SetBool("Spring", true);
                mGatheringAnim.SetBool("Summer", false);
                mGatheringAnim.SetBool("Fall", false);
                mGatheringAnim.SetBool("Winter", false);
            }
            else if (SeasonDateCalc.Instance.mSeason == 2) // �����̶��
            {
                mGatherImageRect.sizeDelta = new Vector2(1090, 590); // �̹��� ������ ���߱�

                mGatheringAnim.SetBool("Spring", false);
                mGatheringAnim.SetBool("Summer", true);
                mGatheringAnim.SetBool("Fall", false);
                mGatheringAnim.SetBool("Winter", false);
            }
            else if (SeasonDateCalc.Instance.mSeason == 3) // �����̶��
            {
                mGatherImageRect.sizeDelta = new Vector2(735, 420); // �̹��� ������ ���߱�

                mGatheringAnim.SetBool("Spring", false);
                mGatheringAnim.SetBool("Summer", false);
                mGatheringAnim.SetBool("Fall", true);
                mGatheringAnim.SetBool("Winter", false);
            }
            else if (SeasonDateCalc.Instance.mSeason == 4) // �ܿ��̶��
            {
                mGatherImageRect.sizeDelta = new Vector2(560, 570); // �̹��� ������ ���߱�

                mGatheringAnim.SetBool("Spring", false);
                mGatheringAnim.SetBool("Summer", false);
                mGatheringAnim.SetBool("Fall", false);
                mGatheringAnim.SetBool("Winter", true);
            }
        }
        // 5�� ���� ä�� �� ��� ���
        Invoke("Gathering", 5.0f);
    }
    void Gathering()
    {
        Debug.Log("ä�� �����, �κ��丮�� ���� ȣ��");

        mGathering.SetActive(false);
        mGatherResult.SetActive(true);

        CancelInvoke(); // �κ�ũ �浹 ������ ���ؼ� ��� ����� ������ ��� �κ�ũ ������
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
