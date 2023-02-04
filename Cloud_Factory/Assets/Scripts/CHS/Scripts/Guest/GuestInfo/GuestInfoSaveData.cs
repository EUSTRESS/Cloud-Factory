using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// GuestInfo중에서 저장해야하는 정보값만을 담은 클래스
//public class GuestInfoSaveData
//{
//    public int[]    mEmotion = new int[20];
//    public int      mSatatisfaction;
//    public int      mSatVariation;
//    public bool     isDisSat;
//    public bool     isCure;
//    public int      mVisitCount;
//    public int      mNotVisitCount;
//    public bool     isChosen;
//    public int      mSitChairIndex;
//    public bool     isUsing;
//}

//// GuestManager에서 저장해야 하는 정보값들을 담은 클래스
//public class GuestManagerSaveData
//{
//    // 상수 선언
//    private const int NUM_OF_GUEST = 20;          
//    private const int NUM_OF_TODAY_GUEST_LIST = 6;

//    public GuestInfoSaveData[] GuestInfos = new GuestInfoSaveData[NUM_OF_GUEST];

//    public bool     isGuestLivingRoom;
//    public bool     isTimeToTakeGuest;
//    public int      mGuestIndex;
//    public int[]    mTodayGuestList = new int[NUM_OF_TODAY_GUEST_LIST];
//    public int      mGuestCount;
//    public int      mGuestTime;
//}

// GuestObject 내부에서 저장되어야 할 정보값들을 담은 클래스
//public class GuestObjectSaveData
//{
//    // Transform
//    public float xPos;
//    public float yPos;
//    public float xScale;

//    // GuestObject.cs
//    public int  mGuestNum;

//    public float mTargetChairXpos;
//    public float  mTargetChairYpos;

//    public int  mTargetChairIndex;
//    public bool isSit;
//    public bool isUsing;
//    public bool isMove;
//    public bool isGotoEntrance;
//    public bool isEndUsingCloud;

//    // WayPoint.cs
//    public int WayNum;
//}

//public class SOWSaveData
//{
//    // 손님 오브젝트 정보 (대기/착석 상태 따로 저장)
//    public List<GuestObjectSaveData> UsingObjectsData = new List<GuestObjectSaveData>();
//    public List<GuestObjectSaveData> WaitObjectsData = new List<GuestObjectSaveData>(); 

//    public int mMaxChairNum;
//    public Dictionary<int, bool> mCheckChairEmpty =  new Dictionary<int, bool>();
//}

