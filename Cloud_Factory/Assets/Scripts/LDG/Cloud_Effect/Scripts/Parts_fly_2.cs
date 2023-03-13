using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Parts_fly_2 : MonoBehaviour
{
    Rigidbody2D Rigidbody;            // 구름파츠의 rigidbody 컴포넌트

    private float fly_speed = 5.0f;
    private float fly_angle = 45.0f;       // 구름파츠 발사각도(파츠위치2,4)
    private float height;

    public float fly_gravity = 9.8f;    // 구름파츠가 받는 중력가속도
    private float flying_time = 0f;     // 구름파츠가 이동한 시간


    private Vector2 pos2 = new Vector2(0, 0);

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        float x = fly_speed * Mathf.Cos(fly_angle * Mathf.Deg2Rad);
        float y = fly_speed * Mathf.Sin(fly_angle * Mathf.Deg2Rad);
        height = y * y * y / (2f * fly_gravity);



        // 구름조각이 받는 중력가속도 설정
        Rigidbody.gravityScale = fly_gravity / Physics2D.gravity.magnitude;
    }

    void FixedUpdate()
    {
        flying_time += Time.fixedDeltaTime;

        // 구름파츠가 이동하는동안 중력에의해 y축으로 이동(2,4번파츠)
        float x1 = fly_speed * Mathf.Cos(fly_angle * Mathf.Deg2Rad);
        float y1 = height - (0.5f * fly_gravity * flying_time * flying_time);
        // 구름파츠의 새로운위치 계산
        Vector2 pos1 = Rigidbody.position + new Vector2(x1, y1) * Time.fixedDeltaTime;
        Rigidbody.MovePosition(pos1);
    }
}