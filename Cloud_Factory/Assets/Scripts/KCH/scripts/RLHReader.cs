using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//GuestObject.prefab에 추가 예정
public class RLHReader : MonoBehaviour
{
	// 불러올 값들 선언
	private Guest mGuestManager;
	private int mGuestNum;                       // 손님의 번호를 넘겨받는다.

	[SerializeField]
	private RLHDB mRLHDB;                 // 대화 내용을 저장해 놓은 DB

	private string mDialogText;                 // 실제로 화면에 출력시킬 내용

	public Text tText;                          // 대화가 진행 될 텍스트

	void SetGuestNum(int guest_num = 0) { mGuestNum = guest_num; }

	// Start is called before the first frame update
	void Awake()
    {
		tText.text = "";
		mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
	}

    void LoadHintInfo()
	{
		List<RLHDBEntity> Hint;
		Hint = mRLHDB.SetHintByGuestNum(mGuestNum);

		if(Hint == null) { return; }

		List<int> satEmotions = new List<int>();

		int leastDiffEmotion =	mGuestManager.SpeakLeastDiffEmotion(mGuestNum);
		int mostDiffEmotion =	mGuestManager.SpeakEmotionDialog(mGuestNum);

		if (mostDiffEmotion != -1) { satEmotions.Add(mostDiffEmotion); }	// 만족도에서 가장 멀리 떨어진 감정을 출력
		if (leastDiffEmotion != -1											// 해당 감정이 존재할 때,
			&& satEmotions.Count > 0										// 만족도 차가 가장 큰 감정이 존재할 때(존재하지 않으면, 차이가 가장 적은 감정도 존재X)
			&& !satEmotions.Contains(leastDiffEmotion))						// 만족도 차가 가장 큰 감정과 가장 작은 감정이 같은 감정이 아닐 때(만족도 범위에 없는 감정이 하나일 경우를 확인)
		{ satEmotions.Add(leastDiffEmotion); }

		if (satEmotions.Count <= 0) { return; }								// 만족도 범위에 모든 감정이 있으면 return;

		for(int num = 0; num < Hint.Count; num++)
		{
			if (Hint[num].GuestID == mGuestNum								// 손님의 번호가 일치
				&& Hint[num].Type == "Hint")								// RHL항목이 Hint일 경우
			{ 
				foreach(int emotion in satEmotions)
				{
					if (Hint[num].Emotion == emotion) { tText.text += Hint[num].KOR; }
				} 
			}
		}
	}
}

/*
 * 추가 할 것들
 * SOWManager.cs 119:0 tempObject.GetComponent<RLHReader>().SetGuestNum(guest_num);
 * */
