using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudFac_UI_Handler : MonoBehaviour
{
    //setActive(false)되어있는 GameObject이기 떄문에, public으로 외부에서 객체를 넣어준다.
    public GameObject CloudSystem;

    // Start is called before the first frame update
    [SerializeField]
    private GameObject Sys_CloudContainer;//index = 0
    [SerializeField]
    private GameObject Sys_CloudCreater;//index = 1
    void Start()
    {
        Sys_CloudContainer = CloudSystem.transform.GetChild(0).gameObject;
        Sys_CloudCreater = CloudSystem.transform.GetChild(1).gameObject;
    }

    public void Btn_move2Creater()
    {
        Sys_CloudCreater.SetActive(true);
    }

    public void Btn_move2Container()
    {
        Sys_CloudContainer.SetActive(true);
    }

    public void Btn_mCreater2Main()
    {
        Sys_CloudCreater.SetActive(false);
    }

    public void Btn_mContainer2Main()
    {
        Sys_CloudContainer.SetActive(false);
    }
}
