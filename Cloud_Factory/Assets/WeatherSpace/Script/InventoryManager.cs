using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    /////////////////
    //서버 저장 변수//
    /////////////////
    public List<IngredientData>  mType;
    public List<int>  mCnt;

    private int mMaxStockCnt = 10; //우선은 10개이하까지 가능
    private int mMaxInvenCnt = 30; //우선은 30개이하까지 가능

    private void Start()
    {
        mType = new List<IngredientData>(); //리스트 초기화
        mCnt = new List<int>(); //리스트 초기화
    }


    //////////////////////////////
    //채집 관련 인벤토리 연동 함수//
    //////////////////////////////
    public bool addStock(KeyValuePair<IngredientData, int> _stock)
    {
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


    //////////////////////////////
    /////////재료 선택 함수////////
    //////////////////////////////
    //재료 선택시에 해당 재료 개수가 0이 되면 리스트에서 제거, 해당 재료개수가 0에서 1이 되면 리스트에 추가.
    public void updateStockCnt(IngredientData _stockDt,bool _switch)
    {
        //switch = true 인 경우에는 재료 선택, switch = false 인 경우에는 재료 취소의 업데이트 경우
        //매개인자는 무조건 인벤토리에 존재 했던, 또는 존재하는 재료이다.
        if (_switch)
            selectStock(_stockDt);
        else
            cancelStock(_stockDt);

        return;
    }

    private void selectStock(IngredientData _stockDt)
    {
        int idx = mType.IndexOf(_stockDt); //index 값 저장.
        int updatedCnt = --mCnt[idx];
        if (updatedCnt != 0) return;

        //남은 재고가 0이라면 아예 리스트에서 삭제
        mType.RemoveAt(idx);
        mCnt.RemoveAt(idx);
    }

    private void cancelStock(IngredientData _stockDt)
    {
        //구름제작 재료 선택에서 취소된 재료가 인벤토리에 있는지 검사.
        //만약 없다면 취소했을 시 리스트에 새 재료 추가해서 개수 +1 한다.
        if (mType.Contains(_stockDt)) mCnt[mType.IndexOf(_stockDt)]++;
        else
        {
            mType.Add(_stockDt);
            mCnt.Add(1);
        }
    }
    
    //인벤토리 목록이 수정된 후에는 무조건 재정렬하여서 UI에 보여주는 목록도 Update해주어야 한다.

    /////////////////////
    //인벤토리 정렬 함수//
    ////////////////////

    //나눠져있는 2개의 리스트를 딕셔너리 형식으로 리턴: 원활한 정렬을 위해서!
    private Dictionary<IngredientData, int> mergeList2Dic()
    {
        Dictionary<IngredientData, int> results = new Dictionary<IngredientData, int>();
        foreach(IngredientData stock in mType)
            results.Add(stock, mCnt[mType.IndexOf(stock)]);
        
        return results;
    }

    //리스트 자체를 정렬한다. 정렬 할 때는 새 리스트를 만들어서 UI에 반영한다.
    private Dictionary<IngredientData, int> sortStock(Emotion _emotion) 
    {
        Dictionary<IngredientData, int> tmpList = mergeList2Dic();
        Dictionary<IngredientData, int> results = new Dictionary<IngredientData, int>();

        //감정별로 분류: 입력들어온 감정이 속해있는 것만 뽑아서 tmpList에 추가한다.
        foreach (KeyValuePair<IngredientData, int> stock in tmpList)
        {
            if (!stock.Key.iEmotion.ContainsKey((int)_emotion)) continue;

            results.Add(stock.Key, stock.Value);
        }
        return results;
    }

    private Dictionary<IngredientData, int> sortStock() // 개수별로 분류:
    {
        Dictionary<IngredientData, int> tmpList = mergeList2Dic();
        Dictionary<IngredientData, int> results = new Dictionary<IngredientData, int>();

        var queryAsc = tmpList.OrderBy(x => x.Value);

        foreach (var dictionary in queryAsc)
            results.Add(dictionary.Key, dictionary.Value);

        return results;
    }
}
