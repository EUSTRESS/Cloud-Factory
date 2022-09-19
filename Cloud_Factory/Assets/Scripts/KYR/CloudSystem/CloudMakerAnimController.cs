using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudMakerAnimController : MonoBehaviour
{
    public GameObject[] factoryObject;
    // Start is called before the first frame update
    private int mResultColorIdx;
    private GameObject mFinalCloud;
    void Start()
    {

        InventoryManager inventoryManager = GameObject.FindWithTag("InventoryManager").transform.GetComponent<InventoryManager>();
        if (inventoryManager.createdCloudData.mEmotions.Count == 0) return;
        mResultColorIdx = inventoryManager.createdCloudData.getBaseCloudColorIdx();
        Debug.Log(mResultColorIdx);//Assets/Resources/Sprite/CloudOnMachine

        factoryObject[0] = transform.GetChild(1).gameObject;// I_Enter
        factoryObject[1] = transform.GetChild(2).gameObject;//I_Exit
        factoryObject[2] = transform.GetChild(3).gameObject;//I_Colored
        mFinalCloud = transform.GetChild(4).gameObject;
        mFinalCloud.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/CloudOnMachine/" + "OC_Maker_" + mResultColorIdx);

        
        StartCoroutine(makingAnimHandler());
    }

    IEnumerator makingAnimHandler()
    {
        factoryObject[0].SetActive(true);
        yield return new WaitForSeconds(2.0f);
        factoryObject[0].SetActive(false);
        factoryObject[1].SetActive(true);
        yield return new WaitForSeconds(4.5f);
        factoryObject[1].SetActive(false);
        
        factoryObject[2].SetActive(true);
        factoryObject[2].GetComponent<Animator>().SetInteger("CloudBaseIndex", mResultColorIdx);
        yield return new WaitForSeconds(1.35f);
        factoryObject[2].SetActive(false);
        mFinalCloud.SetActive(true);
        yield break;
    }
}
