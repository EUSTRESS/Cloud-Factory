using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts_fly : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public float fly_speed;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.position * fly_speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
