using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Cloud_movement : MonoBehaviour
{
    public int ran_num = 0;     // ����Ƣ����� ��ġ�� �����ִ� ù��° ����
    public int ran_num2 = 0;    // ����Ƣ����� ��ġ�� �����ִ� �ι�° ����
    public GameObject Cloud;
    public GameObject part;
    public GameObject part_2;
    public GameObject Parts_fly;
    public GameObject Parts_fly_2;

    public int Target_guestnum = 0;


    public Vector3 Change_Cloud_scale = new Vector3(0f, 0f, 0f);    // �ٲ� ������ scale
    public Vector3 Change_Part_scale = new Vector3(0f, 0f, 0f);     // �ٲ� ������ scale

    public Sprite Change_CloudImage;      // �ٲ� ������ �̹���

    public Sprite Change_PartImage;     // �ٲ� ������ �̹���

    void Awake()
    {
        ran_num = 1;//Random.Range(1, 5);
        ran_num2 = 3;//Get_ran_num();
        GameObject go = Instantiate(Parts_fly);
        GameObject go2 = Instantiate(Parts_fly_2);

        switch (ran_num)
        {
            case 1:
                part.transform.localPosition = new Vector2(-2.3f, 0.8f);
                go.transform.position = part.transform.position;
                Parts_fly.GetComponent<Parts_fly>().Change_speed_1();
                InvokeRepeating("Make_copy", 0.2f, 0.1f);
                break;

            case 2:
                part.transform.localPosition = new Vector2(2.6f, 1.5f);
                go.transform.position = part.transform.position;
                Parts_fly.GetComponent<Parts_fly>().Change_speed_2();
                InvokeRepeating("Make_copy", 0.2f, 0.1f);
                break;

            case 3:
                part.transform.localPosition = new Vector2(-1.4f, -1.5f);
                go.transform.position = part.transform.position;
                Parts_fly.GetComponent<Parts_fly>().Change_speed_3();
                InvokeRepeating("Make_copy", 0.2f, 0.1f);
                break;

            case 4:
                part.transform.localPosition = new Vector2(1.9f, -0.8f);
                go.transform.position = part.transform.position;
                Parts_fly.GetComponent<Parts_fly>().Change_speed_4();
                InvokeRepeating("Make_copy", 0.2f, 0.1f);
                break;

        }

        switch (ran_num2)
        {
            case 1:
                part_2.transform.localPosition = new Vector2(-2.3f, 0.8f);
                go2.transform.position = part_2.transform.position;
                Parts_fly_2.GetComponent<Parts_fly>().Change_speed_1();
                InvokeRepeating("Make_copy_2", 0.2f, 0.1f);
                break;

            case 2:
                part_2.transform.localPosition = new Vector2(2.6f, 1.5f);
                go2.transform.position = part_2.transform.position;
                Parts_fly_2.GetComponent<Parts_fly>().Change_speed_2();
                InvokeRepeating("Make_copy_2", 0.2f, 0.1f);
                break;

            case 3:
                part_2.transform.localPosition = new Vector2(-1.4f, -1.5f);
                go2.transform.position = part_2.transform.position;
                Parts_fly_2.GetComponent<Parts_fly>().Change_speed_3();
                InvokeRepeating("Make_copy_2", 0.2f, 0.1f);
                break;

            case 4:
                part_2.transform.localPosition = new Vector2(1.9f, -0.8f);
                go2.transform.position = part_2.transform.position;
                Parts_fly_2.GetComponent<Parts_fly>().Change_speed_4();
                InvokeRepeating("Make_copy_2", 0.2f, 0.1f);
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

    void Move_part()
    {

    }

    public void DestroyCloud()
    {
        GameObject tempObject = this.transform.parent.transform.gameObject;

        Destroy(tempObject);
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject go = Instantiate(Parts_fly);
            go.transform.position = part.transform.position;

            GameObject go_2 = Instantiate(Parts_fly_2);
            go_2.transform.position = part_2.transform.position;
        }
    }



}