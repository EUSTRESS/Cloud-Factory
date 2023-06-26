using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDragMove : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private bool wasInvalidState = false;
    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        DecoParts decoparts = transform.parent.GetComponent<DecoParts>();
        decoparts.UpdateEditGuidLineUI(true);
        bool trigger = wasInvalidState ? decoparts.canAttached : transform.parent.GetComponent<DecoParts>().CheckIsInValidPlace();
        if (!trigger)
        {
            decoparts.UpdateEditGuidLineUI(false);
            decoparts.canAttached = true;
            wasInvalidState = true;
            return;
        }
        transform.parent.position = eventData.position;
        wasInvalidState = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.parent.GetComponent<DecoParts>().UpdateEditGuidLineUI(false);
    }


}
