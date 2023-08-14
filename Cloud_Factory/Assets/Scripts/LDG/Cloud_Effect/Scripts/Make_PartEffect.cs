using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Make_PartEffect : MonoBehaviour
{
    public GameObject Parts_FadeEffect;
    private GameObject New_FadeEffect;

    public GameObject Parts_TornadoEffect;
    private GameObject New_TornadoEffect;

    public GameObject Parts_BummerEffect;
    private GameObject New_BummerEffect;

    public GameObject Parts_HeartBeatEffect;
    private GameObject New_HeartBeatEffect;

    private float destroy_time;

    void Start()
    {
        //InvokeRepeating("Make_FadeEffect_Cloud", 1.0f, 5.0f);
        //InvokeRepeating("Make_TornadoEffect_Cloud", 1.5f, 5.0f);
        //InvokeRepeating("Make_BummerEffect_Cloud", 1.5f, 5.0f);
        //InvokeRepeating("Make_HeartBeatEffect_Cloud", 2.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        destroy_time += Time.deltaTime;
        if (destroy_time >= 5.0f)
        {
            //Destroy(New_FadeEffect);
            //Destroy(New_TornadoEffect);
            //Destroy(New_BummerEffect);
            //Destroy(New_HeartBeatEffect);
            destroy_time = 0.0f;
        }
    }

    void Make_FadeEffect_Cloud()
    {
        New_FadeEffect = Instantiate(Parts_FadeEffect);
    }

    void Make_TornadoEffect_Cloud()
    {
        New_TornadoEffect = Instantiate(Parts_TornadoEffect);
    }

    void Make_BummerEffect_Cloud()
    {
        New_BummerEffect = Instantiate(Parts_BummerEffect);
    }

    void Make_HeartBeatEffect_Cloud()
    {
        New_HeartBeatEffect = Instantiate(Parts_HeartBeatEffect);
    }
}
