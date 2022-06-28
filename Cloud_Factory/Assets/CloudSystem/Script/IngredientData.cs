using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientData", menuName = "ScriptableObjects/IngredientData", order = 1)]
public class IngredientData : ScriptableObject
{
    public enum Emotion
    {   //PLEASURE부터 0~ 의 값을 갖음
        PLEASURE,
        UNREST,
        SADNESS,
        IRRITATION,
        ACCEPT,
        SUPCON, //SUPRISE+CONFUSION
        DISGUST,
        INTEXPEC, //INTERSTING+EXPECTATION
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
    private Dictionary<int, int> iEmotion;

    public emotioninfo[] emotions;


    public void init()
    {
        iEmotion = new Dictionary<int, int>();
        foreach (emotioninfo emotion in emotions)
        {
            iEmotion.Add(emotion.getKey2Int(), emotion.getValue());
        }
    }
}