using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TutorialData
{
	public bool isTutorial;	
}

public class TutorialManager : MonoBehaviour
{
	// 튜토리얼이 완전히 종료되었는지 체크
	[HideInInspector]
	public bool isTutorial;

	// 옵션은 항상 누를 수 있도록 설정이 필요

	/*
     #튜토리얼 진행도 체크
     날씨의 공간 1
	 응접실
	 날씨의 공간 2 (채집)
	 구름 공장
	 구름 데코
     +구름공장
	 구름 제공
	 손님 배웅
    */
	[HideInInspector]
	public bool[] isFinishedTutorial;

	[Header("화면 터치를 막는 오브젝트")]
	public GameObject emptyScreen;              // 화면 클릭을 막기위한 오브젝트
	private GameObject emptyScreenObject;       // 말풍선과 함께 생성/제거 되는 오브젝트
	private GameObject blockScreenTouchObject;  // 말풍선이 없어도 화면의 터치를 막을 때 사용되는 오브젝트

	[Header("가이드 말풍선 오브젝트")]
	public GameObject guideSpeechBubble;        // 가이드 말풍선 오브젝트(child로 CharImage, Text존재)
	private GameObject guideSpeechBubbleObject; // 오브젝트를 관리하기 위한 오브젝트

	[Header("화살표 UI 오브젝트")]
	public GameObject leftArrowNotInCanvas;
	public GameObject leftArrow;
	public GameObject rightArrow;
	private GameObject arrowObject;
	public GameObject RightMouse_Click;

	[Header ("페이드 아웃 프리펩 오브젝트")]
    public GameObject commonFadeOutScreen;		// 화면을 모두 가리는 공용 Fade Out 스크린
    public GameObject fadeOutScreen1;
    public GameObject giveCloudFadeOutScreen;
    public GameObject giveCloudFadeOutScreen2;
    public GameObject storageFadeOutScreen0;
    public GameObject storageFadeOutScreen;
    public GameObject storageFadeOutExpir;
    public GameObject storageFadeOutReceipt;
    public GameObject storageFadeOutSend;
    public GameObject decoFadeOutScreen;
    public GameObject decoFadeOutDeco;
    public GameObject decoFadeOutVariation;
    private GameObject fadeOutScreenObject;

	[Header("튜토리얼용 뭉티 오브젝트")]
	private GameObject tutorialGuest;

    private Guest mGuestManager;
    private SOWManager mSOWManager;


	private static TutorialManager instance = null;
    void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            isTutorial = true;
            isFinishedTutorial = new bool[9];

            for(int num = 0; num < isFinishedTutorial.Length; num++) { isFinishedTutorial[num] = false; }

            mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();

        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static TutorialManager GetInstance()
    {
	    if (instance != null) return instance;
	    instance = FindObjectOfType<TutorialManager>();
	    if (instance == null) Debug.Log("There's no active Tutorial Manager");
	    return instance;
    }

    public void ChangeAllTutorialStatus()
    {
	    if (isTutorial == false)
	    {
		    for (int num = 0; num < 9; num++)
		    {
			    isFinishedTutorial[num] = true;
		    }
		    DestroyAllObject();
	    }
    }

    void Update()
    {
        if (isTutorial)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                for (int num = 0; num < 9; num++)
                {
                    isFinishedTutorial[num] = true;
                }
                isTutorial = false;
                DestroyAllObject();
                if (SceneManager.GetActiveScene().name == "Space Of Weather")
                {
	                GameObject.Find("B_Option").transform.SetSiblingIndex(5);
	                GameObject.Find("UIManager").GetComponent<CommonUIManager>().gOption.transform.SetSiblingIndex(15);
                }
                else if (SceneManager.GetActiveScene().name == "Cloud Factory")
                {
	                GameObject.Find("B_Option").transform.SetSiblingIndex(7);
	                GameObject.Find("UIManager").GetComponent<CommonUIManager>().gOption.transform.SetSiblingIndex(13);
                }
            }

            // 스토리 소개, 응접실 안내 튜토리얼
            TutorialOfSOW1();

            // 응접실 튜토리얼
            TutorialDrawingRoom();

            // 뭉티 감정 확인, 재료 채집 튜토리얼
            TutorialOfSOW2();

