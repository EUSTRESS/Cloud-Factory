using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class UICollider : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject temp = collision.gameObject.transform.root.gameObject;
        if(temp.tag == "Guest")
        {
            if(temp.GetComponent<SortingGroup>().sortingLayerName == "Guest")
            {
                temp.GetComponent<SortingGroup>().sortingLayerName = "Default";
                collision.gameObject.GetComponent<GuestCollider>().isTriggerWithUI = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject temp = collision.gameObject.transform.root.gameObject;
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
