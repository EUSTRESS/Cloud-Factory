using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using Newtonsoft.Json;

// �κ� �� UI ���
// ���� â, �����ϱ�, �̾��ϱ�
public class LobbyUIManager : MonoBehaviour
{
    [Header("GAME OBJECT")]
    // ������Ʈ Active ����
    public GameObject   gOption;     // �ɼ� ���� ������Ʈ
    public GameObject   gWarning;    // ���ο� ���� ���â

    [Header("TEXT")]
    public Text         tBgmValue;   // BGM ���� �ؽ�Ʈ
    public Text         tSfxValue;   // SFx ���� �ؽ�Ʈ

    [Header("SLIDER")]
    public Slider       sBGM;        // BGM �����̴�
    public Slider       sSFx;        // SFx �����̴�

    private AudioSource mSFx;        // ȿ���� ����� �ҽ�

    void Awake()
    {
        mSFx = GameObject.Find("SFx").GetComponent<AudioSource>();        
    }

    void Update()
    {
        // ���� �������� ������Ʈ
        if (sBGM && sSFx) // null check
        {
            sBGM.value = SceneData.Instance.BGMValue;
            sSFx.value = SceneData.Instance.SFxValue;
        }
        if (tBgmValue && tSfxValue) // null check
        {
            // �Ҽ��� -2 �ڸ����� �ݿø�
            tBgmValue.text = Mathf.Ceil(sBGM.value * 100).ToString();
            tSfxValue.text = Mathf.Ceil(sSFx.value * 100).ToString();
        }
    }

    /*
     * BUTTON�� �Ҵ��� �޼ҵ�
     */

    public void NewGame()
    {        
        mSFx.Play();

        SceneManager.LoadScene("Space Of Weather");

        if (Directory.Exists(Application.dataPath + "/Data/")) // Data ������ ������ �����ϱ�
        {
            Debug.Log("����");
            Directory.Delete(Application.dataPath + "/Data/", true);
        }
        // �����͸� �ʱ�ȭ ��Ű�� �Լ� ȣ���� �ʿ� ����
        // �� Ŭ���� �����ڿ��� �ڵ� �ʱ�ȭ�ȴ�.
    }

    public void ContinueGame()
    {
        mSFx.Play();

        /*
         ����� �� �ѹ� �ε�         
         */

        // newtonsoft library (�������̺�� ��ӵ� Ŭ���� ��� �Ұ���, ��ųʸ� ��� ����)
        // �ε��ϴ� �Լ� ȣ�� �Ŀ� �� �� �ε����� �̵�
        FileStream fSceneBuildIndexStream 
            // �ش� ��ο� �ִ� json ������ ����
            = new FileStream(Application.dataPath + "/Data/SceneBuildIndex.json", FileMode.Open);
        // �����ִ� json ������ byte�迭�� �ִ´�
        byte[] bSceneData = new byte[fSceneBuildIndexStream.Length];
        // ������ �д´�
        fSceneBuildIndexStream.Read(bSceneData, 0, bSceneData.Length);
        fSceneBuildIndexStream.Close();
        // ���ڿ��� ��ȯ�Ѵ�
        string sSceneData = Encoding.UTF8.GetString(bSceneData);

        /*
         ����� ��¥ �ð� ���� �ε�         
         */

        // jsonUitlity (�������̺�� ��ӵ� Ŭ���� ��� ����, ��ųʸ� ��� �Ұ���)
        string mSeasonDatePath = Path.Combine(Application.dataPath + "/Data/", "SeasonDate.json");

        if (File.Exists(mSeasonDatePath)) // null check
        {
            // ���ο� ������Ʈ�� �����ϰ�
            GameObject gSeasonDate = new GameObject();
            string sDateData = File.ReadAllText(mSeasonDatePath);

            Debug.Log(sDateData);
            
            // �����͸� ���ο� ������Ʈ�� ������
            JsonUtility.FromJsonOverwrite(sDateData, gSeasonDate.AddComponent<SeasonDateCalc>());

            // �������(�����) �����͸� ���� ���Ǵ� �����Ϳ� �����ϸ� �ε� ��!
            SeasonDateCalc.Instance.mSecond = gSeasonDate.GetComponent<SeasonDateCalc>().mSecond;
            SeasonDateCalc.Instance.mDay = gSeasonDate.GetComponent<SeasonDateCalc>().mDay;
            SeasonDateCalc.Instance.mSeason = gSeasonDate.GetComponent<SeasonDateCalc>().mSeason;
            SeasonDateCalc.Instance.mYear = gSeasonDate.GetComponent<SeasonDateCalc>().mYear;
        }
        
        // ���ڿ��� int������ �Ľ��ؼ� ���� �ε����� Ȱ���Ѵ�
        SceneManager.LoadScene(int.Parse(sSceneData));
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

    // ���� ����
    public void QuitGame()
    {
        mSFx.Play();
        Application.Quit();
    }

    public void GoCredit()
    {
        // ũ���� ȭ������ ��ȯ
        Debug.Log("ũ����ȭ������ ��ȯ");
    }
    // �����ϱ� ���â
    public void ActiveWarning()
    {
        gWarning.SetActive(true);
    }
    public void UnAcitveWarning()
    {
        gWarning.SetActive(false);
    }
}

