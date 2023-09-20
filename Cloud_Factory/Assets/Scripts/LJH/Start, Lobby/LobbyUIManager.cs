using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using AESWithJava.Con;
using System;

[System.Serializable]
public class SoundData
{
    //public bool isFirstPlay;
    public float mSaveBGM;
    public float mSaveSFx;
}

[System.Serializable]
public class LanguageData
{
    public bool isKorean;
}

[System.Serializable]
public class InitData
{
    public bool isFirstPlay;    
}

[System.Serializable]
public class FirstSatMoongtiData
{
    const int MAX_NUM = 20;
    public bool[] isFirstSatMoongti = new bool[MAX_NUM];
}



// 로비 씬 UI 담당
// 설정 창, 새로하기, 이어하기
public class LobbyUIManager : MonoBehaviour
{
    private SeasonDateCalc mSeason; // 계절 스크립트
    private InventoryManager mInvenManager;
	private LanguageChanger mLanguageChanger;

    [Header("GAME OBJECT")]
    // 오브젝트 Active 관리
    public GameObject   gOption;     // 옵션 게임 오브젝트
    public GameObject   gWarning;    // 새로운 게임 경고창
    public GameObject   gContinueWarning;    // 이어하기 경고창
    public GameObject   gMainBackGround;  // 타이틀 배경화면
    public GameObject   gMainLogo;  // 타이틀 로고

    // INDEX -> [0]: C04 [1]: C07 [2]: C10 [3]:C13 [4]:C14 // 봄 타이틀 뭉티 관리
    public GameObject[] gMoongti = new GameObject[20]; // 전체 뭉티 타이틀 스프라이트 관리
    //public int[]


    [Header("TEXT")]
    public Text         tBgmValue;   // BGM 볼륨 텍스트
    public Text         tSfxValue;   // SFx 볼륨 텍스트

    [Header("SLIDER")]
    public Slider       sBGM;        // BGM 슬라이더
    public Slider       sSFx;        // SFx 슬라이더

    private AudioSource mSFx;        // 효과음 오디오 소스

    [Header("BOOL")]
    public bool[] bSpringMoongti = new bool[5]; // 봄 타이틀 뭉티 Bool로 만족도 5 관리
    public bool isFirstPlay = true; // 처음하는 지 이어하는 지.
    public bool isCreateData = false; // 저장한 데이터가 있는 지 없는 지
    private bool gOptionOn; // 옵션창 켜졌는지 확인하는 bool변수

    [Header("IMAGE")]
    public Image iNewGame;
    public Image iContiueGame;

    [Header("SPRITES")]
    private Sprite sHoveringNew;
    private Sprite sUnHoveringNew;
    private Sprite sHoveringCon;
    private Sprite sUnHoveringCon;

    public Sprite[] sHoverNewLan = new Sprite[8];
    public Sprite[] sUnHoverNewLan = new Sprite[8];
    public Sprite[] sHoverConLan = new Sprite[8];
    public Sprite[] sUnHoverConLan = new Sprite[8];

    public Sprite[] sSeasonBackGround = new Sprite[4];
    public Sprite[] sSeasonLogo = new Sprite[4];

    public GameObject[] sLanguageCheckSprites = new GameObject[2];

    public RectTransform[] gNewConTransform = new RectTransform[2];

    private bool[] bFirstSatMoongti = new bool[20 /*NUM_OF_GUEST*/];

    void Awake()
    {
        mSFx = GameObject.Find("mSFx").GetComponent<AudioSource>();
        mSeason = GameObject.Find("Season Date Calc").GetComponent<SeasonDateCalc>();
        mInvenManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        mLanguageChanger = FindObjectOfType<LanguageChanger>();


        isFirstPlay = true; // 처음 플레이한다고 넣어두고

        // 데이터 폴더가 없다면 생성하기
        if (!File.Exists(Application.dataPath + "/Data/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Data/");
        }

        // 여기서 최초 1회 플레이인지 아닌지 판단.
        Load_InitData();
        // 사운드 로드 및 저장한 이력이 있는 지 판단.
        Load_SoundData();


        Load_SeasonDate(); // 이 때 계절의 정보를 토대로, 만족도 5 뭉티와, 타이틀 화면 변경.
        Load_GuestSatisfaction(); // 만족도 5 뭉티 판별

        Load_LanguageData();

        ChangeLobbyLanguage();
    }

    void Start()
    {

    }

    void Update()
    {
        // 현재 음량으로 업데이트
        if (sBGM && sSFx) // null check
        {
            sBGM.value = SceneData.Instance.BGMValue;
            sSFx.value = SceneData.Instance.SFxValue;
        }
        if (tBgmValue && tSfxValue) // null check
        {
            // 소수점 -2 자리부터 반올림
            tBgmValue.text = Mathf.Ceil(sBGM.value * 100).ToString();
            tSfxValue.text = Mathf.Ceil(sSFx.value * 100).ToString();
        }

        gOptionOn = gOption.activeSelf;
        if (gOptionOn)
        {
            sSFx.onValueChanged.AddListener(ChangeCheck_SFx);
        }
    }

    private void ChangeCheck_SFx(float Change_value)
    {
        sSFx.value = Change_value;
        //mSFx.Play();
    }
    /*
     * BUTTON에 할당할 메소드
     */

    public void NewGame()
    {
        if (true == isFirstPlay)
        {
            isFirstPlay = false; // 최초 플레이 끝.
            SaveLobby_InitData(); // 최초 플레이 끝난 것 저장.
        }      

        SceneData mSceneData = GameObject.Find("SceneDataManager").GetComponent<SceneData>();
        mSceneData.mContinueGmae = false;

        mSFx.Play();

        LoadingSceneController.Instance.LoadScene("Space Of Weather");

        // 데이터를 초기화 시키는 함수 호출할 필요 없이
        // 각 클래스 생성자에서 자동 초기화된다.
        mSeason.Init_Data();
    }

