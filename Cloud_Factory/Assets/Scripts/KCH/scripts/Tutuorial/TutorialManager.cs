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

    /*
     #튜토리얼 진행도 체크
     날씨의 공간 1
	 응접실
	 날씨의 공간 2 (채집)
	 구름 공장
	 구름 데코
	 구름 제공
	 손님 배웅
    */
    [HideInInspector]
	public bool[] isFinishedTutorial;

    public GameObject emptyScreen;          // 화면 클릭을 막기위한 오브젝트
    private GameObject emptyScreenObject;

    public GameObject guideSpeechBubble;    // 가이드 말풍선 오브젝트(child로 charImage, Text, button 존재)
    private GameObject guideSpeechBubbleObject;        // 오브젝트를 관리하기 위한 오브젝트

    public GameObject fadeOutScreen;        // 날씨의 공간(1) 튜토리얼에서 사용되는 FadeOutScreen
    private GameObject fadeOutScreenObject;



	private static TutorialManager instance = null;
    void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            isTutorial = true;
            isFinishedTutorial = new bool[7];

            for(int num = 0; num < isFinishedTutorial.Length; num++) { isFinishedTutorial[num] = false; }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Space Of Weather"
            && !isFinishedTutorial[0]
            && guideSpeechBubbleObject == null
            && fadeOutScreenObject == null)
        { TutorialOfSOW(); }

        // 가이드 말풍선이 없어지면 화면 터치를 막는 emptyScreenObject도 없애준다.
        if(guideSpeechBubbleObject == null
            && emptyScreenObject != null)
        {
            Destroy(emptyScreenObject.gameObject);
        }
    }

    public bool IsGuideSpeechBubbleExist()
    {
        if (guideSpeechBubbleObject != null) { return true; }
        return false;
    }

    // 날씨의 공간(1) 튜토리얼
    // 말풍선을 띄운다. 대사 多 대사 넘기는 버튼 有
    // 응접실 버튼과 느낌표 설명
    // 응접실 외 어둡게, 응접실 버튼만 클릭 가능
    public void TutorialOfSOW()
    {
        InstantiateBasicObjects(0);

    }

    public void FadeOutSpaceOfWeather()
    {
        fadeOutScreenObject = Instantiate(fadeOutScreen);
		fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    // 응접실 튜토리얼
    // 힌트가 나올 때 말풍선을 띄운다
    // DialogManager.cs 284에서 출력
    public void TutorialDrawingRoom()
    {
        InstantiateBasicObjects(1);
	}

    public void FinishTutorial1()
    {
        isFinishedTutorial[0] = true;
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
}
