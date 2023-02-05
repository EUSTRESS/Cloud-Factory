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

    // INDEX -> [0]: C04 [1]: C07 [2]: C10 [3]:C13 [4]:C14
    public GameObject[] gSpringMoongti = new GameObject[5]; // 봄 타이틀 뭉티 관리

    [Header("TEXT")]
    public Text         tBgmValue;   // BGM 볼륨 텍스트
    public Text         tSfxValue;   // SFx 볼륨 텍스트

    [Header("SLIDER")]
    public Slider       sBGM;        // BGM 슬라이더
    public Slider       sSFx;        // SFx 슬라이더

    private AudioSource mSFx;        // 효과음 오디오 소스

    [Header("BOOL")]
    public bool[] bSpringMoongti = new bool[5]; // 봄 타이틀 뭉티 Bool로 만족도 5 관리

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
            case 1: // 봄
                // 봄 뭉티 만족도 5 관리
                if (bSpringMoongti[0])
                    gSpringMoongti[0].SetActive(true);
                if (bSpringMoongti[1])
                    gSpringMoongti[1].SetActive(true);
                if (bSpringMoongti[2])
                    gSpringMoongti[2].SetActive(true);
                if (bSpringMoongti[3])
                    gSpringMoongti[3].SetActive(true);
                if (bSpringMoongti[4])
                    gSpringMoongti[4].SetActive(true);
                break;
            case 2: // 여름
                break;
            case 3: // 가을
                break;
            case 4: // 겨울
                break;
            default:
                break;
        }
    }

    /*
     * BUTTON에 할당할 메소드
     */

    public void NewGame()
    {        
        mSFx.Play();

        LoadingSceneController.Instance.LoadScene("Space Of Weather");

        // 데이터를 초기화 시키는 함수 호출할 필요 없이
        // 각 클래스 생성자에서 자동 초기화된다.
    }

    public void ContinueGame()
    {
        String key = "key"; // 암호화 복호화 키 값

        mSFx.Play();

        /*
         저장된 씬 넘버 로딩         
         */

        // newtonsoft library (모노비헤이비어 상속된 클래스 사용 불가능, 딕셔너리 사용 가능)
        // 로드하는 함수 호출 후에 그 씬 인덱스로 이동
        FileStream fSceneBuildIndexStream
            // 해당 경로에 있는 json 파일을 연다
            = new FileStream(Application.dataPath + "/Data/SceneBuildIndex.json", FileMode.Open);
        // 열려있는 json 값들을 byte배열에 넣는다
        byte[] bSceneData = new byte[fSceneBuildIndexStream.Length];
        // 끝까지 읽는다
        fSceneBuildIndexStream.Read(bSceneData, 0, bSceneData.Length);
        fSceneBuildIndexStream.Close();
        // 문자열로 변환한다
        string sSceneData = Encoding.UTF8.GetString(bSceneData);
        // 복호화
        sSceneData = AESWithJava.Con.Program.Decrypt(sSceneData, key);

       
        // Load_Data
        Load_SeasonDate();
        Load_Inventory();
        Load_Guest();
        Load_SOW();

        // 문자열을 int형으로 파싱해서 빌드 인덱스로 활용한다
        LoadingSceneController.Instance.LoadScene(int.Parse(sSceneData));        

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


    public void ActiveOption()
    {
        mSFx.Play();
        gOption.SetActive(true);
    }
    public void UnActiveOption()
    {
        mSFx.Play();
        gOption.SetActive(false);
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
        gWarning.SetActive(true);
    }
    public void UnAcitveWarning()
    {
        gWarning.SetActive(false);
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

