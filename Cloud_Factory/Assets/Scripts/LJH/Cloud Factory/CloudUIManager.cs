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

public class CloudUIManager : MonoBehaviour
{    
    public Sprite       mDropBoxDown; // 화살표 아래
    public Sprite       mDropBoxUp;   // 화살표 위

    public Image        mArrow;       // 드롭 박스 화살표

    public GameObject   mTemplate;    // 드롭 박스 내용
    public GameObject[] mGiveCloudCheckBox = new GameObject[3]; // 구름 제공화면 체크 박스 3개

    public TMP_Dropdown mSortDropBox; // 드롭박스

    void Update()
    {
        // 화살표 플립
        if (mTemplate.activeSelf) mArrow.sprite = mDropBoxUp;
        else if (!mTemplate.activeSelf) mArrow.sprite = mDropBoxDown;
    }

    // 보관 기간 별로 정렬
    public void SortStorageDate()
    {
        Debug.Log("보관 기간별로 정렬");

        // 감정별로 정렬 + 다른 기능을 사용하는 경우
        if (!mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf)
        {
            mSortDropBox.interactable = false;
            mGiveCloudCheckBox[(int)ECheckBox.Date].SetActive(true);
        }
        else
            ToggleCheckBox(mGiveCloudCheckBox[(int)ECheckBox.Date]);

        mGiveCloudCheckBox[(int)ECheckBox.Number].SetActive(false);
    }
    // 개수 별로 정렬
    public void SortNumber()
    {
        Debug.Log("개수 별로 정렬");

        // 감정별로 정렬 + 다른 기능을 사용하는 경우
        if (!mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf)
        {
            mSortDropBox.interactable = false;
            mGiveCloudCheckBox[(int)ECheckBox.Number].SetActive(true);
        }
        else
            ToggleCheckBox(mGiveCloudCheckBox[(int)ECheckBox.Number]);

        mGiveCloudCheckBox[(int)ECheckBox.Date].SetActive(false);
    }
    // 감정으로 정렬
    public void SortEmotion()
    {
        Debug.Log("감정별로 정렬");
        if (!mGiveCloudCheckBox[(int)ECheckBox.Date].activeSelf
         && !mGiveCloudCheckBox[(int)ECheckBox.Number].activeSelf)
        {
            // 다 꺼지면 안돼
        }
        else
        {
            ToggleDropBox(mSortDropBox);
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
        SceneManager.LoadScene("Cloud Factory");
    }
}
