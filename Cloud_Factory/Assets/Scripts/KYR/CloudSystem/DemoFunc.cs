using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 스크립트 작동 확인을 위한 이벤트 함수를 임시로 만든 스크립트입니다.
 */ 
public class DemoFunc : MonoBehaviour
{
    [SerializeField]
    CloudMakeSystem Cloudmakesystem;

    void Start()
    {
        //Load cloudSyetem with Finding obj tag
        Cloudmakesystem = GameObject.FindWithTag("CloudSystem").GetComponent<CloudMakeSystem>();
    }

    public void makeCloudBtn() //btn of cloudmaker clicked
    {
        Cloudmakesystem.E_createCloud(EventSystem.current.currentSelectedGameObject.name);
    }

    public void go2HealingMemory()
    {

    }

}