    public void ContinueGame()
    {
        SceneData mSceneData = GameObject.Find("SceneDataManager").GetComponent<SceneData>();
        mSceneData.mContinueGmae = true;

        //String key = "key"; // 암호화 복호화 키 값

        mSFx.Play();

        /*
         저장된 씬 넘버 로딩         
         */

        // newtonsoft library (모노비헤이비어 상속된 클래스 사용 불가능, 딕셔너리 사용 가능)
        // 로드하는 함수 호출 후에 그 씬 인덱스로 이동
        //FileStream fSceneBuildIndexStream
        //    // 해당 경로에 있는 json 파일을 연다
        //    = new FileStream(Application.dataPath + "/Data/SceneBuildIndex.json", FileMode.Open);
        //// 열려있는 json 값들을 byte배열에 넣는다
        //byte[] bSceneData = new byte[fSceneBuildIndexStream.Length];
        //// 끝까지 읽는다
        //fSceneBuildIndexStream.Read(bSceneData, 0, bSceneData.Length);
        //fSceneBuildIndexStream.Close();
        //// 문자열로 변환한다
        //string sSceneData = Encoding.UTF8.GetString(bSceneData);
        //// 복호화
        //sSceneData = AESWithJava.Con.Program.Decrypt(sSceneData, key);

       
        // Load_Data
        //Load_SeasonDate();
        Load_Inventory();
        Load_Guest();
        Load_SOW();      
        //Load_SOWManagerData(); // 로비에 매니저가 없어서, 날씨의 공간 들어와서 로딩할것.
        Load_LetterControllerData();

        //Load_History();

        // 문자열을 int형으로 파싱해서 빌드 인덱스로 활용한다
        // 현재 빌드 인덱스가 날씨의 공간이 6이므로 6인데 이거 빌드 인덱스 바뀌면 안됨...
        LoadingSceneController.Instance.LoadScene(6); 

    }

    void Load_SeasonDate()
    {
        string mSeasonDatePath = Path.Combine(Application.dataPath + "/Data/", "SeasonDate.json");

        if (File.Exists(mSeasonDatePath)) // null check
        {
            // 새로운 오브젝트를 생성하고
            GameObject gSeasonDate = new GameObject();
            string sDateData = File.ReadAllText(mSeasonDatePath);
            // 복호화
            //sDateData = AESWithJava.Con.Program.Decrypt(sDateData, key);

#if UNITY_EDITOR
            Debug.Log(sDateData);
#endif

            // 데이터를 새로운 오브젝트에 덮어씌운다
            JsonUtility.FromJsonOverwrite(sDateData, gSeasonDate.AddComponent<SeasonDateCalc>());

            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!
            SeasonDateCalc.Instance.mSecond = gSeasonDate.GetComponent<SeasonDateCalc>().mSecond;
            SeasonDateCalc.Instance.mDay = gSeasonDate.GetComponent<SeasonDateCalc>().mDay;
            SeasonDateCalc.Instance.mSeason = gSeasonDate.GetComponent<SeasonDateCalc>().mSeason;
            SeasonDateCalc.Instance.mYear = gSeasonDate.GetComponent<SeasonDateCalc>().mYear;
        }
        
        if (null == gMainBackGround)
        {
#if UNITY_EDITOR
            Debug.Log("gMangBackGround is null");
#endif
            return;
        }
        
        // 계절별로 바꾼다.
        gMainBackGround.GetComponent<Image>().sprite = sSeasonBackGround[(SeasonDateCalc.Instance.mSeason - 1)];

        if (null == gMainLogo)
        {
#if UNITY_EDITOR
            Debug.Log("gMainLogo is null");
#endif
            return;
        }
        gMainLogo.GetComponent<Image>().sprite = sSeasonLogo[(SeasonDateCalc.Instance.mSeason - 1)];
        

    }

