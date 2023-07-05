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
    public bool IsUsing = false;

    public float random_num;
    public float random_num_2;


    void Awake()
    {
        ran_num = 1;
        //Random.Range(1, 5);
        ran_num2 = 2;
        //Get_ran_num();
        GameObject go = Instantiate(Parts_fly);
        GameObject go2 = Instantiate(Parts_fly_2);

        switch (ran_num)
        {
            case 1:
                random_num = Random.Range(0.1f, 0.7f);
                part.transform.localPosition = new Vector2(-0.7f, 0.3f);
                go.transform.position = part.transform.position;
                InvokeRepeating("Make_copy", 0.5f,random_num);
                break;

            case 2:
                random_num = Random.Range(0.1f, 0.7f);
                part.transform.localPosition = new Vector2(0.7f, 0.3f);
                go.transform.position = part.transform.position;
                InvokeRepeating("Make_copy", 0.5f, random_num);
                break;

            case 3:
                random_num = Random.Range(0.1f, 0.7f);
                part.transform.localPosition = new Vector2(-1.0f, -0.3f);
                go.transform.position = part.transform.position;
                InvokeRepeating("Make_copy", 0.5f, random_num);
                break;

            case 4:
                random_num = Random.Range(0.1f, 0.7f);
                part.transform.localPosition = new Vector2(0.9f, -0.3f);
                go.transform.position = part.transform.position;
                InvokeRepeating("Make_copy", 0.5f, random_num);
                break;

        }

        switch (ran_num2)
        {
            case 1:
                random_num = Random.Range(0.1f, 0.7f);
                part_2.transform.localPosition = new Vector2(-0.7f, 0.3f);
                go2.transform.position = part_2.transform.position;
                InvokeRepeating("Make_copy_2", 1.0f, random_num_2);
                break;

            case 2:
                random_num_2 = Random.Range(0.1f, 0.7f);
                part_2.transform.localPosition = new Vector2(0.7f, 0.3f);
                go2.transform.position = part_2.transform.position;
                InvokeRepeating("Make_copy_2", 1.0f, random_num_2);
                break;

            case 3:
                random_num = Random.Range(0.1f, 0.7f);
                part_2.transform.localPosition = new Vector2(-1.0f, -0.3f);
                go2.transform.position = part_2.transform.position;
                InvokeRepeating("Make_copy_2", 1.0f, random_num_2);
                break;

            case 4:
                random_num = Random.Range(0.1f, 0.7f);
                part_2.transform.localPosition = new Vector2(0.9f, -0.3f);
                go2.transform.position = part_2.transform.position;
                InvokeRepeating("Make_copy_2", 1.0f, random_num_2);
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

    //int Get_ran_num()
    //{
    //    var exclude = new HashSet<int> { ran_num };
    //    var range = Enumerable.Range(1, 4).Where(i => !exclude.Contains(i));

    //    var rand = new System.Random();
    //    int index = rand.Next(1, 4 - exclude.Count);
    //    return range.ElementAt(index);
    //}

    public void DestroyCloud()
    {
        GameObject tempObject = this.transform.parent.transform.gameObject;

        Destroy(this.transform.GetChild(0).gameObject);
        Destroy(this.transform.GetChild(1).gameObject);

        Destroy(tempObject);
    }

    void Update()
    {
        if (IsUsing == true)
        {
            CancelInvoke("Make_copy");
            CancelInvoke("Make_copy_2");
        }
    }
}