using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class YardHandleSystem : MonoBehaviour
{
    public InventoryManager inventoryManager;
    TutorialManager mTutorialManager;
    private SOWManager mSOWManager;

 //   public IngredientList[] mRarityList;
	public Sprite[] mImages;

    private Dictionary<GameObject, int> mYards;

    struct Yard //마당 구조체 정의
    {
        private GameObject self;

        private int gatherCnt;//unsigned int 로 고칠 수 있을까? 고친다면 밑에 조건문은 ㅇ떻게 바꾸지
        private Sprite[] sprites;

        public void init(GameObject gameObj,Sprite[] _sprites) //초기화 함수
        {
            self = gameObj;
            sprites = new Sprite[2];

            if (_sprites.Length != 2) return; //Overflow 방지
            else
                Debug.Log("[Yard Init] Not Right Sprite array input");
            sprites[0] = _sprites[0];
            sprites[1] = _sprites[1];

            updateSprite();
        }

        public bool canGather() //채집 가능 상태 
        {
            if (gatherCnt <= 0) return false; //채집 가능 횟수가 0 이하 이면 가능 횟수 모두 소진.
            else return true;
        }

        public void gather() //채집 하는 행위
        {
            if (gatherCnt == 0) return; //이미 가능 횟수가 모두 소진 되면 채집이 진행되지 않는다.

            gatherCnt--;
            updateSprite();

        }

        private void rndGatSys(GameObject GInventory) //random gather System
        {
            int invenLev = 1; //나중에 GetComponent로 가져올 예정

        }

        private void activeBoard() //채집 팝업 창 활성화
        {

        }

        private void updateSprite()
        {
            //gatherCnt에 따라 yardSprite 바뀜
            if (gatherCnt <= 0) self.GetComponent<SpriteRenderer>().sprite = sprites[0];
            else if (gatherCnt >= 1 && self.GetComponent<SpriteRenderer>().sprite != sprites[1])
                self.GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
    };

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
		mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

		mYards = new Dictionary<GameObject, int>();

        UpdateYardGatherCount(); //Yard 그룹에 속한 yard들을 리스트에 넣어서 관리.
            //이 딕셔너리의 int는 채집 횟수 0이 되면 채집 불가능하다!
    }

    public void UpdateYardGatherCount()
    {
        mYards.Clear();
        for (int i = 0; i < transform.childCount; i++) { mYards.Add(transform.GetChild(i).gameObject, mSOWManager.yardGatherCount[i]); }
	}

    // WeatherUIManager에서 YardHandleSystem에 접근할 때, 채집 가능한 상태인지 알려주기 위한 함수
    public bool CanBeGathered(GameObject iClickedYard)
    {
        if (mYards[iClickedYard] <= 0) return false;
        else return true;
    }
    

    public Dictionary<IngredientData, int>Gathered(GameObject iClickedYard,int totalCnt) //클릭함수
    {
        Dictionary<IngredientData, int> results = getRndGatheredResult(totalCnt);
        Dictionary<IngredientData, int> complete = new Dictionary<IngredientData, int>();
        Debug.Log("[System]총" + results.Count + "가 채집되었습니다!");
        foreach(KeyValuePair<IngredientData, int> result in results)
        {
            if (inventoryManager.addStock(result))
            {
                Debug.Log("[System]채집 성공| 종류:" + result.Key + "|개수: " + result.Value);
                complete.Add(result.Key, result.Value);
            }
            else {
                Debug.Log("[System]채집 실패| 인벤토리가 꽉 찼거나 수집 가능 재료 개수를 초과하였습니다| 종류:." + result.Key + "|개수: " + result.Value);
                complete.Add(result.Key, result.Value);
            }

        }

        mYards[iClickedYard]--;

        return complete; //저장에 성공한 리스트만 반환한다.
    }


    private Dictionary<IngredientData, int> getRndGatheredResult(int totalCnt) //랜덤으로 채집한 리스트 리턴.
    {
        //Key: 재료 종류  Value: 획득할 재료 개수
        Dictionary<IngredientData, int> results= new Dictionary<IngredientData, int>();

        //희귀도 랜덤, 그중에서도 종류 랜덤.
        //Random: 희귀도, 희귀도 내 종류, 재료 수, 채집할 재료 종류 수
        Debug.Log("총 채집할 종류:" + (totalCnt+1));

        // 튜토리얼 시 채집되는 재료 고정 TODO: 기획서에 맞도록 재료 종류, 수량 조정 필요
        if (mTutorialManager.isFinishedTutorial[2] == false
            && mTutorialManager.isTutorial == true)
        {
            results.Add(inventoryManager.mIngredientDatas[0].mItemList[6], 1);
            results.Add(inventoryManager.mIngredientDatas[0].mItemList[7], 1);
        }
        else
        {
            while (results.Count <= totalCnt)
            {
                IngredientData tmp = inventoryManager.mIngredientDatas[getRndRarityType(inventoryManager.minvenLevel) - 1].getRndIngredient();
                Debug.Log(tmp);
                if (results.ContainsKey(tmp)) continue; //중복방지
                results.Add(tmp, Random.Range(1, 6));
                Debug.Log("추가: " + tmp);
            }
        }

        return results;
    }

   
   private int getRndRarityType(int _invenLv) //매개변수: 인벤토리 lv, 인벤토리 lv에 따라서 어떤 희귀도의 재료가 나올지 return
    {
        int randomValue= Random.Range(0,1000);

        // 희귀도가 4인 재료를 채집 가능한지 체크 후 bool 변수로 저장
        bool checkRarity4 = false;
        if (inventoryManager.mIngredientDatas[3].mItemList.Count > 0) { checkRarity4 = true; }
       
        int rarity = 1;
        switch(_invenLv)
        {
            case 1:
                if (checkRarity4){                              // 희귀도 4 존재 시
                    if (randomValue < 970) { rarity = 1; }      // (97%, 0%, 0%, 3%) : rarity 1, 2, 3, 4순
                    else { rarity = 4; }
                }
                else { rarity = 1; }                            // (100%, 0%, 0%, 0%)
                break;
            case 2:
                if (checkRarity4){                              // (77.6%, 19.4%, 0%, 3%)
                    if(randomValue < 776) { rarity = 1; }
                    else if(randomValue >= 776 && randomValue < 970) { rarity = 2; }
                    else { rarity = 4; }
                }
                else {                                          // (80%, 20%, 0%, 0%)
                    if (randomValue < 800) { rarity = 1; }
                    else { rarity = 2; }
                }
                break;
            case 3:
                if (checkRarity4) {                             // (58.2%, 29.1%, 9.7%, 3%)
                    if (randomValue < 582) { rarity = 1; }
                    else if (randomValue >= 582 && randomValue < 873) { rarity = 2; }
                    else if (randomValue >= 873 && randomValue < 970) { rarity = 3; }
                    else { rarity = 4; }
                }
                else {                                          // (60%, 30%, 10%, 0%)
                    if (randomValue < 600) { rarity = 1; }
                    else if (600 <= randomValue && randomValue < 900) { rarity = 2; }
                    else { rarity = 3; }
                }
                break;
            default:
                break;
        }
        Debug.Log("rarity" + rarity);
        return rarity;
    }
}
