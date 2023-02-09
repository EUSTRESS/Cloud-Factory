using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    // 튜토리얼이 완전히 종료되었는지 체크
    [HideInInspector]
	public bool isTutorial;

    /*
     #튜토리얼 진행도 체크
     날씨의 공간 1
	 응접실
	 날씨의 공간 2 (채집)
	 구름 공장
	 구름 데코
	 구름 제공
	 손님 배웅
    */
    [HideInInspector]
	public bool[] isFinishedTutorial;               
                  

	private static TutorialManager instance = null;
    void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            isTutorial = true;
            isFinishedTutorial = new bool[7];
            for(int num = 0; num < isFinishedTutorial.Length; num++) { isFinishedTutorial[num] = false; }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
