using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Parts_fly : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public static float fly_speed; // 파츠의 날라가는 속도

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.position * fly_speed;
    }

    //void fly()
    //{
    //    rigidbody = GetComponent<Rigidbody2D>();
    //    rigidbody.velocity = transform.position * fly_speed;
    //}

    public void change_speed_1_2()  // 왼쪽위와 오른쪽위에서 생성되는 파츠가날라가는속도
    {
        fly_speed = 10.0f;
    }

    public void change_speed_3()    // 왼쪽밑에서 생성되는 파츠가 날라가는 속도
    {
        fly_speed = 25.0f;
    }

    public void change_speed_4()    // 오른쪽밑에서 생성되는 파츠가 날라가는 속도
    {
        fly_speed = 15.0f;
    }

}
