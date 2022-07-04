using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechText : MonoBehaviour
{
    public GameObject gSpeechBubble;
    public GameObject gOkNoGroup;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("delay", 3.0f);
    }

    void delay()
    {
        gSpeechBubble.SetActive(false);
        gOkNoGroup.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
