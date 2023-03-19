using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateFadeOutScreen : MonoBehaviour
{
    public GameObject iFadeOutGif;

    public void InstantiateGif()
    {
        GameObject temp_screen = Instantiate(iFadeOutGif);
        temp_screen.transform.SetParent(GameObject.Find("Canvas").transform);
        temp_screen.transform.localPosition = Vector3.zero;

        Destroy(temp_screen, 0.84f);
    }
}