            // 구름 제작 기계 소개 및 안내 튜토리얼
            TutorialCloudFactory1();

            // 구름 제작 튜토리얼
            TutorialGiveCloud();

            // 구름 데코하러 가는 법 안내 튜토리얼
            TutorialCloudFactory2();

            // 구름 데코 튜토리얼
            TutorialCloudDeco();

            // 구름 제공 튜토리얼
            TutorialCloudStorage();

            // 구름 제공 후 처리 튜토리얼
            TutorialSOW3();

			// Guide 말풍선 상태에 따른, 화면 터치를 막는 오브젝트 상태 변경
			ChangeEmptyScreenObjectStatus();

			// Arrow UI 오브젝트 상태 변경
			ChangeArrowObjectStatus();
        }
	}

    public bool IsGuideSpeechBubbleExist()
    {
        if (guideSpeechBubbleObject != null) { return true; }
        return false;
    }

    public void SetActiveGuideSpeechBubble(bool _bool)
    {
	    if (guideSpeechBubbleObject == null) return;
	    
	    guideSpeechBubbleObject.SetActive(_bool);
    }
    
	public GameObject GetActiveGuideSpeechBubble()		{ return guideSpeechBubbleObject; }
    public void SetActiveFadeOutScreen(bool _bool)      { if (fadeOutScreenObject != null) { fadeOutScreenObject.SetActive(_bool); } }
	public void SetActiveArrowUIObject(bool _bool)		{ if (arrowObject != null) { arrowObject.SetActive(_bool); } }
	public void DestoryBlockTouchObject()				{ if (blockScreenTouchObject != null) Destroy(blockScreenTouchObject); }

    // 날씨의 공간(1) 튜토리얼
    // 말풍선을 띄운다. 대사 多 대사 넘기는 버튼 有
    // 응접실 버튼과 느낌표 설명
    // 응접실 외 어둡게, 응접실 버튼만 클릭 가능
    public void TutorialOfSOW1()
    {
		if (SceneManager.GetActiveScene().name == "Space Of Weather"
				&& !isFinishedTutorial[0]
				&& guideSpeechBubbleObject == null
				&& fadeOutScreenObject == null)
		{
			mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
			InstantiateBasicObjects(0);
		}
    }

    // 응접실 튜토리얼
    public void TutorialDrawingRoom()
    {
		if (SceneManager.GetActiveScene().name == "Drawing Room"
				&& !isFinishedTutorial[1]
				&& guideSpeechBubbleObject == null
				&& fadeOutScreenObject == null)
		{ InstantiateBasicObjects(1); }
	}

    // 채집 튜토리얼
    public void TutorialOfSOW2()
    {
		if (SceneManager.GetActiveScene().name == "Space Of Weather"
				&& isFinishedTutorial[0]
				&& !isFinishedTutorial[2]
				&& guideSpeechBubbleObject == null
				&& fadeOutScreenObject == null)
		{ tutorialGuest = mSOWManager.mUsingGuestObjectList[0]; InstantiateBasicObjects(2); }
    }

    public void TutorialCloudFactory1()
    {
		if (SceneManager.GetActiveScene().name == "Cloud Factory"
				&& !isFinishedTutorial[3]
				&& guideSpeechBubbleObject == null
				&& fadeOutScreenObject == null)
		{ InstantiateBasicObjects(3); }
    }

    public void TutorialGiveCloud()
    {
		if (SceneManager.GetActiveScene().name == "Give Cloud"
	            && !isFinishedTutorial[4]
	            && guideSpeechBubbleObject == null
	            && fadeOutScreenObject == null)
		{ InstantiateBasicObjects(4); }
    }

    public void TutorialCloudFactory2()
    {
		if (SceneManager.GetActiveScene().name == "Cloud Factory"
	        && isFinishedTutorial[3]
	        && !isFinishedTutorial[5]
	        && guideSpeechBubbleObject == null
	        && fadeOutScreenObject == null)
		{ InstantiateBasicObjects(5); }
    }

    public void TutorialCloudDeco()
    {
		if (SceneManager.GetActiveScene().name == "DecoCloud"
			&& !isFinishedTutorial[6]
			&& guideSpeechBubbleObject == null
			&& fadeOutScreenObject == null)
		{ InstantiateBasicObjects(6); }
    }

    public void TutorialCloudStorage()
    {
		if (SceneManager.GetActiveScene().name == "Cloud Storage"
			&& !isFinishedTutorial[7]
			&& guideSpeechBubbleObject == null
			&& fadeOutScreenObject == null)
		{ InstantiateBasicObjects(7); }
		;
    }

    public void TutorialSOW3()
    {
		if (SceneManager.GetActiveScene().name == "Space Of Weather"
			&& isFinishedTutorial[0]
			&& isFinishedTutorial[2]
			&& !isFinishedTutorial[8]
			&& guideSpeechBubbleObject == null
			&& fadeOutScreenObject == null)
		{ InstantiateBasicObjects(8); }
	}

	public void FadeOutScreen()
	{
		fadeOutScreenObject = Instantiate(commonFadeOutScreen);
		fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

	public void FadeOutSpaceOfWeather()
	{
		fadeOutScreenObject = Instantiate(fadeOutScreen1);
		fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

	public void FadeOutGiveCloud()
	{
		fadeOutScreenObject = Instantiate(giveCloudFadeOutScreen);
		fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}
	
	public void FadeOutGiveCloud2()
	{
		fadeOutScreenObject = Instantiate(giveCloudFadeOutScreen2);
		fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		if (LanguageManager.GetInstance().GetCurrentLanguage() == "Korean")
		{ fadeOutScreenObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "제작하기"; }
		else { fadeOutScreenObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "CREATE"; }
	}

	public void FadeOutCloudStorage0()
	{
		fadeOutScreenObject = Instantiate(storageFadeOutScreen0);
		fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

    public void FadeOutCloudStorage()
    {
		fadeOutScreenObject = Instantiate(storageFadeOutScreen);
		fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

    public void FadeOutCloudStorageSend()
    {
	    fadeOutScreenObject = Instantiate(storageFadeOutSend);
	    fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
	    fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	    if (LanguageManager.GetInstance().GetCurrentLanguage() == "Korean")
	    { fadeOutScreenObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "구름 제공"; }
	    else { fadeOutScreenObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "SEND"; }
    }
    
    public void FadeOutCloudExpir()
    {
	    fadeOutScreenObject = Instantiate(storageFadeOutExpir);
	    fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
	    fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
    
    public void FadeOutCloudReceipt()
    {
	    fadeOutScreenObject = Instantiate(storageFadeOutReceipt);
	    fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
	    fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    public void FadeOutDecoCloud()
    {
		fadeOutScreenObject = Instantiate(decoFadeOutScreen);
		fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}
    
    public void FadeOutDecoCloudDeco()
    {
	    fadeOutScreenObject = Instantiate(decoFadeOutDeco);
	    fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
	    fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
    
    public void FadeOutDecoCloudVariation()
    {
	    fadeOutScreenObject = Instantiate(decoFadeOutVariation);
	    fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
	    fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
    }



    // Tutorial 간 모든 씬에서 출력되는 기본 오브젝트(emptyScreenObject, guideSpeechBubbleObject)를 생성해준다.
	public void InstantiateBasicObjects(int dialog_index)
    {
		emptyScreenObject = Instantiate(emptyScreen);
		emptyScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		emptyScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);

		// 말풍선 오브젝트 생성
		guideSpeechBubbleObject = Instantiate(guideSpeechBubble);
		guideSpeechBubbleObject.transform.SetParent(GameObject.Find("Canvas").transform);
		guideSpeechBubbleObject.transform.localPosition = new Vector3(0f, -340f, 0f);
		guideSpeechBubbleObject.GetComponent<GuideBubbleScript>().SetDialogIndex(dialog_index);
		guideSpeechBubbleObject.GetComponent<GuideBubbleScript>().emptyScreen = emptyScreenObject;

        if(SceneManager.GetActiveScene().name == "Space Of Weather"
            || SceneManager.GetActiveScene().name == "Drawing Room"
            || SceneManager.GetActiveScene().name == "Cloud Factory")
        { 
            GameObject.Find("B_Option").transform.SetAsLastSibling();

            GameObject option_object = GameObject.Find("UIManager").GetComponent<CommonUIManager>().gOption;
			option_object.transform.SetAsLastSibling();
		}
	}

    public void ChangeEmptyScreenObjectStatus()
    {
		// 가이드 말풍선이 없어지면 화면 터치를 막는 emptyScreenObject도 없애준다.
		if (guideSpeechBubbleObject == null
			&& emptyScreenObject != null)
		{ Destroy(emptyScreenObject.gameObject); }

		//가이드 말풍선의 상태에 따라 화면 터치를 막는 emptyScreenObject의 상태도 변경
		if (guideSpeechBubbleObject != null)
		{
			if (guideSpeechBubbleObject.activeSelf == false
			&& emptyScreenObject.activeSelf == true)
			{ emptyScreenObject.SetActive(false); }

			else if (guideSpeechBubbleObject.activeSelf == true
			&& emptyScreenObject.activeSelf == false)
			{ emptyScreenObject.SetActive(true); }
		}

		if ((isFinishedTutorial[1] == true
			&& isFinishedTutorial[2] == false
			&& mSOWManager.mUsingGuestObjectList.Count > 0
			&& mSOWManager.mUsingGuestObjectList[0].GetComponent<GuestObject>().isSpeakEmotion == true
			&& guideSpeechBubbleObject.activeSelf == false
			&& blockScreenTouchObject != null)
			||
			(isFinishedTutorial[0] == true
			&& isFinishedTutorial[2] == true
			&& isFinishedTutorial[8] == false
			&& mSOWManager.mUsingGuestObjectList.Count > 0
			&& mGuestManager.mGuestInfo[0].isUsing == true
			&& guideSpeechBubbleObject.activeSelf == false
			&& blockScreenTouchObject != null)
			||
			(isFinishedTutorial[0] == true
			&& isFinishedTutorial[2] == true
			&& isFinishedTutorial[8] == false
			&& tutorialGuest == null
			&& guideSpeechBubbleObject.activeSelf == false
			&& blockScreenTouchObject != null))
		{
			if (arrowObject != null) { arrowObject.SetActive(false); }
			SetActiveGuideSpeechBubble(true);
			Destroy(blockScreenTouchObject);
		}
	}

	public void ChangeArrowObjectStatus()
	{
		if (isFinishedTutorial[1] == true
			&& isFinishedTutorial[2] == false
			&& arrowObject == null
			&& guideSpeechBubbleObject.activeSelf == false
			&& mSOWManager.mUsingGuestObjectList[0].GetComponent<GuestObject>().isSit == true)
		{
			InstantiateArrowUIObject(mSOWManager.mUsingGuestObjectList[0].transform.localPosition, -1.75f, 1f, false);
			arrowObject.transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
		}


		if (SceneManager.GetActiveScene().name == "Cloud Factory"
			&& isFinishedTutorial[4] == true
			&& isFinishedTutorial[5] == false
			&& arrowObject != null
			&& arrowObject.activeSelf == false
			&& GameObject.Find("B_GoDecoCloud"))
		{ SetActiveArrowUIObject(true); }
	}

	public void InstantiateBlockScreenTouchObject()
    {
        blockScreenTouchObject = Instantiate(emptyScreen);
		blockScreenTouchObject.transform.SetParent(GameObject.Find("Canvas").transform);
		blockScreenTouchObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

	public void InstantiateArrowUIObject(Vector3 target_position, float xpos_difference = 0f, float ypos_difference = 0f, bool in_canvas = true)
	{
		if(in_canvas == false) { arrowObject = Instantiate(leftArrowNotInCanvas); }
		else if (xpos_difference <= 0) { arrowObject = Instantiate(leftArrow); }
		else { arrowObject = Instantiate(rightArrow); }

		if (in_canvas == true) { arrowObject.transform.SetParent(GameObject.Find("Canvas").transform); }
		arrowObject.transform.localPosition = new Vector3(xpos_difference, ypos_difference, 0f) + target_position;
	}

	public void DestroyAllObject()
    {
        if(guideSpeechBubbleObject != null) { Destroy(guideSpeechBubbleObject); }
        if(fadeOutScreenObject != null)		{ Destroy(fadeOutScreenObject); }
        if(emptyScreenObject != null)		{ Destroy(emptyScreenObject); }
        if(blockScreenTouchObject != null)	{ Destroy(blockScreenTouchObject); }
		if(arrowObject != null)				{ Destroy(arrowObject); }
    }
}
