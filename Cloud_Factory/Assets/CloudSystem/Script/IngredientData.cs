using System.Collections.Generic;
using UnityEngine;

public enum Emotion
{   //PLEASURE���� 0~ �� ���� ����
    PLEASURE, //���
    UNREST, //�Ҿ�
    SADNESS, //����
    IRRITATION, //¥��
    ACCEPT,//����
    SUPCON, //SUPRISE+CONFUSION ���,ȥ��
    DISGUST, //����
    INTEXPEC, //INTERSTING+EXPECTATION ����,���
    LOVE,
    OBED, //����.
    AWE,
    CONTRAY,//�ݴ�
    BLAME,
    DESPISE,
    AGGRESS,//AGGRESSION ���ݼ�
    OPTIMISM,//��r��, ��õ
    BITTER,
    LOVHAT, //LOVE AND HATRED
    FREEZE,
    CHAOTIC,//ȥ��������
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
        return (int)Key; //Emotion Enum �� ������(index��)���� ��ȯ�ؼ� ����
    }

    public int getValue()
    {
        return Value;
    }
}

[CreateAssetMenu(fileName = "IngredientData", menuName = "ScriptableObjects/IngredientData", order = 1)]
public class IngredientData : ScriptableObject
{
    

    

    public string ingredientName; //��� �̸�

    public Sprite image;// �̹���

    //��͵� : ��͵��� ���� ������ ���� ���� �� ������ �޶�����.
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