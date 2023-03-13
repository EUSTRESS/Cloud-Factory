using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Parts_fly : MonoBehaviour
{
    // 왼쪽위 1번위치와 왼쪽밑 3번위치에서 나오는 구름파츠의 속도방향결정하는 스크립트
    Rigidbody2D Rigidbody;            // 구름파츠의 rigidbody 컴포넌트

    private float fly_speed_2 = -5.0f;        // 구름파츠 실질적인 이동속도
    private float fly_speed= 5.0f;          // 구름파츠 초기이동속도
    private float fly_angle = 45.0f;       // 구름파츠 발사각도(파츠위치2,4)
    private float height;               // 구름파츠의 최고높이

    public float fly_gravity = 9.8f;    // 구름파츠가 받는 중력가속도
    private float flying_time = 0f;     // 구름파츠가 이동한 시간


    private Vector2 pos1 = new Vector2(0, 0);

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();

        float x = fly_speed_2 * Mathf.Cos(fly_angle * Mathf.Deg2Rad);
        float y = fly_speed * Mathf.Sin(fly_angle * Mathf.Deg2Rad);
        height = y * y * y / (2f * fly_gravity);

        // 구름조각이 받는 중력가속도 설정
        Rigidbody.gravityScale = fly_gravity / Physics2D.gravity.magnitude;
    }

    void FixedUpdate()
    {
        flying_time += Time.fixedDeltaTime;

        // 구름파츠가 이동하는동안 중력에의해 y축으로 이동(1,3,번파츠)
        float x2 = fly_speed_2 * Mathf.Cos(fly_angle * Mathf.Deg2Rad);
        float y2 = height - (0.5f * fly_gravity * flying_time * flying_time);
        // 구름파츠의 새로운위치 계산
        Vector2 pos2 = Rigidbody.position + new Vector2(x2, y2) * Time.fixedDeltaTime;
        Rigidbody.MovePosition(pos2);
    }
}
