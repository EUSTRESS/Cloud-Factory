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

    public CloudMakeSystem cloudMakeSystem;
    private Dropdown    mDropdown;    // 드롭다운 클래스

    private InventoryContainer inventoryContainer; //yeram

    void Start()
    {
        mDropdown = GetComponent<Dropdown>();
        inventoryContainer = GameObject.Find("I_Scroll View Inventory").transform.Find("Viewport").transform.Find("Content").GetChild(0).GetComponent<InventoryContainer>();

    }
    void Update()
    {
        // 화살표 플립
        if (mTemplate.activeSelf) mArrow.sprite = mDropBoxUp;
        else if (!mTemplate.activeSelf) mArrow.sprite = mDropBoxDown;
    }

    // 감정으로 정렬
    public void SortEmotion()
    {
        if (!mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf)
        {
            mSortDropBox.interactable = true;
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(true);

            Debug.Log("현재 인덱스 : " + mDropdown.mDropdownIndex);
            Debug.Log("인덱스 받아와서 현재 적용되어 있는 인덱스로 감정별 정렬 메소드 호출");
            inventoryContainer.OnDropdownEvent();
        }
        else
        {
            mSortDropBox.interactable = false;
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(false);
            inventoryContainer.cancelDropdownEvent();
        }
    }

    // 돌아가기 버튼
    public void GoToCloudFactory()
    {
        LoadingSceneController.Instance.LoadScene("Cloud Factory");
    }
    public void MakeCloud()
    {
		Debug.Log("구름 제작 메소드 호출");

		bool isMakingCloud = GameObject.Find("I_CloudeGen").GetComponent<CloudMakeSystem>().isMakingCloud;
		bool isMtrlListEmpty = GameObject.Find("I_CloudeGen").GetComponent<CloudMakeSystem>().d_selectMtrlListEmpty();

		// 이미 구름을 조합 중이거나, 조합칸에 재료가 없을 때 버튼을 눌러도 아무 일도 일어나지 않도록 한다.
        // 구름이 이미 만들어진 상태는 애초에 더이상 재료를 조합칸에 넣을 수 없기 때문에 따라 return 처리 하지 않음
		if (isMakingCloud || isMtrlListEmpty) { Debug.Log("조합이 불가능합니다."); return; }     
		cloudMakeSystem.E_createCloud(EventSystem.current.currentSelectedGameObject.name);
    }
}
