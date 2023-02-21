using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudAnim : MonoBehaviour
{
    GameObject parent;

    public void DestroyCloud()
    {
        parent = this.transform.parent.transform.gameObject;
        Destroy(parent);
    }
}
