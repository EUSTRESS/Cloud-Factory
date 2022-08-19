using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// ���� UI ���
// ����, ��¥ ������Ʈ
// �� �̵�
public class CommonUIManager : MonoBehaviour
{
    [Header("GAME OBJECT")]
    // ������Ʈ Active ����
    public GameObject   gOption;       // �ɼ� ���� ������Ʈ
    public GameObject   gGuideBook;

    [Header("TEXT")]
    public Text         tDate;         // ��¥ �ؽ�Ʈ
    public Text         tYear;         // ��¥ �ؽ�Ʈ
    [Space (3f)]
    public Text         tBgmValue;     // BGM ���� �ؽ�Ʈ
    public Text         tSfxValue;     // SFx ���� �ؽ�Ʈ

    [Header("SLIDER")]
    public Slider       sBGM;          // BGM �����̴�
    public Slider       sSFx;          // SFx �����̴�

    [Header("SPRITES")]
    public Sprite[]     sSeasons = new Sprite[4]; // �� ���� ���� �ܿ� �޷�

    [Header("IMAGES")]
    public Image        iSeasons;      // �޷� �̹���

    private AudioSource mSFx;          // ȿ���� ����� �ҽ� ����
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Lobby" ||
            SceneManager.GetActiveScene().name == "Space Of Weather" ||
            SceneManager.GetActiveScene().name == "Drawing Room" ||
            SceneManager.GetActiveScene().name == "Cloud Factory")
            mSFx = GameObject.Find("mSFx").GetComponent<AudioSource>();        
    }

    void Update()
    {
        if (tDate && tYear && iSeasons && SeasonDateCalc.Instance) // null check
        {
            tDate.text = SeasonDateCalc.Instance.mDay.ToString() + "��";
            tYear.text = SeasonDateCalc.Instance.mYear.ToString() + "����";

            if (SeasonDateCalc.Instance.mSeason == 1)
                iSeasons.sprite = sSeasons[0]; // ��
            else if (SeasonDateCalc.Instance.mSeason == 2)
                iSeasons.sprite = sSeasons[1]; // ����
            else if (SeasonDateCalc.Instance.mSeason == 3)
                iSeasons.sprite = sSeasons[2]; // ����
            else if (SeasonDateCalc.Instance.mSeason == 4)
                iSeasons.sprite = sSeasons[3]; // �ܿ�
        }

        // ���� �������� ������Ʈ
        if (sBGM && sSFx && SceneData.Instance)           // null check
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
    
    // �� �̵� ��ư��
    public void GoSpaceOfWeather()
    {        
        LoadingSceneController.Instance.LoadScene("Space Of Weather");
    }
    public void GoCloudFactory()
    {
        LoadingSceneController.Instance.LoadScene("Cloud Factory");
    }
    public void GoDrawingRoom()
    {
        LoadingSceneController.Instance.LoadScene("Drawing Room");
    }
    public void GoRecordOfHealing()
    {
        // ġ���� ��� ���� �� �ε��� ����
        SceneData.Instance.prevSceneIndex = SceneManager.GetActiveScene().buildIndex;

        LoadingSceneController.Instance.LoadScene("Record Of Healing");
    }
    public void GoPrevScene()
    {
        // ġ���� ����� ������ ������ �̵�   
        LoadingSceneController.Instance.LoadScene(SceneData.Instance.prevSceneIndex);
    }
    public void GoGiveCloud()
    {
        LoadingSceneController.Instance.LoadScene("Give Cloud");
        GameObject.Find("InventoryManager").GetComponent<InventoryManager>().go2CloudFacBtn();
    }
    public void GoCloudStorage()
    {
        LoadingSceneController.Instance.LoadScene("Cloud Storage");
    }

    // �ɼ�â Ȱ��ȭ, ��Ȱ��ȭ
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
    public void ActiveGuideBook()
    {
        gGuideBook.SetActive(true);
    }
    public void UnActiveGuideBook()
    {
        gGuideBook.SetActive(false);
    }

    // ���� ����
    public void QuitGame()
    {
        mSFx.Play();
        Application.Quit();
    }
}
