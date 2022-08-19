using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "IngredientDataList", menuName = "ScriptableObjects/IngredientDataList", order = 2)]
public class IngredientList : ScriptableObject
{
    public List<IngredientData> mItemList;
   
    public void init()
    {
        for (int i = 0; i < mItemList.Count; i++)
            mItemList[i].init();
    }

    public IngredientData getRndIngredient()
    {
        int randomValue = Random.Range(0, mItemList.Count);
        return mItemList[randomValue];
    }
}
