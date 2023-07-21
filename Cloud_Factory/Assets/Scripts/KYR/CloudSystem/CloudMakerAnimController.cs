using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudMakerAnimController : MonoBehaviour
{
    public GameObject[] factoryObject;
    // Start is called before the first frame update
    private int mResultColorIdx;

    private bool[] isAnimProgressed = new bool[3];

	private GameObject mFinalCloud;

    InventoryManager inventoryManager;

	void Start()
    {

        inventoryManager = GameObject.FindWithTag("InventoryManager").transform.GetComponent<InventoryManager>();
        if (inventoryManager.createdCloudData.mEmotions.Count == 0)
        {
            this.gameObject.GetComponent<Button>().enabled = true;
            return;
        }
        //애니메이션이 얼마나 진행되었는지 InventoryManager.cs의 CloudData에서 받아옴
        for(int idx = 0; idx < 3; idx++) { isAnimProgressed[idx] = inventoryManager.createdCloudData.getAnimProgressed(idx); }
        this.gameObject.GetComponent<Button>().enabled = false;
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
        if (!isAnimProgressed[0]) {
            factoryObject[0].SetActive(true);
            yield return new WaitForSeconds(2.0f);
            factoryObject[0].SetActive(false);
            inventoryManager.createdCloudData.setAnimProgressed(0, true);
        }
        if (!isAnimProgressed[1]) {
            factoryObject[1].SetActive(true);
            yield return new WaitForSeconds(5.5f);
            factoryObject[1].SetActive(false);

            inventoryManager.createdCloudData.setAnimProgressed(1, true);
		}
        if (!isAnimProgressed[2]) {
            factoryObject[2].SetActive(true);

            factoryObject[2].GetComponent<Animator>().SetInteger("CloudBaseIndex", mResultColorIdx);
            yield return new WaitForSeconds(1.35f);
            factoryObject[2].SetActive(false);

            inventoryManager.createdCloudData.setAnimProgressed(2, true);
		}

        mFinalCloud.SetActive(true);
        yield break;
    }
}
