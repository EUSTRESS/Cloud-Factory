using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    SOWManager SOWManager;
    InventoryManager InventoryManager;
    StoragedCloudData CloudData;

    bool isCloudGive;

    public GameObject CloudObject;

    public int cloudSpeed;

    private GameObject tempCLoud;


    // 처음 받아와야 하는 값
    // 1) 날아갈 의자의 인덱스
    // 2) 어떤 구름을 생성하는지에 대한 값

    // 내부에서 수행해야할 기능
    // 1) 구름 생성
    // 2) 구름 정보 초기화
    // 3) 구름을 지정된 의자로 보내기

    private void Awake()
    {
        isCloudGive = false;
        cloudSpeed = 3;
        SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        InventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isCloudGive = true;
            Debug.Log("p");
        }

        // 구름 보관함에서 값이 넘어오면 구름을 생성
        if (isCloudGive)
        {
            SpawnCloud();
            //MoveCloud();

            isCloudGive = false;
        }
    }

    // 구름 생성
    public void SpawnCloud()
    {
        // 구름 인스턴스 생성
        tempCLoud = Instantiate(CloudObject);
        tempCLoud.transform.position = this.transform.position;

        // 목표 의자 위치 설정
        tempCLoud.GetComponent<CloudObject>().SetTargetChair();

        // 임시로 인벤토리에 들어있는 구름 중, 맨 앞에 있는 구름의 값을 가져온다.
        CloudData = InventoryManager.mCloudStorageData.mDatas[0];

        tempCLoud.GetComponent<CloudObject>().SetValue(CloudData.mFinalEmotions);


    }


    // 구름 이동
    public void MoveCloud()
    {
        Transform t_target = tempCLoud.GetComponent<CloudObject>().targetChairPos;

        tempCLoud.transform.position = Vector2.MoveTowards(transform.position, t_target.position, cloudSpeed * Time.deltaTime);
    }


}
