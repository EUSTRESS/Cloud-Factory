using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

// 보관 인벤토리 UI
public class StorageUIManager : MonoBehaviour
{
    public Sprite       mDropBoxDown; // 화살표 아래
    public Sprite       mDropBoxUp;   // 화살표 위

    public Image        mArrow;       // 드롭 박스 화살표

    public GameObject   mTemplate;    // 드롭 박스 내용
    public GameObject[] mGiveCloudCheckBox = new GameObject[3]; // 구름 제공화면 체크 박스 2개

    public TMP_Dropdown mSortDropBox; // 드롭박스

    private UIDropdown    mDropdown;    // 드롭다운 클래스
    void Start()
    {
        mDropdown = GetComponent<UIDropdown>();
        // 기본 정렬
        // 개수별로 체크박스 활성화 --> 메소드 호출
        if (mGiveCloudCheckBox[(int)ECheckBox.Number].activeSelf == true)
        {
            Debug.Log("개수 별로 정렬 메소드 호출");
        }
    }
    void Update()
    {
        // 화살표 플립
        if (mTemplate.activeSelf) mArrow.sprite = mDropBoxUp;
        else if (!mTemplate.activeSelf) mArrow.sprite = mDropBoxDown;
    }

    // 개수 별로 정렬
    public void SortNumber()
    {
        // 호출 최소화, 여기는 2개밖에 없어서 그냥 이렇게 해도 됌
        // 하나는 무조건 활성화 되어있어야 하기 때문에
        if (!mGiveCloudCheckBox[(int)ECheckBox.Number].activeSelf)
        {
            mSortDropBox.interactable = false;
            mGiveCloudCheckBox[(int)ECheckBox.Number].SetActive(true);
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(false);

            Debug.Log("개수 별로 정렬 메소드 호출");
            InventoryContainer inventoryContainer = gameObject.GetComponent<UIDropdown>().inventoryContainer;
            inventoryContainer.sortWithCnt();
        }        
    }
    // 감정으로 정렬
    public void SortEmotion()
    {
        InventoryContainer inventoryContainer = gameObject.GetComponent<UIDropdown>().inventoryContainer;
        inventoryContainer.activeDropDown();

        // 호출 최소화
        if (!mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf)
        {

            mSortDropBox.interactable = true;
            mGiveCloudCheckBox[(int)ECheckBox.Number].SetActive(false);
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(true);

            Debug.Log("현재 인덱스 : " + mDropdown.mDropdownIndex);
            Debug.Log("인덱스 받아와서 현재 적용되어 있는 인덱스로 감정별 정렬 메소드 호출");    
        }
    }

    // 돌아가기 버튼
    public void GoToCloudFactory()
    {
        SceneManager.LoadScene("Cloud Factory");
    }

    // 구름 제작 버튼
    public void MakeCloud()
    {
        Debug.Log("구름 제작 호출");
    }
}
