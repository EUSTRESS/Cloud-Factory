using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "IngredientDataList", menuName = "ScriptableObjects/IngredientDataList", order = 2)]
public class IngredientList : ScriptableObject
{
    public List<IngredientData> itemList;
}
