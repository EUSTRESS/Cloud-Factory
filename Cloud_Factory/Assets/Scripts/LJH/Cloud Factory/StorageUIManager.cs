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

    private Dropdown    mDropdown;    // 드롭다운 클래스
    void Start()
    {
        mDropdown = GetComponent<Dropdown>();
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
        }
        else
        {
            mSortDropBox.interactable = false;
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(false);
        }
    }

    // 돌아가기 버튼
    public void GoToCloudFactory()
    {
        LoadingSceneController.Instance.LoadScene("Cloud Factory");
    }
}
