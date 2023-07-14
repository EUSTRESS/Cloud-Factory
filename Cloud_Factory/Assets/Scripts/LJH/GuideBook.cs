using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideBook : MonoBehaviour
{
    private static int DEFAULT_MAX_INFO_COUNT = 12;
    
    public Transform ingrViewPort1;
    public Transform ingrViewPort2;
    public Transform cloudViewPort;

    public GameObject emotionPageGroup;
    
    public GameObject gRareGroup; // 재료 희귀도 분류 UI
    public GameObject gPlantGroup_main;

    public GameObject gNextBtn; // 다음 페이지 버튼
    public GameObject gPrevBtn;

    public Image iNextBtn; // 다음 페이지 버튼 이미지
    public Image iPrevBtn;

    public Image[] iRareChange = new Image[3]; // 희귀도에 따라서 바뀌는 UI (배경, 페이지(다음, 이전) 넘김)
    public Sprite[] sRareBG = new Sprite[5]; // 배경 [0] 기본
    public Sprite[] sRarePage = new Sprite[5]; // 페이지 넘김 [0] 기본

    public Text tGuideText; // 도감 가이드 텍스트

    private int gPageIndex; // 페이지 인덱스
    [SerializeField]private bool[] gChapter = new bool[3]; // 페이지 대분류 , 0 : 감정 ~
    [SerializeField]private bool[] gPlantChapter = new bool[4]; // 재료 페이지 분류 , 0 : 희귀도 1 ~

    private int Emotion_page_num;
    private int Cloud_page_num;
    public GameObject[] Emotion_Page = new GameObject[4];
    public GameObject[] Cloud_Page = new GameObject[4];
    private static int Plant_num = 3;
    
    public GameObject[] QM_Info = new GameObject[12];

    public GameObject ingrExist;
    public GameObject ingrEmpty;
    public GameObject cloudBasic;

    private InventoryManager mInventoryManager;
    // 채집 기록이 있는 재료 정보가 저장되는 변수
    // ingredientHistory[0]: 채집된 적이 있는 희귀도 1 재료 리스트
    // 이름 가져오는 법: ingredientHistory[rarity - 1][index].dataName;
    // 이미지 가져오는 법 : ingredientHistory[rarity - 1][index].image;
    private List<List<IngredientData>> ingredientHistory;
    private List<Sprite> cloudHistory;

    
    private void Awake()
    {
        gPageIndex = 1; // 기본 1 페이지부터 시작
        gChapter[0] = true; // 기본 감정 도감
        gPlantChapter[0] = true; // 기본 희귀도 1
        Emotion_page_num = 0;
        Cloud_page_num = 0;

        for(int i = 0; i < Plant_num; i++)
        {
            //Plant_get[i] = false;
        }

        mInventoryManager = FindObjectOfType<InventoryManager>();
        ingredientHistory = new List<List<IngredientData>>();
        for (int i = 0; i < mInventoryManager.ingredientHistory.Count; i++)
        {
            ingredientHistory.Add(new List<IngredientData>());
        }

        cloudHistory = new List<Sprite>();

        UpdateGuideBook();
    }

    void Update()
    {

    }

    public void UpdateGuideBook()
    {
        ingrViewPort1.gameObject.SetActive(false);
        ingrViewPort2.gameObject.SetActive(false);
        cloudViewPort.gameObject.SetActive(false);
        emotionPageGroup.SetActive(false);

        for (int i = 0; i < emotionPageGroup.transform.childCount; i++)
        {
            emotionPageGroup.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        if (gChapter[0])
        {
            tGuideText.text = "감정 도감" + gPageIndex.ToString() + "페이지";
            emotionPageGroup.SetActive(true);
            if (gPageIndex <= emotionPageGroup.transform.childCount)
                emotionPageGroup.transform.GetChild(gPageIndex - 1).gameObject.SetActive(true);
            cloudViewPort.gameObject.SetActive(false);
            UpdateGuideUI(0);
            Update_Emotion_Page(true, gPageIndex - 1);
            Update_Cloud_Page(false, gPageIndex - 1);
        }            
        else if(gChapter[1])
        {
            tGuideText.text = "구름 도감" + gPageIndex.ToString() + "페이지";
            UpdateCloudGuideBook();
            UpdateGuideUI(0);
            Update_Emotion_Page(false, gPageIndex - 1);
            Update_Cloud_Page(true, gPageIndex - 1);
        }
            
        else if (gChapter[2])
        {
            if (gPlantChapter[0])
            {
                tGuideText.text = "희귀도1 재료 도감" + gPageIndex.ToString() + "페이지";
                ClearIngrGuideBook();
                UpdateIngredientHistory(1);
                UpdateIngrGuideBook(1);
                UpdateGuideUI(1);
                Update_Emotion_Page(false, gPageIndex - 1);
                Update_Cloud_Page(false, gPageIndex - 1);
            }
                
            else if (gPlantChapter[1])
            {
                tGuideText.text = "희귀도2 재료 도감" + gPageIndex.ToString() + "페이지";
                ClearIngrGuideBook();
                UpdateIngredientHistory(2);
                UpdateIngrGuideBook(2);
                UpdateGuideUI(2);
                Update_Emotion_Page(false, gPageIndex - 1);
                Update_Cloud_Page(false, gPageIndex - 1);
            }               
            else if (gPlantChapter[2])
            {
                tGuideText.text = "희귀도3 재료 도감" + gPageIndex.ToString() + "페이지";
                ClearIngrGuideBook();
                UpdateIngredientHistory(3);
                UpdateIngrGuideBook(3);
                UpdateGuideUI(3);
                Update_Emotion_Page(false, gPageIndex - 1);
                Update_Cloud_Page(false, gPageIndex - 1);
            }             
            else if (gPlantChapter[3])
            {
                tGuideText.text = "희귀도4 재료 도감" + gPageIndex.ToString() + "페이지";
                ClearIngrGuideBook();
                UpdateIngredientHistory(4);
                UpdateIngrGuideBook(4);
                UpdateGuideUI(4);
                Update_Emotion_Page(false, gPageIndex - 1);
                Update_Cloud_Page(false, gPageIndex - 1);
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

    private void UpdateIngredientHistory(int rarity)
    {
        this.ingredientHistory[rarity - 1] = mInventoryManager.ingredientHistory[rarity - 1];
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
        UpdateGuideBook();
    }
    public void ClickPrevBtn()
    {
        if (gPageIndex > 1)
            --gPageIndex;
        UpdateGuideBook();
    }

    #region 감정 구름 재료 버튼
    public void ActiveFeel()
    {
        ChangeChapter(false, false, 1, true, false, false);
        UpdateGuideBook();
    }
    public void ActiveCloud()
    {
        ChangeChapter(false, false, 1, false, true, false);
        UpdateGuideBook();
    }
    public void ActivePlant()
    {
        ChangeChapter(true, true, 1, false, false, true);
        UpdateGuideBook();
        ClickRare1();
    }
    #endregion

    void ChangeChapter(bool _bRare, bool _bGroupPlant ,int _iPageIndex, bool _bFeel, bool _bCloud, bool _bPlant)
    {
        gRareGroup.SetActive(_bRare);
        gPlantGroup_main.SetActive(_bGroupPlant); // 재료 UI
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
        UpdateGuideBook();
    }

    void Update_Cloud_Page(bool page_on, int Page_num)
    {
        //Cloud_page_num = Page_num;
        //for (int i = 0; i < 4; i++)
        //{
        //    Page_num = 1;
        //    Cloud_Page[i].SetActive(false);
        //}
        //if (page_on)
        //{
        //    Cloud_Page[Cloud_page_num].SetActive(true);
        //}
    }

    void Update_Emotion_Page(bool page_on, int Page_num)
    {
        //Emotion_page_num = Page_num;
        //for (int i = 0; i < 4; i++)
        //{
        //    Page_num = 1;
        //    Emotion_Page[i].SetActive(false);
        //}
        //if (page_on)
        //{
        //    Emotion_Page[Emotion_page_num].SetActive(true);
        //}
    }

    private void ClearIngrGuideBook()
    {
        for (int i = ingrViewPort1.childCount; i > 0; i--)
        {
            Destroy(ingrViewPort1.GetChild(i - 1).gameObject);
        }
        
        for (int i = ingrViewPort2.childCount; i > 0; i--)
        {
            Destroy(ingrViewPort2.GetChild(i - 1).gameObject);
        }
    }

    private void UpdateIngrGuideBook(int rarity)
    {
        ingrViewPort1.gameObject.SetActive(true);
        ingrViewPort2.gameObject.SetActive(true);
        
        int ingrIndex = (gPageIndex - 1) * DEFAULT_MAX_INFO_COUNT;
        int i;
        for (i = ingrIndex; i < ingrIndex + (DEFAULT_MAX_INFO_COUNT / 2); i++)
        {
            if (i < ingredientHistory[rarity - 1].Count)
            {
                GameObject temp = Instantiate(ingrExist, ingrViewPort1, true);
                temp.transform.GetChild(0).GetComponent<Image>().sprite = ingredientHistory[rarity - 1][i].image;
                temp.transform.GetChild(1).GetComponent<Text>().text = ingredientHistory[rarity - 1][i].dataName.ToString();
            }
            else
            {
                GameObject temp = Instantiate(ingrEmpty, ingrViewPort1, true);
            }
        }
        
        for (; i < ingrIndex + DEFAULT_MAX_INFO_COUNT; i++)
        {
            if (i < ingredientHistory[rarity - 1].Count)
            {
                GameObject temp = Instantiate(ingrExist, ingrViewPort2, true);
                temp.transform.GetChild(0).GetComponent<Image>().sprite = ingredientHistory[rarity - 1][i].image;
                temp.transform.GetChild(1).GetComponent<Text>().text = ingredientHistory[rarity - 1][i].dataName.ToString();
            }
            else
            {
                GameObject temp = Instantiate(ingrEmpty, ingrViewPort2, true);
            }
        }
    }

    private void UpdateCloudGuideBook()
    {
        cloudViewPort.gameObject.SetActive(true);
        this.cloudHistory = mInventoryManager.cloudHistory;
        
        for (int i = cloudViewPort.childCount; i > 0; i--)
        {
            Destroy(cloudViewPort.GetChild(i - 1).gameObject);
        }
        
        int cloudIndex = (gPageIndex - 1) * (DEFAULT_MAX_INFO_COUNT / 2);
        
        for (int i = cloudIndex; i < cloudIndex + (DEFAULT_MAX_INFO_COUNT / 2); i++)
        {
            GameObject temp = Instantiate(cloudBasic, cloudViewPort, true);
            
            if (i < cloudHistory.Count) temp.GetComponent<Image>().sprite = cloudHistory[i];
        }
    }
}
