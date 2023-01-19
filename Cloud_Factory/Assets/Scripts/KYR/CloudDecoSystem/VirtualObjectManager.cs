using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VirtualGameObject
{
    //Transform
    public Vector3 mPosition;
    public Quaternion mRotation;
    public Vector3 mScale;

    public Sprite mImage;

    public VirtualGameObject(Vector3 position, Quaternion rotation, Vector3 scale, Sprite image)
    {
        mPosition = position;
        mRotation = rotation;
        mScale = scale;
        mImage = image;
    }
}

//구름 하나를 구성하는 오브젝트
public class VirtualObjectManager : MonoBehaviour
{
    public GameObject OBPrefab; //게임오브젝트 안에 버튼과 이미지 컴포넌트가 있는 Prefab
    public GameObject convertVirtualToGameObject(VirtualGameObject VObject)
    {
        GameObject result;

        result = Instantiate(OBPrefab, VObject.mPosition, VObject.mRotation);

        result.GetComponent<Image>().sprite = VObject.mImage;

        

        return result;
    }
}
