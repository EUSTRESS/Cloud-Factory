using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.Windows;
public class CloudContainer : MonoBehaviour
{
    //Interface UI
    public List<GameObject> mUiInvenStocks;
    public GameObject[] mTxtInfoPrefab;
    public Sprite mDefaultSprite;

    public StoragedCloudData mSelecedCloud;

    [SerializeField]
    InventoryManager inventoryManager;

    [SerializeField]
    private List<StoragedCloudData> mUiStocksData; //UI상에 보여지는 StocksData

    /////////////////////
    //인벤토리 정렬 UI//
    ////////////////////
    [SerializeField]
    private Dropdown mDropDown;

    private int mSortedCnt; //선택정렬된개수
    private List<StoragedCloudData> mSortedData; //UI상에 보여지는 StocksData


    // Start is called before the first frame update
    void Start()
    {
        mDropDown = GameObject.Find("UIManager").GetComponent<Dropdown>();
        mDropDown.mDropdown.onValueChanged.AddListener(delegate
        {
            OnDropdownEvent();
        });
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();

        inventoryManager.mInventoryContainer = this.gameObject;
        initInven(inventoryManager.mCloudStorageData.mDatas, "public");
    }

    /////////////////////
    //Button Interact///
    ////////////////////
    public void clicked() //matarial in inventory selected
    {
        if (inventoryManager == null)
            inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();

        //해당 구름 선택 UI 표시
        Transform selected = EventSystem.current.currentSelectedGameObject.transform;
        mSelecedCloud = mUiStocksData[selected.GetSiblingIndex()];
        Debug.Log("구름 선택:" + mUiStocksData[selected.GetSiblingIndex()]);
    }

    public void unclicked() //matarial in cloudmaker deselected
    {
        GameObject target = EventSystem.current.currentSelectedGameObject;
        Sprite sprite = target.GetComponent<Image>().sprite;

        if (sprite.name == "Circle") return; //예외처리

        //해당 구름 선택 UI 표시 취소
    }

    public void cancelDropdownEvent()
    {
        mDropDown.mDropdown.interactable = false;
        mSortedCnt = mUiStocksData.Count;
        //clearInven(mUiStocksData);
        initInven(mUiStocksData, "private");
       // updateInven(mUiStocksData);
    }

    //DropDown public Method
    public void OnDropdownEvent()
    {
        Debug.Log("[DropdownEvent] {" + mDropDown.mDropdown.value + "} clicked.");
        mDropDown.mDropdownIndex = mDropDown.mDropdown.value;
        mSortedData = new List<StoragedCloudData>(); //init
        mSortedData = sortStock(mDropDown.mDropdownIndex);
        mSortedCnt = mSortedData.Count;
        //clearInven(mSortedData);
       // initInven(mSortedData, "private");
        //updateInven(mSortedData);
    }

  
    public void initInven(List<StoragedCloudData> invenData, string order)
    {
        if (order == "public")
        {
            mUiStocksData = new List<StoragedCloudData>();
            mUiStocksData = invenData; //UI목록에 복붙!

            int tmp = 0;
            foreach (StoragedCloudData stock in mUiStocksData)
            {
                GameObject invenUI = transform.GetChild(tmp).gameObject;
                mUiInvenStocks.Add(invenUI); //UIInvenGameObject List 추가.
                tmp++;
            }
        }


        setInven(invenData);
    }

