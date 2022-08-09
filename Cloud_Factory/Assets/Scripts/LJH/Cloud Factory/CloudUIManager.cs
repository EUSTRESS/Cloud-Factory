using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum ECheckBox // 체크박스 인덱스
{
    Date,
    Number,
    Emotion
}

// 구름 공장 UI
public class CloudUIManager : MonoBehaviour
{    
    public Sprite       mDropBoxDown; // 화살표 아래
    public Sprite       mDropBoxUp;   // 화살표 위

    public Image        mArrow;       // 드롭 박스 화살표

    public GameObject   mTemplate;    // 드롭 박스 내용
    public GameObject[] mGiveCloudCheckBox = new GameObject[3]; // 구름 제공화면 체크 박스 3개

    public TMP_Dropdown mSortDropBox; // 드롭박스

    private Dropdown mDropdown;       // 드롭다운 클래스

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

    // 보관 기간 별로 정렬
    public void SortStorageDate()
    {
        if (!mGiveCloudCheckBox[(int)ECheckBox.Date].activeSelf)
        {
            mSortDropBox.interactable = false;
            mGiveCloudCheckBox[(int)ECheckBox.Date].SetActive(true);
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(false);

            Debug.Log("보관 기간별로 정렬 메소드 호출");
        }
        else
        {
            ToggleCheckBox(mGiveCloudCheckBox[(int)ECheckBox.Date]);
        }

    }

    // 감정으로 정렬
    public void SortEmotion()
    {
        if (!mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf)
        {
            mSortDropBox.interactable = transform;
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(true);
            mGiveCloudCheckBox[(int)ECheckBox.Date].SetActive(false);

            Debug.Log("현재 인덱스 : " + mDropdown.mDropdownIndex);
            Debug.Log("인덱스 받아와서 현재 적용되어 있는 인덱스로 감정별 정렬 메소드 호출");
        }
        else
        {
            ToggleCheckBox(mGiveCloudCheckBox[(int)ECheckBox.Emotion]);
        }
    }

    // 체크표시 토글
    void ToggleCheckBox(GameObject CheckBox)
    {
        if (!CheckBox.activeSelf)
            CheckBox.SetActive(true);
        else
            CheckBox.SetActive(false);
    }
    // 드롭박스 활성화 토글
    void ToggleDropBox(TMP_Dropdown DropDown)
    {
        if (!DropDown.interactable)
            DropDown.interactable = true;
        else
            DropDown.interactable = false;
    }
    // 돌아가기 버튼
    public void GoToCloudFactory()
    {
        LoadingSceneController.Instance.LoadScene("Cloud Factory");
    }
}
