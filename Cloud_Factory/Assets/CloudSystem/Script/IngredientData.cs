using UnityEngine;

[CreateAssetMenu(fileName = "IngredientData", menuName = "ScriptableObjects/IngredientData", order = 1)]
public class IngredientData : ScriptableObject
{
    public string ingredientName; //재료 이름
    public Sprite image;// 이미지
}