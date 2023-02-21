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

    public Vector3 cloud_position;

    private Vector3 Check_cloud_Position = new Vector3(0f, 0f, 0f);

    private bool Out_part = false;

    private bool Out_part_2 = false;




    public Vector3 Change_Cloud_scale = new Vector3(0f, 0f, 0f);    // 바꿀 구름의 scale
    public Vector3 Change_Part_scale = new Vector3(0f, 0f, 0f);     // 바꿀 파츠의 scale

    public Sprite Change_CloudImage;      // 바꿀 구름의 이미지

    public Sprite Change_PartImage;     // 바꿀 파츠의 이미지

    void Awake()
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
                InvokeRepeating("Check_Position", 0.49f, 0.9f);
                InvokeRepeating("Make_copy", 0.5f, 1.0f);
                break;

            case 2:
                part.transform.localPosition = new Vector2(2.6f, 1.5f);
                go.transform.position = part.transform.position;
                InvokeRepeating("Check_Position", 0.49f, 0.9f);
                InvokeRepeating("Make_copy", 0.5f, 1.0f);

                break;

            case 3:
                part.transform.localPosition = new Vector2(-1.4f, -1.5f);
                go.transform.position = part.transform.position;
                InvokeRepeating("Check_Position", 0.49f, 0.9f);
                InvokeRepeating("Make_copy", 0.5f, 1.0f);
                break;

            case 4:
                part.transform.localPosition = new Vector2(1.9f, -0.8f);
                go.transform.position = part.transform.position;
                InvokeRepeating("Check_Position", 0.49f, 0.9f);
                InvokeRepeating("Make_copy", 0.5f, 1.0f);
                break;

        }

        switch (ran_num2)
        {
            case 1:
                part_2.transform.localPosition = new Vector2(-2.3f, 0.8f);
                go2.transform.position = part_2.transform.position;
                InvokeRepeating("Check_Position_2", 0.9f, 0.9f);
                InvokeRepeating("Make_copy_2", 1f, 1f);
                break;

            case 2:
                part_2.transform.localPosition = new Vector2(2.6f, 1.5f);
                go2.transform.position = part_2.transform.position;
                InvokeRepeating("Check_Position_2", 0.9f, 0.9f);
                InvokeRepeating("Make_copy_2", 1f, 1f);
                break;

            case 3:
                part_2.transform.localPosition = new Vector2(-1.4f, -1.5f);
                go2.transform.position = part_2.transform.position;
                InvokeRepeating("Check_Position_2", 0.9f, 0.9f);
                InvokeRepeating("Make_copy_2", 1f, 1f);
                break;

            case 4:
                part_2.transform.localPosition = new Vector2(1.9f, -0.8f);
                go2.transform.position = part_2.transform.position;
                InvokeRepeating("Check_Position_2", 0.9f, 0.9f);
                InvokeRepeating("Make_copy_2", 1f, 1f);
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

    void Check_Position()       // 1번째 파츠 속도제어
    {
        Check_cloud_Position = CloudSpawner.cloud_ps;
        // 4사분면
        if(Check_cloud_Position.x > 0 && Check_cloud_Position.y < 0)
        {
            if(ran_num == 1)
            {
                Parts_fly.GetComponent<Parts_fly>().Fourquadrant_Change_speed_1();
            }
            else if (ran_num == 2)
            {
                Parts_fly.GetComponent<Parts_fly>().Fourquadrant_Change_speed_2();
            }
            else if (ran_num == 3)
            {
                Parts_fly.GetComponent<Parts_fly>().Fourquadrant_Change_speed_3();
            }
            else if (ran_num == 4)
            {
                Parts_fly.GetComponent<Parts_fly>().Fourquadrant_Change_speed_4();
            }
        }

        // 1사분면
        else if (Check_cloud_Position.x > 0 && Check_cloud_Position.y >= 0)
        {
            if (ran_num == 1)
            {
                Parts_fly.GetComponent<Parts_fly>().Onequadrant_Change_speed_1();
            }
            else if (ran_num == 2)
            {
                Parts_fly.GetComponent<Parts_fly>().Onequadrant_Change_speed_2();
            }
            else if (ran_num == 3)
            {
                Parts_fly.GetComponent<Parts_fly>().Onequadrant_Change_speed_3();
            }
            else if (ran_num == 4)
            {
                Parts_fly.GetComponent<Parts_fly>().Onequadrant_Change_speed_4();
            }
        }
        // 1사분면 x좌표가 0에 가까울때
        else if( Check_cloud_Position.x>0 && Check_cloud_Position.x <1 && Check_cloud_Position.y >= 0)
        {
            if(ran_num == 1)
            {
                Out_part = true;
                Parts_fly.GetComponent<Parts_fly>().Onequadrant_2case_Change_speed_1();
            }
            else if(ran_num == 2)
            {
                Out_part_2 = true;
                Parts_fly.GetComponent<Parts_fly>().Onequadrant_2case_Change_speed_2();
            }
            else if (ran_num == 3)
            {
                Out_part = true;
                Parts_fly.GetComponent<Parts_fly>().Onequadrant_2case_Change_speed_3();
            }
            else if (ran_num == 4)
            {
                Out_part_2 = true;
                Parts_fly.GetComponent<Parts_fly>().Onequadrant_2case_Change_speed_4();
            }
        }


        // 2사분면
        else if(Check_cloud_Position.x<=0 && Check_cloud_Position.y >= 0)
        {
            if (ran_num == 1)
            {
                Parts_fly.GetComponent<Parts_fly>().Twoquadrant_Change_speed_1();
            }
            else if (ran_num == 2)
            {
                Parts_fly.GetComponent<Parts_fly>().Twoquadrant_Change_speed_2();
            }
            else if (ran_num == 3)
            {
                Parts_fly.GetComponent<Parts_fly>().Twoquadrant_Change_speed_3();
            }
            else if (ran_num == 4)
            {
                Parts_fly.GetComponent<Parts_fly>().Twoquadrant_Change_speed_4();
            }
        }
        Out_part = false;
        Out_part_2 = false;
    }

    void Check_Position_2()     // 2번째파츠 속도제어
    {
        Check_cloud_Position = CloudSpawner.cloud_ps;
        // 4사분면
        if (Check_cloud_Position.x > 0 && Check_cloud_Position.y < 0)
        {
            if (ran_num2 == 1)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Fourquadrant_Change_speed_1();
            }
            else if (ran_num2 == 2)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Fourquadrant_Change_speed_2();
            }
            else if (ran_num2 == 3)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Fourquadrant_Change_speed_3();
            }
            else if (ran_num2 == 4)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Fourquadrant_Change_speed_4();
            }
        }

        // 1사분면
        else if(Check_cloud_Position.x>0 && Check_cloud_Position.y >= 0)
        {
            if (ran_num2 == 1)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_Change_speed_1();
            }
            else if (ran_num2 == 2)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_Change_speed_2();
            }
            else if (ran_num2 == 3)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_Change_speed_3();
            }
            else if (ran_num2 == 4)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_Change_speed_4();
            }
        }


        // 1사분면 x가 0에 가까울때
        else if(Check_cloud_Position.x > 0 && Check_cloud_Position.x < 1 && Check_cloud_Position.y >= 0)
        {
            if (ran_num2 == 1)
            {
                Out_part = true;
                Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_2case_Change_speed_1();
            }
            else if (ran_num2 == 2)
            {
                Out_part_2 = true;
                Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_2case_Change_speed_2();
            }
            else if (ran_num2 == 3)
            {
                Out_part = true;
                Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_2case_Change_speed_3();
            }
            else if (ran_num2 == 4)
            {
                Out_part_2 = true;
                Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_2case_Change_speed_4();
            }
        }

        // 2사분면
        else if(Check_cloud_Position.x <= 0 && Check_cloud_Position.y >= 0)
        {
            if (ran_num2 == 1)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Twoquadrant_Change_speed_1();
            }
            else if (ran_num2 == 2)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Twoquadrant_Change_speed_2();
            }
            else if (ran_num2 == 3)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Twoquadrant_Change_speed_3();
            }
            else if (ran_num2 == 4)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Twoquadrant_Change_speed_4();
            }
        }
        Out_part = false;
        Out_part_2 = false;
    }


    int Get_ran_num()
    {
        var exclude = new HashSet<int> { ran_num };
        var range = Enumerable.Range(1, 4).Where(i => !exclude.Contains(i));

        var rand = new System.Random();
        int index = rand.Next(1, 4 - exclude.Count);
        return range.ElementAt(index);
    }

    void FixedUpdate()
    {
        if (Check_cloud_Position.x > 0 && Check_cloud_Position.x < 2 && Check_cloud_Position.y >= 0)
        {
            if (Out_part == true)
            {
                Physics2D.gravity = new Vector2(-2f, -9.81f);
            }
            else if (Out_part_2 == true)
            {
                Physics2D.gravity = new Vector2(0f, -9.81f);

            }
        }
    }
}