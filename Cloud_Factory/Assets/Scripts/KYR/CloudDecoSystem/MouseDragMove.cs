using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDragMove : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {   
        transform.parent.position = eventData.position;
        transform.parent.GetChild(0).gameObject.SetActive(true);
        transform.parent.GetChild(1).gameObject.SetActive(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.parent.GetChild(0).gameObject.SetActive(false);
        transform.parent.GetChild(1).gameObject.SetActive(false);
    }


}
