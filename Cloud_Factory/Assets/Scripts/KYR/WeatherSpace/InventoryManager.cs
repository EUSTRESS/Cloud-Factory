using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public IngredientList[] mIngredientDatas; // 모든 재료 정보를 갖고 있는 리스트 scriptable data

    private bool mIsSceneChange = false;

    [SerializeField]
    private GameObject mInventoryContainer;

    public void setDataList(List<IngredientList> Ltotal)
    {
        mIngredientDatas = new IngredientList[3];
        int idx = 0;
        foreach (IngredientList rarity in Ltotal)
        {
            mIngredientDatas[idx] = rarity;
            idx++;
        }
    }
    //Debug를 위한 임시 Button 함수. 나중에 삭제할 예정
    public void go2CloudFacBtn()
    {
        mIsSceneChange = true;
    }

    /////////////////
    /////Singlton////
    /////////////////
    static InventoryManager _instance = null; //싱글톤 객체
    public static InventoryManager Instance() //static 함수, 공유하고자 하는 외부에서 사용할 것.
    {
        return _instance; //자기자신 리턴
    }

    void Start()
    {
        if (_instance == null) // 게임 시작되면 자기자신을 넣는다.
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else  //다른 씬으로 넘어갔다가 back 했을 때 새로운 복제 오브젝트를 방지하기 위한 조건문.
        {
            if(this != _instance)
            {
                Destroy(this.gameObject);
            }
        }

        mType = new List<IngredientData>(); //리스트 초기화
        mCnt = new List<int>(); //리스트 초기화
    }

    private void Update()
    {
        //씬이 구름공장씬으로 이동하면 한번만 인벤토리가 Update 된다.
        if (mIsSceneChange && SceneManager.GetActiveScene().name == "Give Cloud")
        {
            //Hierachy창에서 InventoryContainer을 찾는다.
            mInventoryContainer = GameObject.Find("Cloud-System").transform.Find("PU_CloudCreater").transform.Find("Inventory").transform.Find("Container").gameObject;
            //Inventory level 확인 후 리스트 크기 조정
            //데이터 상의 인벤토리 목록, 컨테이너에 전달
            mInventoryContainer.transform.GetComponent<InventoryContainer>().initInven(mergeList2Dic(),"public");
            mIsSceneChange = false;
        }

    }
    /////////////////
    //서버 저장 변수//
    /////////////////
    public List<IngredientData>  mType;
    public List<int>  mCnt;

    public int minvenLevel=1;

    private int mMaxStockCnt = 10; //우선은 10개이하까지 가능
    private int mMaxInvenCnt; //우선은 10개이하까지 가능




    //////////////////////////////
    //채집 관련 인벤토리 연동 함수//
    //////////////////////////////
    public bool addStock(KeyValuePair<IngredientData, int> _stock)
    {
        mMaxInvenCnt = getInvenSize(minvenLevel);
        if (mType.Count >= mMaxInvenCnt) return false; // 인벤토리 자체가 아예 가득 찬 경우 return false
        //인벤토리에 자리가 있는 경우
        if (!mType.Contains(_stock.Key)) //인벤에 없이 새로 들어오는 경우는 그냥 넣고 return true
        {
            mType.Add(_stock.Key);
            mCnt.Add(_stock.Value);
            return true;
        }

        //인벤토리 안에 들어오는 재료 종류가 이미 있는 경우
        int idx = mType.IndexOf(_stock.Key); //index 값 저장.

        if (mCnt[idx] >= mMaxStockCnt) return false;//인벤토리 재료 당 저장가능 개수 제한
        int interver = mMaxStockCnt - (_stock.Value+ mCnt[idx]); //저장가능 개수 - (새로운게 추가됐을 떄 인벤토리에 저장될개수) = 버려지는 재고
        if (interver >= 0) mCnt[idx] = mMaxStockCnt; //차이가 0보다 크면 어차피 Max Cnt
        else
            mCnt[idx] += _stock.Value; //해당 아이템 카운트 추가.

        return true;
    }

    private int getInvenSize(int invenLv)
    {
        int invensize = 0;
        switch (invenLv)
        {
            case 1:
                invensize = 8;
                break;
            case 2:
                invensize = 12;
                break;
            case 3:
                invensize = 24;
                break;
            default:
                break;
        }

        return invensize;
    }

    //나눠져있는 2개의 리스트를 딕셔너리 형식으로 리턴: 원활한 정렬을 위해서!
    private Dictionary<IngredientData, int> mergeList2Dic()
    {
        Dictionary<IngredientData, int> results = new Dictionary<IngredientData, int>();
        foreach (IngredientData stock in mType)
            results.Add(stock, mCnt[mType.IndexOf(stock)]);

        return results;
    }
}
