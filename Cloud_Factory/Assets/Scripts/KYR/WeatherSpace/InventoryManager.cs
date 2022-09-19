using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json; // LJH, Json Namespace

// LJH, Data 저장할 임시 복사 공간, Monobehaviour 상속 금지
[System.Serializable]
public class InventoryData
{
    public List<IngredientData> mType;
    public List<int> mCnt;   

    public int minvenLevel = 1;

    public int mMaxStockCnt = 10; //우선은 10개이하까지 가능
    public int mMaxInvenCnt; //우선은 10개이하까지 가능
}

[System.Serializable]
public class CloudStorageData
{
    public List<StoragedCloudData> mDatas;

    public CloudStorageData()
    {
        mDatas = new List<StoragedCloudData>();
    }
}

[System.Serializable]
public class StoragedCloudData
{
    public GameObject mFinalCloud; //다 꾸민 구름 오브젝트.
    public List<EmotionInfo> mFinalEmotions; //구름 꾸미기 이후의 최종 감정.

    public StoragedCloudData(GameObject _cloudObject, List<EmotionInfo> _FinalEmotions)
    {
        mFinalCloud = _cloudObject ;
        mFinalEmotions = _FinalEmotions;
    }

}
[System.Serializable]
public class CloudData
{
    public int mShelfLife;
    public List<EmotionInfo> mEmotions;

    private bool mState; //0 = 폐기 1 = 가능
    private Sprite mcloudBaseImage;
    private Sprite mcloudDecoBaseImage;

    private List<Sprite> mcloudParts; //무조건 있음 필수!
    private List<List<Sprite>> mdecoImages; //2차원 리스트: L M S 사이즈 필요! 최대 2개

    private List<EmotionInfo> mFinalEmotions; //구름 꾸미기 이후의 최종 감정.
    public CloudData(List<EmotionInfo> Emotions)
    {
        mEmotions = Emotions;
        mFinalEmotions = new List<EmotionInfo>();
        //계산식함수로 자동으로 데이터 세팅
        setShelfLife(mEmotions);
        setCloudImage(mEmotions);
        setDecoImage(mEmotions);
    }

    public int getDecoPartsCount()
    {
        return mdecoImages.Count;
    }

    public List<EmotionInfo> getFinalEmotion()
    {
        return mFinalEmotions;
    }
 
    public Sprite getBaseCloudSprite()
    {
        return mcloudBaseImage;
    }

    public Sprite getForDecoCloudSprite()
    {
        return mcloudDecoBaseImage;
    }
    public List<Sprite> getSizeDifferParts(int _idx)
    {
        return mdecoImages[_idx];
    }

    public List<Sprite> getCloudParts()
    {
        return mcloudParts;
    }

    public int getBaseCloudColorIdx()
    {
        return (int)mEmotions[0].Key;
    }

    public List<int> getMaxDecoPartsCount()
    {
        List<int> Lresult = new List<int>();

        for (int i = 1; i < mEmotions.Count; i++)
        {
            int value = mEmotions[i].Value;

            int iReuslt = (value % 10 >= 0 && value % 10 <= 4) ? value - (value % 10) : (value + 10) - (value % 10);
            Lresult.Add(iReuslt / 10);
        }

        return Lresult;
    }

    public void addFinalEmotion(List<int> _value)
    {
        for(int i = 0; i < mEmotions.Count; i++)
        {
            mFinalEmotions.Add(new EmotionInfo(mEmotions[i].Key, mEmotions[i].Value * _value[i]));
        }
       
    }


    //Private method
    private void setShelfLife(List<EmotionInfo> Emotions)
    {
        //감정에 따라 맞는 보관기간
    }
    private void setCloudImage(List<EmotionInfo> Emotions)
    {
        //감정에 따라 맞는 base 구름이미지
        string targetImgName = ((int)mEmotions[0].Key).ToString();
        if ((int)mEmotions[0].Key < 8)
            targetImgName = "0";
        mcloudBaseImage = Resources.Load<Sprite>("Sprite/CloudBase/2union/" + "OC_Cloud2_" + ((int)mEmotions[0].Key).ToString());
        mcloudDecoBaseImage = Resources.Load<Sprite>("Sprite/CloudBase/DecoSpaceVer/" + "OC_Cloud_" + ((int)mEmotions[0].Key).ToString());
    }
    private void setDecoImage(List<EmotionInfo> Emotions)
    {
        //구름 조각 파츠
        mcloudParts = new List<Sprite>();
        mcloudParts.Add(Resources.Load<Sprite>("Sprite/CloudDeco/CloudParts/OC_" + ((int)mEmotions[0].Key).ToString() + "_piece_" + "1"));
        mcloudParts.Add(Resources.Load<Sprite>("Sprite/CloudDeco/CloudParts/OC_" + ((int)mEmotions[0].Key).ToString() + "_piece_" + "2"));
        mcloudParts.Add(Resources.Load<Sprite>("Sprite/CloudDeco/CloudParts/OC_" + ((int)mEmotions[0].Key).ToString() + "_piece_" + "3"));

        //Assets/Resources/Sprite/CloudDeco/CloudParts/OC_0_piece_3.png

        //감정 파츠
        mdecoImages = new List<List<Sprite>>();
        //감정에 따라 맞는 데코 이미지
        for (int i = 1; i < Emotions.Count;i++)
        {
            List<Sprite> decoList = new List<Sprite>();
            decoList.Add(Resources.Load<Sprite>("Sprite/CloudDeco/L/" + "OC_L_" + ((int)mEmotions[i].Key).ToString()));
            decoList.Add(Resources.Load<Sprite>("Sprite/CloudDeco/M/" + "OC_M_" + ((int)mEmotions[i].Key).ToString()));
            decoList.Add(Resources.Load<Sprite>("Sprite/CloudDeco/S/" + "OC_S_" + ((int)mEmotions[i].Key).ToString()));
            mdecoImages.Add(decoList);
        }
    }
}

[System.Serializable]
//구름 및 재료 인벤토리 관련 매니저
public class InventoryManager : MonoBehaviour
{
    public IngredientList[] mIngredientDatas; // 모든 재료 정보를 갖고 있는 리스트 scriptable data

    private bool mIsSceneChange = false;

    [SerializeField]
    public GameObject mInventoryContainer;

    //구름 데코 관련
    public CloudData createdCloudData = null;

    

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

        mCloudStorageData = new CloudStorageData();
    }

    /////////////////
    //서버 저장 변수//
    /////////////////
    public CloudStorageData mCloudStorageData; //구름 인벤토리 데이터 리스트 클래스.

    public List<IngredientData>  mType;
    public List<int>  mCnt;

    public int minvenLevel=1;

    public int mMaxStockCnt = 10; //우선은 10개이하까지 가능
    public int mMaxInvenCnt; //우선은 10개이하까지 가능

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
    public Dictionary<IngredientData, int> mergeList2Dic()
    {
        Dictionary<IngredientData, int> results = new Dictionary<IngredientData, int>();
        foreach (IngredientData stock in mType)
            results.Add(stock, mCnt[mType.IndexOf(stock)]);

        return results;
    }


    //////////////////////////////
    //구름인벤토리 관련 함수//
    //////////////////////////////
    ///
    //데코 되어진 구름 오브젝트 저장
    public void addStock(GameObject _cloudObject)
    {
        mCloudStorageData.mDatas.Add(new StoragedCloudData(Instantiate(_cloudObject, _cloudObject.transform.position, _cloudObject.transform.rotation), createdCloudData.getFinalEmotion()));
    }
}
