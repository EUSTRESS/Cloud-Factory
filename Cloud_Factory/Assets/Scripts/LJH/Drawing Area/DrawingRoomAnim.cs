using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 응접실에 누가 방문했을 때 나오는 UI

public class DrawingRoomAnim : MonoBehaviour
{
    private Guest mGuestManager;
    public GameObject mExM;
    private bool isOnetime;

    void Awake()
    {
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();

    }
    void Update()
    {
        if (mGuestManager.isGuestInLivingRoom && !isOnetime)
        {
            mExM.SetActive(true);
            isOnetime = true;
        }
        else if (!mGuestManager.isGuestInLivingRoom && isOnetime)
        {
            mExM.SetActive(true);
            isOnetime = false;
        }
    }
}
