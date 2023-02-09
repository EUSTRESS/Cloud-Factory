using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Parts_fly : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    public float fly_speed_x; // x방향으로 파츠의 날라가는 속도
    public float fly_speed_y; // y방향으로 파츠의 날라가는 속도


    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = new Vector2(transform.position.x * fly_speed_x, transform.position.y * fly_speed_y);
    }

    public void Change_speed_1()  // 왼쪽위에서 생성되는 파츠가날라가는속도
    {
        fly_speed_x = -0.5f;
        fly_speed_y = -5.0f;
    }

    public void Change_speed_2()  // 오른쪽위에서 생성되는 파츠가날라가는속도
    {
        fly_speed_x = 0.5f;
        fly_speed_y = -5.0f;
    }

    public void Change_speed_3()    // 왼쪽밑에서 생성되는 파츠가 날라가는 속도
    {
        fly_speed_x = -0.5f;
        fly_speed_y = -5.0f;
    }

    public void Change_speed_4()    // 오른쪽밑에서 생성되는 파츠가 날라가는 속도
    {
        fly_speed_x = 0.5f;
        fly_speed_y = -5.0f;
    }

}
