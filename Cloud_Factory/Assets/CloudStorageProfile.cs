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

    // 가장 앞에 있는 프로필 오브젝트의 인덱스 정보
    [SerializeField]
    int frontProfileInfo;
    public GameObject mGetCloudContainer;

    // 화면상에 나오고 있는 손님의 손님번호
    int frontGuestIndex;

    // 구름을 제공받을 수 있는 손님들의 손님번호 리스트
    [SerializeField]
    int[]   UsingGuestNumList;
    int     UsingGuestIndex;

    // 구름을 제공받을 수 있는 손님들의 리스트의 길이
    int numOfUsingGuestList;

    void Awake()
    {
        SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        GuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        UIManager = GameObject.Find("UIManager").GetComponent<RecordUIManager>();

        // 착석중인 손님 중, 구름을 제공받지 못한 손님만 제공 가능 리스트에 넣는다.
        List<int> temp = new List<int>();
        foreach(int i in SOWManager.mUsingGuestList)
        {
            if (GuestManager.mGuestInfo[i].isUsing == false)
            { 
                temp.Add(i);
                Debug.Log(i + "번 손님은 구름 제공이 가능합니다.");
            }
            else
            {
                Debug.Log(i + "번 손님은 구름 제공이 가능하지 않습니다.");
            }
        }
        UsingGuestNumList = temp.ToArray();

        numOfUsingGuestList = UsingGuestNumList.Length;

        frontProfileInfo = 0;
        UsingGuestIndex = 0;

        if (numOfUsingGuestList != 0)
            frontGuestIndex = UsingGuestNumList[UsingGuestIndex];

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
        frontProfileInfo = (frontProfileInfo + 1 ) % 3;
        UsingGuestIndex++;

        // 맨앞에 나온 손님의 인덱스가 
        if (UsingGuestIndex >= numOfUsingGuestList)
        {
            UsingGuestIndex = 0;
        }
        frontGuestIndex = UsingGuestNumList[UsingGuestIndex];

        updateProfileList();
    }

    public void GetPrevProfile()
    {
        // 다음 프로필을 불러온다.
        frontProfileInfo = (frontProfileInfo - 1 + 3) % 3;
        UsingGuestIndex--;
        
        if (UsingGuestIndex < 0)
        {
            UsingGuestIndex = numOfUsingGuestList - 1;
        }

        frontGuestIndex = UsingGuestNumList[UsingGuestIndex];
        updateProfileList();
    }

    void initProfileList()
    {
        updateProfileList();
    }

    void updateProfileList()
    {

        GameObject Profile = Profiles[frontProfileInfo];
        // Button
        btGiveBtn = Profile.transform.GetChild(1).GetComponent<Button>();

        // 손님이 없는 경우에는 정보 업데이트를 하지 않는다.
        if (numOfUsingGuestList == 0)
        {
            btGiveBtn.interactable = false;
            Debug.Log("구름제공이 가능한 손님이 존재하지 않습니다.");
            return;   
        }

        // Image
        Image iProfile = Profile.transform.GetChild(0).GetComponent<Image>();

        // 나이 이름 직업
        Text tName = Profile.transform.GetChild(7).GetComponent<Text>();
        Text tAge = Profile.transform.GetChild(8).GetComponent<Text>();
        Text tJob = Profile.transform.GetChild(9).GetComponent<Text>();

        // 만족도, 한 줄 요약
        Text tSat = Profile.transform.GetChild(10).GetComponent<Text>();
        Text tSentence = Profile.transform.GetChild(11).GetComponent<Text>();

        // 뭉티 정보를 가져온다.
        GuestInfos info = GuestManager.GetComponent<Guest>().mGuestInfo[frontGuestIndex];

        // 가져온 정보값을 이용하여 채운다.
        iProfile.sprite = UIManager.sBasicProfile[frontGuestIndex];
        tName.text = info.mName;
        tAge.text = "" + info.mAge;
        tJob.text = info.mJob;


        tSat.text = "" + info.mSatatisfaction;
		tSentence.text = info.mSentence;

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

        //구름 제공 예외처리
        if (storagedCloudData == null)
        {
            return;
        }

        int guestNum = frontGuestIndex;
        GuestManager.mGuestInfo[guestNum].isUsing = true;

        //구름인벤토리에서 사용된 구름 삭제
        mGetCloudContainer.GetComponent<CloudContainer>().deleteSelectedCloud();

        // 리스트에서 사용받은 손님 제거하기
        SOWManager sow = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        int count = sow.mUsingGuestList.Count;

        SOWManager.SetCloudData(guestNum, storagedCloudData);

        SceneManager.LoadScene("Space Of Weather");

        Debug.Log("구름제공 메소드 호출");
    }
}