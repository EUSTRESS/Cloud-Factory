using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public enum Emotion
{   //PLEASURE부터 0~ 의 값을 갖음
    PLEASURE, //기쁨 0
    UNREST, //불안 1 
    SADNESS, //슬픔 2
    IRRITATION, //짜증 3
    ACCEPT,//수용 4
    SUPCON, //SUPRISE+CONFUSION 논란,혼란 5
    DISGUST, //혐오 6
    INTEXPEC, //INTERSTING+EXPECTATION 관심,기대 7
    LOVE, //8
    OBED, //순종. 9
    AWE,//10
    CONTRAY,//반대 11
    BLAME,//12
    DESPISE,//13
    AGGRESS,//AGGRESSION 공격성 14
    OPTIMISM,//낙r관, 낙천 15
    BITTER,//16
    LOVHAT, //LOVE AND HATRED 17
    FREEZE,//18
    CHAOTIC,//혼란스러움 19
    NONE //20
}

[System.Serializable]
public class EmotionInfo
{
    [SerializeField]
    public Emotion Key;
    [SerializeField]
    public int Value;

    public EmotionInfo(Emotion _Key, int _Value)
    {
        Key = _Key;
        Value = _Value;
    }

    public int getKey2Int()
    {
        return (int)Key; //Emotion Enum 의 고유값(index값)으로 변환해서 제공
    }

    public int getValue()
    {
        return Value;
    }
}

[CreateAssetMenu(fileName = "IngredientData", menuName = "ScriptableObjects/IngredientData", order = 1)]
public class IngredientData : ScriptableObject
{


    // LJH, 변수마다 직렬화
    [SerializeField]
    public string dataName; //재료 이름

    // LJH, 스프라이트는 직렬화 불가능이어서 JsonIgnore안하면 오류
    [JsonIgnore]
    public Sprite image;// 이미지

    //희귀도 : 희귀도에 따라서 감정의 구성 종류 및 개수가 달라진다.
    [SerializeField]
    public int rarity;

    [SerializeField]
    public EmotionInfo[] emotions;

    [SerializeField]
    public Dictionary<int, int> iEmotion;

    public void init()
    {
        iEmotion = new Dictionary<int, int>();
        foreach (EmotionInfo emotion in emotions)
        {
            iEmotion.Add(emotion.getKey2Int(), emotion.getValue());
        }
    }

    public void rematchSpriteData(IngredientList[] dataList)
    {
        IngredientData dataBase = dataList[rarity - 1].mItemList.Find(item => dataName == item.dataName);
        image = dataBase.image;
    }
}