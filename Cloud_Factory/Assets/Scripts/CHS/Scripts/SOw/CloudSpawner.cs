using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    }

    // 구름 생성
    public void SpawnCloud(int guestNum, StoragedCloudData storagedCloudData)
    {
        // 구름 인스턴스 생성
        tempCLoud = Instantiate(CloudObject);
        tempCLoud.transform.position = this.transform.position;

        // 목표 의자 위치 설정
        tempCLoud.GetComponent<CloudObject>().SetTargetChair(guestNum);

        // 임시로 인벤토리에 들어있는 구름 중, 맨 앞에 있는 구름의 값을 가져온다.
        CloudData = storagedCloudData;

        tempCLoud.GetComponent<CloudObject>().SetValue(CloudData.mFinalEmotions);
        tempCLoud.GetComponent<CloudObject>().SetGuestNum(guestNum);



        tempCLoud.GetComponent<CloudObject>().SetSprite(ConvertTextureWithAlpha(CloudData.mTexImage));

        tempCLoud.transform.localScale = new Vector3(0.11f, 0.12f, 0.5f);
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
            //Debug.Log("IMAGE LocalScale" + mspriteToMerge[i].texture.width + "///" + mspriteToMerge[i].texture.height);
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
