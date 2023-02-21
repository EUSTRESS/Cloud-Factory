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
        if (part_prefab.transform.position.y < -3.0f)   // �� �߾ӱ��� y��ǥ�� -1.5������ �������� ������Ʈ�ı�
        {
            Destroy(part_prefab);
        }
        else if (part_prefab_2.transform.position.y < -3.0f)
        {
            Destroy(part_prefab_2);
        }
        else if (part_prefab.transform.position.x < limitMin.x || part_prefab.transform.position.x > limitMax.x || part_prefab.transform.position.y < limitMin.y || part_prefab.transform.position.y > limitMax.y)
        {
            Destroy(part_prefab, 0.05f);
        }
        else if (part_prefab_2.transform.position.x < limitMin.x || part_prefab_2.transform.position.x > limitMax.x || part_prefab_2.transform.position.y < limitMin.y || part_prefab_2.transform.position.y > limitMax.y)
        {
            Destroy(part_prefab_2, 0.05f);
        }
    }
}
