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
public class InitData
{
    public bool isFirstPlay;    
}



// 로비 씬 UI 담당
// 설정 창, 새로하기, 이어하기
public class LobbyUIManager : MonoBehaviour
{
    private SeasonDateCalc mSeason; // 계절 스크립트
    private InventoryManager mInvenManager;

    [Header("GAME OBJECT")]
    // 오브젝트 Active 관리
    public GameObject   gOption;     // 옵션 게임 오브젝트
    public GameObject   gWarning;    // 새로운 게임 경고창
    public GameObject   gContinueWarning;    // 이어하기 경고창

    // INDEX -> [0]: C04 [1]: C07 [2]: C10 [3]:C13 [4]:C14 // 봄 타이틀 뭉티 관리
    public GameObject[] gSpringMoongti = new GameObject[20]; // 전체 뭉티 타이틀 스프라이트 관리


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

    [Header("IMAGE")]
    public Image iNewGame;
    public Image iContiueGame;

    [Header("SPRITES")]
    public Sprite sHoveringNew;
    public Sprite sUnHoveringNew;
    public Sprite sHoveringCon;
    public Sprite sUnHoveringCon;

    void Awake()
    {
        mSFx = GameObject.Find("mSFx").GetComponent<AudioSource>();
        mSeason = GameObject.Find("Season Date Calc").GetComponent<SeasonDateCalc>();
        mInvenManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();

        isFirstPlay = true; // 처음 플레이한다고 넣어두고
        // 여기서 최초 1회 플레이인지 아닌지 판단.
        Load_InitData();
        // 사운드 로드 및 저장한 이력이 있는 지 판단.
        Load_SoundData();
        

        Load_GuestSatisfaction(); // 만족도 5 뭉티 판별
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

        switch (mSeason.mSeason)
        {
            // 저장한 값 로딩하는 과정에서 SetActive 바로해버리면 된다.

            //case 1: // 봄
            //    // 봄 뭉티 만족도 5 관리
            //    if (bSpringMoongti[0])
            //        gSpringMoongti[0].SetActive(true);
            //    if (bSpringMoongti[1])
            //        gSpringMoongti[1].SetActive(true);
            //    if (bSpringMoongti[2])
            //        gSpringMoongti[2].SetActive(true);
            //    if (bSpringMoongti[3])
            //        gSpringMoongti[3].SetActive(true);
            //    if (bSpringMoongti[4])
            //        gSpringMoongti[4].SetActive(true);
            //    break;
            //case 2: // 여름
            //    break;
            //case 3: // 가을
            //    break;
            //case 4: // 겨울
            //    break;
            //default:
            //    break;
        }
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
        Load_SeasonDate();
        Load_Inventory();
        Load_Guest();
        Load_SOW();      
        //Load_SOWManagerData(); // 로비에 매니저가 없어서, 날씨의 공간 들어와서 로딩할것.
        Load_LetterControllerData();

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

            Debug.Log(sDateData);

            // 데이터를 새로운 오브젝트에 덮어씌운다
            JsonUtility.FromJsonOverwrite(sDateData, gSeasonDate.AddComponent<SeasonDateCalc>());

            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!
            SeasonDateCalc.Instance.mSecond = gSeasonDate.GetComponent<SeasonDateCalc>().mSecond;
            SeasonDateCalc.Instance.mDay = gSeasonDate.GetComponent<SeasonDateCalc>().mDay;
            SeasonDateCalc.Instance.mSeason = gSeasonDate.GetComponent<SeasonDateCalc>().mSeason;
            SeasonDateCalc.Instance.mYear = gSeasonDate.GetComponent<SeasonDateCalc>().mYear;
        }

    }

    void Load_Inventory()
    {
        string mInvenDataPath = Path.Combine(Application.dataPath + "/Data/", "InventoryData.json");
        // 파일 스트림 개방
        FileStream stream = new FileStream(Application.dataPath + "/Data/InventoryData.json", FileMode.Open);

        if (File.Exists(mInvenDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bInventoryData = new byte[stream.Length];
            // 읽어오기
            stream.Read(bInventoryData, 0, bInventoryData.Length);
            stream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jInventoryData = Encoding.UTF8.GetString(bInventoryData);
            Debug.Log(jInventoryData);

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
    void Load_Guest()
    {
        string mGuestManagerDataPath = Path.Combine(Application.dataPath + "/Data/", "GuestManagerData.json");
        // 파일 스트림 개방
        FileStream ManagerStream = new FileStream(Application.dataPath + "/Data/GuestManagerData.json", FileMode.Open);

        if (File.Exists(mGuestManagerDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bGuestInfoData = new byte[ManagerStream.Length];
            // 읽어오기
            ManagerStream.Read(bGuestInfoData, 0, bGuestInfoData.Length);
            ManagerStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jGuestInfoData = Encoding.UTF8.GetString(bGuestInfoData);
            Debug.Log(jGuestInfoData);

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

                    if (info == null) Debug.Log("Info Null");

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
        // 파일 스트림 개방
        FileStream ManagerStream = new FileStream(Application.dataPath + "/Data/GuestManagerData.json", FileMode.Open);

        if (File.Exists(mGuestManagerDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bGuestInfoData = new byte[ManagerStream.Length];
            // 읽어오기
            ManagerStream.Read(bGuestInfoData, 0, bGuestInfoData.Length);
            ManagerStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jGuestInfoData = Encoding.UTF8.GetString(bGuestInfoData);
            Debug.Log(jGuestInfoData);

            // 역직렬화
            GuestManagerSaveData dGuestInfoData = JsonConvert.DeserializeObject<GuestManagerSaveData>(jGuestInfoData);
            if (null == dGuestInfoData) // 저장된 데이터 없으면 리턴
                return;
            // 이어하기 시, 필요한 정보값들을 불러와서 갱신한다. (GuestManager)


            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!

            // guest manager 로딩
            /*저장할 데이터 값*/

            {
                const int NUM_OF_GUEST = 20;
                GuestInfos GuestInfos = new GuestInfos();

                for (int i = 0; i < NUM_OF_GUEST; i++)
                {
                    GuestInfoSaveData info = dGuestInfoData.GuestInfos[i];

                    if (info == null) Debug.Log("Info Null");

                    if (5 == info.mSatatisfaction) // 만족도가 5인 뭉티
                    {
                        gSpringMoongti[i].SetActive(true);
                    }
                }
                //mGuestManagerData.GuestInfos = mGuestManager.mGuestInfo.Clone() as GuestInfoSaveData[];
            }
        }
    }

    void Load_SOW()
    {
        string mSOWSaveDataPath = Path.Combine(Application.dataPath + "/Data/", "SOWSaveData.json");
        // 파일 스트림 개방
        FileStream SOWSaveStream = new FileStream(Application.dataPath + "/Data/SOWSaveData.json", FileMode.Open);

        if (File.Exists(mSOWSaveDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bSOWSaveData = new byte[SOWSaveStream.Length];
            // 읽어오기
            SOWSaveStream.Read(bSOWSaveData, 0, bSOWSaveData.Length);
            SOWSaveStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jSOWSaveData = Encoding.UTF8.GetString(bSOWSaveData);
            Debug.Log(jSOWSaveData);

            // 역직렬화
            SOWSaveData dSOWSaveData = JsonConvert.DeserializeObject<SOWSaveData>(jSOWSaveData);
            if (null == dSOWSaveData) // 저장된 데이터 없으면 리턴
                return;
            // 데이터 직렬화
            string jData = JsonConvert.SerializeObject(dSOWSaveData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
            //Debug.Log("=======Load : dSOWSaveData =========");
            //Debug.Log(jData);
            //Debug.Log("=======Load=========");

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
            //Debug.Log("=======Load : sowInfo =========");
            //Debug.Log(jAData);
            //Debug.Log("=======Load=========");

            GuestManager.SaveSOWdatas = sowInfo;
            GuestManager.isLoad = true;

            //string jBData = JsonConvert.SerializeObject(GuestManager.SaveSOWdatas);
            //Debug.Log("=======Load :  GuestManager.SaveSOWdatas  =========");
            //Debug.Log(jBData);
            //Debug.Log("=======Load=========");

        }
    }
    void Load_Tutorial()
    {
        string mTutorialSaveDataPath = Path.Combine(Application.dataPath + "/Data/", "TutorialData.json");
        // 파일 스트림 개방
        FileStream TutorialSaveStream = new FileStream(Application.dataPath + "/Data/TutorialData.json", FileMode.Open);

        if (File.Exists(mTutorialSaveDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bTutorialSaveData = new byte[TutorialSaveStream.Length];
            // 읽어오기
            TutorialSaveStream.Read(bTutorialSaveData, 0, bTutorialSaveData.Length);
            TutorialSaveStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jTutorialSaveData = Encoding.UTF8.GetString(bTutorialSaveData);
            Debug.Log(jTutorialSaveData);

            // 역직렬화
            TutorialData dTutorialSaveData = JsonConvert.DeserializeObject<TutorialData>(jTutorialSaveData);
            if (null == dTutorialSaveData) // 저장된 데이터 없으면 리턴
                return;
            // 데이터 직렬화
            //string jData = JsonConvert.SerializeObject(dTutorialSaveData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
            //Debug.Log("=======Load : dSOWSaveData =========");
            //Debug.Log(jData);
            //Debug.Log("=======Load=========");

            // 이어하기 시, 필요한 정보값들을 불러와서 갱신한다. (GuestManager)
            TutorialManager tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();

            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!

            tutorialManager.isTutorial = dTutorialSaveData.isTutorial;
            isCreateData = !tutorialManager.isTutorial; // 튜토리얼 유무로 저장했는 지 판별.
        }
    }

    void Load_SOWManagerData()
    {
        string mSowManagerSaveDataPath = Path.Combine(Application.dataPath + "/Data/", "SOWManagerData.json");
        // 파일 스트림 개방
        FileStream SOWmanageSaveStream = new FileStream(Application.dataPath + "/Data/SOWManagerData.json", FileMode.Open);

        if (File.Exists(mSowManagerSaveDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bSOWManagerSaveData = new byte[SOWmanageSaveStream.Length];
            // 읽어오기
            SOWmanageSaveStream.Read(bSOWManagerSaveData, 0, bSOWManagerSaveData.Length);
            SOWmanageSaveStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jSOWManagerSaveData = Encoding.UTF8.GetString(bSOWManagerSaveData);
            Debug.Log(jSOWManagerSaveData);

            // 역직렬화
            SOWManagerSaveData dSOWManagerSaveData = JsonConvert.DeserializeObject<SOWManagerSaveData>(jSOWManagerSaveData);
            if (null == dSOWManagerSaveData) // 저장된 데이터 없으면 리턴
                return;
            // 데이터 직렬화
            //string jData = JsonConvert.SerializeObject(dSOWManagerSaveData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
            //Debug.Log("=======Load : dSOWSaveData =========");
            //Debug.Log(jData);
            //Debug.Log("=======Load=========");

            // 이어하기 시, 필요한 정보값들을 불러와서 갱신한다. (GuestManager)
            SOWManager mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!

            mSOWManager.yardGatherCount = dSOWManagerSaveData.yardGatherCount.Clone() as int[];

        }
    }


    void Load_LetterControllerData()
    {
        string mLetterControllerDataPath = Path.Combine(Application.dataPath + "/Data/", "LetterControllerData.json");
        // 파일 스트림 개방
        FileStream LetterControllerStream = new FileStream(Application.dataPath + "/Data/LetterControllerData.json", FileMode.Open);

        if (File.Exists(mLetterControllerDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bLetterControllerData = new byte[LetterControllerStream.Length];
            // 읽어오기
            LetterControllerStream.Read(bLetterControllerData, 0, bLetterControllerData.Length);
            LetterControllerStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jLetterControllerData = Encoding.UTF8.GetString(bLetterControllerData);
            Debug.Log(jLetterControllerData);

            // 역직렬화
            LetterControllerData dLetterControllerData = JsonConvert.DeserializeObject<LetterControllerData>(jLetterControllerData);
            if (null == dLetterControllerData) // 저장된 데이터 없으면 리턴
                return;
            // 데이터 직렬화
            string jData = JsonConvert.SerializeObject(dLetterControllerData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
            //Debug.Log("=======Load : dSOWSaveData =========");
            //Debug.Log(jData);
            //Debug.Log("=======Load=========");

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
        // 파일 스트림 개방
        FileStream SoundDataStream = new FileStream(Application.dataPath + "/Data/SoundData.json", FileMode.Open);

        if (File.Exists(mSoundDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bSoundData = new byte[SoundDataStream.Length];
            // 읽어오기
            SoundDataStream.Read(bSoundData, 0, bSoundData.Length);
            SoundDataStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jSoundData = Encoding.UTF8.GetString(bSoundData);
            Debug.Log(jSoundData);

            // 역직렬화
            SoundData dSoundData = JsonConvert.DeserializeObject<SoundData>(jSoundData);
            if (null == dSoundData) // 저장된 데이터 없으면 리턴
                return;
            // 데이터 직렬화
            string jData = JsonConvert.SerializeObject(dSoundData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
            //Debug.Log("=======Load : dSOWSaveData =========");
            //Debug.Log(jData);
            //Debug.Log("=======Load=========");


            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!
            SceneData.Instance.BGMValue = dSoundData.mSaveBGM;
            SceneData.Instance.SFxValue = dSoundData.mSaveSFx;
            //isFirstPlay = dInitData.isFirstPlay;
        }
    }

    void Load_InitData() // 이어할 데이터가 있는 지, 새롭게 플레이 하는 지, 이전에 소리를 저장한 데이터가 있는 지
    {
        string mInitDataPath = Path.Combine(Application.dataPath + "/Data/", "InitData.json");
        // 파일 스트림 개방
        FileStream InitDataStream = new FileStream(Application.dataPath + "/Data/InitData.json", FileMode.Open);

        if (File.Exists(mInitDataPath)) // 해당 파일이 생성되었으면 불러오기
        {
            // 복호화는 나중에 한번에 하기
            // 스트림 배열만큼 바이트 배열 생성
            byte[] bInitData = new byte[InitDataStream.Length];
            // 읽어오기
            InitDataStream.Read(bInitData, 0, bInitData.Length);
            InitDataStream.Close();

            // jsondata를 스트링 타입으로 가져오기
            string jInitData = Encoding.UTF8.GetString(bInitData);
            Debug.Log(jInitData);

            // 역직렬화
            InitData dInitData = JsonConvert.DeserializeObject<InitData>(jInitData);
            if (null == dInitData) // 저장된 데이터 없으면 리턴
                return;
            // 데이터 직렬화
            string jData = JsonConvert.SerializeObject(dInitData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
            //Debug.Log("=======Load : dSOWSaveData =========");
            //Debug.Log(jData);
            //Debug.Log("=======Load=========");


            // 덮어씌워진(저장된) 데이터를 현재 사용되는 데이터에 갱신하면 로딩 끝!
            isFirstPlay = dInitData.isFirstPlay;
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
        Debug.Log(jSoundData);
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
        Debug.Log(jInitData);
        // 해당 파일 스트림에 적는다.                
        stream.Write(bInitData, 0, bInitData.Length);
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
        Debug.Log("크레딧화면으로 전환");
    }
    // 새로하기 경고창
    public void ActiveWarning()
    {
        if (isFirstPlay)  // 바로 새로운 게임 스타트         
            NewGame();
        else
            gWarning.SetActive(true);
    }
    public void ActiveContinueWarning()
    {
        Load_Tutorial(); // 여기서 튜토리얼이 끝났다면 최초 1회 저장을 한 것이니까 여기서 이어하기 경고창 판별하면 된다.

        if (isCreateData)  // 저장한 데이터 있으면 그냥 로드
            ContinueGame();
        else
            gContinueWarning.SetActive(true);
    }

    public void UnAcitveWarning()
    {
        gWarning.SetActive(false);
    }

    public void UnAcitveContinueWarning()
    {
        gContinueWarning.SetActive(false);
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
        if (SceneManager.GetActiveScene().name == "Eng_Lobby")
            SceneManager.LoadScene("Lobby");
    }
    public void ChangeEng()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
            SceneManager.LoadScene("Eng_Lobby");
    }
}

