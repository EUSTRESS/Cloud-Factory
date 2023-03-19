using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LetterController : MonoBehaviour
{
	private int originalDay;

	private int[] satGuestList;
	private int listCount;

	public GameObject letterObject;

	private Guest mGuestManager;
	private SeasonDateCalc mSeasonDateCalc;

	// Start is called before the first frame update
	void Awake()
	{
		mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
		mSeasonDateCalc = GameObject.Find("Season Date Calc").GetComponent<SeasonDateCalc>();

		satGuestList = new int[6];
		listCount = 0;
		InitSatGuestList();

		originalDay = mSeasonDateCalc.mDay;
	}

	// Update is called once per frame
	void Update()
	{
		if (SceneManager.GetActiveScene().name == "Space Of Weather"
			&& originalDay != mSeasonDateCalc.mDay)
		{
			originalDay = mSeasonDateCalc.mDay;
			for (int num = 0; num < 6; num++)
			{
				if (satGuestList[num] == -1) { break; }
				InstantiateLetter(satGuestList[num]);
			}
			InitSatGuestList();
		}
	}

	public void SetSatGuestList(int guest_num)
	{
		satGuestList[listCount++] = guest_num;
	}

	private void InstantiateLetter(int guest_num)
	{
		GameObject letter = Instantiate(letterObject);
		letter.transform.SetParent(GameObject.Find("Canvas").transform);
		letter.transform.localPosition = new Vector3(-565f, -75f, 0f);

		RLHReader rRLHReader = GameObject.Find("UIManager").GetComponent<RLHReader>();

		letter.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = rRLHReader.LoadLetterInfo(guest_num);
	}

	private void InitSatGuestList()
	{
		for (int num = 0; num < 6; num++)
		{
			satGuestList[num] = -1;
		}
		listCount = 0;
	}
}