    void Load_Inventory()
    {
        string mInvenDataPath = Path.Combine(Application.dataPath + "/Data/", "InventoryData.json");
       
        if (File.Exists(mInvenDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 파일 스트림 개방
            FileStream stream = new FileStream(Application.dataPath + "/Data/InventoryData.json", FileMode.Open);

            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bInventoryData = new byte[stream.Length];
            // 읽어오기
            stream.Read(bInventoryData, 0, bInventoryData.Length);
            stream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jInventoryData = Encoding.UTF8.GetString(bInventoryData);
#if UNITY_EDITOR
            Debug.Log(jInventoryData);
#endif

            // 역직렬화
            InventoryData dInventoryData = JsonConvert.DeserializeObject<InventoryData>(jInventoryData);

            if (null == dInventoryData) // 저장된 데이터 없으면 리턴
                return;

            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!
            mInvenManager.mType = dInventoryData.mType.ToList();
            mInvenManager.mCnt = dInventoryData.mCnt.ToList();
            mInvenManager.minvenLevel = dInventoryData.minvenLevel;
            mInvenManager.mMaxInvenCnt = dInventoryData.mMaxInvenCnt;
            mInvenManager.mMaxStockCnt = dInventoryData.mMaxStockCnt;
        }
    }
    void Load_History()
    {
        string mHistoryDataPath = Path.Combine(Application.dataPath + "/Data/", "HistoryData.json");

        if (File.Exists(mHistoryDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 파일 스트림 개방
            FileStream stream = new FileStream(Application.dataPath + "/Data/HistoryData.json", FileMode.Open);

            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bHistoryData = new byte[stream.Length];
            // 읽어오기
            stream.Read(bHistoryData, 0, bHistoryData.Length);
            stream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jHistoryData = Encoding.UTF8.GetString(bHistoryData);
#if UNITY_EDITOR
            Debug.Log(jHistoryData);
#endif

            // 역직렬화
            HistoryData dHistoryData = JsonConvert.DeserializeObject<HistoryData>(jHistoryData);

            if (null == dHistoryData) // 저장된 데이터 없으면 리턴
                return;

            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!
            mInvenManager.ingredientHistory = dHistoryData.mIngredientHistoryDatas.ToList();
            mInvenManager.ingredientHistoryPath = dHistoryData.mIngredientHistoryPath.ToList();
            mInvenManager.cloudHistoryPath = dHistoryData.mCloudHistoryPath.ToList();

            // dHistoryData에 있는 스프라이트 경로들은 스프라이트 Load함수 따로 만들어서 mInvenManager에 Sprite에 각자 넣어줘야함.     
            //foreach(List<IngredientData> iList in mInvenManager.ingredientHistory)
            for(int i = 0; i < mInvenManager.ingredientHistory.Count; i++)
            {
                for(int j = 0; j < mInvenManager.ingredientHistory[i].Count; j++)
                {
                    foreach (KeyValuePair<string, string> TempPair in mInvenManager.ingredientHistoryPath[i])
                    {
                        mInvenManager.ingredientHistory[i][j].image = Resources.Load<Sprite>(TempPair.Value); // 스프라이트 로드하기.          
                    }
                }
            }

            for(int i = 0; i < mInvenManager.cloudHistoryPath.Count; i++)
            {
                mInvenManager.cloudHistory[i] = Resources.Load<Sprite>(mInvenManager.cloudHistoryPath[i]);
            }

        }
    }
    void Load_Guest()
    {
        string mGuestManagerDataPath = Path.Combine(Application.dataPath + "/Data/", "GuestManagerData.json");
       
        if (File.Exists(mGuestManagerDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 파일 스트림 개방
            FileStream ManagerStream = new FileStream(Application.dataPath + "/Data/GuestManagerData.json", FileMode.Open);

            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bGuestInfoData = new byte[ManagerStream.Length];
            // 읽어오기
            ManagerStream.Read(bGuestInfoData, 0, bGuestInfoData.Length);
            ManagerStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jGuestInfoData = Encoding.UTF8.GetString(bGuestInfoData);
#if UNITY_EDITOR
            Debug.Log(jGuestInfoData);
#endif

            // 역직렬화
            GuestManagerSaveData dGuestInfoData = JsonConvert.DeserializeObject<GuestManagerSaveData>(jGuestInfoData);
            if (null == dGuestInfoData) // 저장된 데이터 없으면 리턴
                return;

            // 이어하기 시, 필요한 정보값들을 불러와서 갱신한다. (GuestManager)
            Guest GuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();

            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!

            // guest manager 로딩
            /*저장할 데이터 값*/

            {
                const int NUM_OF_GUEST = 20;
                GuestInfos GuestInfos = new GuestInfos();

                for (int i = 0; i < NUM_OF_GUEST; i++)
                {
                    GuestInfoSaveData info = dGuestInfoData.GuestInfos[i];

                    if (info == null)
                    {
#if UNITY_EDITOR
                        Debug.Log("Info Null");
#endif
                    }

                    GuestManager.mGuestInfo[i].mEmotion = info.mEmotion.Clone() as int[]; ;
                    GuestManager.mGuestInfo[i].mSatatisfaction = info.mSatatisfaction;
                    GuestManager.mGuestInfo[i].mSatVariation = info.mSatVariation;
                    GuestManager.mGuestInfo[i].isChosen = info.isChosen;
                    GuestManager.mGuestInfo[i].isDisSat = info.isDisSat;
                    GuestManager.mGuestInfo[i].isCure = info.isCure;
                    GuestManager.mGuestInfo[i].mVisitCount = info.mVisitCount;
                    GuestManager.mGuestInfo[i].mNotVisitCount = info.mNotVisitCount;
                    GuestManager.mGuestInfo[i].mSitChairIndex = info.mSitChairIndex;
                    GuestManager.mGuestInfo[i].isUsing = info.isUsing;
                }
                //mGuestManagerData.GuestInfos = mGuestManager.mGuestInfo.Clone() as GuestInfoSaveData[];
            }


            GuestManager.isGuestInLivingRoom =  /*불러오는 데이터 값*/dGuestInfoData.isGuestLivingRoom;
            GuestManager.isTimeToTakeGuest = dGuestInfoData.isTimeToTakeGuest;
            GuestManager.mGuestIndex = dGuestInfoData.mGuestIndex;
            GuestManager.mTodayGuestList = dGuestInfoData.mTodayGuestList.Clone() as int[];
            GuestManager.mGuestCount = dGuestInfoData.mGuestCount;
            GuestManager.mGuestTime = dGuestInfoData.mGuestTime;
        }
    }
    void Load_GuestSatisfaction()
    {
        string mGuestManagerDataPath = Path.Combine(Application.dataPath + "/Data/", "GuestManagerData.json");
        
        if (File.Exists(mGuestManagerDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 파일 스트림 개방
            FileStream ManagerStream = new FileStream(Application.dataPath + "/Data/GuestManagerData.json", FileMode.Open);

            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bGuestInfoData = new byte[ManagerStream.Length];
            // 읽어오기
            ManagerStream.Read(bGuestInfoData, 0, bGuestInfoData.Length);
            ManagerStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jGuestInfoData = Encoding.UTF8.GetString(bGuestInfoData);
#if UNITY_EDITOR
            Debug.Log(jGuestInfoData);
#endif

            // 역직렬화
            GuestManagerSaveData dGuestInfoData = JsonConvert.DeserializeObject<GuestManagerSaveData>(jGuestInfoData);
            if (null == dGuestInfoData) // 저장된 데이터 없으면 리턴
                return;
            // 이어하기 시, 필요한 정보값들을 불러와서 갱신한다. (GuestManager)


            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!

            // 1회 이상 만족도 5를 채운 뭉티인지 체크하기 위해, Load한다.
            Load_FirstSatMoongtiData();

            // guest manager 로딩
            /*저장할 데이터 값*/

            {
                const int NUM_OF_GUEST = 20;
                GuestInfos GuestInfos = new GuestInfos();

                for (int i = 0; i < NUM_OF_GUEST; i++)
                {
                    switch (SeasonDateCalc.Instance.mSeason)
                    {
                        case 1: // 봄
                            if (3 != i && 6 != i && 9 != i && 12 != i && 13 != i)
                            {
                                gMoongti[i].SetActive(false);
                                continue;
                            }
                            break;
                        case 2: // 여름
                            if (0 != i && 1 != i && 8 != i && 18 != i && 19 != i)
                            {
                                gMoongti[i].SetActive(false);
                                continue;
                            }
                            break;
                        case 3: // 가을
                            if (2 != i && 5 != i && 10 != i && 14 != i && 17 != i)
                            {
                                gMoongti[i].SetActive(false);
                                continue;
                            }
                            break;
                        case 4: // 겨울
                            if (4 != i && 7 != i && 11 != i && 15 != i && 16 != i)
                            {
                                gMoongti[i].SetActive(false);
                                continue;
                            }
                            break;
                        default:
                            break;
                    }

                    GuestInfoSaveData info = dGuestInfoData.GuestInfos[i];
                    if (info == null)
                    {
#if UNITY_EDITOR
                        Debug.Log("Info Null");
#endif
                    }

                    // TEST CODE
                    //info.mSatatisfaction = 5;                    

                    // 게임 플레이에서 1회 이상, 만족도 5가 채워진 뭉티인 경우 현재 만족도에 상관없이 띄워준다.
                    if (true == bFirstSatMoongti[i])
                    {
                        gMoongti[i].SetActive(true);
                        continue;
                    }

                    if (5 == info.mSatatisfaction) // 만족도가 5인 뭉티
                    {     
                        // 뭉티 나오는 거는 바뀜 예정이니 출력안함.
                        gMoongti[i].SetActive(true);

                        // 아직 최초로 만족도 5가 채워지지 않았다면. 채워준다.
                        if (false == bFirstSatMoongti[i])
                        {
                            bFirstSatMoongti[i] = true;                            
                        }
                    }
                }
                //mGuestManagerData.GuestInfos = mGuestManager.mGuestInfo.Clone() as GuestInfoSaveData[];

                // 위의 반복문에서 갱신된 데이터를 Json 파일에 저장해놓는다.
                Save_FirstSatMoongtiData();

            }
        }
    }

    void Load_FirstSatMoongtiData()
    {
        string mInitDataPath = Path.Combine(Application.dataPath + "/Data/", "FirstSatMoongtiData.json");

        if (File.Exists(mInitDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 파일 스트림 개방
            FileStream FirstSatDataStream = new FileStream(Application.dataPath + "/Data/FirstSatMoongtiData.json", FileMode.Open);

            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bFirstSatData = new byte[FirstSatDataStream.Length];
            // 읽어오기
            FirstSatDataStream.Read(bFirstSatData, 0, bFirstSatData.Length);
            FirstSatDataStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jFirstSatData = Encoding.UTF8.GetString(bFirstSatData);
#if UNITY_EDITOR
            Debug.Log(jFirstSatData);
#endif

            // 역직렬화
            FirstSatMoongtiData dFristSatData = JsonConvert.DeserializeObject<FirstSatMoongtiData>(jFirstSatData);
            if (null == dFristSatData) // 저장된 데이터 없으면 리턴
                return;
            // 데이터 직렬화
            string jData = JsonConvert.SerializeObject(dFristSatData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
#if UNITY_EDITOR
            //Debug.Log("=======Load : dSOWSaveData =========");
            //Debug.Log(jData);
            //Debug.Log("=======Load=========");
#endif


            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!
            const int MAX_NUM = 20;
            for(int i = 0; i < MAX_NUM; ++i)
            {
                bFirstSatMoongti[i] = dFristSatData.isFirstSatMoongti[i];
            }
            //isFirstPlay = dInitData.isFirstPlay;
        }
    }
    void Save_FirstSatMoongtiData()
    {
        // 파일이 있다면
        if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "FirstSatMoongtiData.json")))
        {
            // 삭제
            System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "FirstSatMoongtiData.json"));

        }

        FileStream stream = new FileStream(Application.dataPath + "/Data/FirstSatMoongtiData.json", FileMode.OpenOrCreate);

        // 저장할 변수가 담긴 클래스 생성
        FirstSatMoongtiData mFirstSatData = new FirstSatMoongtiData();

        // 데이터 업데이트
        const int NUM_MAX = 20;
        for (int i = 0; i < NUM_MAX; i++) 
        {
            mFirstSatData.isFirstSatMoongti[i] = bFirstSatMoongti[i];
        }       

        // 데이터 직렬화
        string jFirstSatData = JsonConvert.SerializeObject(mFirstSatData);

        // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
        byte[] bFirstSatData = Encoding.UTF8.GetBytes(jFirstSatData);
#if UNITY_EDITOR
        Debug.Log(jFirstSatData);
#endif
        // 해당 파일 스트림에 적는다.                
        stream.Write(bFirstSatData, 0, bFirstSatData.Length);
        // 스트림 닫기
        stream.Close();
    }

    void Load_SOW()
    {
        string mSOWSaveDataPath = Path.Combine(Application.dataPath + "/Data/", "SOWSaveData.json");
        
        if (File.Exists(mSOWSaveDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 파일 스트림 개방
            FileStream SOWSaveStream = new FileStream(Application.dataPath + "/Data/SOWSaveData.json", FileMode.Open);

            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bSOWSaveData = new byte[SOWSaveStream.Length];
            // 읽어오기
            SOWSaveStream.Read(bSOWSaveData, 0, bSOWSaveData.Length);
            SOWSaveStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jSOWSaveData = Encoding.UTF8.GetString(bSOWSaveData);
#if UNITY_EDITOR
            Debug.Log(jSOWSaveData);
#endif

            // 역직렬화
            SOWSaveData dSOWSaveData = JsonConvert.DeserializeObject<SOWSaveData>(jSOWSaveData);
            if (null == dSOWSaveData) // 저장된 데이터 없으면 리턴
                return;
            // 데이터 직렬화
            string jData = JsonConvert.SerializeObject(dSOWSaveData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
#if UNITY_EDITOR
            //Debug.Log("=======Load : dSOWSaveData =========");
            //Debug.Log(jData);
            //Debug.Log("=======Load=========");
#endif

            // 이어하기 시, 필요한 정보값들을 불러와서 갱신한다. (GuestManager)
            Guest GuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();

            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!

            SOWSaveData sowInfo = new SOWSaveData();
            {
                sowInfo.UsingObjectsData = dSOWSaveData.UsingObjectsData.ToList();
                sowInfo.WaitObjectsData = dSOWSaveData.WaitObjectsData.ToList();
                sowInfo.mMaxChairNum = dSOWSaveData.mMaxChairNum;
                sowInfo.mCheckChairEmpty = new Dictionary<int, bool>(dSOWSaveData.mCheckChairEmpty);
            }

            //string jAData = JsonConvert.SerializeObject(sowInfo);
#if UNITY_EDITOR
            //Debug.Log("=======Load : sowInfo =========");
            //Debug.Log(jAData);
            //Debug.Log("=======Load=========");
#endif

            GuestManager.SaveSOWdatas = sowInfo;
            GuestManager.isLoad = true;

            //string jBData = JsonConvert.SerializeObject(GuestManager.SaveSOWdatas);
#if UNITY_EDITOR
            //Debug.Log("=======Load :  GuestManager.SaveSOWdatas  =========");
            //Debug.Log(jBData);
            //Debug.Log("=======Load=========");
#endif

        }
    }
    void Load_Tutorial()
    {
        string mTutorialSaveDataPath = Path.Combine(Application.dataPath + "/Data/", "TutorialData.json");
       
        if (File.Exists(mTutorialSaveDataPath)) // 해당 파일이 생성되었으면 불러오기
        { 
            // 파일 스트림 개방
            FileStream TutorialSaveStream = new FileStream(Application.dataPath + "/Data/TutorialData.json", FileMode.Open);

            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bTutorialSaveData = new byte[TutorialSaveStream.Length];
            // 읽어오기
            TutorialSaveStream.Read(bTutorialSaveData, 0, bTutorialSaveData.Length);
            TutorialSaveStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jTutorialSaveData = Encoding.UTF8.GetString(bTutorialSaveData);
#if UNITY_EDITOR
            Debug.Log(jTutorialSaveData);
#endif

            // 역직렬화
            TutorialData dTutorialSaveData = JsonConvert.DeserializeObject<TutorialData>(jTutorialSaveData);
            if (null == dTutorialSaveData) // 저장된 데이터 없으면 리턴
                return;
            // 데이터 직렬화
            //string jData = JsonConvert.SerializeObject(dTutorialSaveData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
#if UNITY_EDITOR
            //Debug.Log("=======Load : dSOWSaveData =========");
            //Debug.Log(jData);
            //Debug.Log("=======Load=========");
#endif

            // 이어하기 시, 필요한 정보값들을 불러와서 갱신한다. (GuestManager)
            TutorialManager tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();

            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!

            tutorialManager.isTutorial = dTutorialSaveData.isTutorial;
            tutorialManager.ChangeAllTutorialStatus();
            isCreateData = !tutorialManager.isTutorial; // 튜토리얼 유무로 저장했는 지 판별.
        }
    }

    void Load_SOWManagerData()
    {
        string mSowManagerSaveDataPath = Path.Combine(Application.dataPath + "/Data/", "SOWManagerData.json");
       
        if (File.Exists(mSowManagerSaveDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 파일 스트림 개방
            FileStream SOWmanageSaveStream = new FileStream(Application.dataPath + "/Data/SOWManagerData.json", FileMode.Open);

            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bSOWManagerSaveData = new byte[SOWmanageSaveStream.Length];
            // 읽어오기
            SOWmanageSaveStream.Read(bSOWManagerSaveData, 0, bSOWManagerSaveData.Length);
            SOWmanageSaveStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jSOWManagerSaveData = Encoding.UTF8.GetString(bSOWManagerSaveData);
#if UNITY_EDITOR
            Debug.Log(jSOWManagerSaveData);
#endif

            // 역직렬화
            SOWManagerSaveData dSOWManagerSaveData = JsonConvert.DeserializeObject<SOWManagerSaveData>(jSOWManagerSaveData);
            if (null == dSOWManagerSaveData) // 저장된 데이터 없으면 리턴
                return;
            // 데이터 직렬화
            //string jData = JsonConvert.SerializeObject(dSOWManagerSaveData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
#if UNITY_EDITOR
            //Debug.Log("=======Load : dSOWSaveData =========");
            //Debug.Log(jData);
            //Debug.Log("=======Load=========");
#endif

            // 이어하기 시, 필요한 정보값들을 불러와서 갱신한다. (GuestManager)
            SOWManager mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!

            mSOWManager.yardGatherCount = dSOWManagerSaveData.yardGatherCount.Clone() as int[];

        }
    }


    void Load_LetterControllerData()
    {
        string mLetterControllerDataPath = Path.Combine(Application.dataPath + "/Data/", "LetterControllerData.json");
        
        if (File.Exists(mLetterControllerDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 파일 스트림 개방
            FileStream LetterControllerStream = new FileStream(Application.dataPath + "/Data/LetterControllerData.json", FileMode.Open);

            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bLetterControllerData = new byte[LetterControllerStream.Length];
            // 읽어오기
            LetterControllerStream.Read(bLetterControllerData, 0, bLetterControllerData.Length);
            LetterControllerStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jLetterControllerData = Encoding.UTF8.GetString(bLetterControllerData);
#if UNITY_EDITOR
            Debug.Log(jLetterControllerData);
#endif

            // 역직렬화
            LetterControllerData dLetterControllerData = JsonConvert.DeserializeObject<LetterControllerData>(jLetterControllerData);
            if (null == dLetterControllerData) // 저장된 데이터 없으면 리턴
                return;
            // 데이터 직렬화
            string jData = JsonConvert.SerializeObject(dLetterControllerData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
#if UNITY_EDITOR
            //Debug.Log("=======Load : dSOWSaveData =========");
            //Debug.Log(jData);
            //Debug.Log("=======Load=========");
#endif

            // 이어하기 시, 필요한 정보값들을 불러와서 갱신한다. (GuestManager)
            LetterController mLetterController = GameObject.Find("GuestManager").GetComponent<LetterController>();

            //if (mLetterController == null)
            //    return;

            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!

            mLetterController.satGuestList = dLetterControllerData.satGuestList.Clone() as int[];
            mLetterController.listCount = dLetterControllerData.listCount;

        }
    }

    void Load_SoundData() // 이어할 데이터가 있는 지, 새롭게 플레이 하는 지, 이전에 소리를 저장한 데이터가 있는 지
    {
        string mSoundDataPath = Path.Combine(Application.dataPath + "/Data/", "SoundData.json");
        
        if (File.Exists(mSoundDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 파일 스트림 개방
            FileStream SoundDataStream = new FileStream(Application.dataPath + "/Data/SoundData.json", FileMode.Open);


            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bSoundData = new byte[SoundDataStream.Length];
            // 읽어오기
            SoundDataStream.Read(bSoundData, 0, bSoundData.Length);
            SoundDataStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jSoundData = Encoding.UTF8.GetString(bSoundData);
#if UNITY_EDITOR
            Debug.Log(jSoundData);
#endif

            // 역직렬화
            SoundData dSoundData = JsonConvert.DeserializeObject<SoundData>(jSoundData);
            if (null == dSoundData) // 저장된 데이터 없으면 리턴
                return;
            // 데이터 직렬화
            string jData = JsonConvert.SerializeObject(dSoundData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
#if UNITY_EDITOR
            //Debug.Log("=======Load : dSOWSaveData =========");
            //Debug.Log(jData);
            //Debug.Log("=======Load=========");
#endif


            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!
            SceneData.Instance.BGMValue = dSoundData.mSaveBGM;
            SceneData.Instance.SFxValue = dSoundData.mSaveSFx;
            //isFirstPlay = dInitData.isFirstPlay;
        }
    }

    void Load_InitData() // 이어할 데이터가 있는 지, 새롭게 플레이 하는 지, 이전에 소리를 저장한 데이터가 있는 지
    {
        string mInitDataPath = Path.Combine(Application.dataPath + "/Data/", "InitData.json");
       
        if (File.Exists(mInitDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 파일 스트림 개방
            FileStream InitDataStream = new FileStream(Application.dataPath + "/Data/InitData.json", FileMode.Open);

            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bInitData = new byte[InitDataStream.Length];
            // 읽어오기
            InitDataStream.Read(bInitData, 0, bInitData.Length);
            InitDataStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jInitData = Encoding.UTF8.GetString(bInitData);
            
            #if UNITY_EDITOR
            Debug.Log(jInitData);
            #endif

            // 역직렬화
            InitData dInitData = JsonConvert.DeserializeObject<InitData>(jInitData);
            if (null == dInitData) // 저장된 데이터 없으면 리턴
                return;
            // 데이터 직렬화
            string jData = JsonConvert.SerializeObject(dInitData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
#if UNITY_EDITOR
            //Debug.Log("=======Load : dSOWSaveData =========");
            //Debug.Log(jData);
            //Debug.Log("=======Load=========");
#endif


            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!
            isFirstPlay = dInitData.isFirstPlay;
            //isFirstPlay = dInitData.isFirstPlay;
        }
    }

    void Load_LanguageData() // 이어할 데이터가 있는 지, 새롭게 플레이 하는 지, 이전에 소리를 저장한 데이터가 있는 지
    {
        string mInitDataPath = Path.Combine(Application.dataPath + "/Data/", "LanguageData.json");

        if (File.Exists(mInitDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 파일 스트림 개방
            FileStream LanguageDataStream = new FileStream(Application.dataPath + "/Data/LanguageData.json", FileMode.Open);

            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bLanguageData = new byte[LanguageDataStream.Length];
            // 읽어오기
            LanguageDataStream.Read(bLanguageData, 0, bLanguageData.Length);
            LanguageDataStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jLanguageData = Encoding.UTF8.GetString(bLanguageData);
#if UNITY_EDITOR
            Debug.Log(jLanguageData);
#endif

            // 역직렬화
            LanguageData dLanguageData = JsonConvert.DeserializeObject<LanguageData>(jLanguageData);
            if (null == dLanguageData) // 저장된 데이터 없으면 리턴
                return;
            // 데이터 직렬화
            string jData = JsonConvert.SerializeObject(dLanguageData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
#if UNITY_EDITOR
            //Debug.Log("=======Load : dSOWSaveData =========");
            //Debug.Log(jData);
            //Debug.Log("=======Load=========");
#endif


            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!
            LanguageManager mLanguageManager = GameObject.Find("LanguageManager").GetComponent<LanguageManager>();
            if (null != mLanguageManager)
                mLanguageManager.SetLanguageData(dLanguageData.isKorean);
           
            //isFirstPlay = dInitData.isFirstPlay;
        }
    }


    public void ActiveOption()
    {
        mSFx.Play();
        gOption.SetActive(true);
    }
    public void UnActiveOption()
    {
        mSFx.Play();
        gOption.SetActive(false);
        SaveLobby_SoundData();
        // 옵션창 끌 때 한/영 저장함.
        LanguageManager mLanguageManager = GameObject.Find("LanguageManager").GetComponent<LanguageManager>();
        if (null != mLanguageManager)
            Save_IsKorean(mLanguageManager.GetIsKorean());
    }

    // 로비에 저장 매니저 없으니까 임시로 함수 생성.
    public void SaveLobby_SoundData()
    {
        //SoundManager mSoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        //if (mSoundManager == null)
        //    return;

        // 파일이 있다면
        if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "SoundData.json")))
        {
            // 삭제
            System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "SoundData.json"));

        }
        // 삭제 후 다시 개방
        // 이유는, 동적으로 생성 될 경우에 json을 초기화 하지 않고 덮어 씌우기 때문에 전에 있던 데이터보다 적을 경우
        // 뒤에 남는 쓰레기 값들로 인하여 역직렬화 오류 발생함
        // 동적으로 생성하는 경우가 아닌 경우 (ex, 현재 씬 인덱스 등)은 상관 없음
        // 파일 스트림 개방
        FileStream stream = new FileStream(Application.dataPath + "/Data/SoundData.json", FileMode.OpenOrCreate);

        // 저장할 변수가 담긴 클래스 생성
        SoundData mSoundData = new SoundData();

        // 데이터 업데이트
        mSoundData.mSaveBGM = SceneData.Instance.BGMValue;
        mSoundData.mSaveSFx = SceneData.Instance.SFxValue;
        //mInitData.isFirstPlay = false; // 저장했으니까 처음 플레이가 아님.

        // 데이터 직렬화
        string jSoundData = JsonConvert.SerializeObject(mSoundData);

        // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
        byte[] bSoundData = Encoding.UTF8.GetBytes(jSoundData);
#if UNITY_EDITOR
        Debug.Log(jSoundData);
#endif
        // 해당 파일 스트림에 적는다.                
        stream.Write(bSoundData, 0, bSoundData.Length);
        // 스트림 닫기
        stream.Close();
    }

    public void SaveLobby_InitData()
    {
        //SoundManager mSoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        //if (mSoundManager == null)
        //    return;

        // 파일이 있다면
        if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "InitData.json")))
        {
            // 삭제
            System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "InitData.json"));

        }
        // 삭제 후 다시 개방
        // 이유는, 동적으로 생성 될 경우에 json을 초기화 하지 않고 덮어 씌우기 때문에 전에 있던 데이터보다 적을 경우
        // 뒤에 남는 쓰레기 값들로 인하여 역직렬화 오류 발생함
        // 동적으로 생성하는 경우가 아닌 경우 (ex, 현재 씬 인덱스 등)은 상관 없음
        // 파일 스트림 개방
        FileStream stream = new FileStream(Application.dataPath + "/Data/InitData.json", FileMode.OpenOrCreate);

        // 저장할 변수가 담긴 클래스 생성
        InitData mInitData = new InitData();

        // 데이터 업데이트
        mInitData.isFirstPlay = isFirstPlay;
        //mInitData.isFirstPlay = false; // 저장했으니까 처음 플레이가 아님.

        // 데이터 직렬화
        string jInitData = JsonConvert.SerializeObject(mInitData);

        // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
        byte[] bInitData = Encoding.UTF8.GetBytes(jInitData);
#if UNITY_EDITOR
        Debug.Log(jInitData);
#endif
        // 해당 파일 스트림에 적는다.                
        stream.Write(bInitData, 0, bInitData.Length);
        // 스트림 닫기
        stream.Close();
    }

    public void Save_IsKorean(bool isKorean)
    {
        // 파일이 있다면
        if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "LanguageData.json")))
        {
            // 삭제
            System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "LanguageData.json"));

        }

        FileStream stream = new FileStream(Application.dataPath + "/Data/LanguageData.json", FileMode.OpenOrCreate);

        // 저장할 변수가 담긴 클래스 생성
        LanguageData mLanguageData = new LanguageData();

        // 데이터 업데이트       
        mLanguageData.isKorean = isKorean;
        //mInitData.isFirstPlay = false; // 저장했으니까 처음 플레이가 아님.

        // 데이터 직렬화
        string jLanguageData = JsonConvert.SerializeObject(mLanguageData);

        // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
        byte[] bLanguageData = Encoding.UTF8.GetBytes(jLanguageData);
