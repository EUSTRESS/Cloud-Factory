using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//GuestObject.prefab에 추가 예정
public class RLHReader : MonoBehaviour
{
	// 불러올 값들 선언
	private Guest mGuestManager;

	[SerializeField]
	private RLHDB mRLHDB;						// 대화 내용을 저장해 놓은 DB

	private string mDialogText;                 // 실제로 화면에 출력시킬 내용

	private string tText;						// 대화가 저장 될 텍스트

	// Start is called before the first frame update
	void Awake()
    {
		tText = "";
		mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
	}

	//ProfileManager.cs
	public string LoadRecordInfo(int guest_num)
	{
		List<RLHDBEntity> Record;
		Record = mRLHDB.SetHintByGuestNum(guest_num);

		if(Record == null) { return ""; }

		for(int num = 0; num < Record.Count; num++)
		{
			if (Record[num].GuestID == guest_num + 1
				&& Record[num].Type == "record")
			{ tText = "";  tText = Record[num].KOR; }
		}
		return tText;
	}

	//GuestObject.cs
    public void LoadHintInfo(int guest_num)
	{
		List<RLHDBEntity> Hint;
		Hint = mRLHDB.SetHintByGuestNum(guest_num);

		if(Hint == null) { return; }

		List<int> satEmotions = new List<int>();

		int leastDiffEmotion =	mGuestManager.SpeakLeastDiffEmotion(guest_num);
		int mostDiffEmotion =	mGuestManager.SpeakEmotionDialog(guest_num);

		if (mostDiffEmotion != -1) { satEmotions.Add(mostDiffEmotion); }	// 만족도에서 가장 멀리 떨어진 감정을 출력
		if (leastDiffEmotion != -1											// 해당 감정이 존재할 때,
			&& satEmotions.Count > 0										// 만족도 차가 가장 큰 감정이 존재할 때(존재하지 않으면, 차이가 가장 적은 감정도 존재X)
			&& !satEmotions.Contains(leastDiffEmotion))						// 만족도 차가 가장 큰 감정과 가장 작은 감정이 같은 감정이 아닐 때(만족도 범위에 없는 감정이 하나일 경우를 확인)
		{ satEmotions.Add(leastDiffEmotion); }

		if (satEmotions.Count <= 0) { return; }								// 만족도 범위에 모든 감정이 있으면 return;

		for(int num = 0; num < Hint.Count; num++)
		{
			if (Hint[num].GuestID == guest_num + 1							// 손님의 번호가 일치
				&& Hint[num].Type == "hint")								// RHL항목이 hint일 경우
			{
				foreach(int emotion in satEmotions)
				{
					if (Hint[num].Emotion == emotion) { tText += Hint[num].KOR; tText += "\n"; }
				} 
			}
		}
	}

	// UIManager.object (Scene Of Weather)
	public string LoadLetterInfo(int guest_num)
	{
		List<RLHDBEntity> letter;
		letter= mRLHDB.SetHintByGuestNum(guest_num);

		if (letter == null) { return ""; }

		for (int num = 0; num < letter.Count; num++)
		{
			if (letter[num].GuestID == guest_num + 1
				&& letter[num].Type == "letter")
			{ tText = ""; tText = letter[num].KOR; }
		}
		return tText;
	}

	public string PrintHintText()
	{
		return tText;
	}
}
