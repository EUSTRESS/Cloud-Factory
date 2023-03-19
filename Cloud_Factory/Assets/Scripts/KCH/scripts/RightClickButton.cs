using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	private float clickedTime;
	private bool isClicked;

	private TutorialManager mTutorialManager;

	void Awake()
	{
		clickedTime = 0f;
		isClicked = false;

		mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
	}

	void Update()
	{
		if (mTutorialManager.isFinishedTutorial[7] == false)
		{
			if (isClicked) { clickedTime += Time.deltaTime; }
			if(clickedTime >= 1.0f) { mTutorialManager.SetActiveFadeOutScreen(false); mTutorialManager.SetActiveGuideSpeechBubble(true); clickedTime = 0f; }
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
}
