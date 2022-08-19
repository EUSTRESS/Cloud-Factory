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

    private UIDropdown    mDropdown;    // ��Ӵٿ� Ŭ����
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

    // ���� ���� ����
    public void SortNumber()
    {
        // ȣ�� �ּ�ȭ, ����� 2���ۿ� ��� �׳� �̷��� �ص� ��
        // �ϳ��� ������ Ȱ��ȭ �Ǿ��־�� �ϱ� ������
        if (!mGiveCloudCheckBox[(int)ECheckBox.Number].activeSelf)
        {
            mSortDropBox.interactable = false;
            mGiveCloudCheckBox[(int)ECheckBox.Number].SetActive(true);
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(false);

            Debug.Log("���� ���� ���� �޼ҵ� ȣ��");
            InventoryContainer inventoryContainer = gameObject.GetComponent<UIDropdown>().inventoryContainer;
            inventoryContainer.sortWithCnt();
        }        
    }
    // �������� ����
    public void SortEmotion()
    {
        InventoryContainer inventoryContainer = gameObject.GetComponent<UIDropdown>().inventoryContainer;
        inventoryContainer.activeDropDown();

        // ȣ�� �ּ�ȭ
        if (!mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf)
        {

            mSortDropBox.interactable = true;
            mGiveCloudCheckBox[(int)ECheckBox.Number].SetActive(false);
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(true);

            Debug.Log("���� �ε��� : " + mDropdown.mDropdownIndex);
            Debug.Log("�ε��� �޾ƿͼ� ���� ����Ǿ� �ִ� �ε����� ������ ���� �޼ҵ� ȣ��");    
        }
    }

    // ���ư��� ��ư
    public void GoToCloudFactory()
    {
        SceneManager.LoadScene("Cloud Factory");
    }

    // ���� ���� ��ư
    public void MakeCloud()
    {
        Debug.Log("���� ���� ȣ��");
    }
}
