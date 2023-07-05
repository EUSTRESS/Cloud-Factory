using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using TMPro;


public class InventoryContainer : MonoBehaviour
{
    //Interface UI
    public List<GameObject> mUiInvenStocks;
    public GameObject[] mTxtInfoPrefab;
    public Sprite mDefaultSprite;

    //Reference Class
    [SerializeField]
    CloudMakeSystem Cloudmakesystem;
    [SerializeField]
    InventoryManager inventoryManager;

    private Dictionary<IngredientData, int> mUiStocksData; //UI상에 보여지는 StocksData

    /////////////////////
    //인벤토리 정렬 UI//
    ////////////////////
    [SerializeField]
    private Dropdown mDropDown;
    
    private int mSortedCnt; //선택정렬된개수
    private Dictionary<IngredientData, int> mSortedData; //UI상에 보여지는 StocksData


    
    void Start()
    {
        Cloudmakesystem = GameObject.FindWithTag("CloudSystem").GetComponent<CloudMakeSystem>();
        mDropDown = GameObject.Find("UIManager").GetComponent<Dropdown>();
        mDropDown.mDropdown.onValueChanged.AddListener(delegate
        {
            OnDropdownEvent();
        });
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();

        inventoryManager.mInventoryContainer = this.gameObject;
        initInven(inventoryManager.mergeList2Dic(), "public");
    }

    /////////////////////
    //Button Interact///
    ////////////////////
    public void clicked() //matarial in inventory selected
    {
		bool isMakingCloud = GameObject.Find("I_CloudeGen").GetComponent<CloudMakeSystem>().isMakingCloud;              // 구름이 제작 중인지 확인
        bool isMtrlListFull = GameObject.Find("I_CloudeGen").GetComponent<CloudMakeSystem>().d_selectMtrlListFull();    // 조합되는 재료 칸이 가득찼는지 확인
        int createdCloudData = inventoryManager.createdCloudData.mEmotions.Count;                                       // 구름이 제작되어 데코 대기중인 상태를 제작된 구름의 감정의 개수로 관리(제작 전이면 0, 이후로는 1이상)

        // 조합되는 재료 칸이 모두 찼거나, 구름 제작 중, 후에 재료를 더 넣을 수 없도록 한다.
		if (isMakingCloud || isMtrlListFull || createdCloudData > 0) { return; }

		if (inventoryManager == null)
            inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();

        string name = EventSystem.current.currentSelectedGameObject.name;
        Cloudmakesystem.E_Selected(name);

        updateStockCnt(name, true);
    }

    public void unclicked() //matarial in cloudmaker deselected
    {
        GameObject target = EventSystem.current.currentSelectedGameObject;
        Sprite sprite = target.GetComponent<Image>().sprite;

        bool isMakingCloud = GameObject.Find("I_CloudeGen").GetComponent<CloudMakeSystem>().isMakingCloud;  // 구름이 제작 중인지 확인
		int createdCloudData = inventoryManager.createdCloudData.mEmotions.Count;

		//클릭한 재료의 칸이 비었거나, 구름 제작 중, 후에 재료를 뺄 수 없도록 한다.
		if (sprite.name == "M_default" || isMakingCloud || createdCloudData > 0) { return; }  
        if (sprite.name == "Circle") return; //예외처리

        updateStockCnt(getDataWithSprite(sprite.name).dataName, false);

        Cloudmakesystem.E_UnSelected(target.name);
        Debug.Log("클릭");
    }
   
    public void sortWithCnt() //Button Interaction Function
    {
        Dictionary<IngredientData, int> targetDt = new Dictionary<IngredientData, int>();

        if (mDropDown.mDropdown.interactable)
            targetDt = sortStock(mSortedData);
        else
            targetDt = sortStock(mUiStocksData);

        updateInven(targetDt);
    }

    public void cancelDropdownEvent()
    {
        mDropDown.mDropdown.interactable = false;
        mSortedCnt = mUiStocksData.Count;
        clearInven(mUiStocksData);
        initInven(mUiStocksData, "private");
        updateInven(mUiStocksData);
    }

