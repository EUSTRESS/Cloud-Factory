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
    // 처음 받아와야 하는 값
    // 1) Cloud Spawner로부터 정보값을 받아온다.
    public Transform targetChairPos;

    SOWManager SOWManager;
    Guest GuestManager;

    public int mGuestNum;                       // 타겟 손님 번호
    public int cloudSpeed;                      // 구름 속도
    public List<EmotionInfo> mFinalEmotions;    // 변환할 감정값 리스트

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
            Debug.Log(mFinalEmotions.Count);

            // 감정 값 변환함수 호출 후, 제거
            Destroy(this.gameObject);
            Debug.Log("구름을 화면상에서 제거");

            // 구름을 받은 손님을 사용상태로 변경
            GuestManager.mGuestInfos[mGuestNum].isUsing = true;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, targetChairPos.position, cloudSpeed * Time.deltaTime);
        }
    }

    public void SetTargetChair(int guestNum)
    {
        targetChairPos = SOWManager.mChairPos[GuestManager.mGuestInfos[guestNum].mSitChairIndex].transform;
    }

    public void SetValue(List<EmotionInfo> CloudData)
    {
        mFinalEmotions = CloudData;
    }

    public void SetGuestNum(int _guestNum)
    {
        mGuestNum = _guestNum;
    }

    public void SetSprite(Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

}
