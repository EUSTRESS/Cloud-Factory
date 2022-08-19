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

    private UIDropdown mDropdown;       // ��Ӵٿ� Ŭ����

    void Start()
    {
        mDropdown = GetComponent<UIDropdown>();
        // �⺻ ����
        // �������� üũ�ڽ� Ȱ��ȭ --> �޼ҵ� ȣ��
        if (mGiveCloudCheckBox[(int)ECheckBox.Number].activeSelf == true)
        {
            Debug.Log("���� ���� ���� �޼ҵ� ȣ��");
        }
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
        // �������� ���� + �ٸ� ����� ����ϴ� ���
        // �ϳ��� ����� ��쿡�� ������ 1�� �̻� �����־�� �ϱ� ������
        if (!mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf)
        {
            mSortDropBox.interactable = false;
            mGiveCloudCheckBox[(int)ECheckBox.Date].SetActive(true);
        }
        else // �������ζ� �ٸ� ��� ����ϸ� ��� ����
             ToggleCheckBox(mGiveCloudCheckBox[(int)ECheckBox.Date]);
            

        mGiveCloudCheckBox[(int)ECheckBox.Number].SetActive(false);

        // �Ⱓ���� üũ�ڽ��� Ȱ��ȭ �Ǿ����� ��쿡 �޼ҵ� ȣ���Ͽ� �����Ѵ�
        if (mGiveCloudCheckBox[(int)ECheckBox.Date].activeSelf == true)
        {
            Debug.Log("���� �Ⱓ���� ���� �޼ҵ� ȣ��");
        }

    }
    // ���� ���� ����
    public void SortNumber()
    {
        // �������� ���� + �ٸ� ����� ����ϴ� ���
        // �ϳ��� ����� ��쿡�� ������ 1�� �̻� �����־�� �ϱ� ������
        if (!mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf)
        {
            mSortDropBox.interactable = false;
            mGiveCloudCheckBox[(int)ECheckBox.Number].SetActive(true);
        }
        else // �������ζ� �ٸ� ��� ����ϸ� ��� ����
            ToggleCheckBox(mGiveCloudCheckBox[(int)ECheckBox.Number]);

        mGiveCloudCheckBox[(int)ECheckBox.Date].SetActive(false);

        // �������� üũ�ڽ� Ȱ��ȭ --> �޼ҵ� ȣ��
        if (mGiveCloudCheckBox[(int)ECheckBox.Number].activeSelf == true)
        {
            Debug.Log("���� ���� ���� �޼ҵ� ȣ��");
        }
    }
    // �������� ����
    public void SortEmotion()
    {
        if (!mGiveCloudCheckBox[(int)ECheckBox.Date].activeSelf
         && !mGiveCloudCheckBox[(int)ECheckBox.Number].activeSelf)
        {
            // �� ������ �ȵ�
        }
        else
        {
            // ��ӹڽ�, üũ�ڽ� ���
            ToggleDropBox(mSortDropBox);
            ToggleCheckBox(mGiveCloudCheckBox[(int)ECheckBox.Emotion]);
        }
        
        // �������� üũ�ڽ� Ȱ��ȭ --> �޼ҵ� ȣ��
        if (mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf == true)
        {
            Debug.Log("���� �ε��� : " + mDropdown.mDropdownIndex);
            Debug.Log("�ε��� �޾ƿͼ� ���� ����Ǿ� �ִ� �ε����� ������ ���� �޼ҵ� ȣ��");
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
        SceneManager.LoadScene("Cloud Factory");
    }
}
