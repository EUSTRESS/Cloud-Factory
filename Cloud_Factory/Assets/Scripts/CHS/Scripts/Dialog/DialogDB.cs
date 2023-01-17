using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class DialogDB : ScriptableObject
{
	// Excel에 지정한 Sheet이름과 동일한 List<DialogDBEntity> 변수를 생성
	public List<DialogDBEntity> DialogText1;
	public List<DialogDBEntity> DialogText2;
	public List<DialogDBEntity> DialogText3;
	public List<DialogDBEntity> DialogText4;
	public List<DialogDBEntity> DialogText5;
	public List<DialogDBEntity> DialogText6;
	public List<DialogDBEntity> DialogText7;
	public List<DialogDBEntity> DialogText8;
	public List<DialogDBEntity> DialogText9;
	public List<DialogDBEntity> DialogText10;
	public List<DialogDBEntity> DialogText11;
	public List<DialogDBEntity> DialogText12;
	public List<DialogDBEntity> DialogText13;
	public List<DialogDBEntity> DialogText14;
	public List<DialogDBEntity> DialogText15;
	public List<DialogDBEntity> DialogText16;
	public List<DialogDBEntity> DialogText17;
	public List<DialogDBEntity> DialogText18;
	public List<DialogDBEntity> DialogText19;
	public List<DialogDBEntity> DialogText20;

	// 모든 DialogText를 관리하는 List
	public List<List<DialogDBEntity>> DialogTexts;

	// 더 좋은 방법이 생기면 바꿀 예정	
	public List<DialogDBEntity> SetDialogByGuestNum(int guestNum)
	{

		switch (guestNum)
		{
			case 0:
				return DialogText1;
			case 1:
				return DialogText2;
			case 2:
				return DialogText3;
			case 3:
				return DialogText4;
			case 4:
				return DialogText5;
			case 5:
				return DialogText6;
			case 6:
				return DialogText7;
			case 7:
				return DialogText8;
			case 8:
				return DialogText9;
			case 9:
				return DialogText10;
			case 10:
				return DialogText11;
			case 11:
				return DialogText12;
			case 12:
				return DialogText13;
			case 13:
				return DialogText14;
			case 14:
				return DialogText15;
			case 15:
				return DialogText16;
			case 16:
				return DialogText17;
			case 17:
				return DialogText18;
			case 18:
				return DialogText19;
			case 19:
				return DialogText20;
			default:
				return null;
		}
	}

}
