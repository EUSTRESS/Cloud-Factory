using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cloud_movement : MonoBehaviour
{
    public int num = 0;
    //public float limintMin = -5.0f;
    //private bool Act = false;
    public GameObject ReceiveCloud;
    public GameObject Cloud;
    public GameObject part;
    public GameObject Parts_fly;

    //public Sprite CloudImage;

    //void Awake()
    //{
    //    Cloud.GetComponent<Image>().sprite = ReceiveCloud.GetComponent<VirtualGameObject>().mImage;
    //}

    

    void Start()
    {

        

        num = Random.Range(1, 5);
        GameObject go = Instantiate(Parts_fly);

        switch (num)
        {
            case 1:
                part.transform.localPosition = new Vector2(-0.5f, 0.23f);
                go.transform.position = part.transform.position;
                Parts_fly.GetComponent<Parts_fly>().Change_speed_1();
                InvokeRepeating("Make_copy", 2f, 2f);
                break;

            case 2:
                part.transform.localPosition = new Vector2(0.57f, 0.31f);
                go.transform.position = part.transform.position;
                Parts_fly.GetComponent<Parts_fly>().Change_speed_2();
                InvokeRepeating("Make_copy", 2f, 2f);
                break;

            case 3:
                part.transform.localPosition = new Vector2(-0.29f, -0.29f);
                go.transform.position = part.transform.position;
                Parts_fly.GetComponent<Parts_fly>().Change_speed_3();
                InvokeRepeating("Make_copy", 2f, 2f);
                break;

            case 4:
                part.transform.localPosition = new Vector2(0.34f, -0.1f);
                go.transform.position = part.transform.position;
                Parts_fly.GetComponent<Parts_fly>().Change_speed_4();
                InvokeRepeating("Make_copy", 2f, 2f);
                break;

        }



    }

    void Make_copy()
    {
        GameObject go = Instantiate(Parts_fly);
        go.transform.position = part.transform.position;
    }

    

    void Update()
    {
        //CloudImage = ReceiveCloud.GetComponent<VirtualGameObject>().mImage;

        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject go = Instantiate(Parts_fly);
            go.transform.position = part.transform.position;
        }
    }



}