#if UNITY_EDITOR
        Debug.Log(jLanguageData);
#endif
        // 해당 파일 스트림에 적는다.                
        stream.Write(bLanguageData, 0, bLanguageData.Length);
        // 스트림 닫기
        stream.Close();
    }

    // 게임 종료
    public void QuitGame()
    {
        mSFx.Play();
        Application.Quit();
    }

    public void GoCredit()
    {
        // 크레딧 화면으로 전환
#if UNITY_EDITOR
        Debug.Log("크레딧화면으로 전환");
#endif
    }
    // 새로하기 경고창
    public void ActiveWarning()
    {
        //// 수정 필요
        //NewGame();
        //return;
        
        if (isFirstPlay)  // 바로 새로운 게임 스타트         
            NewGame();
        else
            gWarning.SetActive(true);

        mSFx.Play();
    }
    public void ActiveContinueWarning()
    {
        Load_Tutorial(); // 여기서 튜토리얼이 끝났다면 최초 1회 저장을 한 것이니까 여기서 이어하기 경고창 판별하면 된다.

        if (isCreateData)  // 저장한 데이터 있으면 그냥 로드
            ContinueGame();
        else
            gContinueWarning.SetActive(true);

        mSFx.Play();
    }

    public void UnAcitveWarning()
    {
        gWarning.SetActive(false);

        mSFx.Play();
    }

    public void UnAcitveContinueWarning()
    {
        gContinueWarning.SetActive(false);

        mSFx.Play();
    }
    public void HoveringNewGame()
    {
        iNewGame.sprite = sHoveringNew;
    }
    public void UnHoveringNewGame()
    {
        iNewGame.sprite = sUnHoveringNew;
    }
    public void HoveringContinueGame()
    {
        iContiueGame.sprite = sHoveringCon;
    }
    public void UnHoveringContinueGame()
    {
        iContiueGame.sprite = sUnHoveringCon;
    }

    // 한영 버전의 씬으로 전환 전환할 때 해당 씬 인덱스들을 활용해서 전환
    // 전환되면 bool등을 활ㅇ용해서 영어->영어, 한글->한글로 이용
    public void ChangeKor()
    {
        // 일단은 로비만 되니까 
        if (SceneManager.GetActiveScene().name != "Lobby") return;

        LanguageManager.GetInstance().SetKorean();
        ChangeLobbyLanguage();

        mSFx.Play();

    }

    public void ChangeEng()
    {
        if (SceneManager.GetActiveScene().name != "Lobby") return;

        LanguageManager.GetInstance().SetEnglish();
        ChangeLobbyLanguage();

        mSFx.Play();
    }


    private void ChangeLobbyLanguage()
    {
        if (LanguageManager.GetInstance().GetCurrentLanguage() == "Korean")
        {
            sLanguageCheckSprites[0].SetActive(true);
            sLanguageCheckSprites[1].SetActive(false);

            float Season = SeasonDateCalc.Instance.mSeason;
            int iIndex = 0;
            if (1 == Season)
            {
                iIndex = 0;
                gNewConTransform[0].anchoredPosition = new Vector3(15, 43, 0);
                gNewConTransform[1].anchoredPosition = new Vector3(123, -57, 0);

                iNewGame.GetComponent<RectTransform>().sizeDelta = new Vector2(310, 165);
                iContiueGame.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 190);
            }
            else if (2 == Season)
            {
                iIndex = 2;
                gNewConTransform[0].anchoredPosition = new Vector3(90, 45, 0);
                gNewConTransform[1].anchoredPosition = new Vector3(200, -55, 0);

                iNewGame.GetComponent<RectTransform>().sizeDelta = new Vector2(258, 148);
                iContiueGame.GetComponent<RectTransform>().sizeDelta = new Vector2(294, 162);
            }
            else if (3 == Season)
            {
                iIndex = 4;
                gNewConTransform[0].anchoredPosition = new Vector3(-145, 125, 0);
                gNewConTransform[1].anchoredPosition = new Vector3(-60, 45, 0);

                iNewGame.GetComponent<RectTransform>().sizeDelta = new Vector2(238, 137);
                iContiueGame.GetComponent<RectTransform>().sizeDelta = new Vector2(279, 147);
            }
            else if (4 == Season)
            {
                iIndex = 6;
                gNewConTransform[0].anchoredPosition = new Vector3(-90, 65, 0);
                gNewConTransform[1].anchoredPosition = new Vector3(-12, -55, 0);

                iNewGame.GetComponent<RectTransform>().sizeDelta = new Vector2(297, 151);
                iContiueGame.GetComponent<RectTransform>().sizeDelta = new Vector2(346, 135);
            }
            sHoveringNew = sHoverNewLan[iIndex];
            sUnHoveringNew = sUnHoverNewLan[iIndex];
            sHoveringCon = sHoverConLan[iIndex];
            sUnHoveringCon = sUnHoverConLan[iIndex];
        }
        else
        {
            
            sLanguageCheckSprites[0].SetActive(false);
            sLanguageCheckSprites[1].SetActive(true);

            float Season = SeasonDateCalc.Instance.mSeason;
            int iIndex = 1;
            if (1 == Season)
            {
                iIndex = 1;
                gNewConTransform[0].anchoredPosition = new Vector3(30, 25, 0);
                gNewConTransform[1].anchoredPosition = new Vector3(105, -72.5f, 0);

                iNewGame.GetComponent<RectTransform>().sizeDelta = new Vector2(380, 185);
                iContiueGame.GetComponent<RectTransform>().sizeDelta = new Vector2(370, 225);
            }
            else if (2 == Season)
            {
                iIndex = 3;
                gNewConTransform[0].anchoredPosition = new Vector3(100, 40, 0);
                gNewConTransform[1].anchoredPosition = new Vector3(197, -65, 0);

                iNewGame.GetComponent<RectTransform>().sizeDelta = new Vector2(309, 164);
                iContiueGame.GetComponent<RectTransform>().sizeDelta = new Vector2(285, 158);
            }
            else if (3 == Season)
            {
                iIndex = 5;
                gNewConTransform[0].anchoredPosition = new Vector3(-101, 130, 0);
                gNewConTransform[1].anchoredPosition = new Vector3(-101, 35, 0);

                iNewGame.GetComponent<RectTransform>().sizeDelta = new Vector2(340, 144);
                iContiueGame.GetComponent<RectTransform>().sizeDelta = new Vector2(320, 153);
            }
            else if (4 == Season)
            {
                iIndex = 7;
                gNewConTransform[0].anchoredPosition = new Vector3(-55, 60, 0);
                gNewConTransform[1].anchoredPosition = new Vector3(-25, -60, 0);

                iNewGame.GetComponent<RectTransform>().sizeDelta = new Vector2(352, 137);
                iContiueGame.GetComponent<RectTransform>().sizeDelta = new Vector2(331, 131);
            }
            sHoveringNew = sHoverNewLan[iIndex];
            sUnHoveringNew = sUnHoverNewLan[iIndex];
            sHoveringCon = sHoverConLan[iIndex];
            sUnHoveringCon = sUnHoverConLan[iIndex];
        }

        mLanguageChanger.ChangeLanguageInLobby();
        iNewGame.sprite = sUnHoveringNew;
        iContiueGame.sprite = sUnHoveringCon;
    }
}

