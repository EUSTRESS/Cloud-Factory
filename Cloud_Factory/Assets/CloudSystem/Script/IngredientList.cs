using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "IngredientDataList", menuName = "ScriptableObjects/IngredientDataList", order = 2)]
public class IngredientList : ScriptableObject
{
    public List<IngredientData> itemList;

    public void init()
    {
        for (int i = 0; i < itemList.Count; i++)
            itemList[i].init();
    }
    public IngredientData getRndIngredient()
    {
        int randomValue = Random.Range(0, itemList.Count);
        return itemList[randomValue];
    }
}
