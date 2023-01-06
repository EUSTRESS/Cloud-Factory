using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GuestCollider : MonoBehaviour
{
    public bool isTriggerWithUI = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject temp = collision.gameObject.transform.root.gameObject;
        if (temp.tag == "Guest" && temp.GetComponent<SortingGroup>().sortingLayerName == "Guest")
        {
            if (this.gameObject.transform.position.y < collision.gameObject.transform.position.y)
            {
                temp.GetComponent<SortingGroup>().sortingLayerName = "Default";
            }
            else
            {
                temp.GetComponent<SortingGroup>().sortingLayerName = "Guest";
                //Debug.Log("this : " + this.gameObject.transform.position.y +"collision " + temp.transform.position.y);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject temp = collision.gameObject.transform.root.gameObject;
        if (temp.tag == "Guest")
        {
            if (temp.GetComponent<SortingGroup>().sortingLayerName == "Default" && !isTriggerWithUI)
            {
                temp.GetComponent<SortingGroup>().sortingLayerName = "Guest";
            }
        }
    }
}
