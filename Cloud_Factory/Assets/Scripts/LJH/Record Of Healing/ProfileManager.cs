using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [Header("Profile")]
    public GameObject[] iProfileBG = new GameObject[3];         // 프로필 종이 3장 참조
    private GameObject tempProfile;                             // 프로필 변경 시 임시로 저장할 공간
    private Image[] sProfileColor = new Image[3];               // 프로필 종이의 색깔을 저장할 이미지 스크립트 참조
    private Color[] cProfileColor = new Color[3];               // 각 프로필 종이의 색깔 저장

    public GameObject tDialogText;

    [Header("Profile BackGround")]
    public Sprite sProfileBG;
    public Sprite sDialogBG;
    public Sprite sUpsetBG;
    public Sprite sUpsetDialogBG;

    public Image dialogBGImage;

    [Header("Profile Image")]
    public Sprite[] sCuredProfile = new Sprite[20]; // 치유된 프로필
	public Sprite[] sBasicProfile = new Sprite[20]; // 기본 프로필
	public Sprite[] sUpsetProfile = new Sprite[20]; // 화난 프로필

	[Header("Upset Stamp")]
	public GameObject[] iCloudStamp = new GameObject[3];        // 제공받은 구름에 표시할 스탬프
	public GameObject iDialogStamp;                             // dialog에 표시할 스탬프
	private GameObject tempStamp;



	private int[] disSatGuestList;      // 불만 뭉티의 번호들을 저장할 배열

    private GuestInfos[] mGuestInfo;    // GuestManager로 부터 받은 Guest Info

    private int nextProfileIndex;       // next 버튼 클릭시 추가할 손님의 번호
    private int prevProfileIndex;       // prev 버튼 클릭시 추가할 손님의 번호

    private bool isUpset;               // '불만 뭉티만 보기'가 활성화 되었는지 확인하는 불 변수

	Guest mGuestManager;
    RecordUIManager mUIManager;

	void Awake()
    {
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mUIManager = GameObject.Find("UIManager").GetComponent<RecordUIManager>();

        mGuestInfo = mGuestManager.mGuestInfo;

        for(int num = 0; num < 3; num++)
        {
            sProfileColor[num] = iProfileBG[num].GetComponent<Image>();
			cProfileColor[num] = sProfileColor[num].color;

		}
		InitProfile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 초기 프로필 설정
    public void InitProfile()
    {
		isUpset = false;

		for (int num = 0; num < 3; num++) {
            ChangeProfileInfo(num, num); 
            nextProfileIndex = 3; 
            prevProfileIndex = 19; 
        }

        // 제일 앞에있는 프로필의 뭉티가 불만 뭉티일 경우, dialog 종이 변경, 아닐시 원래대로 돌리기
        UpdateDialogPaper();

		// GuestManager에서 불만 뭉티의 수와 목록을 받아 저장함
		disSatGuestList = new int[mGuestManager.DisSatGuestList().Length];
		disSatGuestList = mGuestManager.DisSatGuestList();
	}

    // 불만 뭉티만 보기 버튼을 눌렀을 때 실행
    public void InitUpsetProfile()
    {
        isUpset = true;

        // 버튼을 누르면 불만 뭉티 리스트를 최신화
        disSatGuestList = new int[mGuestManager.DisSatGuestList().Length];
        disSatGuestList = mGuestManager.DisSatGuestList();

        // 불만 뭉티가 없을 때, 프로필 비워서 내보내기
        if (disSatGuestList.Length <= 0) { for (int num = 0; num < 3; num++) { ChangeProfileEmpty(num); } }
        else {
            // Profile Index의 초기값을 찾아가는 과정, nextProfileIndex의 경우는 나올 수 있는 불만 뭉티의 수가 다양하기 때문에 동적으로 설정
            prevProfileIndex = disSatGuestList.Length - 1;
            nextProfileIndex = prevProfileIndex;
            for (int num = 0; num < 4; num++)
            {
                nextProfileIndex++;
                CheckIndexInRange(disSatGuestList.Length);
                if (num < 3)
                {
                    ChangeProfileInfo(num, disSatGuestList[nextProfileIndex]);
                }
            }
        }
    }

    public void UpdateNextProfile()
    {
        if (isUpset) { Invoke("InvokeUpdateUpsetNextProfile", 0.18f); }
        else { Invoke("InvokeUpdateAllNextProfile", 0.7f); }
    }

    public void UpdatePrevProfile()
    {
        if (isUpset) { Invoke("InvokeUpdateUpsetPrevProfile", 0.18f); }
        else { Invoke("InvokeUpdateAllPrevProfile", 0.18f); }
    }

    // next button클릭시 프로필 업데이트
    private void InvokeUpdateAllNextProfile()
    {
        //next, prev 손님의 번호가 범위를 벗어나지 않도록 조정
		CheckIndexInRange(mGuestInfo.Length);

        //맨 앞 프로필 종이: iProfileBG[0]으로 고정이 되도록 교환
        SwapProfile(0, 2);

        //맨 뒤로가는 종이의 정보 갱신
		ChangeProfileInfo(2, nextProfileIndex);

        UpdateDialogPaper();

		nextProfileIndex++;
		prevProfileIndex++;
	}

    // prev button클릭시 프로필 업데이트
    private void InvokeUpdateAllPrevProfile()
    {
        CheckIndexInRange(mGuestInfo.Length);

        SwapProfile(2, 0);

		ChangeProfileInfo(0, prevProfileIndex);

        UpdateDialogPaper();

		nextProfileIndex--;
		prevProfileIndex--;
	}

    // 불만 뭉티만 표시할 경우 disSatGuestList를 참조하기 때문에 전체 뭉티 표시와 같이 사용하기 위해선 인수를 받아야 함 >> invoke 사용 불가능으로 함수 분리
    private void InvokeUpdateUpsetNextProfile()
    {
        CheckIndexInRange(disSatGuestList.Length);

        SwapProfile(0, 2);

        // 불만 뭉티만 표시할 경우 빈 프로필이 발생할 수 있으므로 체크 해줌
		if (disSatGuestList.Length <= 0) { ChangeProfileEmpty(2); }
        else { ChangeProfileInfo(2, disSatGuestList[nextProfileIndex]); }

		nextProfileIndex++;
		prevProfileIndex++;
	}

	private void InvokeUpdateUpsetPrevProfile()
	{
		CheckIndexInRange(disSatGuestList.Length);

        SwapProfile(2, 0);

		if (disSatGuestList.Length <= 0) { ChangeProfileEmpty(0); }
		else { ChangeProfileInfo(0, disSatGuestList[prevProfileIndex]); }

		nextProfileIndex--;
		prevProfileIndex--;
	}

    // 다음으로 추가되어야 할 프로필의 index가 배열 내에 있는 index인지 체크 후, 범위를 넘어갈 경우 변경해줌
	private void CheckIndexInRange(int max_list_index)
    {
		if (nextProfileIndex >= max_list_index) { nextProfileIndex = 0; }
		else if (nextProfileIndex < 0) { nextProfileIndex = max_list_index - 1; }
		if (prevProfileIndex < 0) { prevProfileIndex = max_list_index - 1; }
		else if (prevProfileIndex >= max_list_index) { prevProfileIndex = 0; }
	}

    private void ChangeProfileInfo(int profile_num, int profile_index)
    {
        // 프로필 정보 변경
		if (iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.activeSelf == false)
		        { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.SetActive(true); }

        // 뭉티의 상태(치유, 불만, 기본)에 따른 이미지 변경
        if (mGuestInfo[profile_index].isDisSat) { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = sUpsetProfile[profile_index]; }
        else if (mGuestInfo[profile_index].isCure) { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = sCuredProfile[profile_index]; }
        else { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = sBasicProfile[profile_index]; }

		iProfileBG[profile_num].transform.Find("T_Name(INPUT)").gameObject.GetComponent<Text>().text = mGuestInfo[profile_index].mName;
		iProfileBG[profile_num].transform.Find("T_Age(INPUT)").gameObject.GetComponent<Text>().text = mGuestInfo[profile_index].mAge.ToString();
		iProfileBG[profile_num].transform.Find("T_Job(INPUT)").gameObject.GetComponent<Text>().text = mGuestInfo[profile_index].mJob;

        // 뭉티의 상태에 따른 프로필 종이 종류, 색깔 변경
        // 불만 뭉티일 경우
        if (mGuestInfo[profile_index].isDisSat) {
			iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = sUpsetProfile[profile_index];
			iProfileBG[profile_num].GetComponent<Image>().sprite = sUpsetBG; 
            iProfileBG[profile_num].GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f); 
        }
        // 정상적인 뭉티일 경우
        else { 
            iProfileBG[profile_num].GetComponent<Image>().sprite = sProfileBG; 
            for(int num = 0; num < 3; num++)
            {
                if (iProfileBG[profile_num].GetComponent<Image>() == sProfileColor[num])
                {
                    iProfileBG[profile_num].GetComponent<Image>().color = cProfileColor[num];
                    break;
                }
            }
        }

        // 해당 뭉티가 불만 뭉티일 경우 스탬프 출력
		if (mGuestInfo[profile_index].isDisSat && !iCloudStamp[profile_num].activeSelf) { iCloudStamp[profile_num].SetActive(true); }
        else if (!mGuestInfo[profile_index].isDisSat && iCloudStamp[profile_num].activeSelf) { iCloudStamp[profile_num].SetActive(false); }
	}

	// 뭉티 리스트가 비어있는 경우(불만 뭉티가 한 명도 없는 경우)에 출력: 불만 뭉티만 표시 전용
	private void ChangeProfileEmpty(int profile_num)
    {
        if (iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.activeSelf == true) 
                { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.SetActive(false); }
		iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = null;
		iProfileBG[profile_num].transform.Find("T_Name(INPUT)").gameObject.GetComponent<Text>().text = " ";
		iProfileBG[profile_num].transform.Find("T_Age(INPUT)").gameObject.GetComponent<Text>().text = " ";
		iProfileBG[profile_num].transform.Find("T_Job(INPUT)").gameObject.GetComponent<Text>().text = " ";
	}

    // dialog text를 불만 뭉티인지 아닌지에 따라 출력 여부 결정
    private void UpdateDialogPaper()
    {
		if (iCloudStamp[0].activeSelf)
		{
			iDialogStamp.SetActive(true);
			tDialogText.SetActive(false);
			dialogBGImage.sprite = sUpsetDialogBG;
		}
		else
		{
			iDialogStamp.SetActive(false);
			tDialogText.SetActive(true);
			dialogBGImage.sprite = sDialogBG;
		}
	}

    // Swap iProfileBG, iCloudStamp
    private void SwapProfile(int start_num, int end_num)
    {
        GameObject temp_object;
        if(start_num < end_num)
        {
            temp_object = iProfileBG[start_num];
            for(int num = start_num; num < end_num; num++) { iProfileBG[num] = iProfileBG[num + 1]; }
            iProfileBG[end_num] = temp_object;

			temp_object = iCloudStamp[start_num];
			for (int num = start_num; num < end_num; num++) { iCloudStamp[num] = iCloudStamp[num + 1]; }
			iCloudStamp[end_num] = temp_object;
		}
        else
        {
			temp_object = iProfileBG[start_num];
			for (int num = start_num; num > end_num; num--) { iProfileBG[num] = iProfileBG[num - 1]; }
			iProfileBG[end_num] = temp_object;

			temp_object = iCloudStamp[start_num];
			for (int num = start_num; num > end_num; num--) { iCloudStamp[num] = iCloudStamp[num - 1]; }
			iCloudStamp[end_num] = temp_object;
		}
	}
}
