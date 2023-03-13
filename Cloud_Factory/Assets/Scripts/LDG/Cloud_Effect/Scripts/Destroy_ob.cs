using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_ob : MonoBehaviour
{

    public GameObject part_prefab;
    public GameObject part_prefab_2;
    public Vector2 limitMax = new Vector2(8.9f, 5.0f);
    public Vector2 limitMin = new Vector2(-8.9f, -5.0f);
    void Start()
    {
        
    }

    void Update()
    {
        if (part_prefab.transform.position.y < -2.0f)   // ¸Ç Áß¾Ó±âÁØ yÁÂÇ¥°¡ -1.5¹ØÀ¸·Î ³»·Á°¡¸é ¿ÀºêÁ§Æ®ÆÄ±«
        {
            Destroy(part_prefab);
        }
        else if (part_prefab_2.transform.position.y < -2.0f)
        {
            Destroy(part_prefab_2);
        }
        if (part_prefab.transform.position.x < limitMin.x || part_prefab.transform.position.x > limitMax.x || part_prefab.transform.position.y < limitMin.y || part_prefab.transform.position.y > limitMax.y)
        {
            Destroy(part_prefab);
        }
        else if (part_prefab_2.transform.position.x < limitMin.x || part_prefab_2.transform.position.x > limitMax.x || part_prefab_2.transform.position.y < limitMin.y || part_prefab_2.transform.position.y > limitMax.y)
        {
            Destroy(part_prefab_2);
        }
    }
}
