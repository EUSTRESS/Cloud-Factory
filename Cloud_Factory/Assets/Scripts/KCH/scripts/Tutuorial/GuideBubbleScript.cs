using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideBubbleScript : MonoBehaviour
{
	private Text tGuideText;

	private string[,] mDialog;

	[HideInInspector]
	private int mDialogIndex;		// 몇 번째 Dialog를 불러올 것인지 선정(외부에서 입력 받음)
	private int currentDialogNum;   // 최근 텍스트 넘버
	[HideInInspector]
	private int presentDialogNum;    // 현재 텍스트 넘버, currentDialogNum != presentDialogNum일 때, currentDialogNum <= presentDialogNum && Update Text

	TutorialManager mTutorialManager;

	void Awake()
	{
		tGuideText = transform.Find("Text").gameObject.GetComponent<Text>();
		mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();

		InitDialog();
	}

	void Update()
	{
		ReadDialog();
	}

	public void SetDialogIndex(int idx = 0) { mDialogIndex = idx; }

	// Dialog 초기화, 비어있는 string은 " "으로 관리
	private void InitDialog()
	{
		mDialogIndex = 0;
		currentDialogNum = -1;
		presentDialogNum = 0;

		mDialog = new string[7, 30];

		for (int num1 = 0; num1 < 7; num1++)
		{ for(int num2 = 0; num2 < 30; num2++)
			{
				mDialog[num1, num2] = " ";
			}
		}

		mDialog[0, 0] = "test1";
		mDialog[0, 1] = "test2";
		mDialog[0, 2] = "이게 응접실 버튼...";
		mDialog[0, 3] = "응접실에 느낌표 표시가 있으면....";
		mDialog[0, 4] = "응접실에 가서 누가 왔는지 확인해봅시다.";
		mDialog[0, 5] = "BlackOut1";

		mDialog[1, 0] = "뭉티가 말하는 힌트를 보고 현재 느끼는 감정을 잘 파악해보자.";
	}


	private void ReadDialog()
	{
		if (currentDialogNum == presentDialogNum) { return; }
		else currentDialogNum = presentDialogNum;

		if(mDialog[mDialogIndex, currentDialogNum] == " ") { 
			Destroy(this.gameObject);
			return;
		}

		// 첫 번째 날씨의 공간 화면 페이드 아웃
		// 튜토리얼 종료 확인 여부는 응접실 버튼에서 처리
		if(mDialog[mDialogIndex, currentDialogNum] == "BlackOut1") {
			mTutorialManager.FadeOutSpaceOfWeather();
			Destroy(this.gameObject);
			return;
		}

		tGuideText.text = mDialog[mDialogIndex, currentDialogNum];
	}

	public void UpdateText()
	{
		presentDialogNum++;
	}
}
