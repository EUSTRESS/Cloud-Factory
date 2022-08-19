using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class SaveUnitManager : MonoBehaviour
{
    // SaveUnitManager �ν��Ͻ��� ��� ���� ����
    private static SaveUnitManager instance = null;
        
    // ��� ���� �־� ���� ���̱� ������ �ߺ��� �ı�ó��
    // ��� ������ ����ǰ� �ε�� ������ �𸣱� ������
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

    // Awake->OnEnable->Start������ �����ֱ�
    void OnEnable()
    {
        // �� �Ŵ����� sceneLoaded�� ü���� �Ǵ�.
        // �� �߰�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ü���� �ɾ �� �Լ��� �� ������ ȣ��ȴ�.
    // ���� ����� ������ ȣ��ȴ�.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {       
        if (!Directory.Exists(Application.dataPath + "/Data/")) // Data ������ ������ �����ϱ�
        {
            Debug.Log("����");
            Directory.CreateDirectory(Application.dataPath + "/Data/");
        }

        // �κ񿡼��� ������ �ʿ䰡 ����
        if (scene.name != "Lobby" && SceneData.Instance) // null check && lobby ����
        {
            //================================================================================//
            //==================================���� �� ����==================================//
            //================================================================================//

            // ���Ӱ� �ε��� ���� �����͸� �����Ѵ�
            SceneData.Instance.currentSceneIndex = scene.buildIndex;
                        
            // �����ϴ� �Լ� ȣ��
            // �ϴ��� �ϳ��ϱ� �̷��� �ְ� �������� Ŭ���� ���� �����ϱ�
            FileStream fSceneBuildIndexStream
                // ���� ��� + ���� ���� ���� ��ο� json ���� / ���� SAVE
                = new FileStream(Application.dataPath + "/Data/SceneBuildIndex.json", FileMode.OpenOrCreate);

            // sData�� ������ ����ȭ�Ѵ�        
            // ���� �� �ε��� ����
            string sSceneData = JsonConvert.SerializeObject(SceneData.Instance.currentSceneIndex);

            // text �����ͷ� ���ڵ��Ѵ�
            byte[] bSceneData = Encoding.UTF8.GetBytes(sSceneData);

            // text �����͸� �ۼ��Ѵ�
            fSceneBuildIndexStream.Write(bSceneData, 0, bSceneData.Length);
            fSceneBuildIndexStream.Close();

            //================================================================================//
            //=================================��¥ ���� ����=================================//
            //================================================================================//

            // jsonUtility
            string mSeasonDatePath = Path.Combine(Application.dataPath + "/Data/", "SeasonDate.json");

            // �����ϴ� ���� Ŭ���� ����
            // Class�� Json���� �ѱ�� self ���� �ݺ��� �Ͼ�� ������
            // �ܺζ��̺귯���� �����ϰ� ����Ƽ Utility�� Ȱ���Ѵ�.

            // �ϳ��� json���Ͽ� �����ϱ� ���ؼ� Ŭ���� ���Ӱ� ���� �� Ŭ���� ������ ����
            // ���ο� ������Ʈ�� Ŭ���� ���� �� ������Ʈ
            GameObject gSeasonDate = new GameObject();
            SeasonDateCalc seasonDate = gSeasonDate.AddComponent<SeasonDateCalc>();

            // ������Ʈ
            seasonDate.mSecond = SeasonDateCalc.Instance.mSecond;
            seasonDate.mDay = SeasonDateCalc.Instance.mDay;
            seasonDate.mSeason = SeasonDateCalc.Instance.mSeason;
            seasonDate.mYear = SeasonDateCalc.Instance.mYear;

            // Ŭ������ �ɹ��������� json���Ϸ� ��ȯ�Ѵ� (class, prettyPrint) true�� �б� ���� ���·� ��������
            // seasonDataSaveBox Ŭ���� ������ json ��ȯ
            string sSeasonData = JsonUtility.ToJson(gSeasonDate.GetComponent<SeasonDateCalc>(), true);
            Debug.Log(sSeasonData);

            File.WriteAllText(mSeasonDatePath, sSeasonData);

        }        
    }

    // ����� ��
    void OnDisable()
    {
        // ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}