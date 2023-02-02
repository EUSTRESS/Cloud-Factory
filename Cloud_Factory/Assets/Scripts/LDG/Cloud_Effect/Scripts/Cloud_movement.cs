using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_movement : MonoBehaviour
{
    public int num = 0;
    public GameObject Parts;
    public GameObject Parts_fly;

    

    void Start()
    {
        num = 1;// Random.Range(1, 5);
        GameObject go = Instantiate(Parts_fly);
        switch (num)
        {
            case 1:
                Parts.transform.localPosition = new Vector2(-0.5f, 0.23f);
                go.transform.position = Parts.transform.position;
                Parts_fly.GetComponent<Parts_fly>().change_speed_1();
                break;

            case 2:
                Parts.transform.localPosition = new Vector2(0.57f, 0.31f);
                go.transform.position = Parts.transform.position;
                Parts_fly.GetComponent<Parts_fly>().change_speed_2();
                break;

            case 3:
                Parts.transform.localPosition = new Vector2(-0.29f, -0.29f);
                go.transform.position = Parts.transform.position;
                Parts_fly.GetComponent<Parts_fly>().change_speed_3();
                break;

            case 4:
                Parts.transform.localPosition = new Vector2(0.34f, -0.1f);
                go.transform.position = Parts.transform.position;
                Parts_fly.GetComponent<Parts_fly>().change_speed_4();
                break;

        }



    }
    void Update()
    {



        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject go = Instantiate(Parts_fly);
            go.transform.position = Parts.transform.position;
        }
    }
}
