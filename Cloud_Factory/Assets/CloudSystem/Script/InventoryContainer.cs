using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using TMPro;
public class InventoryContainer : MonoBehaviour
{

    public List<GameObject> mUiInvenStocks;
    public GameObject[] mTxtInfoPrefab;
    public Sprite mDefaultSprite;

    [SerializeField]
    CloudMakeSystem Cloudmakesystem;
    [SerializeField]
    InventoryManager inventoryManager;

    private StorageUIManager storageUIManager;

    private Dictionary<IngredientData, int> mUiStocksData; //UI�� �������� StocksData
    
    /////////////////////
    //�κ��丮 ���� UI//
    ////////////////////
    [SerializeField]
    private TMP_Dropdown mDropDown;
    private int mSortedCnt; //�������ĵȰ���
    private Dictionary<IngredientData, int> mSortedData; //UI�� �������� StocksData
    void Start()
    {
        Cloudmakesystem = GameObject.FindWithTag("CloudSystem").GetComponent<CloudMakeSystem>();
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();

        storageUIManager = GameObject.Find("UIManager").GetComponent<StorageUIManager>();
       // mDropDown = GameObject.Find("D_Sort").GetComponent<TMP_Dropdown>(); //���� ������ ������Ʈ�� �˻� ����.
    }

    /////////////////////
    //Button Interact///
    ////////////////////
    public void clicked() //matarial in inventory selected
    {
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

        if (sprite.name == "Circle") return; //����ó��

        updateStockCnt(getDataWithSprite(sprite.name).ingredientName, false);

        Cloudmakesystem.E_UnSelected(target.name);
        Debug.Log("Ŭ��");
    }
   
    public void sortWithCnt() //Button Interaction Function
    {
        Dictionary<IngredientData, int> targetDt = new Dictionary<IngredientData, int>();

        if (mDropDown.interactable)
            targetDt = sortStock(mSortedData);
        else
            targetDt = sortStock(mUiStocksData);

        updateInven(targetDt);
    }

    public void activeDropDown()
    {
        if (mDropDown.interactable)
        {
           // mDropDown.interactable = false;
            mSortedCnt = mUiStocksData.Count;
            clearInven(mUiStocksData);
            initInven(mUiStocksData, "private");
            updateInven(mUiStocksData);
        }
        else
        {
            //mDropDown.interactable = true;
            //mDropDown.value = 0;
            OnDropdownEvent();
        }
    }

    //DropDown public Method
    public void OnDropdownEvent()
    {     
        Debug.Log("[DropdownEvent] {" + mDropDown.value + "} clicked.");

        mSortedData = new Dictionary<IngredientData, int>(); //init
        mSortedData = sortStock(mDropDown.value);
        mSortedCnt = mSortedData.Count;
        clearInven(mSortedData);
        initInven(mSortedData, "private");
        updateInven(mSortedData);
    }

    //������ �������� ���� �������� �Ѿ ��, ������ ä�� �κ��丮 �����͸� ���������� UI�κ��丮�� �Ѱ��ش�.
    public void initInven(Dictionary<IngredientData, int> invenData, string order)
    {
        if(order == "public")
        {
            mUiStocksData = new Dictionary<IngredientData, int>();
            mUiStocksData = invenData; //UI��Ͽ� ����!

            int tmp = 0;
            foreach (KeyValuePair<IngredientData, int> stock in mUiStocksData)
            {
                GameObject invenUI = transform.GetChild(tmp).gameObject;
                mUiInvenStocks.Add(invenUI); //UIInvenGameObject List �߰�.
                tmp++;
            }
        }
       

        setInven(invenData);
    }

    private void setInven(Dictionary<IngredientData, int> _mData)
    {

        //invenData�� invenContainer(UI)List�� �־��ش�.
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
                nameTxt.transform.GetComponent<TMP_Text>().text = stock.Key.ingredientName.ToString();
            }

