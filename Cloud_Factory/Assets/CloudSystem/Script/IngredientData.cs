using System.Collections.Generic;
using UnityEngine;

public enum Emotion
{   //PLEASURE부터 0~ 의 값을 갖음
    PLEASURE, //기쁨
    UNREST, //불안
    SADNESS, //슬픔
    IRRITATION, //짜증
    ACCEPT,//수용
    SUPCON, //SUPRISE+CONFUSION 논란,혼란
    DISGUST, //혐오
    INTEXPEC, //INTERSTING+EXPECTATION 관심,기대
    LOVE,
    OBED, //순종.
    AWE,
    CONTRAY,//반대
    BLAME,
    DESPISE,
    AGGRESS,//AGGRESSION 공격성
    OPTIMISM,//낙r관, 낙천
    BITTER,
    LOVHAT, //LOVE AND HATRED
    FREEZE,
    CHAOTIC,//혼란스러움
    NONE
}

[System.Serializable]
public struct EmotionInfo
{
    [SerializeField]
    public Emotion Key;
    [SerializeField]
    public int Value;

    public void init(Emotion _Key, int _Value)
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
    

    

    public string ingredientName; //재료 이름

    public Sprite image;// 이미지

    //희귀도 : 희귀도에 따라서 감정의 구성 종류 및 개수가 달라진다.
    [SerializeField]
    private int rarity;

    [SerializeField]
    private EmotionInfo[] emotions;

    public Dictionary<int, int> iEmotion;

   


    public void init()
    {
        iEmotion = new Dictionary<int, int>();
        foreach (EmotionInfo emotion in emotions)
        {
            iEmotion.Add(emotion.getKey2Int(), emotion.getValue());
        }
    }
}