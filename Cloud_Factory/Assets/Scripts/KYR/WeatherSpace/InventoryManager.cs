using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public IngredientList[] mIngredientDatas; // ��� ��� ������ ���� �ִ� ����Ʈ scriptable data

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
    //Debug�� ���� �ӽ� Button �Լ�. ���߿� ������ ����
    public void go2CloudFacBtn()
    {
        mIsSceneChange = true;
    }

    /////////////////
    /////Singlton////
    /////////////////
    static InventoryManager _instance = null; //�̱��� ��ü
    public static InventoryManager Instance() //static �Լ�, �����ϰ��� �ϴ� �ܺο��� ����� ��.
    {
        return _instance; //�ڱ��ڽ� ����
    }

    void Start()
    {
        if (_instance == null) // ���� ���۵Ǹ� �ڱ��ڽ��� �ִ´�.
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else  //�ٸ� ������ �Ѿ�ٰ� back ���� �� ���ο� ���� ������Ʈ�� �����ϱ� ���� ���ǹ�.
        {
            if(this != _instance)
            {
                Destroy(this.gameObject);
            }
        }

        mType = new List<IngredientData>(); //����Ʈ �ʱ�ȭ
        mCnt = new List<int>(); //����Ʈ �ʱ�ȭ
    }

    private void Update()
    {
        //���� ������������� �̵��ϸ� �ѹ��� �κ��丮�� Update �ȴ�.
        if (mIsSceneChange && SceneManager.GetActiveScene().name == "Give Cloud")
        {
            //Hierachyâ���� InventoryContainer�� ã�´�.
            mInventoryContainer = GameObject.Find("Cloud-System").transform.Find("PU_CloudCreater").transform.Find("Inventory").transform.Find("Container").gameObject;
            //Inventory level Ȯ�� �� ����Ʈ ũ�� ����
            //������ ���� �κ��丮 ���, �����̳ʿ� ����
            mInventoryContainer.transform.GetComponent<InventoryContainer>().initInven(mergeList2Dic(),"public");
            mIsSceneChange = false;
        }

    }
    /////////////////
    //���� ���� ����//
    /////////////////
    public List<IngredientData>  mType;
    public List<int>  mCnt;

    public int minvenLevel=1;

    private int mMaxStockCnt = 10; //�켱�� 10�����ϱ��� ����
    private int mMaxInvenCnt; //�켱�� 10�����ϱ��� ����




    //////////////////////////////
    //ä�� ���� �κ��丮 ���� �Լ�//
    //////////////////////////////
    public bool addStock(KeyValuePair<IngredientData, int> _stock)
    {
        mMaxInvenCnt = getInvenSize(minvenLevel);
        if (mType.Count >= mMaxInvenCnt) return false; // �κ��丮 ��ü�� �ƿ� ���� �� ��� return false
        //�κ��丮�� �ڸ��� �ִ� ���
        if (!mType.Contains(_stock.Key)) //�κ��� ���� ���� ������ ���� �׳� �ְ� return true
        {
            mType.Add(_stock.Key);
            mCnt.Add(_stock.Value);
            return true;
        }

        //�κ��丮 �ȿ� ������ ��� ������ �̹� �ִ� ���
        int idx = mType.IndexOf(_stock.Key); //index �� ����.

        if (mCnt[idx] >= mMaxStockCnt) return false;//�κ��丮 ��� �� ���尡�� ���� ����
        int interver = mMaxStockCnt - (_stock.Value+ mCnt[idx]); //���尡�� ���� - (���ο�� �߰����� �� �κ��丮�� ����ɰ���) = �������� ���
        if (interver >= 0) mCnt[idx] = mMaxStockCnt; //���̰� 0���� ũ�� ������ Max Cnt
        else
            mCnt[idx] += _stock.Value; //�ش� ������ ī��Ʈ �߰�.

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

    //�������ִ� 2���� ����Ʈ�� ��ųʸ� �������� ����: ��Ȱ�� ������ ���ؼ�!
    private Dictionary<IngredientData, int> mergeList2Dic()
    {
        Dictionary<IngredientData, int> results = new Dictionary<IngredientData, int>();
        foreach (IngredientData stock in mType)
            results.Add(stock, mCnt[mType.IndexOf(stock)]);

        return results;
    }
}
