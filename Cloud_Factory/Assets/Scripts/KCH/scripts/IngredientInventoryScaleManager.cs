using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientInventoryScaleManager : MonoBehaviour
{
    public Sprite[]     inventorySprite = new Sprite[2];  // 인벤토리 이미지 (2, 3단계)
    public GameObject   inventoryCover;                   // 인벤토리 가림막
    public GameObject   inventoryLayOut;                  // 인벤토리 레이아웃
    private GameObject  inventoryContent;                  // 인벤토리 content

    InventoryManager mInventoryManager;
    // Start is called before the first frame update
    void Awake()
    {
        mInventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();       // invLevel을 받아오기 위한 참조
        inventoryContent = inventoryLayOut.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        updateInventory();
    }

    void updateInventory()
    {
        int invLevel = mInventoryManager.minvenLevel;

        if(invLevel <= 2)
        {
            inventoryLayOut.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1200f);
			inventoryContent.GetComponent<Image>().sprite = inventorySprite[0];
			inventoryContent.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = inventorySprite[0];
			inventoryContent.transform.GetChild(0).gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 870f);
		}
        else
        {
			inventoryLayOut.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1275f);
			inventoryContent.GetComponent<Image>().sprite = inventorySprite[1];
			inventoryContent.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = inventorySprite[1];
            inventoryContent.transform.GetChild(0).gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1740f);
		}

        if(invLevel >= 2) { inventoryCover.SetActive(false); }
        else { inventoryCover.SetActive(true); }
    }


}
