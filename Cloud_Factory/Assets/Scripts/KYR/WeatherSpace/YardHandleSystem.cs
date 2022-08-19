using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class YardHandleSystem : MonoBehaviour
{
    public InventoryManager inventoryManager;

 //   public IngredientList[] mRarityList;
    public Sprite[] mImages;

    private Dictionary<GameObject, int> mYards;

    struct Yard //���� ����ü ����
    {
        private GameObject self;

        private int gatherCnt;//unsigned int �� ��ĥ �� ������? ��ģ�ٸ� �ؿ� ���ǹ��� ������ �ٲ���
        private Sprite[] sprites;

        public void init(GameObject gameObj,Sprite[] _sprites) //�ʱ�ȭ �Լ�
        {
            self = gameObj;
            sprites = new Sprite[2];

            if (_sprites.Length != 2) return; //Overflow ����
            else
                Debug.Log("[Yard Init] Not Right Sprite array input");
            sprites[0] = _sprites[0];
            sprites[1] = _sprites[1];
        }

        public bool canGather() //ä�� ���� ���� 
        {
            if (gatherCnt <= 0) return false; //ä�� ���� Ƚ���� 0 ���� �̸� ���� Ƚ�� ��� ����.
            else return true;
        }

        public void gather() //ä�� �ϴ� ����
        {
            if (gatherCnt == 0) return; //�̹� ���� Ƚ���� ��� ���� �Ǹ� ä���� ������� �ʴ´�.

            gatherCnt--;
            updateSprite();

        }

        private void rndGatSys(GameObject GInventory) //random gather System
        {
            int invenLev = 1; //���߿� GetComponent�� ������ ����

        }

        private void activeBoard() //ä�� �˾� â Ȱ��ȭ
        {

        }

        private void updateSprite()
        {
            //gatherCnt�� ���� yardSprite �ٲ�
            if (gatherCnt == 0) self.GetComponent<SpriteRenderer>().sprite = sprites[0];
            else if (gatherCnt >= 0 && self.GetComponent<SpriteRenderer>().sprite != sprites[1])
                self.GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
    };

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();

        mYards = new Dictionary<GameObject, int>();

        for (int i = 0; i < transform.childCount; i++)      
            mYards.Add(transform.GetChild(i).gameObject,2); //Yard �׷쿡 ���� yard���� ����Ʈ�� �־ ����.
            //�� ��ųʸ��� int�� ä�� Ƚ�� 0�� �Ǹ� ä�� �Ұ����ϴ�!
    }

    

    public void  Gathered(GameObject iClickedYard,int totalCnt) //Ŭ���Լ�
    {
        if (mYards[iClickedYard] == 0) return;

        Dictionary<IngredientData, int> results = getRndGatheredResult(totalCnt);
        Debug.Log("[System]��" + results.Count + "�� ä���Ǿ����ϴ�!");
        foreach(KeyValuePair<IngredientData, int> result in results)
        {
            if(inventoryManager.addStock(result)) Debug.Log("[System]ä�� ����| ����:" + result.Key + "|����: " + result.Value);
            else
                Debug.Log("[System]ä�� ����| �κ��丮�� �� á�ų� ���� ���� ��� ������ �ʰ��Ͽ����ϴ�| ����:."+ result.Key + "|����: " + result.Value);

        }

        mYards[iClickedYard]--;
       // if (mYards[iClickedYard] == 0) iClickedYard.GetComponent<Image>().sprite = mImages[0];
    }

    private Dictionary<IngredientData, int> getRndGatheredResult(int totalCnt) //�������� ä���� ����Ʈ ����.
    {
        //Key: ��� ����  Value: ȹ���� ��� ����
        Dictionary<IngredientData, int> results= new Dictionary<IngredientData, int>();

        //��͵� ����, ���߿����� ���� ����.
        //Random: ��͵�, ��͵� �� ����, ��� ��, ä���� ��� ���� ��
        Debug.Log("�� ä���� ����:" + totalCnt);
        while (results.Count <= totalCnt)
        {
            IngredientData tmp = inventoryManager.mIngredientDatas[getRndRarityType(inventoryManager.minvenLevel) -1].getRndIngredient();
            Debug.Log(tmp);
            if (results.ContainsKey(tmp)) continue; //�ߺ�����
            results.Add(tmp, Random.Range(1, 6));
            Debug.Log("�߰�: " + tmp);
        }

        return results;
    }

   
   private int getRndRarityType(int _invenLv) //�Ű�����: �κ��丮 lv, �κ��丮 lv�� ���� � ��͵��� ��ᰡ ������ return
    {
        int randomValue= Random.Range(0,100);
       
        int rarity = 1;
        switch(_invenLv)
        {
            case 1: //100%�� Ȯ���� rarity = 1;
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
        Debug.Log("rarity" + rarity);
        return rarity;
    }
}