using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YardHandleSystem : MonoBehaviour
{
    public IngredientData[] ingredients;
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
    void Start()
    {
        for (int i = 0; i < ingredients.Length; i++)
        {
            ingredients[i].init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
