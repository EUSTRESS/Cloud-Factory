using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_ob : MonoBehaviour
{

    public GameObject part_prefab;
    //public float prefab_x = 
    public Vector2 limitMax = new Vector2(8.9f, 5.0f);
    public Vector2 limitMin = new Vector2(-8.9f, -5.0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (part_prefab.transform.position.y < -1.5f)
        {
            Destroy(part_prefab);
        }
        //else if(part_prefab.transform.position.x <  - 3 || part_prefab.transform.position.x > 3 )
        //{
        //    Destroy(part_prefab);
        //}
        //Destroy(part_prefab, 0.5f);
        //if(part_prefab.transform.position.x < limitMin.x || part_prefab.transform.position.x > limitMax.x || part_prefab.transform.position.y < limitMin.y || part_prefab.transform.position.y > limitMax.y)
        //{
        //    Destroy(part_prefab, 0.05f);
        //}
    }
}