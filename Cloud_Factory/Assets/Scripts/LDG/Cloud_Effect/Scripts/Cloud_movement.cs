using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Cloud_movement : MonoBehaviour
{
    public int ran_num = 0;     // 파츠튀어나오는 위치를 정해주는 첫번째 변수
    public int ran_num2 = 0;    // 파츠튀어나오는 위치를 정해주는 두번째 변수
    public GameObject Cloud;
    public GameObject part;
    public GameObject part_2;
    public GameObject Parts_fly;
    public GameObject Parts_fly_2;

    public int Cloud_Speed = 0;

    public int Target_guestnum = 0;


    public Vector3 Change_Cloud_scale = new Vector3(0f, 0f, 0f);  // 바꿀 구름의 scale

    public Vector3 Change_Part_scale = new Vector3(0f, 0f, 0f);     // 바꿀 파츠의 scale

    public Sprite Change_CloudImage;      // 바꿀 구름의 이미지

    public Sprite Change_PartImage;     // 바꿀 파츠의 이미지

    //public Sprite CloudImage;

    //void Awake()
    //{
    //    Cloud.GetComponent<Image>().sprite = ReceiveCloud.GetComponent<VirtualGameObject>().mImage;
    //}



    void Start()
    {



        ran_num = Random.Range(1, 5);
        ran_num2 = Get_ran_num();
        GameObject go = Instantiate(Parts_fly);
        GameObject go2 = Instantiate(Parts_fly_2);

        switch (ran_num)
        {
            case 1:
                part.transform.localPosition = new Vector2(-2.3f, 0.8f);
                go.transform.position = part.transform.position;
                Parts_fly.GetComponent<Parts_fly>().Change_speed_1();
                InvokeRepeating("Make_copy", 2f, 2f);
                break;

            case 2:
                part.transform.localPosition = new Vector2(2.6f, 1.5f);
                go.transform.position = part.transform.position;
                Parts_fly.GetComponent<Parts_fly>().Change_speed_2();
                InvokeRepeating("Make_copy", 2f, 2f);
                break;

            case 3:
                part.transform.localPosition = new Vector2(-1.4f, -1.5f);
                go.transform.position = part.transform.position;
                Parts_fly.GetComponent<Parts_fly>().Change_speed_3();
                InvokeRepeating("Make_copy", 2f, 2f);
                break;

            case 4:
                part.transform.localPosition = new Vector2(1.9f, -0.8f);
                go.transform.position = part.transform.position;
                Parts_fly.GetComponent<Parts_fly>().Change_speed_4();
                InvokeRepeating("Make_copy", 2f, 2f);
                break;

        }

        switch (ran_num2)
        {
            case 1:
                part_2.transform.localPosition = new Vector2(-2.3f, 0.8f);
                go2.transform.position = part_2.transform.position;
                Parts_fly_2.GetComponent<Parts_fly>().Change_speed_1();
                InvokeRepeating("Make_copy_2", 2f, 2f);
                break;

            case 2:
                part_2.transform.localPosition = new Vector2(2.6f, 1.5f);
                go2.transform.position = part_2.transform.position;
                Parts_fly_2.GetComponent<Parts_fly>().Change_speed_2();
                InvokeRepeating("Make_copy_2", 2f, 2f);
                break;

            case 3:
                part_2.transform.localPosition = new Vector2(-1.4f, -1.5f);
                go2.transform.position = part_2.transform.position;
                Parts_fly_2.GetComponent<Parts_fly>().Change_speed_3();
                InvokeRepeating("Make_copy_2", 2f, 2f);
                break;

            case 4:
                part_2.transform.localPosition = new Vector2(1.9f, -0.8f);
                go2.transform.position = part_2.transform.position;
                Parts_fly_2.GetComponent<Parts_fly>().Change_speed_4();
                InvokeRepeating("Make_copy_2", 2f, 2f);
                break;

        }



    }

    void Make_copy()
    {
        GameObject go = Instantiate(Parts_fly);
        go.transform.position = part.transform.position;
    }

    void Make_copy_2()
    {
        GameObject go_2 = Instantiate(Parts_fly_2);
        go_2.transform.position = part_2.transform.position;
    }

    int Get_ran_num()
    {
        var exclude = new HashSet<int> { ran_num };
        var range = Enumerable.Range(1, 4).Where(i => !exclude.Contains(i));

        var rand = new System.Random();
        int index = rand.Next(1, 4 - exclude.Count);
        return range.ElementAt(index);
    }


    void Update()
    {
        //CloudImage = ReceiveCloud.GetComponent<VirtualGameObject>().mImage;

        if (CloudSpawner.Cloud_Spawn == true)
        {
            Change_Cloud_scale = CloudSpawner.Cloud_scale;
            Cloud.transform.localScale = Change_Cloud_scale;
            Change_CloudImage = CloudSpawner.Cloud_sprite;
            Cloud.GetComponent<SpriteRenderer>().sprite = Change_CloudImage;

            Change_Part_scale = CloudSpawner.Part_scale;
            Parts_fly.transform.localScale = Change_Part_scale;
            Change_PartImage = CloudSpawner.Part_sprite;
            Parts_fly.GetComponent<SpriteRenderer>().sprite = Change_PartImage;

            Parts_fly_2.transform.localScale = Change_Part_scale;
            Parts_fly_2.GetComponent<SpriteRenderer>().sprite = Change_PartImage;
        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject go = Instantiate(Parts_fly);
            go.transform.position = part.transform.position;

            GameObject go_2 = Instantiate(Parts_fly_2);
            go_2.transform.position = part_2.transform.position;
        }
    }



}