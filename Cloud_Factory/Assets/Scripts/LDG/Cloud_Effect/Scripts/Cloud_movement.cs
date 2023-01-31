using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_movement : MonoBehaviour
{
    public int num = 0;
    //public float limintMin = -5.0f;
    //private bool Act = false;
    public GameObject cloud_part;        // 
    public GameObject parts;        // 프리팹을 인스턴스화하여 담을 게임오브젝트
    public GameObject Parts_fly;    // part_fly 스크립트 담겨져있는 프리팹담을 게임오브젝트

    void Start()
    {
        num = Random.Range(1, 5); // 네가지위치 무작위생성하는 변수
        //GameObject go = Instantiate(parts);
        switch (num)
        {
            case 1:
                cloud_part.transform.localPosition = new Vector2(-0.5f, 0.23f);         // 왼쪽위 
                InvokeRepeating("fly", 2f, 2f);                                                   // 파츠가 날라가는 함수 반복하는것(함수가 2초후부터 실행되고 2초주기로 실행)
                Parts_fly.GetComponent<Parts_fly>().change_speed_1_2();              // 
                break;

            case 2:
                cloud_part.transform.localPosition = new Vector2(0.57f, 0.31f);
                InvokeRepeating("fly", 2f, 2f);
                Parts_fly.GetComponent<Parts_fly>().change_speed_1_2();
                break;

            case 3:
                cloud_part.transform.localPosition = new Vector2(-0.29f, -0.29f);
                InvokeRepeating("fly", 2f, 2f);
                Parts_fly.GetComponent<Parts_fly>().change_speed_3();
                break;

            case 4:
                cloud_part.transform.localPosition = new Vector2(0.34f, -0.1f);
                InvokeRepeating("fly", 2f, 2f);
                Parts_fly.GetComponent<Parts_fly>().change_speed_4();
                break;


        }




    }

    void fly()
    {
        GameObject go = Instantiate(parts);
        go.transform.position = cloud_part.transform.position;
    }

    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject go = Instantiate(parts);
            go.transform.position = cloud_part.transform.position;
        }
    }
}
