using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class UICollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
#if UNITY_EDITOR
        Debug.Log(collision.gameObject.name);
#endif
        if (collision.gameObject.tag != "Guest")
            return;

        GameObject temp = collision.gameObject.transform.root.Find("Body").gameObject;
        if(temp !=null)
        {
#if UNITY_EDITOR
            Debug.Log("null");
#endif
        }
        if (temp.tag == "Guest")
        {
            if(temp.GetComponent<SortingGroup>().sortingLayerName == "Guest")
            {
#if UNITY_EDITOR
                Debug.Log("Guest In");
#endif
                temp.GetComponent<SortingGroup>().sortingLayerName = "Default";
                collision.gameObject.GetComponent<GuestCollider>().isTriggerWithUI = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Guest")
            return;

        GameObject temp = collision.gameObject.transform.root.Find("Body").gameObject;
        if (temp.tag.Equals("Guest"))
        {
            if (temp.GetComponent<SortingGroup>().sortingLayerName == "Default")
            {
                temp.GetComponent<SortingGroup>().sortingLayerName = "Guest";
                collision.gameObject.GetComponent<GuestCollider>().isTriggerWithUI = false;
            }
        }
    }
}
