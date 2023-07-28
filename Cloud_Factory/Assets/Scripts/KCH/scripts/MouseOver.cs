using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float duration;
    private bool isOver;

    public GameObject ingrBubble;
    
    // Start is called before the first frame update
    void Start()
    {
        ingrBubble.SetActive(false);
        duration = 0f;
        isOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOver) duration += Time.deltaTime;
        if (duration >= 1.0f)
        {
            ingrBubble.SetActive(true);
            isOver = false;
            duration = 0f;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isOver = true;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        ingrBubble.SetActive(false);
        duration = 0f;
        isOver = false;
    }

}
