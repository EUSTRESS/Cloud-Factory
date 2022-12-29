using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct GavedGuestInfo
{
    int mGuestNum;
    Dictionary<Emotion, int> emotionList;
}

public class CloudObject : MonoBehaviour
{
    public Transform targetChairPos;

    SOWManager SOWManager;
    Guest GuestManager;

    public int mGuestNum;                       // 타겟 손님 번호
    public int cloudSpeed;                      // 구름 속도
    public List<EmotionInfo> mFinalEmotions;    // 변환할 감정값 리스트

    // 처음 받아와야 하는 값
    // 1) Cloud Spawner로부터 정보값을 받아온다.

    // 내부에서 수행해야 할 기능
    // 1) 손님과의 충돌 처리 (충돌 시 사용한다는 판정)
    // 2) 충돌 시 해당 손님의 감정값이 변화하게끔 설정

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        GuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();

        // 테스트를 위한 고정값 넣기
        cloudSpeed = 1;

        // 테스트를 위한 고정 값 넣기
        targetChairPos = SOWManager.mChairPos[0].transform;
    }


    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(this.transform.position.x - targetChairPos.position.x) < 0.1f)
        {
            // 감정 값 변환함수 호출
            for (int i = 0; i < mFinalEmotions.Count; ++i)
            {
                GuestManager.SetEmotion(mGuestNum, (int)mFinalEmotions[i].Key, mFinalEmotions[i].Value);
                Debug.Log(mGuestNum + "번 손님 감정변환 함수 호출" + (int)mFinalEmotions[i].Key + " " + mFinalEmotions[i].Value);
            }

            // TODO : 구름을 사용한 손님의 정보값 갱신 (LIST : 감정 값, 상하한선 계산, 만족도 갱신)

            // 감정 상하한선 검사
            if (GuestManager.IsExcessLine(mGuestNum) != -1)
                GuestManager.mGuestInfo[mGuestNum].isDisSat = true;
            else
                Debug.Log("상하한선을 침범하지 않았습니다.");

            // 만족도 값 갱신
            GuestManager.RenewakSat(mGuestNum);

            // 감정 값 변환함수 호출 후, 제거
            Destroy(this.gameObject);

            // 구름을 받은 손님을 사용상태로 변경
            GuestManager.mGuestInfo[mGuestNum].isUsing = true;
        }
        else
        {
            Vector3 temp;
            temp = new Vector3(-0.4f, 1.4f, 0f);

            // TODO : 의자를 앉는 방향에 따라서 targetChairPos에 대한 값을 변환시켜준다.
            transform.position = Vector2.MoveTowards(transform.position, targetChairPos.position + temp, cloudSpeed * Time.deltaTime);
        }
    }


    // 아래의 4개의 함수는 구름이 생성될 때 스포너에서 사용하는 함수이다.

    public void SetTargetChair(int guestNum)
    {
        targetChairPos = SOWManager.mChairPos[GuestManager.mGuestInfo[guestNum].mSitChairIndex].transform;
    }

    public void SetValue(List<EmotionInfo> CloudData)
    {
        mFinalEmotions = CloudData;
    }

    public void SetGuestNum(int _guestNum)
    {
        mGuestNum = _guestNum;
    }

    public void SetSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

}
