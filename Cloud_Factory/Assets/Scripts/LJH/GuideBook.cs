using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideBook : MonoBehaviour
{
    public GameObject gRareGroup; // 재료 희귀도 분류 UI
    public GameObject gPlantGroup; // 재료 UI

    public GameObject gNextBtn; // 다음 페이지 버튼
    public GameObject gPrevBtn;

    public Image iNextBtn; // 다음 페이지 버튼 이미지
    public Image iPrevBtn;

    public Image[] iRareChange = new Image[3]; // 희귀도에 따라서 바뀌는 UI (배경, 페이지(다음, 이전) 넘김)
    public Sprite[] sRareBG = new Sprite[5]; // 배경 [0] 기본
    public Sprite[] sRarePage = new Sprite[5]; // 페이지 넘김 [0] 기본

    public Text tGuideText; // 도감 가이드 텍스트

    private int gPageIndex; // 페이지 인덱스
    private bool[] gChapter = new bool[3]; // 페이지 대분류 , 0 : 감정 ~
    private bool[] gPlantChapter = new bool[4]; // 재료 페이지 분류 , 0 : 희귀도 1 ~

    private void Awake()
    {
        gPageIndex = 1; // 기본 1 페이지부터 시작
        gChapter[0] = true; // 기본 감정 도감
        gPlantChapter[0] = true; // 기본 희귀도 1
    }

    void Update()
    {
        if (gChapter[0])
        {
            tGuideText.text = "감정 도감" + gPageIndex.ToString() + "페이지";
            UpdateGuideUI(0);
        }            
        else if(gChapter[1])
        {
            tGuideText.text = "구름 도감" + gPageIndex.ToString() + "페이지";
            UpdateGuideUI(0);
        }
            
        else if (gChapter[2])
        {
            if (gPlantChapter[0])
            {
                tGuideText.text = "희귀도1 재료 도감" + gPageIndex.ToString() + "페이지";
                UpdateGuideUI(1);
            }
                
            else if (gPlantChapter[1])
            {
                tGuideText.text = "희귀도2 재료 도감" + gPageIndex.ToString() + "페이지";
                UpdateGuideUI(2);
            }               
            else if (gPlantChapter[2])
            {
                tGuideText.text = "희귀도3 재료 도감" + gPageIndex.ToString() + "페이지";
                UpdateGuideUI(3);
            }             
            else if (gPlantChapter[3])
            {
                tGuideText.text = "희귀도4 재료 도감" + gPageIndex.ToString() + "페이지";
                UpdateGuideUI(4);
            }             
        }

        if (gPageIndex >= 4)
        {
            gNextBtn.SetActive(false);
            iNextBtn.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0 / 255f);
        }            
        else
            gNextBtn.SetActive(true);

        if (gPageIndex <= 1)
        {
            gPrevBtn.SetActive(false);
            iPrevBtn.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0 / 255f);
        }            
        else
            gPrevBtn.SetActive(true);
    }

    void UpdateGuideUI(int _iIndex)
    {
        iRareChange[0].sprite = sRareBG[_iIndex];
        iRareChange[1].sprite = sRarePage[_iIndex];
        iRareChange[2].sprite = sRarePage[_iIndex];
    }

    #region 버튼이미지호버링
    public void ActiveNextBtnImage()
    {
        if (gPageIndex < 4)
            iNextBtn.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
    }
    public void UnActiveNextBtnImage()
    {
        iNextBtn.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0 / 255f);
    }
    public void ActivePrevBtnImage()
    {
        if (gPageIndex > 1)
            iPrevBtn.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
    }
    public void UnActivePrevBtnImage()
    {
        iPrevBtn.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0 / 255f);
    }
    #endregion

    public void ClickNextBtn()
    {
        if (gPageIndex < 4)
            ++gPageIndex;
    }
    public void ClickPrevBtn()
    {
        if (gPageIndex > 1)
            --gPageIndex;
    }

    #region 감정 구름 재료 버튼
    public void ActiveFeel()
    {
        ChangeChapter(false, false, 1, true, false, false);
    }
    public void ActiveCloud()
    {
        ChangeChapter(false, false, 1, false, true, false);
    }
    public void ActivePlant()
    {
        ChangeChapter(true, true, 1, false, false, true);
        ClickRare1();
    }
    #endregion

    void ChangeChapter(bool _bRare, bool _bGroupPlant ,int _iPageIndex, bool _bFeel, bool _bCloud, bool _bPlant)
    {
        gRareGroup.SetActive(_bRare);
        gPlantGroup.SetActive(_bGroupPlant); // 재료 UI
        gPageIndex = 1; // 페이지 초기화
        gChapter[0] = _bFeel;
        gChapter[1] = _bCloud;
        gChapter[2] = _bPlant;
    }

    #region 희귀도
    public void ClickRare1()
    {
        UpdateRareUI(0);
    }
    public void ClickRare2()
    {
        UpdateRareUI(1);
    }
    public void ClickRare3()
    {
        UpdateRareUI(2);
    }
    public void ClickRare4()
    {
        UpdateRareUI(3);
    }
    #endregion

    void UpdateRareUI(int _iIndex)
    {
        for (int i = 0; i < 4; i++)
        {
            gPlantChapter[i] = false;
        }
        gPlantChapter[_iIndex] = true;
        gPageIndex = 1;
    }
}
