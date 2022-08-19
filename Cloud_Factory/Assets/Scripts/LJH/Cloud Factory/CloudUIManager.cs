using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum ECheckBox // üũ�ڽ� �ε���
{
    Date,
    Number,
    Emotion
}

// ���� ���� UI
public class CloudUIManager : MonoBehaviour
{    
    public Sprite       mDropBoxDown; // ȭ��ǥ �Ʒ�
    public Sprite       mDropBoxUp;   // ȭ��ǥ ��

    public Image        mArrow;       // ��� �ڽ� ȭ��ǥ

    public GameObject   mTemplate;    // ��� �ڽ� ����
    public GameObject[] mGiveCloudCheckBox = new GameObject[3]; // ���� ����ȭ�� üũ �ڽ� 3��

    public TMP_Dropdown mSortDropBox; // ��ӹڽ�

    private Dropdown mDropdown;       // ��Ӵٿ� Ŭ����

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

    // ���� �Ⱓ ���� ����
    public void SortStorageDate()
    {
        if (!mGiveCloudCheckBox[(int)ECheckBox.Date].activeSelf)
        {
            mSortDropBox.interactable = false;
            mGiveCloudCheckBox[(int)ECheckBox.Date].SetActive(true);
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(false);

            Debug.Log("���� �Ⱓ���� ���� �޼ҵ� ȣ��");
        }
        else
        {
            ToggleCheckBox(mGiveCloudCheckBox[(int)ECheckBox.Date]);
        }

    }

    // �������� ����
    public void SortEmotion()
    {
        if (!mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf)
        {
            mSortDropBox.interactable = transform;
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(true);
            mGiveCloudCheckBox[(int)ECheckBox.Date].SetActive(false);

            Debug.Log("���� �ε��� : " + mDropdown.mDropdownIndex);
            Debug.Log("�ε��� �޾ƿͼ� ���� ����Ǿ� �ִ� �ε����� ������ ���� �޼ҵ� ȣ��");
        }
        else
        {
            ToggleCheckBox(mGiveCloudCheckBox[(int)ECheckBox.Emotion]);
            ToggleDropBox(mSortDropBox);
        }
    }

    // üũǥ�� ���
    void ToggleCheckBox(GameObject CheckBox)
    {
        if (!CheckBox.activeSelf)
            CheckBox.SetActive(true);
        else
            CheckBox.SetActive(false);
    }
    // ��ӹڽ� Ȱ��ȭ ���
    void ToggleDropBox(TMP_Dropdown DropDown)
    {
        if (!DropDown.interactable)
            DropDown.interactable = true;
        else
            DropDown.interactable = false;
    }
    // ���ư��� ��ư
    public void GoToCloudFactory()
    {
        LoadingSceneController.Instance.LoadScene("Cloud Factory");
    }
}
