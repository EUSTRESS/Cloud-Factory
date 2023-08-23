using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json; // LJH, Json Namespace
using System.Text.RegularExpressions;

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
    public List<EmotionInfo> mFinalEmotions; //구름 꾸미기 이후의 최종 감정.
    public List<GameObject> mPartsList; //구름 꾸미기 이후의 최종 감정.
    public int mdate;
    public GameObject mBase;
    public VirtualGameObject mVBase;
    public List<VirtualGameObject> mVPartsList; //구름 꾸미기 이후의 최종 감정.

    public List<IngredientData> mIngredientDatas;

    [SerializeField]
    private int mCloudTypeNum;
    public StoragedCloudData(List<EmotionInfo> _FinalEmotions, GameObject _base, List<GameObject> _mPartsList, List<IngredientData> ingr_datas)
    {
        mdate = 10; //일단 기본으로 세팅
        mFinalEmotions = _FinalEmotions;
        mBase = _base;
        mPartsList = _mPartsList;
        mIngredientDatas= ingr_datas;

        //VirtualSetting
        mVPartsList = new List<VirtualGameObject>();
        mVBase = new VirtualGameObject(mBase.transform.position, mBase.transform.rotation, mBase.transform.GetComponent<RectTransform>().rect.width, mBase.transform.GetComponent<RectTransform>().rect.height, mBase.GetComponent<Image>().sprite);
        for (int i = 0; i < mPartsList.Count; i++)
        {
            mVPartsList.Add(new VirtualGameObject(_mPartsList[i].transform.localPosition, _mPartsList[i].transform.rotation, _mPartsList[i].transform.GetComponent<RectTransform>().rect.width, _mPartsList[i].transform.GetComponent<RectTransform>().rect.height, _mPartsList[i].GetComponent<Image>().sprite));
        }

        SetCloudTypeNumber();

    }

    private void SetCloudTypeNumber()
    {
        Sprite sprite = mVBase.mImage;
        string sspriteNum = Regex.Replace(mVBase.mImage.name, @"[^0-9]", ""); //숫자만 추출
        mCloudTypeNum = int.Parse(sspriteNum);
    }

    public int GetCloudTypeNum()
    {
        return mCloudTypeNum;
    }

    public int GetIngredientDataNum()
    {
        return mIngredientDatas.Count;
    }
}

[System.Serializable]
public class CloudData
{
    public int mShelfLife;
    public List<EmotionInfo> mEmotions;

    
    private bool[] isCloudAnimProgressed;
    private bool mState; //0 = 폐기 1 = 가능
    private Sprite mcloudBaseImage;
    private Sprite mcloudDecoBaseImage;
    private List<IngredientData> mIngredientDatas;

    private List<Sprite> mcloudParts; //무조건 있음 필수!
    private List<List<Sprite>> mdecoImages; //2차원 리스트: L M S 사이즈 필요! 최대 2개

    private List<EmotionInfo> mFinalEmotions; //구름 꾸미기 이후의 최종 감정.

    private int mIngredientDtCount;
    public CloudData(List<EmotionInfo> Emotions,int shelfLife, List<IngredientData> ingr_datas)
    {
        mEmotions = Emotions;
        mShelfLife = shelfLife;
        mIngredientDatas= ingr_datas;
        mIngredientDtCount = mIngredientDatas.Count;
        mFinalEmotions = new List<EmotionInfo>();
        isCloudAnimProgressed = new bool[3];
        setAnimProgressed();
        //계산식함수로 자동으로 데이터 세팅
        setShelfLife();
        setCloudImage(mEmotions);
        setDecoImage(mEmotions);
    }

    public CloudData()
    {
        mEmotions = new List<EmotionInfo>();
		isCloudAnimProgressed = new bool[3];
		setAnimProgressed();
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

    public bool getAnimProgressed(int anim_num)
    {
        if(anim_num > 2) { return false; }
        return isCloudAnimProgressed[anim_num];
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
            mFinalEmotions.Add(new EmotionInfo(mEmotions[i].Key, mEmotions[i].Value * _value[i]/3));
        }
       
    }
    private void setAnimProgressed()
    {
        for(int idx = 0; idx < 3; idx++)
        {
            isCloudAnimProgressed[idx] = false;
        }
	}

    public void setAnimProgressed(int anim_num, bool anim_bool)
    {
        isCloudAnimProgressed[anim_num] = anim_bool;
    }

    //Private method
    private void setShelfLife()
    {
        //감정에 따라 맞는 보관기간-> 사용한 재료의 개수에 따라서 달라진다.
    }

