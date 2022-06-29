using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class YardHandleSystem : MonoBehaviour
{
    public IngredientList[] Lrarity;

    private bool isAble;
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
            if (gatherCnt == 0) self.GetComponent<SpriteRenderer>().sprite = sprites[0];
            else if (gatherCnt >= 0 && self.GetComponent<SpriteRenderer>().sprite != sprites[1])
                self.GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
    };
    // Start is called before the first frame update
    void Awake()
    {
        Lrarity = new IngredientList[3]; //희귀도 1,2,3 ... 4(는 추후 추가)

        for (int i = 0; i < Lrarity.Length; i++)
            Lrarity[i].init();
 
    }

    private List<IngredientData> getRndGatheredResult() //랜덤으로 채집한 리스트 3개 리턴.
    {
        List<IngredientData> result = new List<IngredientData>();
        //희귀도 랜덤, 그중에서도 종류 랜덤.
        int cnt = 0;
        while(true)
        {
            if (cnt >= 3) break;
            IngredientData tmp = Lrarity[getRndRarityType(1)].getRndIngredient();
            if (result.Contains(tmp)) continue; //중복방지
            cnt++;
            result.Add(tmp);
        }

        return result;
    }

   
   private int getRndRarityType(int _invenLv) //매개변수: 인벤토리 lv, 인벤토리 lv에 따라서 어떤 희귀도의 재료가 나올지 return
    {
        int randomValue= Random.Range(0,100);
        int rarity = 0;
        switch(_invenLv)
        {
            case 1: //100%의 확률로 rarity = 1;
                rarity = 1;
                break;
            case 2:
                if (randomValue < 80) rarity = 1;
                else rarity = 2;
                break;
            case 3:
                if (randomValue < 60) rarity = 1;
                else if (60 <= randomValue && randomValue < 90) rarity = 2;
                else rarity = 3;
                break;
            default:
                break;
        }

        return rarity;
    }
}
