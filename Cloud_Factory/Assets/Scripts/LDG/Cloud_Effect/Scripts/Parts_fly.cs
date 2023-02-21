using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Parts_fly : MonoBehaviour
{
    Rigidbody2D Rigidbody;

    int R_Num1 = 0;
    int R_Num2 = 0;

    Cloud_movement cloud_Movement;



    public float fly_speed_x;    // x방향으로 파츠의 날라가는 속도
    public float fly_speed_y;   // y방향으로 파츠의 날라가는 속도
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = new Vector2(transform.position.x * fly_speed_x, transform.position.y * fly_speed_y);
    }

    void Update()
    {
 
    }

    // 4사분면(x>0,y<0)에서 날라가는 파츠속도
    public void Fourquadrant_Change_speed_1()  // 왼쪽위에서 생성되는 파츠가날라가는속도
    {
        fly_speed_x = -1.0f;
        fly_speed_y = -9.0f;
    }

    public void Fourquadrant_Change_speed_2()  // 오른쪽위에서 생성되는 파츠가날라가는속도
    {
        fly_speed_x = 1.0f;
        fly_speed_y = -9.0f;
    }

    public void Fourquadrant_Change_speed_3()    // 왼쪽밑에서 생성되는 파츠가 날라가는 속도
    {
        fly_speed_x = -1.0f;
        fly_speed_y = -9.0f;
    }

    public void Fourquadrant_Change_speed_4()    // 오른쪽밑에서 생성되는 파츠가 날라가는 속도
    {
        fly_speed_x = 1.0f;
        fly_speed_y = -9.0f;
    }

    // 중간위치의 의자로 이동할때사용하는 파츠의 속도및 방향
    // 1사분면(x>0,y>0)에서 날라가는 파츠속도
    public void Onequadrant_Change_speed_1()  // 왼쪽위에서 생성되는 파츠가날라가는속도
    {
        fly_speed_x = -1.0f;
        fly_speed_y = 9.0f;
    }

    public void Onequadrant_Change_speed_2()  // 오른쪽위에서 생성되는 파츠가날라가는속도
    {
        fly_speed_x = 0.5f;
        fly_speed_y = 9.0f;
    }

    public void Onequadrant_Change_speed_3()    // 왼쪽밑에서 생성되는 파츠가 날라가는 속도
    {
        fly_speed_x = -1.0f;
        fly_speed_y = 9.0f;
    }

    public void Onequadrant_Change_speed_4()    // 오른쪽밑에서 생성되는 파츠가 날라가는 속도
    {
        fly_speed_x = 0.5f;
        fly_speed_y = 9.0f;
    }



    // 1사분면에서 x가 0에 가까워질수록 파츠의 속도변경
    public void Onequadrant_2case_Change_speed_1()  // 왼쪽위에서 생성되는 파츠가날라가는속도
    {
        fly_speed_x = -60.0f;
        fly_speed_y = 0.5f;
    }

    public void Onequadrant_2case_Change_speed_2()  // 오른쪽위에서 생성되는 파츠가날라가는속도
    {
        fly_speed_x = 1.0f;
        fly_speed_y = 0.5f;
    }

    public void Onequadrant_2case_Change_speed_3()    // 왼쪽밑에서 생성되는 파츠가 날라가는 속도
    {
        fly_speed_x = -60.0f;
        fly_speed_y = 0.5f;
    }

    public void Onequadrant_2case_Change_speed_4()    // 오른쪽밑에서 생성되는 파츠가 날라가는 속도
    {
        fly_speed_x = 1.0f;
        fly_speed_y = 0.5f;
    }



    // 2사분면(x<0,y>0)에서 날라가는 파츠의속도
    public void Twoquadrant_Change_speed_1()  // 왼쪽위에서 생성되는 파츠가날라가는속도
    {
        fly_speed_x = 1.0f;
        fly_speed_y = 9.0f;
    }

    public void Twoquadrant_Change_speed_2()  // 오른쪽위에서 생성되는 파츠가날라가는속도
    {
        fly_speed_x = -0.5f;
        fly_speed_y = 9.0f;
    }

    public void Twoquadrant_Change_speed_3()    // 왼쪽밑에서 생성되는 파츠가 날라가는 속도
    {
        fly_speed_x = 1.0f;
        fly_speed_y = 9.0f;
    }

    public void Twoquadrant_Change_speed_4()    // 오른쪽밑에서 생성되는 파츠가 날라가는 속도
    {
        fly_speed_x = -0.5f;
        fly_speed_y = 9.0f;
    }

}
