using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_ob : MonoBehaviour
{

    public GameObject part_prefab;
    public GameObject part_prefab_2;
    public Vector2 limitMax = new Vector2(8.9f, 5.0f);
    public Vector2 limitMin = new Vector2(-8.9f, -5.0f);
    //public Vector2 Cloud_position = new Vector3(0f, 0f, 0f);

    //private int pos_Num;
    //private int pos_Num_2;

    void Start()
    {
        //Cloud_movement cloud_Movement = GameObject.Find("MoveCloud").GetComponent<Cloud_movement>();
        //pos_Num = cloud_Movement.ran_num;
        //pos_Num_2 = cloud_Movement.ran_num2;
    }

    void Update()
    {
        //Cloud_position = FindObjectOfType<CloudSpawner>().Cloud_ps;


        if (part_prefab.transform.position.x < limitMin.x || part_prefab.transform.position.x > limitMax.x || part_prefab.transform.position.y < limitMin.y || part_prefab.transform.position.y > limitMax.y)
        {
            Destroy(gameObject);
        }
        else if (part_prefab_2.transform.position.x < limitMin.x || part_prefab_2.transform.position.x > limitMax.x || part_prefab_2.transform.position.y < limitMin.y || part_prefab_2.transform.position.y > limitMax.y)
        {
            Destroy(gameObject);
        }
    }
}
