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

    public int Target_guestnum = 0;


    public Vector3 Change_Cloud_scale = new Vector3(0f, 0f, 0f);    // 바꿀 구름의 scale
    public Vector3 Change_Part_scale = new Vector3(0f, 0f, 0f);     // 바꿀 파츠의 scale

    public Sprite Change_CloudImage;      // 바꿀 구름의 이미지

    public Sprite Change_PartImage;     // 바꿀 파츠의 이미지

    public Vector3 real_cloud_ps = new Vector3(0f, 0f, 0f);

    private bool Part1_out = false;
    private bool Part2_out = false;

    private bool Part1_out_2 = false;

    public bool IsUsing = false;

    

    void Awake()
    {
        ran_num = Random.Range(1, 5);
        ran_num2 = Get_ran_num();
        GameObject go = Instantiate(Parts_fly);
        GameObject go2 = Instantiate(Parts_fly_2);



        switch (ran_num)
        {
            case 1:
                part.transform.localPosition = new Vector2(-0.7f, 0.3f);
                go.transform.position = part.transform.position;
                InvokeRepeating("Check_position1", 0.49f, 0.99f);
                InvokeRepeating("Make_copy", 0.5f, 1.0f);
                break;

            case 2:
                part.transform.localPosition = new Vector2(0.7f, 0.3f);
                go.transform.position = part.transform.position;
                InvokeRepeating("Check_position1", 0.49f, 0.99f);
                InvokeRepeating("Make_copy", 0.5f, 1.0f);
                break;

            case 3:
                part.transform.localPosition = new Vector2(-1.0f, -0.3f);
                go.transform.position = part.transform.position;
                InvokeRepeating("Check_position1", 0.49f, 0.99f);
                InvokeRepeating("Make_copy", 0.5f, 1.0f);
                break;

            case 4:
                part.transform.localPosition = new Vector2(0.9f, -0.3f);
                go.transform.position = part.transform.position;
                InvokeRepeating("Check_position1", 0.49f, 0.99f);
                InvokeRepeating("Make_copy", 0.5f, 1.0f);
                break;

        }

        switch (ran_num2)
        {
            case 1:
                part_2.transform.localPosition = new Vector2(-0.7f, 0.3f);
                go2.transform.position = part_2.transform.position;
                InvokeRepeating("Check_position2", 0.99f, 0.99f);
                InvokeRepeating("Make_copy_2", 1.0f, 1.0f);
                break;

            case 2:
                part_2.transform.localPosition = new Vector2(0.7f, 0.3f);
                go2.transform.position = part_2.transform.position;
                InvokeRepeating("Check_position2", 0.99f, 0.99f);
                InvokeRepeating("Make_copy_2", 1.0f, 1.0f);
                break;

            case 3:
                part_2.transform.localPosition = new Vector2(-1.0f, -0.3f);
                go2.transform.position = part_2.transform.position;
                InvokeRepeating("Check_position2", 0.99f, 0.99f);
                InvokeRepeating("Make_copy_2", 1.0f, 1.0f);
                break;

            case 4:
                part_2.transform.localPosition = new Vector2(0.9f, -0.3f);
                go2.transform.position = part_2.transform.position;
                InvokeRepeating("Check_position2", 0.99f, 0.99f);
                InvokeRepeating("Make_copy_2", 1.0f, 1.0f);
                break;

        }
    }


    void Check_position1()
    {
        Part1_out = false;
        Part2_out = false;
        Part1_out_2 = false;
        real_cloud_ps = this.transform.parent.transform.position;
        // 4사분면 속도함수 호출
        if(real_cloud_ps.x > 0 && real_cloud_ps.y < 0)
        {
            if(ran_num == 1)    // 왼쪽위
            {
                Parts_fly.GetComponent<Parts_fly>().Fourquadrant_change_speed1();
            }
            else if(ran_num ==2)    // 오른쪽위
            {
                Parts_fly.GetComponent<Parts_fly>().Fourquadrant_change_speed2();
            }
            else if(ran_num == 3)   // 왼쪽밑
            {
                Parts_fly.GetComponent<Parts_fly>().Fourquadrant_change_speed3();
            }
            else if(ran_num == 4)   // 오른쪽밑
            {
                Parts_fly.GetComponent<Parts_fly>().Fourquadrant_change_speed4();
            }
        }
        

        // 1사분면 속도함수 호출
        else if(real_cloud_ps.x >0 && real_cloud_ps.y > 0)
        {
            if(ran_num == 1)
            {
                Parts_fly.GetComponent<Parts_fly>().Onequadrant_change_speed1();
                if (real_cloud_ps.x < 2 && real_cloud_ps.x > 0 && real_cloud_ps.y > 0)
                {
                    Part1_out = true;
                    Parts_fly.GetComponent<Parts_fly>().Onequadrant_2case_change_speed1();
                }
            }
            else if (ran_num == 2)
            {
                Parts_fly.GetComponent<Parts_fly>().Onequadrant_change_speed2();
                if (real_cloud_ps.x < 2 && real_cloud_ps.x > 0 && real_cloud_ps.y > 0)
                {
                    Part2_out = true;
                    Parts_fly.GetComponent<Parts_fly>().Onequadrant_2case_change_speed2();
                }
            }
            else if (ran_num == 3)
            {
                Parts_fly.GetComponent<Parts_fly>().Onequadrant_change_speed3();
                if(real_cloud_ps.x <= 2 && real_cloud_ps.x > 0.7 && real_cloud_ps.y > 0.3)
                {
                    Part1_out = true;
                    Parts_fly.GetComponent<Parts_fly>().Onequadrant_2case_change_speed3();
                }
                else if(real_cloud_ps.x < 4.5 && real_cloud_ps.x > 2 && real_cloud_ps.y > 0.3)
                {
                    Part1_out = true;
                    Parts_fly.GetComponent<Parts_fly>().Onequadrant_3case_change_speed3();
                }
                else if(real_cloud_ps.x < 6 && real_cloud_ps.x > 4.5 && real_cloud_ps.y <= 0.3)
                {
                    Parts_fly.GetComponent<Parts_fly>().Onequadrant_4case_change_speed3();
                }
                else if(real_cloud_ps.x <= 0.7 && real_cloud_ps.x > 0 && real_cloud_ps.y > 0.3)
                {
                    Parts_fly.GetComponent<Parts_fly>().Onequadrant_5case_change_speed3();
                }
            }
            else if (ran_num == 4)
            {
                Parts_fly.GetComponent<Parts_fly>().Onequadrant_change_speed4();
                if(real_cloud_ps.x <= 2 && real_cloud_ps.x > 0 && real_cloud_ps.y > 0.3)
                {
                    Part2_out = true;
                    Parts_fly.GetComponent<Parts_fly>().Onequadrant_2case_change_speed4();
                }
                else if(real_cloud_ps.x < 4 && real_cloud_ps.x > 2 && real_cloud_ps.y > 0.3)
                {
                    Parts_fly.GetComponent<Parts_fly>().Onequadrant_3case_change_speed4();
                }
                else if(real_cloud_ps.x < 6 && real_cloud_ps.x > 4 && real_cloud_ps.y <= 0.3)
                {
                    Parts_fly.GetComponent<Parts_fly>().Onequadrant_4case_change_speed4();
                }
            }
        }


        // 2사분면 속도함수 호출
        else if(real_cloud_ps.x<0 && real_cloud_ps.y > 0)
        {
            if (ran_num == 1)
            {
                Parts_fly.GetComponent<Parts_fly>().Twoquadrant_change_speed1();
                if(real_cloud_ps.x <0 && real_cloud_ps.x > -1 && real_cloud_ps.y > 0)
                {
                    Parts_fly.GetComponent<Parts_fly>().Twoquadrant_2case_change_speed1();
                }
            }
            else if (ran_num == 2)
            {
                Parts_fly.GetComponent<Parts_fly>().Twoquadrant_change_speed2();
                if(real_cloud_ps.x < 0 && real_cloud_ps.x > -1 && real_cloud_ps.y > 0)
                {
                    Parts_fly.GetComponent<Parts_fly>().Twoquadrant_2case_change_speed2();
                }
            }
            else if (ran_num == 3)
            {
                Parts_fly.GetComponent<Parts_fly>().Twoquadrant_change_speed3();
            }
            else if (ran_num == 4)
            {
                Parts_fly.GetComponent<Parts_fly>().Twoquadrant_change_speed4();
            }
        }
    }

    void Check_position2()
    {
        Part1_out = false;
        Part2_out = false;
        Part1_out_2 = false;
        real_cloud_ps = this.transform.parent.transform.position;
        // 4사분면 속도함수 호출
        if (real_cloud_ps.x > 0 && real_cloud_ps.y < 0)
        {
            if (ran_num2 == 1)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Fourquadrant_change_speed1();
            }
            else if (ran_num2 == 2)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Fourquadrant_change_speed2();
            }
            else if (ran_num2 == 3)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Fourquadrant_change_speed3();
            }
            else if (ran_num2 == 4)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Fourquadrant_change_speed4();
            }
        }



        // 1사분면 속도함수호출
        else if(real_cloud_ps.x >0 && real_cloud_ps.y > 0)
        {
            if(ran_num2 == 1)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_change_speed1();
                if (real_cloud_ps.x < 2 && real_cloud_ps.x > 0 && real_cloud_ps.y > 0)
                {
                    Part1_out = true;
                    Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_2case_change_speed1();
                }
            }
            else if (ran_num2 == 2)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_change_speed2();
                if (real_cloud_ps.x < 2 && real_cloud_ps.x > 0 && real_cloud_ps.y > 0)
                {
                    Part2_out = true;
                    Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_2case_change_speed2();
                }
            }
            else if (ran_num2 == 3)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_change_speed3();
                if(real_cloud_ps.x <= 2 && real_cloud_ps.x > 0.7 && real_cloud_ps.y > 0.3)
                {
                    Part1_out = true;
                    Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_2case_change_speed3();
                }
                else if(real_cloud_ps.x < 4.5 && real_cloud_ps.x > 2 && real_cloud_ps.y > 0.3)
                {
                    Part1_out = true;
                    Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_3case_change_speed3();
                }
                else if(real_cloud_ps.x < 6 && real_cloud_ps.x > 4.5 && real_cloud_ps.y <= 0.3)
                {
                    Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_4case_change_speed3();
                }
                else if(real_cloud_ps.x <= 0.7 && real_cloud_ps.x >0 && real_cloud_ps.y > 0.3)
                {
                    Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_5case_change_speed3();
                }
            }
            else if (ran_num2 == 4)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_change_speed4();
                if (real_cloud_ps.x <= 2 && real_cloud_ps.x > 0 && real_cloud_ps.y > 0.3)
                {
                    Part2_out = true;
                    Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_2case_change_speed4();
                }
                else if(real_cloud_ps.x < 4 && real_cloud_ps.x > 2 && real_cloud_ps.y > 0.3)
                {
                    Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_3case_change_speed4();
                }
                else if(real_cloud_ps.x < 6 && real_cloud_ps.x > 4 && real_cloud_ps.y <= 0.3)
                {
                    Parts_fly_2.GetComponent<Parts_fly>().Onequadrant_4case_change_speed4();
                }
            }
        }



        // 2사분면 속도함수호출
        else if(real_cloud_ps.x <0 && real_cloud_ps.y > 0)
        {
            if (ran_num2 == 1)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Twoquadrant_change_speed1();
                if(real_cloud_ps.x <0 && real_cloud_ps.x >-1 && real_cloud_ps.y >0)
                {
                    Parts_fly_2.GetComponent<Parts_fly>().Twoquadrant_2case_change_speed1();
                }
            }
            else if (ran_num2 == 2)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Twoquadrant_change_speed2();
                if(real_cloud_ps.x < 0 && real_cloud_ps.x > -1 && real_cloud_ps.y > 0)
                {
                    Parts_fly_2.GetComponent<Parts_fly>().Twoquadrant_2case_change_speed2();
                }
            }
            else if (ran_num2 == 3)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Twoquadrant_change_speed3();
            }
            else if (ran_num2 == 4)
            {
                Parts_fly_2.GetComponent<Parts_fly>().Twoquadrant_change_speed4();
            }
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


    public void DestroyCloud()
    {
        GameObject tempObject = this.transform.parent.transform.gameObject;

        Destroy(tempObject);
    }

    private void FixedUpdate()
    {
        if (Part1_out == true)
        {
            Physics2D.gravity = new Vector2(-2f, -9.81f);
        }
        else if (Part2_out == true)
        {
            Physics2D.gravity = new Vector2(0f, -9.81f);
        }
        else if(Part1_out_2 == true)
        {
            Physics2D.gravity = new Vector2(-3f, -9.81f);
        }
        else if (Part1_out == false || Part2_out == false || Part1_out_2 == false)
        {
            Physics2D.gravity = new Vector2(0f, -9.81f);
        }
    }


    void Update()
    {
        SOWManager sowManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        GuestObject guest_object = sowManager.mUsingGuestObjectList[0].GetComponent<GuestObject>();
        IsUsing = guest_object.isUsing;
        if(IsUsing == true)
        {
            CancelInvoke("Check_position");
            CancelInvoke("Make_copy");

            CancelInvoke("Check_position2");
            CancelInvoke("Make_copy_2");
        }
    }



}