using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 ��ũ��Ʈ �۵� Ȯ���� ���� �̺�Ʈ �Լ��� �ӽ÷� ���� ��ũ��Ʈ�Դϴ�.
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
