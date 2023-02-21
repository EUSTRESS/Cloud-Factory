using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public GameObject emptyScreen;              // 화면 클릭을 막기위한 오브젝트
    private GameObject emptyScreenObject;       // 말풍선과 함께 생성/제거 되는 오브젝트
    private GameObject blockScreenTouchObject;  // 말풍선이 없어도 화면의 터치를 막을 때 사용되는 오브젝트

    public GameObject guideSpeechBubble;    // 가이드 말풍선 오브젝트(child로 charImage, Text, button 존재)
    private GameObject guideSpeechBubbleObject;        // 오브젝트를 관리하기 위한 오브젝트

    public GameObject commonFadeOutScreen;   // 화면을 모두 가리는 공용 Fade Out 스크린
    public GameObject fadeOutScreen1;
    public GameObject storageFadeOutScreen;
    private GameObject fadeOutScreenObject;

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

        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.L))
		{
			for(int num = 0; num < 8; num++)
            {
                isFinishedTutorial[num] = true;
            }
            isTutorial = false;
            if(guideSpeechBubbleObject != null) { Destroy(guideSpeechBubbleObject); }
            if (fadeOutScreenObject != null) { Destroy(fadeOutScreenObject); }
            if (emptyScreenObject != null) { Destroy(emptyScreenObject); }
            if(blockScreenTouchObject != null) { Destroy(blockScreenTouchObject); }

            Debug.Log("Skip Tutorial");
		}

		if (SceneManager.GetActiveScene().name == "Space Of Weather"
            && !isFinishedTutorial[0]
            && guideSpeechBubbleObject == null
            && fadeOutScreenObject == null)
        {
			mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
			TutorialOfSOW1(); 
        }

		if (SceneManager.GetActiveScene().name == "Drawing Room"
			&& !isFinishedTutorial[1]
			&& guideSpeechBubbleObject == null
			&& fadeOutScreenObject == null)
		{ TutorialDrawingRoom(); }

		if (SceneManager.GetActiveScene().name == "Space Of Weather"
            && isFinishedTutorial[0]
            && !isFinishedTutorial[2]
            && guideSpeechBubbleObject == null
            && fadeOutScreenObject == null)
        { TutorialOfSOW2(); }

        if (SceneManager.GetActiveScene().name == "Cloud Factory"
            && !isFinishedTutorial[3]
            && guideSpeechBubbleObject == null
            && fadeOutScreenObject == null)
        { TutorialCloudFactory1(); }

        if(SceneManager.GetActiveScene().name == "Give Cloud"
            && !isFinishedTutorial[4]
            && guideSpeechBubbleObject == null
            && fadeOutScreenObject == null)
        { TutorialGiveCloud(); }

		if (SceneManager.GetActiveScene().name == "Cloud Factory"
			&& !isFinishedTutorial[5]
            && isFinishedTutorial[4]
			&& guideSpeechBubbleObject == null
			&& fadeOutScreenObject == null)
		{ TutorialCloudFactory2(); }

		if (SceneManager.GetActiveScene().name == "DecoCloud"
			&& !isFinishedTutorial[6]
			&& guideSpeechBubbleObject == null
			&& fadeOutScreenObject == null)
		{ TutorialCloudDeco(); }

		if (SceneManager.GetActiveScene().name == "Cloud Storage"
			&& !isFinishedTutorial[7]
			&& guideSpeechBubbleObject == null
			&& fadeOutScreenObject == null)
		{ TutorialCloudStorage(); }

		// 가이드 말풍선이 없어지면 화면 터치를 막는 emptyScreenObject도 없애준다.
		if (guideSpeechBubbleObject == null
			&& emptyScreenObject != null)
		{ Destroy(emptyScreenObject.gameObject); }

        //가이드 말풍선의 상태에 따라 화면 터치를 막는 emptyScreenObject의 상태도 변경
        if(guideSpeechBubbleObject != null)
        {
            if(guideSpeechBubbleObject.activeSelf == false
			&& emptyScreenObject.activeSelf == true)
			{ emptyScreenObject.SetActive(false); }

            else if(guideSpeechBubbleObject.activeSelf == true
			&& emptyScreenObject.activeSelf == false)
			{ emptyScreenObject.SetActive(true); }
		}

        if (isFinishedTutorial[1] == true
            &&isFinishedTutorial[2] == false
            && mSOWManager.mUsingGuestObjectList.Count > 0
			&& mSOWManager.mUsingGuestObjectList[0].GetComponent<GuestObject>().isSpeakEmotion == true
            && guideSpeechBubbleObject.activeSelf == false
            && blockScreenTouchObject != null)
        {
            guideSpeechBubbleObject.SetActive(true);
            Destroy(blockScreenTouchObject);
        }

	}

    public bool IsGuideSpeechBubbleExist()
    {
        if (guideSpeechBubbleObject != null) { return true; }
        return false;
    }

    public void SetActiveGuideSpeechBubble(bool _bool)  { if (guideSpeechBubbleObject != null) { guideSpeechBubbleObject.SetActive(_bool); } }
    public void SetActiveFadeOutScreen(bool _bool)      { if (fadeOutScreenObject != null) { fadeOutScreenObject.SetActive(_bool); } }

    // 날씨의 공간(1) 튜토리얼
    // 말풍선을 띄운다. 대사 多 대사 넘기는 버튼 有
    // 응접실 버튼과 느낌표 설명
    // 응접실 외 어둡게, 응접실 버튼만 클릭 가능
    public void TutorialOfSOW1()
    {
        InstantiateBasicObjects(0);

    }

    // 응접실 튜토리얼
    public void TutorialDrawingRoom()
    {
        InstantiateBasicObjects(1);
	}

    // 채집 튜토리얼(감정표현 추가 필요)
    public void TutorialOfSOW2()
    {
        InstantiateBasicObjects(2);
    }

    public void TutorialCloudFactory1()
    {
        InstantiateBasicObjects(3);
    }

    public void TutorialGiveCloud()
    {
        InstantiateBasicObjects(4);
    }

    public void TutorialCloudFactory2()
    {
        InstantiateBasicObjects(5);
    }

    public void TutorialCloudDeco()
    {
        InstantiateBasicObjects(6);
    }

    public void TutorialCloudStorage()
    {
        InstantiateBasicObjects(7);
    }

    public void FinishTutorial1()
    {
        isFinishedTutorial[0] = true;
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

    public void FadeOutCloudStorage()
    {
		fadeOutScreenObject = Instantiate(storageFadeOutScreen);
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
	}

    public void InstantiateBlockScreenTouchObject()
    {
        blockScreenTouchObject = Instantiate(emptyScreen);
		blockScreenTouchObject.transform.SetParent(GameObject.Find("Canvas").transform);
		blockScreenTouchObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}
}
