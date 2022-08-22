using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDragRotate : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float rotSpeed = 0.01f;
    public Vector2 initialMousePos;
    public Vector2 currentMousePos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialMousePos = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentMousePos = Input.mousePosition;
        //처음 드래그 시작할때 마우스 위치 - 현재 위치
        transform.parent.Rotate(0, 0, (initialMousePos.x - currentMousePos.x) * rotSpeed);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

 
}
