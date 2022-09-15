using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmotionsTable", menuName = "ScriptableObjects/EmotionsTable", order = 3)]
public class EmotionsTable : ScriptableObject
{
    [System.Serializable]
    public struct EmotionTableData
    {
        public List<Emotion> EmotionDataC;
    }

    public List<EmotionTableData> EmotionDataR; //EmotionData Colums


    public Emotion getCombineResult(Emotion emo1, Emotion emo2)
    {
        //if (emo1 >= Emotion.IRRITATION || emo2 >= Emotion.IRRITATION)
        //    return Emotion.NONE;
        Debug.Log("Emo1 idx" + emo1+ "      Emo2 idx" + emo2);
        return EmotionDataR[(int)emo1].EmotionDataC[(int)emo2];
    }

}