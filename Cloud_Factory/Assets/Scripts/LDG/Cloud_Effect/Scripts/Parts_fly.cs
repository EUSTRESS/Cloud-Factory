using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Parts_fly : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    public static float fly_speed; // 파츠의 날라가는 속도

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = transform.position * fly_speed;
    }

    public void Change_speed_1()  // 왼쪽위에서 생성되는 파츠가날라가는속도
    {
        fly_speed = -1.0f;
    }

    public void Change_speed_2()  // 오른쪽위에서 생성되는 파츠가날라가는속도
    {
        fly_speed = 1.0f;
    }

    public void Change_speed_3()    // 왼쪽밑에서 생성되는 파츠가 날라가는 속도
    {
        fly_speed = -2.5f;
    }

    public void Change_speed_4()    // 오른쪽밑에서 생성되는 파츠가 날라가는 속도
    {
        fly_speed = 1.5f;
    }

}
