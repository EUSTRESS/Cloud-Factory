using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using AESWithJava.Con;
using System;

// �κ� �� UI ���
// ���� â, �����ϱ�, �̾��ϱ�
public class LobbyUIManager : MonoBehaviour
{
    private SeasonDateCalc mSeason; // ���� ��ũ��Ʈ

    [Header("GAME OBJECT")]
    // ������Ʈ Active ����
    public GameObject   gOption;     // �ɼ� ���� ������Ʈ
    public GameObject   gWarning;    // ���ο� ���� ���â

    // INDEX -> [0]: C04 [1]: C07 [2]: C10 [3]:C13 [4]:C14
    public GameObject[] gSpringMoongti = new GameObject[5]; // �� Ÿ��Ʋ ��Ƽ ����

    [Header("TEXT")]
    public Text         tBgmValue;   // BGM ���� �ؽ�Ʈ
    public Text         tSfxValue;   // SFx ���� �ؽ�Ʈ

    [Header("SLIDER")]
    public Slider       sBGM;        // BGM �����̴�
    public Slider       sSFx;        // SFx �����̴�

    private AudioSource mSFx;        // ȿ���� ����� �ҽ�

    [Header("BOOL")]
    public bool[] bSpringMoongti = new bool[5]; // �� Ÿ��Ʋ ��Ƽ Bool�� ������ 5 ����

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

        switch (mSeason.mSeason)
        {
            case 1: // ��
                // �� ��Ƽ ������ 5 ����
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
            case 2: // ����
                break;
            case 3: // ����
                break;
            case 4: // �ܿ�
                break;
            default:
                break;
        }
    }

    /*
     * BUTTON�� �Ҵ��� �޼ҵ�
     */

    public void NewGame()
    {        
        mSFx.Play();

        LoadingSceneController.Instance.LoadScene("Space Of Weather");

        // �����͸� �ʱ�ȭ ��Ű�� �Լ� ȣ���� �ʿ� ����
        // �� Ŭ���� �����ڿ��� �ڵ� �ʱ�ȭ�ȴ�.
    }

    public void ContinueGame()
    {
        String key = "key"; // ��ȣȭ ��ȣȭ Ű ��

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
        // ��ȣȭ
        sSceneData = AESWithJava.Con.Program.Decrypt(sSceneData, key);

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
            // ��ȣȭ
            //sDateData = AESWithJava.Con.Program.Decrypt(sDateData, key);

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
        LoadingSceneController.Instance.LoadScene(int.Parse(sSceneData));
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

    // �ѿ� ������ ������ ��ȯ ��ȯ�� �� �ش� �� �ε������� Ȱ���ؼ� ��ȯ
    // ��ȯ�Ǹ� bool���� Ȱ�����ؼ� ����->����, �ѱ�->�ѱ۷� �̿�
    public void ChangeKor()
    {
        // �ϴ��� �κ� �Ǵϱ� 
        if (SceneManager.GetActiveScene().name == "Eng_Lobby")
            SceneManager.LoadScene("Lobby");
    }
    public void ChangeEng()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
            SceneManager.LoadScene("Eng_Lobby");
    }
}