    private void setInven(List<StoragedCloudData> _mData)
    {

        //invenData를 invenContainer(UI)List에 넣어준다.
        int tmp = 0;
        foreach (StoragedCloudData stock in _mData)
        {
            GameObject invenUI = mUiInvenStocks[tmp];

            if (invenUI.transform.childCount == 0)
            {
                GameObject cntTxt = Instantiate(mTxtInfoPrefab[0]);
                cntTxt.transform.SetParent(invenUI.transform, false);
                cntTxt.transform.GetComponent<TMP_Text>().text = stock.mdate.ToString();
            }

            //버튼 컴포넌트가 없으면 만들어준다.
            if (invenUI.transform.GetComponent<Button>() == null)
            {
                Button btn = invenUI.AddComponent<Button>();
                btn.onClick.AddListener(clicked);
            }
            // Sprite tmp = invenUI.transform.GetComponent<Image>().sprite.width;
            // Rect rect = new Rect(0, 0, invenUI.transform.GetComponent<Image>().sprite.width, stock.mTexImage.height);

            Instantiate(stock.mBase, stock.mBase.transform.position, stock.mBase.transform.rotation);
            


            //Name Upadate
            invenUI.name = stock.mdate.ToString();

            tmp++;
        }
    }
    private void clearInven(List<StoragedCloudData> _mData)
    {
        //현재 있는 게임 오브젝트와 새로 들어오는 개수 비교한다.
        int difference = _mData.Count - mUiInvenStocks.Count;
        //현재 있는게 더 많을 경우 : 차액을 초기화
        if (difference < 0) //difference가 2이면 
        {
            for (int i = _mData.Count; i < mUiInvenStocks.Count; i++)
            {
                mUiInvenStocks[i].name = "000";
                mUiInvenStocks[i].GetComponent<Image>().sprite = mDefaultSprite;

                if (mUiInvenStocks[i].transform.GetComponent<Button>())
                {
                    Destroy(mUiInvenStocks[i].GetComponent<Button>());
                }

                if (mUiInvenStocks[i].transform.childCount != 0)
                {
                    Destroy(mUiInvenStocks[i].transform.GetChild(1).gameObject);
                    Destroy(mUiInvenStocks[i].transform.GetChild(0).gameObject);
                }

            }
        }
        else
        {
            //현재 있는게 더 적을 경우 : 차액만큼 컴포넌트 생성
            for (int i = mUiInvenStocks.Count; i < _mData.Count; i++)
            {
                GameObject invenUI = mUiInvenStocks[i];

                if (invenUI.transform.childCount == 0)
                {
                    GameObject cntTxt = Instantiate(mTxtInfoPrefab[0]);
                    cntTxt.transform.SetParent(invenUI.transform, false);
                    cntTxt.transform.GetComponent<TMP_Text>().text = "0";

                    GameObject nameTxt = Instantiate(mTxtInfoPrefab[1]);
                    nameTxt.transform.SetParent(invenUI.transform, false);
                    nameTxt.transform.GetComponent<TMP_Text>().text = "000";
                }

                //버튼 컴포넌트가 없으면 만들어준다.
                if (invenUI.transform.GetComponent<Button>() == null)
                {
                    Button btn = invenUI.AddComponent<Button>();
                    btn.onClick.AddListener(clicked);
                }

                //Image Update
                invenUI.transform.GetComponent<Image>().sprite = mDefaultSprite;

                //Name Upadate
                invenUI.name = "000";
            }
        }
    }

    //인벤토리 목록이 수정된 후에는 무조건 재정렬하여서 UI에 보여주는 목록도 Update해주어야 한다.

    /////////////////////
    //인벤토리 정렬 함수//
    ////////////////////


    //리스트 자체를 정렬한다. 정렬 할 때는 새 리스트를 만들어서 UI에 반영한다.
    public List<StoragedCloudData> sortStock(int _emotion) //감정별로 분류
    {
         List<StoragedCloudData> results = new List<StoragedCloudData>();

        //감정별로 분류: 입력들어온 감정이 속해있는 것만 뽑아서 tmpList에 추가한다.
        foreach (StoragedCloudData stock in mUiStocksData)
        {
            //해당 감정이 stock의 iEmotion리스트에 존재하는지 확인
           // if (!stock.Key.mFinalEmotions.ContainsKey(_emotion)) continue;
            //감정값으로 정렬

            //내림차순으로 정렬
           // var queryAsc = stock.Key.iEmotion.OrderByDescending(x => x.Value);// int, int

            //첫번째 값이 제일 크므로 제일 큰 값이 매개인자와 같은 감정이라면 추가해준다.
          //  if (_emotion != queryAsc.First().Key) continue;
         //   results.Add(stock.Key, stock.Value);
        }


        return results;
    }

    private Dictionary<IngredientData, int> sortStock(Dictionary<IngredientData, int> _target) // 개수별로 분류:
    {
        Dictionary<IngredientData, int> results = new Dictionary<IngredientData, int>();

        var queryAsc = _target.OrderBy(x => x.Value);

        foreach (var dictionary in queryAsc)
            results.Add(dictionary.Key, dictionary.Value);

        return results;
    }
}
