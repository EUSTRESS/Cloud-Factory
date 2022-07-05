using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class SaveUnitManager : MonoBehaviour
{
    // SaveUnitManager 인스턴스를 담는 전역 변수
    private static SaveUnitManager instance = null;
        
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
        // 로비에서는 저장할 필요가 없음
        if (scene.name != "Lobby" && SceneData.Instance) // null check && lobby 제한
        {
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

            File.WriteAllText(mSeasonDatePath, sSeasonData);
        }        
    }

    // 종료될 때
    void OnDisable()
    {
        // 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}