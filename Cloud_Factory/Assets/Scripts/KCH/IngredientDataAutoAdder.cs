using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 희귀도 4의 재료들의 수확 가능 여부를 판단 후 자동으로 리스트에 추가하는 스크립트
// 날이 바뀔 때마다, SeasonDateCalc.CalcDay()에서 실행
// 처음에는 치유된 뭉티가 없으므로 1일차에서는 실행 X
public class IngredientDataAutoAdder : MonoBehaviour
{
	int guestCount = 20;        // 라이브 서비스 도중 손님의 수 추가 가능성으로 인한 변수 설정

    GuestInfos[] mGuestInfo;

    private Guest mGuestManager;                        // 손님의 정보를 받아오기 위한 참조
    private InventoryManager mInverntoryManager;        // 인벤토리 매니저의 채집 가능 재료 리스트를 불러오기 위한 참조

    // Start is called before the first frame update
    void Start()
    {
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mInverntoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
       
        mGuestInfo = mGuestManager.mGuestInfo;   // GuestManager의 손님 정보를 받아옴
	}

    // 치유된 손님이 있는지 체크하는 함수
    public void CheckIsCured()
    {
        for(int num = 0; num < guestCount; num++)
        {
            if (mGuestInfo[num].isCure) { AddIngredientToList(num); }
        }
    }

    // 치유된 손님이 있을 시, InventoryManager의 mIngredientData[3](희귀도 4를 저장하는 리스트)에 치유된 손님의 재료를 추가
    private void AddIngredientToList(int guest_num)
    {
        // guest_num에 해당하는 손님의 제공 재료 받아오는 과정
        IngredientData data = ScriptableObject.CreateInstance<IngredientData>();
        string tempWord = mGuestInfo[guest_num].mSeed.ToString();
        data.name = tempWord;
        data.dataName = tempWord;
        data.image = Resources.Load<Sprite>("Sprite/Ingredient/Rarity4/" + "M4_" + data.name);
        data.emotions = new EmotionInfo[1];
        data.emotions[0] = new EmotionInfo((Emotion)mGuestInfo[guest_num].mSeed, -1);
        data.init();

        // 재료가 중복되는지 검사
        foreach (IngredientData tempData in mInverntoryManager.mIngredientDatas[3].mItemList)
        {
            if (tempData.dataName == data.dataName) { return; }
        }

        // 재료가 중복되지 않으면 희귀도 4 재료 리스트에 추가
        mInverntoryManager.mIngredientDatas[3].mItemList.Add(data);
    } 
}
