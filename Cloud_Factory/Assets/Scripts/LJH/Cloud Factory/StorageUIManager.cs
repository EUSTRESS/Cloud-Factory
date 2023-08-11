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

    public Image        mMakeLoadingSlider;
    private float       mLoadingCoolDownTime = 2.0f;
    private float       mUpdateTime = 0.0f;
    private float       mUpdateLoadingPer = 0.0f;

    public GameObject   mTemplate;    // 드롭 박스 내용
    public GameObject[] mGiveCloudCheckBox = new GameObject[3]; // 구름 제공화면 체크 박스 2개

    public TMP_Dropdown mSortDropBox; // 드롭박스

    public CloudMakeSystem cloudMakeSystem;
    private Dropdown    mDropdown;    // 드롭다운 클래스   

    private AudioSource mSFx;          // 효과음 오디오 소스 예시, 기본 효과음
    private AudioSource mBGM;

    private InventoryContainer inventoryContainer; //yeram

    private void Awake()
    {
        mSFx = GameObject.Find("mSFx").GetComponent<AudioSource>();
        mBGM = GameObject.Find("mBGM").GetComponent<AudioSource>();

        if (SceneData.Instance) // null check
        {
            // 씬이 변경될 때 저장된 값으로 새로 업데이트
            mBGM.volume = SceneData.Instance.BGMValue;
            mSFx.volume = SceneData.Instance.SFxValue;
            //mSubSFx.volume = SceneData.Instance.SFxValue;
        }
    }

    void Start()
    {
        mDropdown = GetComponent<Dropdown>();
        inventoryContainer = GameObject.Find("I_Scroll View Inventory").transform.Find("Viewport").transform.Find("Content").GetChild(0).GetComponent<InventoryContainer>();

    }
    void Update()
    {
        // 화살표 플립
        //mSortDropBox.
        //if (mSortDropBox.interactable) { 
        //    mArrow.sprite = mDropBoxUp; 
        //}
        //else if (!mTemplate.activeSelf) {
        //    mArrow.sprite = mDropBoxDown; 
        //}

        UpdateMakeLoading();

        
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

        mSFx.Play();
    }

    // 돌아가기 버튼
    public void GoToCloudFactory()
    {
        TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        InventoryManager mInventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        if (mTutorialManager.isFinishedTutorial[4] == false) 
        {
            if(mInventoryManager.createdCloudData.mEmotions.Count > 0) { mTutorialManager.isFinishedTutorial[4] = true; }
            else { return; }
        }

		bool isMakingCloud = GameObject.Find("I_CloudeGen").GetComponent<CloudMakeSystem>().isMakingCloud;
        if (isMakingCloud) { return; }
		LoadingSceneController.Instance.LoadScene("Cloud Factory");
        mSFx.Play();
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

        mSFx.Play();
    }

    void UpdateMakeLoading()
    {
        if (false == cloudMakeSystem.isMakingCloud)
        {
            mUpdateTime = 0.0f;
            mMakeLoadingSlider.fillAmount = 0.0f;
            mMakeLoadingSlider.enabled = false;
            mUpdateLoadingPer = 0.0f;
            return;
        }

        mMakeLoadingSlider.enabled = true;

        if (mUpdateTime > mLoadingCoolDownTime)
        {
            mUpdateTime = 0.0f;
            mMakeLoadingSlider.fillAmount = 0.0f;
            mUpdateLoadingPer = 0.0f;
        }
        else
        {
            float fSpeed = 0.0f;
            // 시간 연출, 딱 끊기는 느낌이 든다면 lerp함수로 보간하면 됨.
            if (0.8f < mUpdateLoadingPer)
            {
                fSpeed = 0.7f;
            }
            else if (0.2f <= mUpdateLoadingPer)
            {
                fSpeed = 1.4f;
            }
            else if (0.2f > mUpdateLoadingPer)
            {
                fSpeed = 0.7f;
            }

            mUpdateTime += fSpeed * Time.deltaTime;
            mUpdateLoadingPer = mUpdateTime / mLoadingCoolDownTime;
            mMakeLoadingSlider.fillAmount = (Mathf.Lerp(0, 100, mUpdateLoadingPer) / 100);
        }
    }
    public void SFx_Play()
    {
        mSFx.Play();
    }
}