    //DropDown public Method
    public void OnDropdownEvent()
    {     
        Debug.Log("[DropdownEvent] {" + mDropDown.mDropdown.value + "} clicked.");
        mDropDown.mDropdownIndex = mDropDown.mDropdown.value;
        mSortedData = new Dictionary<IngredientData, int>(); //init
        mSortedData = sortStock(mDropDown.mDropdownIndex);
        mSortedCnt = mSortedData.Count;
        clearInven(mSortedData);
        initInven(mSortedData, "private");
        updateInven(mSortedData);
    }

    //날씨의 공간에서 구름 공장으로 넘어갈 때, 가상의 채집 인벤토리 데이터를 구름공장의 UI인벤토리로 넘겨준다.
    public void initInven(Dictionary<IngredientData, int> invenData, string order)
    {
        if(order == "public")
        {
            mUiStocksData = new Dictionary<IngredientData, int>();
            mUiStocksData = invenData; //UI목록에 복붙!
           
            int tmp = 0;
            foreach (KeyValuePair<IngredientData, int> stock in mUiStocksData)
            {
                GameObject invenUI = transform.GetChild(tmp).gameObject;
                mUiInvenStocks.Add(invenUI); //UIInvenGameObject List 추가.
                tmp++;
            }
        }
       

        setInven(invenData);

    }

    private void setInven(Dictionary<IngredientData, int> _mData)
    {

        //invenData를 invenContainer(UI)List에 넣어준다.
        int tmp = 0;
        foreach (KeyValuePair<IngredientData, int> stock in _mData)
        {
            GameObject invenUI = mUiInvenStocks[tmp];

            if (invenUI.transform.childCount == 0)
            {
                GameObject cntTxt = Instantiate(mTxtInfoPrefab[0]);
                cntTxt.transform.SetParent(invenUI.transform, false);
                cntTxt.transform.GetComponent<TMP_Text>().text = stock.Value.ToString();

                GameObject nameTxt = Instantiate(mTxtInfoPrefab[1]);
                nameTxt.transform.SetParent(invenUI.transform, false);
                nameTxt.transform.GetComponent<TMP_Text>().text = ""; // stock.Key.dataName.ToString();
                nameTxt.SetActive(false);
            }

            //버튼 컴포넌트가 없으면 만들어준다.
            if (invenUI.transform.GetComponent<Button>() == null)
            {
                Button btn = invenUI.AddComponent<Button>();
                btn.onClick.AddListener(clicked);
            }

            //Image Update
            invenUI.transform.GetComponent<Image>().sprite = stock.Key.image;

            //Name Upadate
            invenUI.name = stock.Key.dataName;

            tmp++;
        }
    }
    private void clearInven(Dictionary<IngredientData, int> _mData)
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
                    nameTxt.transform.GetComponent<TMP_Text>().text = "";
                    nameTxt.SetActive(false);
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

    //해당 data 딕셔너리의 개수 만큼 데이터를 바꾼다.
    private void updateInven(Dictionary<IngredientData, int> _mData)
    {
        int tmp = 0;
        foreach (KeyValuePair<IngredientData, int> data in _mData)
        {
            GameObject stockObj = mUiInvenStocks[tmp];

            //GameObject name
            stockObj.name = data.Key.dataName;
            //이미지
            stockObj.transform.GetComponent<Image>().sprite = data.Key.image;
            //cnt
            stockObj.transform.GetChild(0).GetComponent<TMP_Text>().text = data.Value.ToString();
            //name
            stockObj.transform.GetChild(1).GetComponent<TMP_Text>().text = data.Key.dataName.ToString();

            tmp++; //plus index value
        }
    }

    private IngredientData getDataWithSprite(string _spritename) //Sprite를 매개변수로 해당 아이템 data를 검색한다.
    {
        IngredientData data = inventoryManager.mIngredientDatas[inventoryManager.minvenLevel - 1].mItemList.Find(item => _spritename == item.image.name);

        return data;
    }


