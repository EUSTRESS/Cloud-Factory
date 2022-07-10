using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class InventoryContainer : MonoBehaviour
{
    public List<GameObject> mUiInvenStocks;
    public GameObject[] mTxtInfoPrefab;
    public Sprite mDefaultSprite;

    [SerializeField]
    CloudMakeSystem Cloudmakesystem;
    [SerializeField]
    InventoryManager inventoryManager;

    private Dictionary<IngredientData, int> mUiStocksData; //UI상에 보여지는 StocksData


    //날씨의 공간에서 구름 공장으로 넘어갈 때, 가상의 채집 인벤토리 데이터를 구름공장의 UI인벤토리로 넘겨준다.
    public void initInven(Dictionary<IngredientData, int> invenData)
    {
        mUiStocksData = invenData; //UI목록에 복붙!

        //invenData를 invenContainer(UI)List에 넣어준다.
        int tmp = 0;
        foreach(KeyValuePair<IngredientData, int> stock in mUiStocksData)
        {
            GameObject invenUI = transform.GetChild(tmp).gameObject;
            mUiInvenStocks.Add(invenUI);

            if(invenUI.transform.childCount == 0)
            {
                GameObject cntTxt = Instantiate(mTxtInfoPrefab[0]);
                cntTxt.transform.SetParent(invenUI.transform, false);
                cntTxt.transform.GetComponent<Text>().text = stock.Value.ToString();

                GameObject nameTxt = Instantiate(mTxtInfoPrefab[1]);
                nameTxt.transform.SetParent(invenUI.transform, false);
                nameTxt.transform.GetComponent<Text>().text = stock.Key.ingredientName.ToString();
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
            invenUI.name = stock.Key.ingredientName;

            tmp++;
        }
    }

    //클릭으로 인해 실제 inven Data(Inventory Manager)의 데이터는 바뀌지 않지만, UI상으로는 개수의 변화가 보여야 한다.
    //따라서 예비 인벤토리 리스트의 값을 변경 후 UI도 변경해준다.
    private void updateInven()
    {
       

    }
    //Btn click 함수
    public void clicked() //matarial in inventory selected
    {
        if (inventoryManager == null)
            inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();

        string name = EventSystem.current.currentSelectedGameObject.name;
        Cloudmakesystem.E_Selected(name);
        updateStockCnt(name, true);
    }

    void Start()
    {
        Cloudmakesystem = GameObject.FindWithTag("CloudSystem").GetComponent<CloudMakeSystem>();
        mUiStocksData = new Dictionary<IngredientData, int>();

    }

    //////////////////////////////
    /////////재료 선택 함수////////
    //////////////////////////////
    //재료 선택시에 해당 재료 개수가 0이 되면 리스트에서 제거, 해당 재료개수가 0에서 1이 되면 리스트에 추가.
    public void updateStockCnt(string _dtName, bool _switch)
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
            if (stock.name == _stockDt.ingredientName) 
                result = stock;
        }
        return result;
    }
    private void selectStock(string dataName)
    {
        IngredientData stockDt = inventoryManager.mIngredientDatas[inventoryManager.minvenLevel - 1].mItemList.Find(item => dataName == item.ingredientName);
        
        mUiStocksData[stockDt]--;

        GameObject uiGameObj = findObjectWithData(stockDt);
        uiGameObj.transform.GetChild(0).GetComponent<Text>().text = mUiStocksData[stockDt].ToString();

        if (mUiStocksData[stockDt] != 0) return;

        //남은 재고가 0이라면 아예 리스트에서 삭제
        removeStockInInven(stockDt, uiGameObj);
    }

    private void removeStockInInven(IngredientData stockDt, GameObject uiGameObj)
    {
        mUiStocksData.Remove(stockDt); //리스트에서 해당 data 삭제

        //인벤토리 전체 업데이트

        //1. 인벤토리의 마지막 stock의 컴포넌트 삭제 및 이미지 초기화.
        // tmp instance
        GameObject lastStockInInven = mUiInvenStocks[mUiStocksData.Count];
        lastStockInInven.transform.GetComponent<Image>().sprite = mDefaultSprite; //img초기화
        Destroy(lastStockInInven.transform.GetComponent<Button>()); // button component 삭제
        Destroy(lastStockInInven.transform.GetChild(0).gameObject); // cnt txt 삭제
        Destroy(lastStockInInven.transform.GetChild(1).gameObject); // name txt 삭제

        //2. 나머지 데이터 하나씩 덮어 씌우기.
        int tmp = 0;
        foreach (KeyValuePair<IngredientData, int> data in mUiStocksData)
        {
            GameObject stockObj = mUiInvenStocks[tmp];

            //이미지
            stockObj.transform.GetComponent<Image>().sprite = data.Key.image;
            //cnt
            stockObj.transform.GetChild(0).GetComponent<Text>().text = data.Value.ToString();
            //name
            stockObj.transform.GetChild(1).GetComponent<Text>().text = data.Key.ingredientName.ToString();

            tmp++; //plus index value
        }
    }

    private void cancelStock(string dataName)
    {
        IngredientData _stockDt = inventoryManager.mIngredientDatas[inventoryManager.minvenLevel - 1].mItemList.Find(item => dataName == item.ingredientName);

        //구름제작 재료 선택에서 취소된 재료가 인벤토리에 있는지 검사.
        //만약 없다면 취소했을 시 리스트에 새 재료 추가해서 개수 +1 한다.
        if (mUiStocksData.ContainsKey(_stockDt)) mUiStocksData[_stockDt]++;
        else
            mUiStocksData.Add(_stockDt,1);
        
    }

    //인벤토리 목록이 수정된 후에는 무조건 재정렬하여서 UI에 보여주는 목록도 Update해주어야 한다.

    /////////////////////
    //인벤토리 정렬 함수//
    ////////////////////

   

    //리스트 자체를 정렬한다. 정렬 할 때는 새 리스트를 만들어서 UI에 반영한다.
    private Dictionary<IngredientData, int> sortStock(Emotion _emotion)
    {
        Dictionary<IngredientData, int> results = new Dictionary<IngredientData, int>();

        //감정별로 분류: 입력들어온 감정이 속해있는 것만 뽑아서 tmpList에 추가한다.
        foreach (KeyValuePair<IngredientData, int> stock in mUiStocksData)
        {
            if (!stock.Key.iEmotion.ContainsKey((int)_emotion)) continue;

            results.Add(stock.Key, stock.Value);
        }
        return results;
    }

    private Dictionary<IngredientData, int> sortStock() // 개수별로 분류:
    {
        Dictionary<IngredientData, int> results = new Dictionary<IngredientData, int>();

        var queryAsc = mUiStocksData.OrderBy(x => x.Value);

        foreach (var dictionary in queryAsc)
            results.Add(dictionary.Key, dictionary.Value);

        return results;
    }
}
