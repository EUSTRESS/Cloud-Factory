using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class RLHDB : ScriptableObject
{
	public List<RLHDBEntity> HintText1;

	public List<List<RLHDBEntity>> HintTexts;

	public List<RLHDBEntity> SetHintByGuestNum(int guest_num)
	{
		switch (guest_num)
		{
			case 0:
				return HintText1;
			default:
				return null;
		}
	}
}
