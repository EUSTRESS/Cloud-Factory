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
    public float mWidth;
    public float mHeight;

    public Sprite mImage;

    public VirtualGameObject(Vector3 position, Quaternion rotation, float Width, float Height, Sprite image)
    {
        mPosition = position;
        mRotation = rotation;
        mWidth = Width;
        mHeight = Height;
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
    public GameObject convertVirtualToGameObjectToSprite(VirtualGameObject VObject)
    {
        GameObject result;

        result = Instantiate(OBPrefab, VObject.mPosition, VObject.mRotation);

        result.GetComponent<SpriteRenderer>().sprite = VObject.mImage;

        return result;
    }

    //씬에 버츄얼 오브젝트를 Instantiate 하는 함수
    public GameObject InstantiateVirtualObjectToScene(StoragedCloudData stock,Vector3 InstancePosition)
    {
        //가상 데이터 들을 게임 오브젝트로 Convert 하여 Instantiate 하는 과정.
        GameObject obejct;
        obejct = convertVirtualToGameObject(stock.mVBase);

        RectTransform rectTran = obejct.GetComponent<RectTransform>();
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, stock.mVBase.mHeight);
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, stock.mVBase.mWidth);


        foreach (VirtualGameObject Vpart in stock.mVPartsList)
        {
            GameObject obejctP;
            obejctP = convertVirtualToGameObject(Vpart);

            obejctP.transform.SetParent(obejct.transform);

            obejctP.transform.localPosition = obejctP.transform.position;
            rectTran = obejctP.GetComponent<RectTransform>();
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Vpart.mHeight);
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Vpart.mWidth);


            
        }

        obejct.transform.localPosition = Vector3.zero;
        obejct.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        return obejct;
    }

    public GameObject InstantiateVirtualObjectToSceneToSprite(StoragedCloudData stock, Vector3 InstancePosition)
    {
        //가상 데이터 들을 게임 오브젝트로 Convert 하여 Instantiate 하는 과정.
        GameObject obejct;
        obejct = convertVirtualToGameObjectToSprite(stock.mVBase);

        RectTransform rectTran = obejct.GetComponent<RectTransform>();
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, stock.mVBase.mHeight);
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, stock.mVBase.mWidth);

        // 구름의 소팅 레이어를 변경
        obejct.GetComponent<SpriteRenderer>().sortingLayerName = "Cloud";

        foreach (VirtualGameObject Vpart in stock.mVPartsList)
        {
            GameObject obejctP;
            obejctP = convertVirtualToGameObjectToSprite(Vpart);

            obejctP.transform.SetParent(obejct.transform);

            obejctP.transform.localPosition = obejctP.transform.position;
            rectTran = obejctP.GetComponent<RectTransform>();
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Vpart.mHeight);
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Vpart.mWidth);

            // 파츠의 소팅 레이어를 변경
            obejctP.GetComponent<SpriteRenderer>().sortingLayerName = "Parts";

            float newX = rectTran.localPosition.x * 1.51f / rectTran.rect.width;
            float newY = rectTran.localPosition.y * 0.71f / rectTran.rect.height;

            rectTran.localPosition = new Vector3(newX, newY, 1.0f);
        }

        obejct.transform.localPosition = InstancePosition;
        obejct.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        return obejct;
    }
}