            //��ư ������Ʈ�� ������ ������ش�.
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
    private void clearInven(Dictionary<IngredientData, int> _mData)
    {
        //���� �ִ� ���� ������Ʈ�� ���� ������ ���� ���Ѵ�.
        int difference = _mData.Count - mUiInvenStocks.Count;
        //���� �ִ°� �� ���� ��� : ������ �ʱ�ȭ
        if (difference < 0) //difference�� 2�̸� 
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
            //���� �ִ°� �� ���� ��� : ���׸�ŭ ������Ʈ ����
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

                //��ư ������Ʈ�� ������ ������ش�.
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

    //�ش� data ��ųʸ��� ���� ��ŭ �����͸� �ٲ۴�.
    private void updateInven(Dictionary<IngredientData, int> _mData)
    {
        int tmp = 0;
        foreach (KeyValuePair<IngredientData, int> data in _mData)
        {
            GameObject stockObj = mUiInvenStocks[tmp];

            //GameObject name
            stockObj.name = data.Key.ingredientName;
            //�̹���
            stockObj.transform.GetComponent<Image>().sprite = data.Key.image;
            //cnt
            stockObj.transform.GetChild(0).GetComponent<TMP_Text>().text = data.Value.ToString();
            //name
            stockObj.transform.GetChild(1).GetComponent<TMP_Text>().text = data.Key.ingredientName.ToString();

            tmp++; //plus index value
        }
    }

    private IngredientData getDataWithSprite(string _spritename) //Sprite�� �Ű������� �ش� ������ data�� �˻��Ѵ�.
    {
        IngredientData data = inventoryManager.mIngredientDatas[inventoryManager.minvenLevel - 1].mItemList.Find(item => _spritename == item.image.name);

        return data;
    }


    //////////////////////////////
    /////////��� ���� �Լ�////////
    //////////////////////////////
    //��� ���ýÿ� �ش� ��� ������ 0�� �Ǹ� ����Ʈ���� ����, �ش� ��ᰳ���� 0���� 1�� �Ǹ� ����Ʈ�� �߰�.
    private void updateStockCnt(string _dtName, bool _switch)
    {
        //switch = true �� ��쿡�� ��� ����, switch = false �� ��쿡�� ��� ����� ������Ʈ ���
        //�Ű����ڴ� ������ �κ��丮�� ���� �ߴ�, �Ǵ� �����ϴ� ����̴�.
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
        
        //��ü Data���� ����
        mUiStocksData[stockDt]--;

        GameObject uiGameObj = findObjectWithData(stockDt);

        uiGameObj.transform.GetChild(0).GetComponent<TMP_Text>().text = mUiStocksData[stockDt].ToString();

        if (mUiStocksData[stockDt] != 0) return;

        //���� ��� 0�̶�� �ƿ� ����Ʈ���� ����
        removeStockInInven(stockDt, uiGameObj);
    }

    private bool isStockInSortedLayer(IngredientData stockDt) //���õ� ��ᰡ ���� �������� �ִ� �κ��丮 ���̾ �ִ��� bool�� ��ȯ
    {
        //�ش� ������ stock�� iEmotion����Ʈ�� �����ϴ��� Ȯ��
        bool isContain = stockDt.iEmotion.ContainsKey(mDropDown.value);
        bool isSorting = mDropDown.interactable; //���ķ��̾ ����� �������� Ȯ��.


        if (isSorting) //���� ���� && ���õ� ������ ���� ū ���� ���� ������ ���� ���� ��.
        {
            //������ ������������ ����
            var queryAsc = stockDt.iEmotion.OrderByDescending(x => x.Value);// int, int
            int maxEmo = queryAsc.First().Key;
            if (mDropDown.value != maxEmo)         
                return false;
        }

        return true;
    }
    private void cancelStock(string dataName)
    {
        IngredientData stockDt = inventoryManager.mIngredientDatas[inventoryManager.minvenLevel - 1].mItemList.Find(item => dataName == item.ingredientName);
        GameObject uiGameObj = findObjectWithData(stockDt);

        //�������� ��� ���ÿ��� ��ҵ� ��ᰡ �κ��丮�� �ִ��� �˻�.
        //���� ���ٸ� ������� �� ����Ʈ�� �� ��� �߰��ؼ� ���� +1 �Ѵ�.
        if (mUiStocksData.ContainsKey(stockDt))
        {
            mUiStocksData[stockDt]++;

            if (!isStockInSortedLayer(stockDt)) return;


            if (mDropDown.interactable)
            {
                mSortedData[stockDt]++;
            }


            uiGameObj.transform.GetChild(0).GetComponent<TMP_Text>().text = mUiStocksData[stockDt].ToString();
        }
        else
        {
            if (!isStockInSortedLayer(stockDt))
            {
                mUiStocksData.Add(stockDt, 1); //����Ʈ���� �ش� data �߰�
                return;
            }
            
            addStockInInven(stockDt, uiGameObj);

        }
            
    }

    private void addStockInInven(IngredientData stockDt, GameObject uiGameObj)
    {
        //1. ������ ������ �ϳ��� ���� �����.
        //�κ��丮 ��ü ������Ʈ
        if (!mDropDown.interactable)
            updateInven(mUiStocksData);
        else
            updateInven(mSortedData);

        //Data �߰�
        mUiStocksData.Add(stockDt, 1); //����Ʈ���� �ش� data �߰�
       

        if (!mDropDown.interactable)
            mSortedCnt = mUiStocksData.Count;
        else
        {
            mSortedData.Add(stockDt, 1); //����Ʈ���� �ش� data �߰�
            mSortedCnt = mSortedData.Count;
        }
            

        //2. �κ��丮�� ������ stock�� ������Ʈ �߰� �� �̹��� �ʱ�ȭ.
        // tmp instance
        GameObject lastStockInInven = mUiInvenStocks[mSortedCnt - 1];
        
        //Component �߰�
        GameObject cntTxt = Instantiate(mTxtInfoPrefab[0]);
        cntTxt.transform.SetParent(lastStockInInven.transform, false);
        cntTxt.transform.GetComponent<TMP_Text>().text = mUiStocksData[stockDt].ToString();

        GameObject nameTxt = Instantiate(mTxtInfoPrefab[1]);
        nameTxt.transform.SetParent(lastStockInInven.transform, false);
        nameTxt.transform.GetComponent<TMP_Text>().text = stockDt.ingredientName.ToString();

        Button btn = lastStockInInven.AddComponent<Button>();
        btn.onClick.AddListener(clicked);

        //Image Update
        lastStockInInven.transform.GetComponent<Image>().sprite = stockDt.image; 

        //Name Upadate
        lastStockInInven.name = stockDt.ingredientName; ; //Game Object Name �ʱ�ȭ       
    }

    private void removeStockInInven(IngredientData stockDt, GameObject uiGameObj)
    {
        mUiStocksData.Remove(stockDt); //����Ʈ���� �ش� data ����
        

        //�κ��丮 ��ü ������Ʈ
        if (!mDropDown.interactable) 
            mSortedCnt = mUiStocksData.Count;
        else
        {
            mSortedData.Remove(stockDt); //����Ʈ���� �ش� data ����
            mSortedCnt = mSortedData.Count;
        }
            
        //1. �κ��丮�� ������ stock�� ������Ʈ ���� �� �̹��� �ʱ�ȭ.
        // tmp instance
        GameObject lastStockInInven = mUiInvenStocks[mSortedCnt];
        lastStockInInven.name = "000"; //Game Object Name �ʱ�ȭ
        lastStockInInven.transform.GetComponent<Image>().sprite = mDefaultSprite; //img�ʱ�ȭ
        Destroy(lastStockInInven.transform.GetComponent<Button>()); // button component ����
        Destroy(lastStockInInven.transform.GetChild(0).gameObject); // cnt txt ����
        Destroy(lastStockInInven.transform.GetChild(1).gameObject); // name txt ����

        //2. ������ ������ �ϳ��� ���� �����.
        if (!mDropDown.interactable)
            updateInven(mUiStocksData);
        else
            updateInven(mSortedData);
    }

    

    //�κ��丮 ����� ������ �Ŀ��� ������ �������Ͽ��� UI�� �����ִ� ��ϵ� Update���־�� �Ѵ�.

    /////////////////////
    //�κ��丮 ���� �Լ�//
    ////////////////////


    //����Ʈ ��ü�� �����Ѵ�. ���� �� ���� �� ����Ʈ�� ���� UI�� �ݿ��Ѵ�.
    private Dictionary<IngredientData, int> sortStock(int _emotion) //�������� �з�
    {
        Dictionary<IngredientData, int> results = new Dictionary<IngredientData, int>();

        //�������� �з�: �Էµ��� ������ �����ִ� �͸� �̾Ƽ� tmpList�� �߰��Ѵ�.
        foreach (KeyValuePair<IngredientData, int> stock in mUiStocksData)
        {
            //�ش� ������ stock�� iEmotion����Ʈ�� �����ϴ��� Ȯ��
            if (!stock.Key.iEmotion.ContainsKey(_emotion)) continue;
            //���������� ����

            //������������ ����
            var queryAsc = stock.Key.iEmotion.OrderByDescending(x => x.Value);// int, int

            //ù��° ���� ���� ũ�Ƿ� ���� ū ���� �Ű����ڿ� ���� �����̶�� �߰����ش�.
            if(_emotion != queryAsc.First().Key) continue;
            results.Add(stock.Key, stock.Value);
        }

        
        return results;
    }

    private Dictionary<IngredientData, int> sortStock(Dictionary<IngredientData, int> _target) // �������� �з�:
    {
        Dictionary<IngredientData, int> results = new Dictionary<IngredientData, int>();

        var queryAsc = _target.OrderBy(x => x.Value);

        foreach (var dictionary in queryAsc)
            results.Add(dictionary.Key, dictionary.Value);

        return results;
    }
}
