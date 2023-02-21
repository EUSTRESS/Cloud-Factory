using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudSpawner : MonoBehaviour
{
    SOWManager          SOWManager;         
    InventoryManager    InventoryManager;
    StoragedCloudData   CloudData;

    GameObject cloudMove;

    bool                isCloudGive;        // 창고에서 구름을 제공하였는가
        
    public GameObject   CloudObject;        // 구름 오브젝트 프리팹

    public int          cloudSpeed;         // 구름이 이동하는 속도

    private GameObject  tempCLoud;          // 구름 제공 전 정보값을 채우기 위한 Temp 오브젝트

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

    }

    // 구름을 생성하고 초기화한다.
    public void SpawnCloud(int guestNum, StoragedCloudData storagedCloudData)
    {
        // 구름 인스턴스 생성
        tempCLoud = Instantiate(CloudObject);
        tempCLoud.transform.position = this.transform.position;

        // 목표 의자 위치 설정
        tempCLoud.GetComponent<CloudObject>().SetTargetChair(guestNum);

        // 임시로 인벤토리에 들어있는 구름 중, 맨 앞에 있는 구름의 값을 가져온다.
        CloudData = storagedCloudData;

        tempCLoud.GetComponent<CloudObject>().SetValue(CloudData);
        tempCLoud.GetComponent<CloudObject>().SetGuestNum(guestNum);

        // 구름과 의자의 위치값에 따라서 속도를 조절한다.
        tempCLoud.GetComponent<CloudObject>().SetSpeed();

        // 움직이는 구름의 이펙트를 나타내는 cloudMove에 대한 설정
        cloudMove = tempCLoud.transform.GetChild(0).gameObject;

        // MoveCloud 관련
        {
            Cloud_movement movement = cloudMove.GetComponent<Cloud_movement>();

            // image
            cloudMove.GetComponent<SpriteRenderer>().sprite = storagedCloudData.mVBase.mImage;

            for (int i = 0; i < storagedCloudData.mVPartsList.Count; i++)           // 파츠이미지 스프라이트로 저장
            {
                movement.Parts_fly.GetComponent<SpriteRenderer>().sprite = storagedCloudData.mVPartsList[i].mImage;
                movement.Parts_fly_2.GetComponent<SpriteRenderer>().sprite = storagedCloudData.mVPartsList[i].mImage;
            }

            // scale
            //cloudMove.transform.localScale = new Vector3(0.11f, 0.12f, 0.5f);
            movement.Parts_fly.transform.localScale = new Vector3(0.15f, 0.15f, 0.5f);
            movement.Parts_fly_2.transform.localScale = new Vector3(0.15f, 0.15f, 0.5f);
        }
    }

    private Sprite ConvertTextureWithAlpha(Texture2D target)
    {
        Texture2D newText = new Texture2D(target.width, target.height, TextureFormat.RGBA32, false);

        for (int x = 0; x < newText.width; x++)
        {
            for (int y = 0; y < newText.height; y++)
            {
                newText.SetPixel(x, y, new Color(1, 1, 1, 0));
            }
        }

        for (int x = 0; x < target.width; x++)
        {
            for (int y = 0; y < target.height; y++)
            {
                var color = target.GetPixel(x, y);
                if (target.GetPixel(x, y).a == 1 && target.GetPixel(x, y).g == 1 && target.GetPixel(x, y).b == 1)
                {
                    color.a = 0;
                }

                newText.SetPixel(x, y, color);
            }
        }
        newText.Apply();

        Sprite sprite = Sprite.Create(newText, new Rect(0, 0, newText.width, newText.height), new Vector2(0.5f, 0.5f));

        return sprite;
    }
    // 구름 이동
    public void MoveCloud()
    {
        Transform t_target = tempCLoud.GetComponent<CloudObject>().targetChairPos;
        tempCLoud.transform.position = Vector2.MoveTowards(transform.position, t_target.position, cloudSpeed * Time.deltaTime);
    }


}
