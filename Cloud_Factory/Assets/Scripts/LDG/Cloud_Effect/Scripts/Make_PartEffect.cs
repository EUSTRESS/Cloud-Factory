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

    public GameObject Parts_BounceEffect;
    private GameObject New_BounceEffect;

    public GameObject Parts_TwinkleEffect;
    private GameObject New_TwinkleEffect;

    public GameObject Parts_DiagonalEffect;
    private GameObject New_DiagonalEffect;

    public GameObject Parts_FogEffect;
    private GameObject New_FogEffect;

    private float destroy_time;

    void Start()
    {
        //InvokeRepeating("Make_FadeEffect_Cloud", 0.0f, 5.0f);
        //InvokeRepeating("Make_TornadoEffect_Cloud", 0.0f, 5.0f);
        //InvokeRepeating("Make_BummerEffect_Cloud", 0.0f, 5.0f);
        //InvokeRepeating("Make_HeartBeatEffect_Cloud", 0.0f, 5.0f);
        //InvokeRepeating("Make_BounceEffect_Cloud", 0.0f, 5.0f);
        //InvokeRepeating("Make_TwinkleEffect_Cloud", 0.0f, 5.0f);
        //InvokeRepeating("Make_DiagonalEffect_Cloud", 0.0f, 5.0f);
        InvokeRepeating("Make_FogEffect_Cloud", 0.0f, 5.0f);
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
            //Destroy(New_BounceEffect);
            //Destroy(New_TwinkleEffect);
            //Destroy(New_DiagonalEffect);
            Destroy(New_FogEffect);
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

    void Make_BounceEffect_Cloud()
    {
        New_BounceEffect = Instantiate(Parts_BounceEffect);
    }
    void Make_TwinkleEffect_Cloud()
    {
        New_TwinkleEffect = Instantiate(Parts_TwinkleEffect);
    }

    void Make_DiagonalEffect_Cloud()
    {
        New_DiagonalEffect = Instantiate(Parts_DiagonalEffect);
    }

    void Make_FogEffect_Cloud()
    {
        New_FogEffect = Instantiate(Parts_FogEffect);
    }
}
