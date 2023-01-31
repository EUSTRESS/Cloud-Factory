using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    GameObject Text;
    private void Awake()
    {
        Text = this.gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject;
    }

    void EndBubble()
    {
        this.gameObject.SetActive(false);
    }

    void ActiveFalseText()
    {
        Text.SetActive(false);
    }

    void ActiveText()
    {
        Text.SetActive(true);      
    }
}
