using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	private float clickedTime;
	private bool isClicked;
	private bool isFinishedRightClick;

	private TutorialManager mTutorialManager;

	void Awake()
	{
		clickedTime = 0f;
		isClicked = false;
		isFinishedRightClick = false;

		mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
	}

	void Update()
	{
		if (mTutorialManager.isFinishedTutorial[7] == false)
		{
			if (isClicked && isFinishedRightClick == false) { clickedTime += Time.deltaTime; }
			if(clickedTime >= 0.5f) { mTutorialManager.SetActiveFadeOutScreen(false); mTutorialManager.SetActiveGuideSpeechBubble(true); clickedTime = 0f; isFinishedRightClick = true; }
		}
	}
	
	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			GameObject.Find("Container").GetComponent<CloudContainer>().OnClickedRight(this.gameObject);
			isClicked = true;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			GameObject.Find("Container").GetComponent<CloudContainer>().UnClickedRight(this.gameObject);
			clickedTime = 0f;
			isClicked = false;
		}
	}

	public bool GetIsFinishedRightClick()
	{
		return isFinishedRightClick;
	}
}
