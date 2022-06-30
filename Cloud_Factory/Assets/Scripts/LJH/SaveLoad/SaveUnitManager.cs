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
        // Debug.Log("OnSceneLoaded: " + scene.name + " | SceneBuildIndex: " + scene.buildIndex);

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
        string sSceneData = JsonConvert.SerializeObject(SceneData.Instance.currentSceneIndex);

        // text 데이터로 인코딩한다
        byte[] bSceneData = Encoding.UTF8.GetBytes(sSceneData);

        // text 데이터를 작성한다
        fSceneBuildIndexStream.Write(bSceneData, 0, bSceneData.Length);
        fSceneBuildIndexStream.Close();

        //================================================================================//
        //=================================날짜 계절 저장=================================//
        //================================================================================//

        //SeasonDateDataBox.Instance.mSecond = SeasonDateDataBox.Instance.mSecond;
        //SeasonDateDataBox.Instance.mDay    = SeasonDateDataBox.Instance.mDay;
        //SeasonDateDataBox.Instance.mWeek   = SeasonDateDataBox.Instance.mWeek;
        //SeasonDateDataBox.Instance.mSeason = SeasonDateDataBox.Instance.mSeason;
        //SeasonDateDataBox.Instance.mYear   = SeasonDateDataBox.Instance.mYear;

        FileStream fSeasonDateStream
            = new FileStream(Application.dataPath + "/Data/SeasonDate.json", FileMode.OpenOrCreate);

        //SeasonDateDataBox seasonDate = new SeasonDateDataBox();
        SeasonDateCalc seasonDate = this.gameObject.AddComponent<SeasonDateCalc>();

        // Class를 Json으로 넘기면 self 참조 반복이 일어나기 때문에
        // 외부라이브러리를 제외하고 유니티 Utility를 활용한다.

        // 클래스의 맴버변수들을 json파일로 변환한다 (class, prettyPrint) true면 읽기 좋은 형태로 저장해줌
        string sSeasonData = JsonUtility.ToJson(seasonDate, true);
        Debug.Log(sSeasonData);

        // 인코딩
        byte[] bSeasonData = Encoding.UTF8.GetBytes(sSeasonData);

        // 작성
        fSeasonDateStream.Write(bSeasonData, 0, bSeasonData.Length);
        fSeasonDateStream.Close();
    }

    // 종료될 때
    void OnDisable()
    {
        // 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