    private void setCloudImage(List<EmotionInfo> Emotions)
    {
        //감정에 따라 맞는 base 구름이미지
        string targetImgName = ((int)mEmotions[0].Key).ToString();
        if ((int)mEmotions[0].Key >= 8)//현재 리소스 부재로인한 오류 임시 해결 방책.
        {
            targetImgName = "0";
            mEmotions[0].Key = Emotion.PLEASURE;

        }

        string targetUnion = "";
        switch (mIngredientDtCount)
        {
            case 1:
            case 2:
                targetUnion = "2";
                break;
            case 3:
                targetUnion = "3";
                break;
            case 4:
            default:
                targetUnion = "4";
                break;
        }
        Debug.Log("targetUnion" + targetUnion);
        //mcloudBaseImage = Resources.Load<Sprite>("Sprite/Base/"+ targetUnion + "union/" + "OC_Cloud_" + ((int)mEmotions[0].Key).ToString());
        mcloudBaseImage = Resources.Load<Sprite>("Sprite/Base/"+ targetUnion + "union/" + "OC_Cloud_" + targetImgName);
        mcloudDecoBaseImage = Resources.Load<Sprite>("Sprite/Base/" + targetUnion + "union/" + "Deco/"+ "OC_Cloud_" + ((int)mEmotions[0].Key).ToString());

        if (mcloudBaseImage == null || mcloudDecoBaseImage == null)
            Debug.LogWarning("NO CloudImage");
            
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

    public List<IngredientData> GetIngredientDatas() { return mIngredientDatas; }
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
    private CloudData beginningCloudData;    // 구름이 데코가 끝나면 돌아가야 할 초기값 설정
    public CloudData createdCloudData = null;

    

    public void setDataList(List<IngredientList> Ltotal)
    {
        mIngredientDatas = new IngredientList[4];
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
    public static InventoryManager _instance = null; //싱글톤 객체
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
        beginningCloudData = new CloudData();

        ingredientHistory = new List<List<IngredientData>>();
        for (int i = 0; i < 4; i++)
        {
            ingredientHistory.Add(new List<IngredientData>());
        }

        cloudHistory = new List<Sprite>();
    }

    private void Update()
    {
        foreach (IngredientData stock in mType)
        {
            if (stock.image != null) continue;

            //stock.rematchSpriteData(mIngredientDatas);
            ReAllocateInventoryData();

            break;
        }
    }

    private void ReAllocateInventoryData()
    {
        List<IngredientData> List = new List<IngredientData>();
        foreach (IngredientData stock in mType)
        {
            List.Add(mIngredientDatas[stock.rarity - 1].mItemList.Find(item => stock.dataName == item.dataName));
        }
        mType = List;
    }

    /////////////////
    //서버 저장 변수//
    /////////////////
    public CloudStorageData mCloudStorageData; //구름 인벤토리 데이터 리스트 클래스.

    public List<IngredientData>  mType;
    public List<int>  mCnt;
    public List<List<IngredientData>> ingredientHistory;
    public List<Sprite> cloudHistory;

    public int minvenLevel=3;

    public int mMaxStockCnt = 10; //우선은 10개이하까지 가능
    public int mMaxInvenCnt; //우선은 10개이하까지 가능

    //////////////////////////////
    //채집 관련 인벤토리 연동 함수//
    //////////////////////////////
    public bool addStock(KeyValuePair<IngredientData, int> _stock)
    {        
        //인벤토리에 자리가 있는 경우
        if (mType.Contains(_stock.Key)) 
        {
            //인벤토리 안에 들어오는 재료 종류가 이미 있는 경우
            int idx = mType.IndexOf(_stock.Key); //index 값 저장.

            if (mCnt[idx] >= mMaxStockCnt) return false;//인벤토리 재료 당 저장가능 개수 제한
            int interver = mMaxStockCnt - (_stock.Value + mCnt[idx]); //저장가능 개수 - (새로운게 추가됐을 떄 인벤토리에 저장될개수) = 버려지는 재고
            if (interver <= 0) mCnt[idx] = mMaxStockCnt; //차이가 0보다 크면 어차피 Max Cnt
            else
                mCnt[idx] += _stock.Value; //해당 아이템 카운트 추가.


            return true;

        }
        else//인벤에 없이 새로 들어오는 경우는 그냥 넣고 return true
        {
            mMaxInvenCnt = getInvenSize(minvenLevel);
           if (mType.Count >= mMaxInvenCnt)
            {
                mType.RemoveAt(0);
                mCnt.RemoveAt(0);
            } //꽉찬 경우 가장 먼저 들어온 순서로 삭제후 저장

            mType.Add(_stock.Key);
            mCnt.Add(_stock.Value);

            for (int i = 0; i < ingredientHistory.Count; i++)
            {
                if (!mIngredientDatas[i].mItemList.Contains(_stock.Key)) continue;
                if (!ingredientHistory[i].Contains(_stock.Key)) ingredientHistory[i].Add(_stock.Key);
                break;
            }
            
            return true;
        }

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
        List<GameObject> parts = new List<GameObject>();
        GameObject cloudBase = Instantiate(_cloudObject.gameObject, _cloudObject.transform.position, _cloudObject.transform.rotation);

        for (int i = 0; i < cloudBase.transform.childCount; i++)
        {
            DontDestroyOnLoad(cloudBase.transform.GetChild(i).gameObject);
            parts.Add(cloudBase.transform.GetChild(i).gameObject);
        }

        if(!cloudHistory.Contains(createdCloudData.getBaseCloudSprite())) cloudHistory.Add(createdCloudData.getBaseCloudSprite());

        mCloudStorageData.mDatas.Add(new StoragedCloudData(createdCloudData.getFinalEmotion(), cloudBase, parts, createdCloudData.GetIngredientDatas()));
        createdCloudData = beginningCloudData;
    }
}
