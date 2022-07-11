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
    ROMANCE, //순정만화가 ROMANCE COMICS여서 PURE LOVE보단 나을 것 같아서 이렇게 함.
    AWE,
    CONTRAY,//반대
    BLAME,
    DESPISE,
    AGGRESS,//AGGRESSION 공격성
    OPTIMISM,//낙천
    BITTER,
    LOVHAT, //LOVE AND HATRED
    FREEZE,
    CHAOTIC//혼란스러움
}

[CreateAssetMenu(fileName = "IngredientData", menuName = "ScriptableObjects/IngredientData", order = 1)]
public class IngredientData : ScriptableObject
{
    

    [System.Serializable]
    public struct emotioninfo
    {
        [SerializeField]
        private Emotion Key;
        [SerializeField]
        private int Value;

        void init(Emotion _Key, int _Value)
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

    public string ingredientName; //재료 이름

    public Sprite image;// 이미지

    //희귀도 : 희귀도에 따라서 감정의 구성 종류 및 개수가 달라진다.
    [SerializeField]
    private int rarity;

    [SerializeField]
    private emotioninfo[] emotions;

    public Dictionary<int, int> iEmotion;

   


    public void init()
    {
        iEmotion = new Dictionary<int, int>();
        foreach (emotioninfo emotion in emotions)
        {
            iEmotion.Add(emotion.getKey2Int(), emotion.getValue());
        }
    }
}