    //////////////////////////////
    /////////재료 선택 함수////////
    //////////////////////////////
    //재료 선택시에 해당 재료 개수가 0이 되면 리스트에서 제거, 해당 재료개수가 0에서 1이 되면 리스트에 추가.
    private void updateStockCnt(string _dtName, bool _switch)
    {
        //switch = true 인 경우에는 재료 선택, switch = false 인 경우에는 재료 취소의 업데이트 경우
        //매개인자는 무조건 인벤토리에 존재 했던, 또는 존재하는 재료이다.
        if (_switch)
            selectStock(_dtName);
        else
            cancelStock(_dtName);

        return;
    }

    private GameObject findObjectWithData(IngredientData _stockDt)
    {
        GameObject result = null;
        foreach(GameObject stock in mUiInvenStocks)
        {
            if (stock.name == _stockDt.dataName) 
                result = stock;
        }
        return result;
    }
    private void selectStock(string dataName)
    {
        IngredientData stockDt = inventoryManager.mIngredientDatas[inventoryManager.minvenLevel - 1].mItemList.Find(item => dataName == item.dataName);

        
        mUiStocksData[stockDt]--;

        GameObject uiGameObj = findObjectWithData(stockDt);

        uiGameObj.transform.GetChild(0).GetComponent<TMP_Text>().text = mUiStocksData[stockDt].ToString();

        if (mUiStocksData[stockDt] != 0) return;

        //남은 재고가 0이라면 아예 리스트에서 삭제
        removeStockInInven(stockDt, uiGameObj);
    }

    private bool isStockInSortedLayer(IngredientData stockDt) //선택된 재료가 현재 보여지고 있는 인벤토리 레이어에 있는지 bool값 반환
    {
        //해당 감정이 stock의 iEmotion리스트에 존재하는지 확인
        bool isContain = stockDt.iEmotion.ContainsKey(mDropDown.mDropdown.value);
        bool isSorting = mDropDown.mDropdown.interactable; //정렬레이어가 적용된 상태인지 확인.


        if (isSorting) //정렬 상태 && 선택된 감정의 가장 큰 값이 현재 목차와 맞지 않을 떄.
        {
            //감정값 내림차순으로 정렬
            var queryAsc = stockDt.iEmotion.OrderByDescending(x => x.Value);// int, int
            int maxEmo = queryAsc.First().Key;
            if (mDropDown.mDropdown.value != maxEmo)         
                return false;
        }

        return true;
    }
    private void cancelStock(string dataName)
    {
        IngredientData stockDt = inventoryManager.mIngredientDatas[inventoryManager.minvenLevel - 1].mItemList.Find(item => dataName == item.dataName);
        GameObject uiGameObj = findObjectWithData(stockDt);

        //구름제작 재료 선택에서 취소된 재료가 인벤토리에 있는지 검사.
        //만약 없다면 취소했을 시 리스트에 새 재료 추가해서 개수 +1 한다.
        if (mUiStocksData.ContainsKey(stockDt))
        {
            mUiStocksData[stockDt]++;

            if (!isStockInSortedLayer(stockDt)) return;


            if (mDropDown.mDropdown.interactable)
            {
                mSortedData[stockDt]++;
            }


            uiGameObj.transform.GetChild(0).GetComponent<TMP_Text>().text = mUiStocksData[stockDt].ToString();
        }
        else
        {
            if (!isStockInSortedLayer(stockDt))
            {
                mUiStocksData.Add(stockDt, 1); //리스트에서 해당 data 추가
                return;
            }
            
            addStockInInven(stockDt, uiGameObj);

        }
            
    }

