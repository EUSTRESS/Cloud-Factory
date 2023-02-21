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



    public float fly_speed_x;    // x�������� ������ ���󰡴� �ӵ�
    public float fly_speed_y;   // y�������� ������ ���󰡴� �ӵ�
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = new Vector2(transform.position.x * fly_speed_x, transform.position.y * fly_speed_y);
    }

    void Update()
    {
 
    }

    // 4��и�(x>0,y<0)���� ���󰡴� �����ӵ�
    public void Fourquadrant_Change_speed_1()  // ���������� �����Ǵ� ���������󰡴¼ӵ�
    {
        fly_speed_x = -1.0f;
        fly_speed_y = -9.0f;
    }

    public void Fourquadrant_Change_speed_2()  // ������������ �����Ǵ� ���������󰡴¼ӵ�
    {
        fly_speed_x = 1.0f;
        fly_speed_y = -9.0f;
    }

    public void Fourquadrant_Change_speed_3()    // ���ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed_x = -1.0f;
        fly_speed_y = -9.0f;
    }

    public void Fourquadrant_Change_speed_4()    // �����ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed_x = 1.0f;
        fly_speed_y = -9.0f;
    }

    // �߰���ġ�� ���ڷ� �̵��Ҷ�����ϴ� ������ �ӵ��� ����
    // 1��и�(x>0,y>0)���� ���󰡴� �����ӵ�
    public void Onequadrant_Change_speed_1()  // ���������� �����Ǵ� ���������󰡴¼ӵ�
    {
        fly_speed_x = -1.0f;
        fly_speed_y = 9.0f;
    }

    public void Onequadrant_Change_speed_2()  // ������������ �����Ǵ� ���������󰡴¼ӵ�
    {
        fly_speed_x = 0.5f;
        fly_speed_y = 9.0f;
    }

    public void Onequadrant_Change_speed_3()    // ���ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed_x = -1.0f;
        fly_speed_y = 9.0f;
    }

    public void Onequadrant_Change_speed_4()    // �����ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed_x = 0.5f;
        fly_speed_y = 9.0f;
    }



    // 1��и鿡�� x�� 0�� ����������� ������ �ӵ�����
    public void Onequadrant_2case_Change_speed_1()  // ���������� �����Ǵ� ���������󰡴¼ӵ�
    {
        fly_speed_x = -60.0f;
        fly_speed_y = 0.5f;
    }

    public void Onequadrant_2case_Change_speed_2()  // ������������ �����Ǵ� ���������󰡴¼ӵ�
    {
        fly_speed_x = 1.0f;
        fly_speed_y = 0.5f;
    }

    public void Onequadrant_2case_Change_speed_3()    // ���ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed_x = -60.0f;
        fly_speed_y = 0.5f;
    }

    public void Onequadrant_2case_Change_speed_4()    // �����ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed_x = 1.0f;
        fly_speed_y = 0.5f;
    }



    // 2��и�(x<0,y>0)���� ���󰡴� �����Ǽӵ�
    public void Twoquadrant_Change_speed_1()  // ���������� �����Ǵ� ���������󰡴¼ӵ�
    {
        fly_speed_x = 1.0f;
        fly_speed_y = 9.0f;
    }

    public void Twoquadrant_Change_speed_2()  // ������������ �����Ǵ� ���������󰡴¼ӵ�
    {
        fly_speed_x = -0.5f;
        fly_speed_y = 9.0f;
    }

    public void Twoquadrant_Change_speed_3()    // ���ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed_x = 1.0f;
        fly_speed_y = 9.0f;
    }

    public void Twoquadrant_Change_speed_4()    // �����ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed_x = -0.5f;
        fly_speed_y = 9.0f;
    }

}
