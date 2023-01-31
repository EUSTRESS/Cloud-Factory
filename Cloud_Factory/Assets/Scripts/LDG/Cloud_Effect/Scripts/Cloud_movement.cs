using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_movement : MonoBehaviour
{
    public int num = 0;
    //public float limintMin = -5.0f;
    //private bool Act = false;
    public GameObject cloud;
    public GameObject parts;
    public GameObject Parts_fly;

    void Start()
    {
        num = Random.Range(1, 5);
        GameObject go = Instantiate(parts); 
        switch (num)
        {
            case 1:
                cloud.transform.localPosition = new Vector2(-0.5f, 0.23f);
                go.transform.position = cloud.transform.position;
                Parts_fly.GetComponent<Parts_fly>().change_speed_1_2();
                break;

            case 2:
                cloud.transform.localPosition = new Vector2(0.57f, 0.31f);
                go.transform.position = cloud.transform.position;
                Parts_fly.GetComponent<Parts_fly>().change_speed_1_2();
                break;

            case 3:
                cloud.transform.localPosition = new Vector2(-0.29f, -0.29f);
                go.transform.position = cloud.transform.position;
                Parts_fly.GetComponent<Parts_fly>().change_speed_3();
                break;

            case 4:
                cloud.transform.localPosition = new Vector2(0.34f, -0.1f);
                go.transform.position = cloud.transform.position;
                Parts_fly.GetComponent<Parts_fly>().change_speed_4();
                break;

        }



    }
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject go = Instantiate(parts);
            go.transform.position = cloud.transform.position;
            //Act = true;
        }
    }
}
