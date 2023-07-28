using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 응접실에 누가 방문했을 때 나오는 UI

public class DrawingRoomAnim : MonoBehaviour
{
    private Guest       mGuestManager;
    public  GameObject  mExM;

    void Awake()
    {
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();

    }
    void Update()
    {
        if (mGuestManager.isGuestInLivingRoom)
        {
            mExM.SetActive(true);
        }
        else
        {
            mExM.SetActive(false);
        }
    }
}
