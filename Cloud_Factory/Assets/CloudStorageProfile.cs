using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CloudStorageProfile : MonoBehaviour
{

    public GameObject[] Profiles;
    SOWManager SOWManager;
    Guest GuestManager;
    RecordUIManager UIManager;

    [SerializeField]
    Image[] iPortrait = new Image[20];

    public Button btNextBtn;           // 다음페이지 버튼
    public Button btPrevBtn;           // 이전페이지 버튼
    public Button btGiveBtn;           // 구름 제공 버튼

    // 가장 앞에 있는 프로필의 인덱스 정보
    [SerializeField]
    int frontProfileInfo;
    public GameObject mGetCloudContainer;

    // 화면상에 나오고 있는 손님의 손님번호
    int frontGuestIndex;

    // 구름을 제공받을 수 있는 손님들의 손님번호 리스트
    int[] UsingGuestNumList;

    // 구름을 제공받을 수 있는 손님들의 리스트의 길이
    int numOfUsingGuestList;

    void Awake()
    {
        SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        GuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        UIManager = GameObject.Find("UIManager").GetComponent<RecordUIManager>();

        UsingGuestNumList = SOWManager.mUsingGuestList.ToArray();
        numOfUsingGuestList = SOWManager.mUsingGuestList.Count;

        frontProfileInfo = 0;

        if (numOfUsingGuestList != 0)
            frontGuestIndex = UsingGuestNumList[0];

        Profiles = new GameObject[3];

        Profiles[0] = GameObject.Find("I_ProfileBG1");
        Profiles[1] = GameObject.Find("I_ProfileBG2");
        Profiles[2] = GameObject.Find("I_ProfileBG3");

        btGiveBtn = GameObject.Find("B_CloudGIve").GetComponent<Button>();

        initProfileList();
    }

    public void GetNextProfile()
    {
        // 이전 프로필을 불러온다.
        frontProfileInfo++;
        frontGuestIndex++;

        if (frontProfileInfo >= 3)
        {
            frontProfileInfo = 0;
        }
        if (frontGuestIndex >= numOfUsingGuestList)
        {
            frontGuestIndex = 0;
        }

        updateProfileList();
    }

    public void GetPrevProfile()
    {
        // 다음 프로필을 불러온다.
        frontProfileInfo--;
        frontGuestIndex--;

        if (frontProfileInfo < 0)
        {
            frontProfileInfo = 2;
        }
        if (frontGuestIndex < 0)
        {
            frontGuestIndex = numOfUsingGuestList - 1;
        }

        updateProfileList();
    }

    void initProfileList()
    {
        if (numOfUsingGuestList == 0)
        {
            // 손님이 없는 경우 구름 제공버튼 무효
            btGiveBtn.interactable = false;
        }

        updateProfileList();
    }

    void updateProfileList()
    {
        // 손님이 없는 경우에는 정보 업데이트를 하지 않는다.
        if (numOfUsingGuestList == 0) return;

        GameObject Profile = Profiles[frontProfileInfo];

        // Image
        Image iProfile = Profile.transform.GetChild(0).GetComponent<Image>();

        // Button
        btGiveBtn = Profile.transform.GetChild(1).GetComponent<Button>();

        // 나이 이름 직업
        Text tName = Profile.transform.GetChild(7).GetComponent<Text>();
        Text tAge = Profile.transform.GetChild(8).GetComponent<Text>();
        Text tJob = Profile.transform.GetChild(9).GetComponent<Text>();

        // 만족도, 한 줄 요약
        Text tSat = Profile.transform.GetChild(10).GetComponent<Text>();
        Text tSentence = Profile.transform.GetChild(11).GetComponent<Text>();

        // 뭉티 정보를 가져온다.
        GuestInfo info = GuestManager.GetComponent<Guest>().mGuestInfos[frontGuestIndex];

        // 가져온 정보값을 이용하여 채운다.
        iProfile.sprite = UIManager.sBasicProfile[frontGuestIndex];
        tName.text = info.mName;
        tAge.text = "" + info.mAge;
        tJob.text = info.mJob;

        tSat.text = "" + info.mSatatisfaction;

        // Sentence 관련 업데이트도 필요함 -> 추후 수정


    }

    void updateButton()
    {

    }

    public void GiveCloud()
    {
        SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

        // 구름 제공하는 메소드 호출
        StoragedCloudData storagedCloudData
            = mGetCloudContainer.GetComponent<CloudContainer>().mSelecedCloud;

        int[] tempList = SOWManager.mUsingGuestList.ToArray();

        int guestNum = frontGuestIndex;

        SOWManager.SetCloudData(guestNum, storagedCloudData);

        SceneManager.LoadScene("Space Of Weather");

        Debug.Log("구름제공 메소드 호출");
    }
}