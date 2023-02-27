using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Parts_fly : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    public float fly_speed_x; // x방향으로 파츠의 날라가는 속도
    public float fly_speed_y; // y방향으로 파츠의 날라가는 속도

    public Vector2 final_fly_speed = new Vector2(0f, 0f);

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = new Vector2(transform.position.x * fly_speed_x, transform.position.y * fly_speed_y);
    }


    // 4사분면(x>0,y<0)에서 날라가는 파츠의 속도와방향
    public void Fourquadrant_change_speed1()    // 왼쪽위
    {
        fly_speed_x = -1.0f;
        fly_speed_y = -9.0f;
    }
    public void Fourquadrant_change_speed2()    // 오른쪽위
    {
        fly_speed_x = 0.5f;
        fly_speed_y = -9.0f;
    }
    public void Fourquadrant_change_speed3()    // 왼쪽밑
    {
        fly_speed_x = -1.0f;
        fly_speed_y = -5.0f;
    }
    public void Fourquadrant_change_speed4()    // 오른쪽밑
    {
        fly_speed_x = 0.5f;
        fly_speed_y = -5.0f;
    }

    // 4사분면에서 x가 4에서 0에 가까워질수록 날라가는 파츠의속도와 방향
    // 오른쪽위에서 나오는 파츠의 속도와 방향이 이상해서 따로만든함수
    //public void Fourquadrant_2case_change_speed2()
    //{
    //    fly_speed_x = 0.8f;
    //    fly_speed_y = 18.0f;
    //}



    //------------------------------------------------------------------------------------------------------------------------


    // 1사분면(x>0,y>0)에서 날라가는 파츠의 속도와방향
    public void Onequadrant_change_speed1()
    {
        fly_speed_x = -1.3f;
        fly_speed_y = 6.0f;
    }
    public void Onequadrant_change_speed2()
    {
        fly_speed_x = 1.3f;
        fly_speed_y = 6.0f;
    }
    public void Onequadrant_change_speed3()
    {
        fly_speed_x = -1.3f;
        fly_speed_y = 20.0f;
    }
    public void Onequadrant_change_speed4()
    {
        fly_speed_x = 1.3f;
        fly_speed_y = 20.0f;
    }

    // 1사분면에서 x가 0에 가까워질수록 날라가는 파츠의 속도와방향
    public void Onequadrant_2case_change_speed1()
    {
        fly_speed_x = -2.0f;
        fly_speed_y = 5.0f;
    }
    public void Onequadrant_2case_change_speed2()
    {
        fly_speed_x = 1.5f;
        fly_speed_y = 5.0f;
    }
    public void Onequadrant_2case_change_speed3()
    {
        fly_speed_x = -3.0f;
        fly_speed_y = 5.0f;
    }
    public void Onequadrant_2case_change_speed4()
    {
        fly_speed_x = 1.5f;
        fly_speed_y = 5.0f;
    }

    // 1사분면에서 x가 4.5에서 3에 가까워질수록 날라가는 파츠의속도와방향
    public void Onequadrant_3case_change_speed3()
    {
        fly_speed_x = -0.7f;
        fly_speed_y = 15.0f;
    }
    public void Onequadrant_3case_change_speed4()
    {
        fly_speed_x = 0.7f;
        fly_speed_y = 15.0f;
    }
    // 1사분면에서 파츠3,4좌표가 0.3이하일때 날라가는 파츠의 속도와방향
    public void Onequadrant_4case_change_speed3()
    {
        fly_speed_x = -0.7f;
        fly_speed_y = -40.0f;
    }
    public void Onequadrant_4case_change_speed4()
    {
        fly_speed_x = 0.7f;
        fly_speed_y = -40.0f;
    }

    // 1사분면에서 0.7>x>0에서 날라가는 3번파츠의 속도와방향
    public void Onequadrant_5case_change_speed3()
    {
        fly_speed_x = 10.0f;
        fly_speed_y = 9.0f;
    }



    public void Twoquadrant_2case_change_speed1()
    {
        fly_speed_x = 1.0f;
        fly_speed_y = 4.5f;
    }
    public void Twoquadrant_2case_change_speed2()
    {
        fly_speed_x = -1.0f;
        fly_speed_y = 4.5f;
    }
    public void Twoquadrant_2case_change_speed3()
    {
        fly_speed_x = 10.0f;
        fly_speed_y = 5.0f;
    }
    public void Twoquadrant_2case_change_speed4()
    {
        fly_speed_x = -0.5f;
        fly_speed_y = 4.5f;
    }


    //------------------------------------------------------------------------------------------------------------------------


    // 2사분면(x<0,y>0)에서 날라가는 파츠의 속도와방향
    public void Twoquadrant_change_speed1()
    {
        fly_speed_x = 1.0f;
        fly_speed_y = 9.0f;
    }
    public void Twoquadrant_change_speed2()
    {
        fly_speed_x = -0.5f;
        fly_speed_y = 9.0f;
    }
    public void Twoquadrant_change_speed3()
    {
        fly_speed_x = 1.0f;
        fly_speed_y = 9.0f;
    }
    public void Twoquadrant_change_speed4()
    {
        fly_speed_x = -0.5f;
        fly_speed_y = 9.0f;
    }

    
}
