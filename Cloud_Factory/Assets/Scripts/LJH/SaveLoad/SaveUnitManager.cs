using System.Linq; // list 복사
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using AESWithJava.Con;
using System;
using UnityEditor;

public class SaveUnitManager : MonoBehaviour
{
    // SaveUnitManager 인스턴스를 담는 전역 변수
    private static SaveUnitManager instance = null;

    private InventoryManager mInvenManager;

    // 모든 씬에 넣어 놓을 것이기 때문에 중복은 파괴처리
    // 어느 씬에서 저장되고 로드될 것인지 모르기 때문에
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        mInvenManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }


    // Awake->OnEnable->Start순으로 생명주기
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        // 씬 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    // 씬이 변경될 때마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 데이터 폴더가 없다면 생성하기
        if (!File.Exists(Application.dataPath + "/Data/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Data/");
        }

        // 로비에서는 저장할 필요가 없음
        if (scene.name != "Lobby" && SceneData.Instance) // null check && lobby 제한
        {
            String key = "key";

            //================================================================================//
            //==================================현재 씬 저장==================================//
            //================================================================================//

            // 새롭게 로딩된 씬의 데이터를 저장한다
            SceneData.Instance.currentSceneIndex = scene.buildIndex;

            // 저장하는 함수 호출
            // 일단은 하나니까 이렇게 넣고 많아지면 클래스 만들어서 정리하기
            FileStream fSceneBuildIndexStream
                // 파일 경로 + 내가 만든 폴더 경로에 json 저장 / 모드는 SAVE
                = new FileStream(Application.dataPath + "/Data/SceneBuildIndex.json", FileMode.OpenOrCreate);

            // sData로 변수를 직렬화한다        
            // 현재 씬 인덱스 저장
            string sSceneData = JsonConvert.SerializeObject(SceneData.Instance.currentSceneIndex);
            // 암호화
            sSceneData = AESWithJava.Con.Program.Encrypt(sSceneData, key);

            // text 데이터로 인코딩한다
            byte[] bSceneData = Encoding.UTF8.GetBytes(sSceneData);

            // text 데이터를 작성한다
            fSceneBuildIndexStream.Write(bSceneData, 0, bSceneData.Length);
            fSceneBuildIndexStream.Close();

            //================================================================================//
            //=================================날짜 계절 저장=================================//
            //================================================================================//

            // jsonUtility
            string mSeasonDatePath = Path.Combine(Application.dataPath + "/Data/", "SeasonDate.json");

            // 저장하는 공간 클래스 선언
            // Class를 Json으로 넘기면 self 참조 반복이 일어나기 때문에
            // 외부라이브러리를 제외하고 유니티 Utility를 활용한다.

            // 하나의 json파일에 저장하기 위해서 클래스 새롭게 생성 후 클래스 단위로 저장
            // 새로운 오브젝트에 클래스 선언 후 업데이트
            GameObject gSeasonDate = new GameObject();
            SeasonDateCalc seasonDate = gSeasonDate.AddComponent<SeasonDateCalc>();

            // 업데이트
            seasonDate.mSecond = SeasonDateCalc.Instance.mSecond;
            seasonDate.mDay = SeasonDateCalc.Instance.mDay;
            seasonDate.mSeason = SeasonDateCalc.Instance.mSeason;
            seasonDate.mYear = SeasonDateCalc.Instance.mYear;

            // 클래스의 맴버변수들을 json파일로 변환한다 (class, prettyPrint) true면 읽기 좋은 형태로 저장해줌
            // seasonDataSaveBox 클래스 단위로 json 변환
            string sSeasonData = JsonUtility.ToJson(gSeasonDate.GetComponent<SeasonDateCalc>(), true);
            Debug.Log(sSeasonData);
            // 암호화
            // sSeasonData = AESWithJava.Con.Program.Encrypt(sSeasonData, key);

            Debug.Log(sSeasonData);

            File.WriteAllText(mSeasonDatePath, sSeasonData);

            //******************************************//
            // 예람이꺼 저장(인벤토리)
            if (mInvenManager) // null check
            {
                // 암호화는 나중에 한번에 하기

                // 파일이 있다면
                if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "InventoryData.json")))
                {
                    // 삭제
                    System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "InventoryData.json"));

                }
                // 삭제 후 다시 개방
                // 이유는, 동적으로 생성 될 경우에 json을 초기화 하지 않고 덮어 씌우기 때문에 전에 있던 데이터보다 적을 경우
                // 뒤에 남는 쓰레기 값들로 인하여 역직렬화 오류 발생함
                // 동적으로 생성하는 경우가 아닌 경우 (ex, 현재 씬 인덱스 등)은 상관 없음
                // 파일 스트림 개방
                FileStream stream = new FileStream(Application.dataPath + "/Data/InventoryData.json", FileMode.OpenOrCreate);

                // 저장할 변수가 담긴 클래스 생성
                InventoryData mInventoryData = new InventoryData();

                // 데이터 업데이트
                mInventoryData.mType = mInvenManager.mType.ToList();
                mInventoryData.mCnt = mInvenManager.mCnt.ToList();
                mInventoryData.minvenLevel = mInvenManager.minvenLevel;
                mInventoryData.mMaxInvenCnt = mInvenManager.mMaxInvenCnt;
                mInventoryData.mMaxStockCnt = mInvenManager.mMaxStockCnt;

                // 데이터 직렬화
                string jInventoryData = JsonConvert.SerializeObject(mInventoryData);
                
                // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
                byte[] bInventoryData = Encoding.UTF8.GetBytes(jInventoryData);
                Debug.Log(jInventoryData);
                // 해당 파일 스트림에 적는다.                
                stream.Write(bInventoryData, 0, bInventoryData.Length);
                // 스트림 닫기
                stream.Close();                
            }



        }
        // 종료될 때
        void OnDisable()
        {
            // 제거
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}