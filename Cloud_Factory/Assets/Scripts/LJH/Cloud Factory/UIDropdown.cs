using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDropdown : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown mDropdown; // ��Ӵٿ�

    public int mDropdownIndex;

    public InventoryContainer inventoryContainer;
    // 20���� �����ε� �ϴ� �����̲��� ��ȹ�� ���� ���� Ʋ���� ��������..
    private string[]     mEmotionArray = new string[20] 
    {
        "���",      "�Ҿ�",   "����",      "¥��",     "����",
        "���,ȥ��", "����",   "����,���", "���",     "������ȭ��",
        "��ܽ�",    "�ݴ�",   "��",      "���",     "���ݼ�",
        "��õ",      "������", "����",      "������", "ȥ��������"
    };

    void Awake()
    {
        // �ʱ�ȭ
        mDropdown.ClearOptions();

        // ���ο� �ɼ� ������ ���� OptionData ����
        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        // array�� �ִ� ���ڿ� ������ ����
        foreach (string str in mEmotionArray)
        {
            optionList.Add(new TMP_Dropdown.OptionData(str));
        }

        // ������ optionlist�� dropdown�� �ɼ� ���� �߰�
        mDropdown.AddOptions(optionList);

        // ���� dropdown�� ���õ� �ɼ��� 0������ ����
        mDropdown.value = 0;
    }

    public void OnDropdownEvent(int index)
    {
        mDropdownIndex = index;
        Debug.Log("���� ��Ӵٿ� �ε��� : " + mDropdownIndex);
        Debug.Log("���⼭ �ε����� ���� ������ �� �������� ���� �޼ҵ� ȣ��");
        inventoryContainer.OnDropdownEvent();

    }
}

