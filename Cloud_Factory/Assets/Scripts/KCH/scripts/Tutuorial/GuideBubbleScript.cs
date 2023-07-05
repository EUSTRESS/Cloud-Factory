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

	private TutorialManager mTutorialManager;
	private	SOWManager		mSOWManager;

	void Awake()
	{
		tGuideText = transform.Find("Button").gameObject.transform.Find("Text").GetComponent<Text>();
		mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
		mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

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

		mDialog = new string[9, 15];

		//  mDialog 초기화
		for (int num1 = 0; num1 < 9; num1++)
		{ for(int num2 = 0; num2 < 15; num2++)
			{
				mDialog[num1, num2] = " ";
			}
		}

		LoadText();
	}


	private void ReadDialog()
	{
		if (currentDialogNum == presentDialogNum) { return; }
		else { currentDialogNum = presentDialogNum; }

		if(mDialog[mDialogIndex, currentDialogNum] == " ") {
			this.gameObject.SetActive(false);
			return;
		}

		// 첫 번째 날씨의 공간 화면 페이드 아웃
		// 튜토리얼 종료 확인 여부는 응접실 버튼에서 처리
		ProcessSpecialText();

		tGuideText.text = mDialog[mDialogIndex, currentDialogNum];
	}

	public void LoadText()
	{
		if (LanguageManager.GetInstance().GetCurrentLanguage() == "Korean")
		{
			mDialog[0, 0] = "이곳 클라우드 팩토리는 고민을 가진 \"뭉티\"라는 손님이 방문하는 장소입니다.";
			mDialog[0, 1] = "뭉티는 사람들의 고민에서 태어납니다.";
			mDialog[0, 2] = "뭉티는 사람이 잠들면 일어나고, 대신 고민하며 감정을 표출합니다.";
			mDialog[0, 3] = "뭉티를 맞이할 준비를 해봅시다.";
			mDialog[0, 4] = "손님이 방문하면 응접실에 느낌표가 나타납니다.";
			mDialog[0, 5] = "누가 왔는지 확인해봅시다.";
			// 화살표 UI
			mDialog[0, 6] = "FadeOut00";
			
			mDialog[1, 0] = "이곳은 응접실로, 클라우드 팩토리를 찾아온 손님의 이야기를 들을 수 있는 장소입니다.";
			mDialog[1, 1] = "마침 첫 손님이 오셨네요. 손님의 이야기를 귀담아들어 볼까요?";
			mDialog[1, 2] = "DialogSpace1";
			mDialog[1, 3] = "손님의 말에 나타난 색상을 주의 깊게 봅시다.";
			mDialog[1, 4] = "<color=\"red\">붉은색</color> 글씨는 현재 손님에게 부족한 감정을, <color=\"green\">초록색</color> 글씨는 현재 손님에게 많은 감정을 나타냅니다.";
			mDialog[1, 5] = "DialogSpace2";
			mDialog[1, 6] = "클라우드 팩토리의 첫 번째 손님을 받아봅시다. ";
			// 화살표 UI
			mDialog[1, 7] = "FadeOut10";

			
			mDialog[2, 0] = "받은 손님은 날씨의 공간에서 구름을 기다립니다.";
			mDialog[2, 1] = "손님이 앉으면 클릭해봅시다.";
			// 화살표 UI
			mDialog[2, 2] = "FadeOut20";
			mDialog[2, 3] = "앉은 손님은 클릭하면 현재 제일 약한 감정이나 제일 강한 감정을 보여줍니다. 어느 쪽일지는 이야기를 들어볼 때 추측할 수 있습니다.";
			mDialog[2, 4] = "구름을 제작할 때 필요한 재료를 채집해봅시다. 재료는 텃밭에서 구할 수 있습니다. 각 \'텃밭\'은 하루에 두 번만 채집할 수 있습니다.";
			mDialog[2, 5] = "Gathering";
			mDialog[2, 6] = "채집된 재료의 양은 랜덤입니다. 채집된 재료는 구름 공장의 기계 안에 수집됩니다. 새로 얻은 재료가 버려지지 않으려면 기계 안의 인벤토리에 공간이 충분히 있어야 합니다.";
			mDialog[2, 7] = "이제 구름 공장으로 이동해서 구름을 만들어봅시다.";
			// 화살표 UI
			mDialog[2, 8] = "FadeOut21";

			mDialog[3, 0] = "구름은 구름 제작 기계로 만들 수 있습니다. 클릭해보세요!";
			// 화살표 UI로 수정
			mDialog[3, 1] = "FadeOut30";


			mDialog[4, 0] = "FadeOut40";
			mDialog[4, 1] = "위쪽의 \'감정별\' 옆에 있는 네모를 클릭하면 재료를 감정별로 정리해서 볼 수 있습니다.";
			mDialog[4, 2] = "FadeOut41";
			mDialog[4, 3] = "채집한 재료를 모두 넣어서 구름을 만들어 봅시다. 재료를 2개 이상 클릭하면 오른쪽의 \'만들기\' 버튼이 켜집니다.";
			// 재료 모두 선택 시 화살표 UI로 수정
			mDialog[4, 4] = "FadeOut42";
			mDialog[4, 5] = "구름이 만들어졌으니 다시 공장으로 돌아가 봅시다.";
			// 돌아가기 버튼 화살표 UI로 수정
			mDialog[4, 6] = "FadeOut43";

			mDialog[5, 0] = "방금 만든 구름이 기계에서 나오고 있네요. 구름이 다 나오면 클릭해봅시다.";
			// 구름이 모두 나왔을 때, 화살표 UI
			mDialog[5, 1] = "FadeOut50";


			// fade out emotion list
			mDialog[6, 0] = "FadeOut60";
			mDialog[6, 1] = "이곳은 구름을 꾸밀 수 있는 공간입니다. 위쪽의 장식은 클릭해서 원하는 위치에 갖다 놓을 수 있습니다. 쓸 수 있는 장식의 개수는 장식의 위쪽 숫자로 알 수 있습니다.";
			mDialog[6, 2] = "FadeOut61";
			mDialog[6, 3] = "장식의 옆에 있는 \'증가\' 또는 \'감소\' 버튼을 누르면 재료의 특성에 따라 손님의 감정이 오르거나 내려갑니다.";
			mDialog[6, 4] = "\'증가\'를 누르면 특정 감정을 더 느끼게 되고, \'감소\'를 누르면 덜 느끼게 됩니다.";
			mDialog[6, 5] = "FadeOut62";
			mDialog[6, 6] = "이번 손님은 슬픔을 경험하고 싶으니, 슬픔을 더 느끼도록 슬픔 장식 옆의 \'증가\' 버튼을 눌러봅시다.";
			mDialog[6, 7] = "GuideSpeechOut60";
			// 화살표 UI
			mDialog[6, 8] = "좋습니다. 구름을 완성하려면 위쪽의 <b>모든</b> 장식을 붙이고, 오른쪽에서 각 장식이 손님에게 줄 <b>영향</b>을 골라야 합니다.";
			mDialog[6, 9] = "장식을 한꺼번에 넣고 싶다면 \'자동\' 버튼을 눌러주세요.";
			mDialog[6, 10] = "이제 구름을 완성해봅시다.";
			mDialog[6, 11] = "FadeOut63";
			// 돌아가기 버튼 막기


			// 화살표 UI
			mDialog[7, 0] = "완성된 구름은 구름 보관함에 저장됩니다.";
			mDialog[7, 1] = "구름은 보관 기간이 지나면 사라집니다. 보관 기간별로 보려면 위쪽의 \'보관 기간별\' 옆에 있는 네모를 클릭하면 됩니다.";
			mDialog[7, 2] = "구름을 계속 우클릭하고 있으면 어떤 재료로 만들었는지 볼 수 있습니다.";
			mDialog[7, 3] = "FadeOut70";
			// 우클릭 이벤트 추가 필요
			mDialog[7, 4] = "아까 만들었던 구름을 클릭해봅시다.";
			// 화살표 UI
			mDialog[7, 5] = "FadeOut71";
			mDialog[7, 6] = "오른쪽에서는 손님의 접수지로 정보를 얻을 수 있습니다. \'만족도\'에 주목해주세요! 모든 손님은 자기가 좋아하는 계절에 이 숫자가 5가 될 때까지 재방문합니다. 당신의 목표는 이 숫자가 5가 되기까지 도와주는 것입니다.";
			mDialog[7, 7] = "이 숫자는 손님이 조절하고 싶은 다섯가지 감정 중 하나가 만족스러워졌을 때마다 오르게 됩니다. 반대로, 적당한 양에서 벗어나게 되면 내려갑니다. 어떤 감정을 얼마나 원하는지는 뭉티의 이야기로 알 수 있습니다.";
			mDialog[7, 8] = "올바른 뭉티의 접수지 위에서 \'보내기\' 버튼을 누르면 날씨의 공간으로 구름이 보내집니다. 뭉티의 만족도가 0이 되면 다시는 방문하지 않으니 조심하세요!";
			mDialog[7, 9] = "FadeOut72";

			
			mDialog[8, 0] = "FadeOut80";
			mDialog[8, 1] = "구름을 받은 뭉티는 기분에 대한 힌트를 줍니다.";
			mDialog[8, 1] = "구름을 받고 감정에 변화가 생긴 손님은 집으로 돌아갑니다. 조만간 다시 방문할지도 모르겠네요!";
			mDialog[8, 2] = "FadeOut81";
			mDialog[8, 3] = "클라우드 팩토리의 안내를 이것으로 마칩니다. 여러 뭉티의 안식처가 될 수 있는 클라우드 팩토리가 될 수 있으면 좋겠습니다. 행운을 빕니다!";
			mDialog[8, 4] = "End";
		}
		else
		{
			mDialog[0, 0] = "Here in Cloud Factory, customers called \"Moongty\" will pay a visit.";
			mDialog[0, 1] = "They are born from people\'s worries.";
			mDialog[0, 2] = "Each Moongty wakes up as people fall asleep, to worry and express emotion on their behalf.";
			mDialog[0, 3] = "Shall we welcome them?";
			mDialog[0, 4] = "When customers arrive, you\'ll see an exclamation mark on the \"Office\" button.";
			mDialog[0, 5] = "Let\'s see who\'s here.";
			// 화살표 UI
			mDialog[0, 6] = "FadeOut00";
			
			
			mDialog[1, 0] = "We are now in the therapy office, the place where you get to listen to the customers.";
			mDialog[1, 1] = "We have one right in front of us just in time! Shall we listen to their worries?";
			mDialog[1, 2] = "DialogSpace1";
			mDialog[1, 3] = "Pay attention to the color and use them as hints:";
			mDialog[1, 4] = "<color=\"red\">red</color> words indicate which emotion is too weak, while <color=\"green\">green</color> words imply which emotion is too strong.";
			mDialog[1, 5] = "DialogSpace2";
			mDialog[1, 6] = "Let\'s take in the first customer of Cloud Factory.";
			// 화살표 UI
			mDialog[1, 7] = "FadeOut10";

			 
			mDialog[2, 0] = "The customer will wait for the cloud in the Weather Room.";
			mDialog[2, 1] = "Let\'s wait a little until she takes a seat.";
			// 화살표 UI
			mDialog[2, 2] = "FadeOut20";
			mDialog[2, 3] = "Whenever the customer is clicked, they will show either the most wanted or the least wanted emotion. You\'ll be able to tell its meaning by listening to them in the Office.";
			mDialog[2, 4] = "Let\'s harvest the ingredients needed for making clouds. The ingredients can be collected from the farm. Each \'farm\' can only be harvested twice per day. ";
			mDialog[2, 5] = "Gathering";
			mDialog[2, 6] = "The amount of harvested ingredients is random. Once harvested, they\'ll be collected inside the \'cloud machine\' in the factory. Make sure you have enough slots in your inventory so none of the new ingredients are disposed!";
			mDialog[2, 7] = "Let\'s walk over to the factory section of the place and make some cloud.";
			// 화살표 UI
			mDialog[2, 8] = "FadeOut21";

			mDialog[3, 0] = "You can make clouds with the cloud machine. Give it a click!";
			// 화살표 UI로 수정
			mDialog[3, 1] = "FadeOut30";
			

			mDialog[4, 0] = "FadeOut40";
			mDialog[4, 1] = "Clicking the little box near \'EMOTION ⇅\' will sort the ingredients based on the emotion they affect.";
			mDialog[4, 2] = "FadeOut41";
			mDialog[4, 3] = "For now, let's put all the ingredients into the machine to make a cloud. The \'CREATE\' button will activate once it has 2 or more ingredients.";
			// 재료 모두 선택 시 화살표 UI로 수정
			mDialog[4, 4] = "FadeOut42";
			mDialog[4, 5] = "Now that the cloud is created, let\'s go back to the factory.";
			// 돌아가기 버튼 화살표 UI로 수정
			mDialog[4, 6] = "FadeOut43";

			mDialog[5, 0] = "The cloud you made is coming out of the machine. Let\'s click on the cloud once it\'s completely out.";
			// 구름이 모두 나왔을 때, 화살표 UI
			mDialog[5, 1] = "FadeOut50";

			
			// fade out emotion list
			mDialog[6, 0] = "FadeOut60";
			mDialog[6, 1] = "This is where you decorate the cloud. The decors on the top are placeable by clicking and dropping. The amount of decors you have to place are written on the top of each types of ingredient.";
			mDialog[6, 2] = "FadeOut61";
			mDialog[6, 3] = "The customer\'s change in emotion depends on which buttons you press.";
			mDialog[6, 4] = "\'Up\' will make them feel more of the emotion, but \'down\' makes them feel less of it.";
			mDialog[6, 5] = "FadeOut62";
			mDialog[6, 6] = "This customer wants to feel sad; let\'s press on the \'Up\' button next to the decor that affects sadness, so they feel more of it.";
			mDialog[6, 7] = "GuideSpeechOut60";
			// 화살표 UI
			mDialog[6, 8] = "Excellent! To finish making the cloud, use <b>all</b> decor parts and assign their <b>effects</b> by pressing the buttons.";
			mDialog[6, 9] = "If you want to skip with decoration, press \'AUTO\' button on the top.";
			mDialog[6, 10] = "Let\'s finish off making the cloud.";
			mDialog[6, 11] = "FadeOut63";
			// 돌아가기 버튼 막기


			// 화살표 UI
			mDialog[7, 0] = "Decorated clouds are stored in the cloud storage.";
			mDialog[7, 1] = "All clouds have an expiration date and will disappear once they run out of time. If you want to sort them by how much time they have, click on the little square next to 'TIME LEFT⇅'.";
			mDialog[7, 2] = "You can see what the clouds are made of by keeping RMB clicked on the cloud.";
			mDialog[7, 3] = "FadeOut70";
			// 우클릭 이벤트 추가 필요
			mDialog[7, 4] = "Let\'s click on the cloud you made.";
			// 화살표 UI
			mDialog[7, 5] = "FadeOut71";
			mDialog[7, 6] = "You can see the info of the customers on the right. The \'SATISFACTION\' is what you have to focus on! All customers revisit this factory in their favorite weather until the number reaches 5. Your goal is to help them reach that number.";
			mDialog[7, 7] = "The number will increase whenever one of their five \'key\' emotion reaches the right amount. It decreases once there is too much or too little of certain emotion. Listen to them well so you know which emotion may be the key!";
			mDialog[7, 8] = "Pressing the \'SEND\' button on the right Moongty\'s reception sheet will send the cloud into the Weather Room. Be careful, the customers will never visit again if their SATISFACTION reaches 0! ";
			mDialog[7, 9] = "FadeOut72";

			
			mDialog[8, 0] = "FadeOut80";
			mDialog[8, 1] = "Moongty will provide hints after they receive their cloud.";
			mDialog[8, 2] = "The customer will leave after their emotion is affected. They might revisit after a while!";
			mDialog[8, 3] = "FadeOut81";
			mDialog[8, 4] = "That's all for the guideline. I hope Moongties think of Cloud Factory as their second home. Good luck!";
			mDialog[8, 5] = "End";
		}
	}

	public void UpdateText()
	{
		// 응접실 튜토리얼 후, 날씨의 공간에서 뭉티가 자리에 앉기 전까지 텍스트 넘김을 막는다.
		if (mTutorialManager.isFinishedTutorial[1] == true
			&& mTutorialManager.isFinishedTutorial[2] == false
			&& mSOWManager.mUsingGuestObjectList.Count > 0
			&& mSOWManager.mUsingGuestObjectList[0].GetComponent<GuestObject>().isSit == false
			&& presentDialogNum >= 2)
		{ return; }
		
		presentDialogNum++;
	}

	public void ProcessSpecialText()
	{
		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut00")
		{
			GameObject.Find("GuestManager").GetComponent<Guest>().isTimeToTakeGuest = true;
			GameObject.Find("GuestManager").GetComponent<Guest>().TakeGuest();
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_Drawing Room").transform.SetAsLastSibling();
			mTutorialManager.InstantiateArrowUIObject(GameObject.Find("B_Drawing Room").transform.localPosition, -200f);
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "DialogSpace1")
		{
			GameObject.Find("DialogManager").GetComponent<DialogManager>().ReadDialog();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}
		if (mDialog[mDialogIndex, currentDialogNum] == "DialogSpace2")
		{
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut10")
		{
			GameObject.Find("DialogManager").GetComponent<DialogManager>().gTakeGuestPanel.SetActive(true);
			mTutorialManager.InstantiateArrowUIObject(GameObject.Find("B_OK").transform.localPosition, 300f);
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "Gathering")
		{
			mTutorialManager.FadeOutSpaceOfWeather();
			GameObject.Find("B_GardenSpring").transform.SetAsLastSibling();
			GameObject.Find("GatherGroup").transform.SetAsLastSibling();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut20")
		{
			mTutorialManager.InstantiateBlockScreenTouchObject();
			GameObject.Find("B_Option").transform.SetAsLastSibling();
			GameObject option_object = GameObject.Find("UIManager").GetComponent<CommonUIManager>().gOption;
			option_object.transform.SetAsLastSibling();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut21")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_Cloud Factory").transform.SetAsLastSibling();
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut30")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_GiveCloud").transform.SetAsLastSibling();
			mTutorialManager.InstantiateArrowUIObject(GameObject.Find("B_GiveCloud").transform.localPosition, 300f);
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut40")
		{
			presentDialogNum++;
			currentDialogNum++;
			mTutorialManager.FadeOutGiveCloud();
			this.gameObject.transform.SetAsLastSibling();

		}
		
		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut41")
		{
			presentDialogNum++;
			currentDialogNum++;
			mTutorialManager.SetActiveFadeOutScreen(false);
			mTutorialManager.FadeOutGiveCloud2();
			this.gameObject.transform.SetAsLastSibling();
			

		}
		
		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut42")
		{
			presentDialogNum++;
			currentDialogNum++;
			mTutorialManager.SetActiveFadeOutScreen(false);
			mTutorialManager.InstantiateArrowUIObject(new Vector3(410f, -360f, 0f), 0f);
			mTutorialManager.SetActiveArrowUIObject(false);
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut43")
		{
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut50")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_GiveCloud").transform.SetAsLastSibling();
			mTutorialManager.InstantiateArrowUIObject(GameObject.Find("B_GiveCloud").transform.localPosition + new Vector3(180f, 32f, 0f), 150f);
			mTutorialManager.SetActiveArrowUIObject(false);
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut60")
		{
			presentDialogNum++;
			currentDialogNum++;
			mTutorialManager.FadeOutDecoCloudDeco();
			this.gameObject.transform.SetAsLastSibling();
		}
		
		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut61")
		{
			presentDialogNum++;
			currentDialogNum++;
			mTutorialManager.SetActiveFadeOutScreen(false);
			mTutorialManager.FadeOutDecoCloud();
			this.gameObject.transform.SetAsLastSibling();
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut62")
		{
			presentDialogNum++;
			currentDialogNum++;

		}
		
		if (mDialog[mDialogIndex, currentDialogNum] == "GuideSpeechOut60")
		{
			mTutorialManager.InstantiateArrowUIObject(GameObject.Find("ButtonGroup(1)").transform.Find("PosButton").transform.localPosition, -200f, -260f);
			Debug.Log(GameObject.Find("ButtonGroup(1)").transform.Find("PosButton").transform.localPosition);
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}
		
		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut63")
		{
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut70")
		{
			mTutorialManager.FadeOutCloudStorage0();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut71")
		{
			mTutorialManager.FadeOutCloudStorage();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut72")
		{
			mTutorialManager.FadeOutScreen();
			GameObject tempButton = GameObject.Find("I_ProfileBG1").transform.Find("B_CloudGIve").gameObject;
			tempButton.transform.SetParent(GameObject.Find("Canvas").transform);
			tempButton.transform.SetAsLastSibling();
			mTutorialManager.InstantiateArrowUIObject(GameObject.Find("Canvas").transform.Find("B_CloudGIve").gameObject.transform.localPosition, -200f);
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut80")
		{
			mTutorialManager.InstantiateBlockScreenTouchObject();
			GameObject.Find("B_Option").transform.SetAsLastSibling();
			GameObject option_object = GameObject.Find("UIManager").GetComponent<CommonUIManager>().gOption;
			option_object.transform.SetAsLastSibling();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut81")
		{
			mTutorialManager.InstantiateBlockScreenTouchObject();
			GameObject.Find("B_Option").transform.SetAsLastSibling();
			GameObject option_object = GameObject.Find("UIManager").GetComponent<CommonUIManager>().gOption;
			option_object.transform.SetAsLastSibling();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "End")
		{
			mTutorialManager.isFinishedTutorial[8] = true;
			mTutorialManager.isTutorial = false;
			mTutorialManager.DestroyAllObject();
			GameObject.Find("B_Option").transform.SetSiblingIndex(5);
			GameObject.Find("UIManager").GetComponent<CommonUIManager>().gOption.transform.SetSiblingIndex(15);

			// 튜토리얼 데이터 저장함수
			SaveUnitManager mSaveUnitManager = GameObject.Find("SaveUnitManager").GetComponent<SaveUnitManager>();
			if (null == mSaveUnitManager)
				return;
			mSaveUnitManager.Save_Tutorial();

			return;
		}
	}
}
