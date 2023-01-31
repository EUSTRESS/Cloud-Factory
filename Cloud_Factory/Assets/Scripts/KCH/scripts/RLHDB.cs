using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class RLHDB : ScriptableObject
{
	public List<RLHDBEntity> HintText1;
	public List<RLHDBEntity> HintText2;
	public List<RLHDBEntity> HintText3;
	public List<RLHDBEntity> HintText4;
	public List<RLHDBEntity> HintText5;
	public List<RLHDBEntity> HintText6;
	public List<RLHDBEntity> HintText7;
	public List<RLHDBEntity> HintText8;
	public List<RLHDBEntity> HintText9;
	public List<RLHDBEntity> HintText10;
	public List<RLHDBEntity> HintText11;
	public List<RLHDBEntity> HintText12;
	public List<RLHDBEntity> HintText13;
	public List<RLHDBEntity> HintText14;
	public List<RLHDBEntity> HintText15;
	public List<RLHDBEntity> HintText16;
	public List<RLHDBEntity> HintText17;
	public List<RLHDBEntity> HintText18;
	public List<RLHDBEntity> HintText19;
	public List<RLHDBEntity> HintText20;

	public List<List<RLHDBEntity>> HintTexts;

	public List<RLHDBEntity> SetHintByGuestNum(int guest_num)
	{
		switch (guest_num)
		{
			case 0:
				return HintText1;
			case 1:
				return HintText2;
			case 2:
				return HintText3;
			case 3:
				return HintText4;
			case 4:
				return HintText5;
			case 5:
				return HintText6;
			case 6:
				return HintText7;
			case 7:
				return HintText8;
			case 8:
				return HintText9;
			case 9:
				return HintText10;
			case 10:
				return HintText11;
			case 11:
				return HintText12;
			case 12:
				return HintText13;
			case 13:
				return HintText14;
			case 14:
				return HintText15;
			case 15:
				return HintText16;
			case 16:
				return HintText17;
			case 17:
				return HintText18;
			case 18:
				return HintText19;
			case 19:
				return HintText20;
			default:
				return null;
		}
	}
}
