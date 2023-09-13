using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [Header("Profile")]
    public  GameObject[] iProfileBG = new GameObject[3];         // 프로필 종이 3장 참조
    private GameObject tempProfile;                             // 프로필 변경 시 임시로 저장할 공간
    private Image[] sProfileColor = new Image[3];               // 프로필 종이의 색깔을 저장할 이미지 스크립트 참조
    private Color[] cProfileColor = new Color[3];               // 각 프로필 종이의 색깔 저장
    private List<List<GameObject>> usedCloudObject = new List<List<GameObject>>();  //각 프로필에서 표시할 사용한 구름의 정보 저장
    private int[] profileGuestNum = new int[3];

    public GameObject tDialog;
    public Text tDialogText;

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

	private bool isClickedPrev;
    private bool isClickedNext;



	private int[] disSatGuestList;      // 불만 뭉티의 번호들을 저장할 배열

    private GuestInfos[] mGuestInfo;    // GuestManager로 부터 받은 Guest Info

    private int nextProfileIndex;       // next 버튼 클릭시 추가할 손님의 번호
    private int prevProfileIndex;       // prev 버튼 클릭시 추가할 손님의 번호

    private bool isUpset;               // '불만 뭉티만 보기'가 활성화 되었는지 확인하는 불 변수

	Guest mGuestManager;
    RecordUIManager mUIManager;
    VirtualObjectManager mVirtualObjectManager;
    RLHReader mRLHReader;

	void Awake()
    {
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mUIManager = GameObject.Find("UIManager").GetComponent<RecordUIManager>();
		mVirtualObjectManager = GameObject.Find("VirtualObjectManager").GetComponent<VirtualObjectManager>();
        mRLHReader = GameObject.Find("UIManager").GetComponent<RLHReader>();

		mGuestInfo = mGuestManager.mGuestInfo;

        isClickedNext = false;
        isClickedPrev = false;

        for(int num = 0; num < 3; num++)
        {
			usedCloudObject.Add(new List<GameObject>());
			sProfileColor[num] = iProfileBG[num].GetComponent<Image>();
			cProfileColor[num] = sProfileColor[num].color;
		}

		InitProfile();
	}

	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
    {
    	UpdateDialogPaper();
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

    private void ChangeProfileInfo(int profile_num, int guest_index)
    {
        profileGuestNum[profile_num] = guest_index;

        // Demo Version
        if(mGuestInfo[guest_index].isDisSat == false && mGuestInfo[guest_index].isCure == false)
        {
			if (iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.activeSelf == true)
            { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.SetActive(false); }
			iProfileBG[profile_num].transform.Find("Name").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "";
			iProfileBG[profile_num].transform.Find("Age").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "";
			iProfileBG[profile_num].transform.Find("Job").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "";

			clearUsedCloudList(profile_num);

			iProfileBG[profile_num].GetComponent<Image>().sprite = sProfileBG;
			for (int num = 0; num < 3; num++)
			{
				if (iProfileBG[profile_num].GetComponent<Image>() == sProfileColor[num])
				{
					iProfileBG[profile_num].GetComponent<Image>().color = cProfileColor[num];
					break;
				}
			}
            return;
		}

        // 프로필 정보 변경
		if (iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.activeSelf == false)
		        { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.SetActive(true); }

        // 뭉티의 상태(치유, 불만, 기본)에 따른 이미지 변경
        if (mGuestInfo[guest_index].isDisSat) { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = sUpsetProfile[guest_index]; }
        else if (mGuestInfo[guest_index].isCure) { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = sCuredProfile[guest_index]; }
        else { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = sBasicProfile[guest_index]; }

        if (LanguageManager.GetInstance().GetCurrentLanguage() == "English")
        {
	        iProfileBG[profile_num].transform.Find("Name").transform.GetChild(0).transform.GetChild(0).transform
			        .GetChild(1).GetComponent<Text>().text =
		        mGuestInfo[guest_index].mNameEN;
	        iProfileBG[profile_num].transform.Find("Job").transform.GetChild(0).transform.GetChild(0).transform
			        .GetChild(1).GetComponent<Text>().text =
		        mGuestInfo[guest_index].mJobEN;
        }
        else
        {
	        iProfileBG[profile_num].transform.Find("Name").transform.GetChild(0).transform.GetChild(0).transform
			        .GetChild(1).GetComponent<Text>().text =
		        mGuestInfo[guest_index].mName;
	        iProfileBG[profile_num].transform.Find("Job").transform.GetChild(0).transform.GetChild(0).transform
			        .GetChild(1).GetComponent<Text>().text =
		        mGuestInfo[guest_index].mJob;
        }

	    iProfileBG[profile_num].transform.Find("Age").transform.GetChild(0).transform.GetChild(0).transform
			    .GetChild(1).GetComponent<Text>().text = mGuestInfo[guest_index].mAge.ToString();

	    // 해당 프로필 종이의 사용한 구름의 정보 제거
        clearUsedCloudList(profile_num);
        
        // 사용한 구름의 정보 업데이트
        GameObject cloudPos = iProfileBG[profile_num].transform.Find("I_UsedCloud(INPUT)").gameObject.transform.GetChild(0).GetChild(0).gameObject;
		int posx = 240, posy = -100;
		for (int count = 0; count < mGuestInfo[guest_index].mUsedCloud.Count; count++)
        {
			GameObject tempObject = mVirtualObjectManager.InstantiateVirtualObjectToScene(mGuestInfo[guest_index].mUsedCloud[count], new Vector3(0f, 0f, 0f));
            tempObject.transform.SetParent(cloudPos.transform);
			tempObject.transform.localPosition = new Vector3(posx, posy, 0f);
			tempObject.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);

            usedCloudObject[profile_num].Add(tempObject);

			posx += 160;
		}


        // 뭉티의 상태에 따른 프로필 종이 종류, 색깔 변경
        // 불만 뭉티일 경우
        if (mGuestInfo[guest_index].isDisSat) {
			iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = sUpsetProfile[guest_index];
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
    }

    private void clearUsedCloudList(int profile_num)
    {
        for(int i = 0; i < usedCloudObject[profile_num].Count; i++)
        {
            Destroy(usedCloudObject[profile_num][i].gameObject);
        }
        usedCloudObject[profile_num].Clear();
    }

	// 뭉티 리스트가 비어있는 경우(불만 뭉티가 한 명도 없는 경우)에 출력: 불만 뭉티만 표시 전용
	private void ChangeProfileEmpty(int profile_num)
    {
        if (iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.activeSelf == true) 
                { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.SetActive(false); }
		iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = null;
		iProfileBG[profile_num].transform.Find("Name").transform.GetChild(0).transform.GetChild(0).transform
			.GetChild(1).GetComponent<Text>().text = " ";
		iProfileBG[profile_num].transform.Find("Job").transform.GetChild(0).transform.GetChild(0).transform
			.GetChild(1).GetComponent<Text>().text = " ";
		iProfileBG[profile_num].transform.Find("Age").transform.GetChild(0).transform.GetChild(0).transform
			.GetChild(1).GetComponent<Text>().text = " ";
    }

    // dialog text를 불만 뭉티인지 아닌지에 따라 출력 여부 결정
    // dialog text를 맨 앞 손님의 번호에 맞게 업데이트
    private void UpdateDialogPaper()
    {
        // Demo Version
        if (mGuestInfo[profileGuestNum[0]].isDisSat == false && mGuestInfo[profileGuestNum[0]].isCure == false)
        {
	        if(LanguageManager.GetInstance().GetCurrentLanguage() == "Korean") { tDialogText.text = "아직 충분한 데이터가 모이지 않았습니다."; } 
	        else{ tDialogText.text = "Not enough data's been collected yet."; }
        }
        else { tDialogText.text = mRLHReader.LoadRecordInfo(profileGuestNum[0]); }

        if (isUpset)
        {
	        tDialog.SetActive(false);
	        dialogBGImage.sprite = sUpsetDialogBG;
        }
        else
        {
	        tDialog.SetActive(true);
	        dialogBGImage.sprite = sDialogBG;
        }
    }

    // Swap iProfileBG, iCloudStamp
    private void SwapProfile(int start_num, int end_num)
    {
        GameObject temp_object;
        List<GameObject> temp_cloud_list;
        int temp_guest_num;
        if(start_num < end_num)
        {
            temp_object = iProfileBG[start_num];
            for(int num = start_num; num < end_num; num++) { iProfileBG[num] = iProfileBG[num + 1]; }
            iProfileBG[end_num] = temp_object;

            temp_guest_num = profileGuestNum[start_num];
			for (int num = start_num; num < end_num; num++) { profileGuestNum[num] = profileGuestNum[num + 1]; }
            profileGuestNum[end_num] = temp_guest_num;

			temp_cloud_list = usedCloudObject[start_num];
            for(int num = start_num; num < end_num; num++) { usedCloudObject[num] = usedCloudObject[num + 1]; }
            usedCloudObject[end_num] = temp_cloud_list;
            
		}
        else
        {
			temp_object = iProfileBG[start_num];
			for (int num = start_num; num > end_num; num--) { iProfileBG[num] = iProfileBG[num - 1]; }
			iProfileBG[end_num] = temp_object;

			temp_guest_num = profileGuestNum[start_num];
			for (int num = start_num; num > end_num; num--) { profileGuestNum[num] = profileGuestNum[num - 1]; }
			profileGuestNum[end_num] = temp_guest_num;

			temp_cloud_list = usedCloudObject[start_num];
			for (int num = start_num; num > end_num; num--) { usedCloudObject[num] = usedCloudObject[num - 1]; }
			usedCloudObject[end_num] = temp_cloud_list;
		}
	}
}
