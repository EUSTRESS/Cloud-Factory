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
		tGuideText = transform.Find("Button").gameObject.transform.Find("Text").GetComponent<Text>();
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

		mDialog = new string[8, 30];

		for (int num1 = 0; num1 < 8; num1++)
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
		mDialog[0, 5] = "FadeOut1";

		mDialog[1, 0] = "이곳은 응접실 입니다. 클라우드 팩토리를 찾아온 손님들의 이야기를 들을 수 있는 장소입니다.";
		mDialog[1, 1] = "DialogSpace1";
		mDialog[1, 2] = "뭉티가 말하는 힌트를 보고 현재 느끼는 감정을 잘 파악해보자.";
		mDialog[1, 3] = "DialogSpace2";
		mDialog[1, 4] = "클라우드 팩토리의 첫번째 손님을 받아봅시다.";

		mDialog[2, 0] = "뭉티는 구름을 제공받기 전까지 이 공간에서 대기를 해.";
		mDialog[2, 1] = "뭉티를 클릭하면, 현재 감정에 대한 정보를 얻을 수 있어.";
		mDialog[2, 2] = "구름을 제작하기 위한 재료를 채집해보자.";
		mDialog[2, 3] = "위의 마당을 클릭하면 재료를 채집할 수 있어. 한 번 클릭해볼까?";
		mDialog[2, 4] = "Gathering";
		mDialog[2, 5] = "쉽지? 이렇게 재료 채집을 할 수가 있어.";
		mDialog[2, 6] = "이제 구름공장으로 이동해서 구름을 만들어 보자.";
		mDialog[2, 7] = "FadeOut2";

		mDialog[3, 0] = "구름 제작 기계를 클릭하자.";
		mDialog[3, 1] = "FadeOut3";

		mDialog[4, 0] = "여기서 구름을 제작할 수 있어.";
		mDialog[4, 1] = "재료를 먼저 기계에 넣어볼까?";
		mDialog[4, 2] = "재료를 모두 넣었으면, '제작하기' 버튼을 누르자.";
		mDialog[4, 3] = "FadeOut4";                     
		mDialog[4, 4] = "구름이 만들어졌으니 다시 공장으로 돌아가자.";
		mDialog[4, 5] = "FadeOut5";

		mDialog[5, 0] = "구름이 기계에서 나오면 클릭한다.";
	}


	private void ReadDialog()
	{
		if (currentDialogNum == presentDialogNum) { return; }
		else currentDialogNum = presentDialogNum;

		if(mDialog[mDialogIndex, currentDialogNum] == " ") {
			this.gameObject.SetActive(false);
			return;
		}

		// 첫 번째 날씨의 공간 화면 페이드 아웃
		// 튜토리얼 종료 확인 여부는 응접실 버튼에서 처리
		if(mDialog[mDialogIndex, currentDialogNum] == "FadeOut1") {
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_Drawing Room").transform.SetAsLastSibling();
			this.gameObject.SetActive(false);
			return;
		}

		if(mDialog[mDialogIndex, currentDialogNum] == "DialogSpace1"
			|| mDialog[mDialogIndex, currentDialogNum] == "DialogSpace2")
		{
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "Gathering") {
			mTutorialManager.FadeOutSpaceOfWeather();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
		}

		if(mDialog[mDialogIndex, currentDialogNum] == "FadeOut2")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_Cloud Factory").transform.SetAsLastSibling();
			this.gameObject.SetActive(false); ;
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut3")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_GiveCloud").transform.SetAsLastSibling();
			this.gameObject.SetActive(false);
			return;
		}

		if(mDialog[mDialogIndex, currentDialogNum] == "FadeOut4")
		{
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
		}

		if(mDialog[mDialogIndex, currentDialogNum] == "FadeOut5")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_Back").transform.SetAsLastSibling();
			this.gameObject.SetActive(false);
			return;
		}

		tGuideText.text = mDialog[mDialogIndex, currentDialogNum];
	}

	public void UpdateText()
	{
		presentDialogNum++;
	}
}