    private void addStockInInven(IngredientData stockDt, GameObject uiGameObj)
    {
        //1. 나머지 데이터 하나씩 덮어 씌우기.
        //인벤토리 전체 업데이트
        if (!mDropDown.mDropdown.interactable)
            updateInven(mUiStocksData);
        else
            updateInven(mSortedData);

        //Data 추가
        mUiStocksData.Add(stockDt, 1); //리스트에서 해당 data 추가
       

        if (!mDropDown.mDropdown.interactable)
            mSortedCnt = mUiStocksData.Count;
        else
        {
            mSortedData.Add(stockDt, 1); //리스트에서 해당 data 추가
            mSortedCnt = mSortedData.Count;
        }
            

        //2. 인벤토리의 마지막 stock의 컴포넌트 추가 및 이미지 초기화.
        // tmp instance
        GameObject lastStockInInven = mUiInvenStocks[mSortedCnt - 1];
        
        //Component 추가
        GameObject cntTxt = Instantiate(mTxtInfoPrefab[0]);
        cntTxt.transform.SetParent(lastStockInInven.transform, false);
        cntTxt.transform.GetComponent<TMP_Text>().text = mUiStocksData[stockDt].ToString();

        GameObject nameTxt = Instantiate(mTxtInfoPrefab[1]);
        nameTxt.transform.SetParent(lastStockInInven.transform, false);
        //nameTxt.transform.GetComponent<TMP_Text>().text = stockDt.dataName.ToString();
		nameTxt.transform.GetComponent<TMP_Text>().text = "";

		Button btn = lastStockInInven.AddComponent<Button>();
        btn.onClick.AddListener(clicked);

        //Image Update
        lastStockInInven.transform.GetComponent<Image>().sprite = stockDt.image; 

        //Name Upadate
        lastStockInInven.name = stockDt.dataName; ; //Game Object Name 초기화       
    }

    private void removeStockInInven(IngredientData stockDt, GameObject uiGameObj)
    {
        mUiStocksData.Remove(stockDt); //리스트에서 해당 data 삭제
        

        //인벤토리 전체 업데이트
        if (!mDropDown.mDropdown.interactable) 
            mSortedCnt = mUiStocksData.Count;
        else
        {
            mSortedData.Remove(stockDt); //리스트에서 해당 data 삭제
            mSortedCnt = mSortedData.Count;
        }
            
        //1. 인벤토리의 마지막 stock의 컴포넌트 삭제 및 이미지 초기화.
        // tmp instance
        GameObject lastStockInInven = mUiInvenStocks[mSortedCnt];
        lastStockInInven.name = "000"; //Game Object Name 초기화
        lastStockInInven.transform.GetComponent<Image>().sprite = mDefaultSprite; //img초기화
        Destroy(lastStockInInven.transform.GetComponent<Button>()); // button component 삭제
        Destroy(lastStockInInven.transform.GetChild(0).gameObject); // cnt txt 삭제
        Destroy(lastStockInInven.transform.GetChild(1).gameObject); // name txt 삭제

        //2. 나머지 데이터 하나씩 덮어 씌우기.
        if (!mDropDown.mDropdown.interactable)
            updateInven(mUiStocksData);
        else
            updateInven(mSortedData);
    }

    

    //인벤토리 목록이 수정된 후에는 무조건 재정렬하여서 UI에 보여주는 목록도 Update해주어야 한다.

    /////////////////////
    //인벤토리 정렬 함수//
    ////////////////////


    //리스트 자체를 정렬한다. 정렬 할 때는 새 리스트를 만들어서 UI에 반영한다.
    public Dictionary<IngredientData, int> sortStock(int _emotion) //감정별로 분류
    {
        Dictionary<IngredientData, int> results = new Dictionary<IngredientData, int>();

        //감정별로 분류: 입력들어온 감정이 속해있는 것만 뽑아서 tmpList에 추가한다.
        foreach (KeyValuePair<IngredientData, int> stock in mUiStocksData)
        {
            //해당 감정이 stock의 iEmotion리스트에 존재하는지 확인
            if (!stock.Key.iEmotion.ContainsKey(_emotion)) continue;
            //감정값으로 정렬

            //내림차순으로 정렬
            var queryAsc = stock.Key.iEmotion.OrderByDescending(x => x.Value);// int, int

            //첫번째 값이 제일 크므로 제일 큰 값이 매개인자와 같은 감정이라면 추가해준다.
            if(_emotion != queryAsc.First().Key) continue;
            results.Add(stock.Key, stock.Value);
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
