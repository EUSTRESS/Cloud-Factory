using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 응접실 UI
public class DrawingUIManager : MonoBehaviour
{
    [SerializeField] bool       mTextEnd;      // 텍스트 출력이 끝났는 지?
    [SerializeField] GameObject gSpeechBubble; // 말풍선 오브젝트
    [SerializeField] GameObject gOkNoGroup;    // 수락 거절 오브젝트

    public Text guestNum;
    public Text guestName;
    public Text guestSat;
    public Text guestVC;
    public Text guestSV;


    private Guest mGuestManager;

    void Start()
    {
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        int gn = mGuestManager.mGuestIndex;
        guestNum.text = "Guest Num: " + mGuestManager.mGuestIndex.ToString();
        guestName.text = "Name: " + mGuestManager.mGuestInfo[gn].mName.ToString();
        guestSat.text = "Sat: " + mGuestManager.mGuestInfo[gn].mSatatisfaction.ToString();
        guestVC.text = "VC: " + mGuestManager.mGuestInfo[gn].mVisitCount.ToString();
        guestSV.text = "SV: " + mGuestManager.mGuestInfo[gn].mSatVariation.ToString();


    }
    
    void Update()
    {
        if (mTextEnd == true) // 텍스트 출력이 끝났다면
        {
            gSpeechBubble.SetActive(false);
            gOkNoGroup.SetActive(true);
        }        
    }

    public void ActiveOk()
    {
        gSpeechBubble.SetActive(true);
        gOkNoGroup.SetActive(false);
        mTextEnd = false;

        // 수락했을 때 메소드 호출
        Debug.Log("응접실 수락 메소드 호출");
    }

    public void ActiveNo()
    {
        TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        if (!mTutorialManager.isFinishedTutorial[1]) { return; }

        gSpeechBubble.SetActive(true);
        gOkNoGroup.SetActive(false);
        mTextEnd = false;

        // 거절했을 때 메소드 호출
        Debug.Log("응접실 거절 메소드 호출");
    }
}
