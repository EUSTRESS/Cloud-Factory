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

    private UIDropdown mDropdown;       // 드롭다운 클래스

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

    // 보관 기간 별로 정렬
    public void SortStorageDate()
    {
        // 감정별로 정렬 + 다른 기능을 사용하는 경우
        // 하나만 사용할 경우에는 무조건 1개 이상 켜져있어야 하기 때문에
        if (!mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf)
        {
            mSortDropBox.interactable = false;
            mGiveCloudCheckBox[(int)ECheckBox.Date].SetActive(true);
        }
        else // 감정별로랑 다른 기능 사용하면 토글 가능
             ToggleCheckBox(mGiveCloudCheckBox[(int)ECheckBox.Date]);
            

        mGiveCloudCheckBox[(int)ECheckBox.Number].SetActive(false);

        // 기간별로 체크박스가 활성화 되어있을 경우에 메소드 호출하여 정렬한다
        if (mGiveCloudCheckBox[(int)ECheckBox.Date].activeSelf == true)
        {
            Debug.Log("보관 기간별로 정렬 메소드 호출");
        }

    }
    // 개수 별로 정렬
    public void SortNumber()
    {
        // 감정별로 정렬 + 다른 기능을 사용하는 경우
        // 하나만 사용할 경우에는 무조건 1개 이상 켜져있어야 하기 때문에
        if (!mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf)
        {
            mSortDropBox.interactable = false;
            mGiveCloudCheckBox[(int)ECheckBox.Number].SetActive(true);
        }
        else // 감정별로랑 다른 기능 사용하면 토글 가능
            ToggleCheckBox(mGiveCloudCheckBox[(int)ECheckBox.Number]);

        mGiveCloudCheckBox[(int)ECheckBox.Date].SetActive(false);

        // 개수별로 체크박스 활성화 --> 메소드 호출
        if (mGiveCloudCheckBox[(int)ECheckBox.Number].activeSelf == true)
        {
            Debug.Log("개수 별로 정렬 메소드 호출");
        }
    }
    // 감정으로 정렬
    public void SortEmotion()
    {
        if (!mGiveCloudCheckBox[(int)ECheckBox.Date].activeSelf
         && !mGiveCloudCheckBox[(int)ECheckBox.Number].activeSelf)
        {
            // 다 꺼지면 안돼
        }
        else
        {
            // 드롭박스, 체크박스 토글
            ToggleDropBox(mSortDropBox);
            ToggleCheckBox(mGiveCloudCheckBox[(int)ECheckBox.Emotion]);
        }
        
        // 감정별로 체크박스 활성화 --> 메소드 호출
        if (mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf == true)
        {
            Debug.Log("현재 인덱스 : " + mDropdown.mDropdownIndex);
            Debug.Log("인덱스 받아와서 현재 적용되어 있는 인덱스로 감정별 정렬 메소드 호출");
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
