using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudSpawner : MonoBehaviour
{
    SOWManager          SOWManager;         
    InventoryManager    InventoryManager;
    public StoragedCloudData   CloudData;

    GameObject cloudMove;

    bool                isCloudGive;        // 창고에서 구름을 제공하였는가

    public int          cloudSpeed;         // 구름이 이동하는 속도

    public Vector3 Cloud_ps;

    public RuntimeAnimatorController[] animValue2;
    public RuntimeAnimatorController[] animValue3;
    public RuntimeAnimatorController[] animValue4;


    public GameObject EffectCloudObj;   // 새로운 구름 오브젝트 프리팹

    Make_PartEffect make_PartEffect;

    public GameObject newTempCloud;

    public bool IsUsing;                   // 구름을 사용중인지 체크

    GameObject MainEffectCloudMove;

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

    void Start()
    {
        IsUsing = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 구름을 생성하고 초기화한다.
    public void SpawnCloud(int guestNum, StoragedCloudData storagedCloudData /*QA용*/, int sat)
    {
        // 구름 인스턴스 생성
        newTempCloud = Instantiate(EffectCloudObj);
        newTempCloud.transform.GetChild(0).gameObject.SetActive(true);

        make_PartEffect = newTempCloud.GetComponent<Make_PartEffect>();

        SOWManager SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        if(SOWManager != null)
        {
            SOWManager.mCloudObjectList.Add(newTempCloud);
        }

#if UNITY_EDITOR
        Debug.Log("Instantiate");
#endif
        newTempCloud.transform.position = this.transform.position;
        Cloud_ps = newTempCloud.transform.position;

        // 목표 의자 위치 설정
        newTempCloud.GetComponent<CloudObject>().SetTargetChair(guestNum);
        
#if UNITY_EDITOR
        Debug.Log("SetTargetChair");
#endif

        // 구름을 제공받는 손님의 isGettingCloud 상태를 갱신한다.
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Guest");
            if (gameObjects != null)
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    if(gameObject == null)
                    {
                        continue;
                    }

                    GuestObject guestObject = gameObject.GetComponent<GuestObject>();
                    if (guestObject != null && guestObject.mGuestNum == guestNum)
                    {
                        bool isGettingCloud = gameObject.GetComponent<GuestObject>().isGettingCloud;
                        gameObject.GetComponent<GuestObject>().isGettingCloud = true;
                    }
                }
            }
        }

        // 임시로 인벤토리에 들어있는 구름 중, 맨 앞에 있는 구름의 값을 가져온다.
        CloudData = storagedCloudData;

        CloudObject cloudObject = newTempCloud.GetComponent<CloudObject>(); 
        if (cloudObject != null)
        {
            cloudObject.SetValue(CloudData);
            cloudObject.SetGuestNum(guestNum);

            // 구름과 의자의 위치값에 따라서 속도를 조절한다.
            cloudObject.SetSpeed();

            // QA용
            cloudObject.sat = sat;
        }

        // 움직이는 구름의 이펙트를 나타내는 MaincloudMove에 대한 설정
        MainEffectCloudMove = newTempCloud.transform.GetChild(0).gameObject;

        // MoveCloud 관련
        {
            Make_PartEffect make_PartEffect = MainEffectCloudMove.GetComponent<Make_PartEffect>();
            if(make_PartEffect == null)
            {
                return;
            }

            // TODO : MoveCloud Animator를 종류에 맞게 변경 -> CloudData값을 이용
            // 1. 구름 색상을 지정 (적용 완료)
            // 2. 구름 재료 등급에 따른 애니메이터 나누기

            int cloudColorNumber = storagedCloudData.GetCloudTypeNum();

            // TODO : 희귀도에 따라 외형을 변화시키는 코드 추가
            int IngredientDataNum = storagedCloudData.GetIngredientDataNum();

#if UNITY_EDITOR
            Debug.Log("구름에 사용된 파츠 개수 : " + IngredientDataNum);
#endif

            // Prefab수정및 애니메이션 수정한 부분입니다 - 동규 -
            if (IngredientDataNum <= 2)
            {
                //cloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue3[cloudColorNumber];
                MainEffectCloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue3[cloudColorNumber];
            }
            else if (IngredientDataNum == 3)
            {
                //cloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue2[cloudColorNumber];
                MainEffectCloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue2[cloudColorNumber];
            }
            else
            {
                //cloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue4[cloudColorNumber];
                MainEffectCloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue4[cloudColorNumber];
            }

            //if(cloudMove.GetComponent<Animator>().runtimeAnimatorController)
            //{

            //}
            if (MainEffectCloudMove.GetComponent<Animator>().runtimeAnimatorController)
            {

            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("구름 애니메이터 적용 실패 오류발생! ");
#endif

                if(animValue3[cloudColorNumber])
                {
#if UNITY_EDITOR
                    Debug.Log("구름 애니메이터가 존재하지 않습니다.");
#endif
                }
            }
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
        Transform New_Target = newTempCloud.GetComponent<CloudObject>().targetChairPos;
        newTempCloud.transform.position = Vector2.MoveTowards(transform.position, New_Target.position, cloudSpeed * Time.deltaTime);
    }
}
