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

	// 모든 DialogText를 관리하는 List
	public List<List<DialogDBEntity>> DialogTexts;

	// 더 좋은 방법이 생기면 바꿀 예정	
	public List<DialogDBEntity> SetDialogByGuestNum(int guestNum)
    {
		if (guestNum == 0)
			return DialogText1;
		else if (guestNum == 1)
			return DialogText2;
		else
			return null;
    }

}
