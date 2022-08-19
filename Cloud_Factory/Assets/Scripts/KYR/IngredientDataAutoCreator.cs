using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientDataAutoCreator : MonoBehaviour
{
    // An instance of the ScriptableObject defined above.
    private IngredientList LRarity1;
    private IngredientList LRarity2;
    private IngredientList LRarity3;

    private List<IngredientList> Ltotal;
    void Start()
    {
        Ltotal = new List<IngredientList>();
        init();
        CreateData();
        SendData();
    }
    void init()
    {
        LRarity1 = ScriptableObject.CreateInstance<IngredientList>(); ;
        LRarity1.mItemList = new List<IngredientData>();

        LRarity2 = ScriptableObject.CreateInstance<IngredientList>(); ;
        LRarity2.mItemList = new List<IngredientData>();

        LRarity3 = ScriptableObject.CreateInstance<IngredientList>(); ;
        LRarity3.mItemList = new List<IngredientData>();
    }

    void CreateData()
    {
        //��͵� 1 ��� ����
        for (int i = (int)Emotion.PLEASURE; i <= (int)Emotion.IRRITATION; i++)
        {
            for (int j = (int)Emotion.PLEASURE; j <= (int)Emotion.IRRITATION; j++)
            {
                if (i == j) continue; //101 202�� �Ұ����� �����̹Ƿ� ����ó��
                // Creates an instance of the prefab at the current spawn point.
                IngredientData data = ScriptableObject.CreateInstance<IngredientData>();
               
                // Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
                string tmpWord = i.ToString() + "0" + j.ToString();
                data.name = tmpWord;
                data.dataName = tmpWord;
                data.image = Resources.Load<Sprite>("Sprite/Ingredient/Rarity1/"+ "M1_" + data.name);
                data.rarity = 1;
                data.emotions = new EmotionInfo[2];
                data.emotions[0] = new EmotionInfo((Emotion)i, 30);
                data.emotions[1] = new EmotionInfo((Emotion)j, 15);

                data.init();

                LRarity1.mItemList.Add(data); //IngredientList Scriptable Object�� �߰�.
            }
        }
        //��͵� 2 ��� ����
        for (int i = (int)Emotion.ACCEPT; i <= (int)Emotion.INTEXPEC; i++)
        {
            IngredientData data = ScriptableObject.CreateInstance<IngredientData>();

            // Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
            string tmpWord = i.ToString() + "0" + i.ToString();
            data.name = tmpWord;
            data.dataName = tmpWord;
            data.image = Resources.Load<Sprite>("Sprite/Ingredient/Rarity2/" + "M2_" + data.name);
            data.rarity = 2;
            data.emotions = new EmotionInfo[1];
            data.emotions[0] = new EmotionInfo((Emotion)i, 40);
    
            data.init();

            LRarity2.mItemList.Add(data); //IngredientList Scriptable Object�� �߰�.
        }

        //��͵� 2 ��� ����
        for (int i = (int)Emotion.PLEASURE; i <= (int)Emotion.INTEXPEC; i++)
        {
            IngredientData data = ScriptableObject.CreateInstance<IngredientData>();

            // Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
            string tmpWord = i.ToString() + "1" + i.ToString();
            data.name =  tmpWord;
            data.dataName = tmpWord;
            data.image = Resources.Load<Sprite>("Sprite/Ingredient/Rarity3/" + "M3_" +data.name);
            data.rarity = 3;
            data.emotions = new EmotionInfo[1];
            data.emotions[0] = new EmotionInfo((Emotion)i, 60);

            data.init();

            LRarity3.mItemList.Add(data); //IngredientList Scriptable Object�� �߰�.
        }


        Ltotal.Add(LRarity1);
        Ltotal.Add(LRarity2);
        Ltotal.Add(LRarity3);
    }

    void SendData()
    {
        gameObject.GetComponent<InventoryManager>().setDataList(Ltotal);
    }
}
