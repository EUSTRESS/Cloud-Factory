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



}
