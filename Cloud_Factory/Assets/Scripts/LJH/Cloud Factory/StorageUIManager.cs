using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

// ���� �κ��丮 UI
public class StorageUIManager : MonoBehaviour
{
    public Sprite       mDropBoxDown; // ȭ��ǥ �Ʒ�
    public Sprite       mDropBoxUp;   // ȭ��ǥ ��

    public Image        mArrow;       // ��� �ڽ� ȭ��ǥ

    public GameObject   mTemplate;    // ��� �ڽ� ����
    public GameObject[] mGiveCloudCheckBox = new GameObject[3]; // ���� ����ȭ�� üũ �ڽ� 2��

    public TMP_Dropdown mSortDropBox; // ��ӹڽ�

    private Dropdown    mDropdown;    // ��Ӵٿ� Ŭ����
    void Start()
    {
        mDropdown = GetComponent<Dropdown>();
    }
    void Update()
    {
        // ȭ��ǥ �ø�
        if (mTemplate.activeSelf) mArrow.sprite = mDropBoxUp;
        else if (!mTemplate.activeSelf) mArrow.sprite = mDropBoxDown;
    }

    // �������� ����
    public void SortEmotion()
    {
        if (!mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf)
        {
            mSortDropBox.interactable = true;
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(true);

            Debug.Log("���� �ε��� : " + mDropdown.mDropdownIndex);
            Debug.Log("�ε��� �޾ƿͼ� ���� ����Ǿ� �ִ� �ε����� ������ ���� �޼ҵ� ȣ��");
        }
        else
        {
            mSortDropBox.interactable = false;
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(false);
        }
    }

    // ���ư��� ��ư
    public void GoToCloudFactory()
    {
        LoadingSceneController.Instance.LoadScene("Cloud Factory");
    }
    public void MakeCloud()
    {
        Debug.Log("���� ���� �޼ҵ� ȣ��");
    }
}
