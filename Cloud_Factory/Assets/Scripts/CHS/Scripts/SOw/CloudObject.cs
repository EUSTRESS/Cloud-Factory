using System.Collections.Generic;
using System.Collections;
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

    [Header("구름 정보")]
    public int mGuestNum;                       // 타겟 손님 번호
    public int cloudSpeed;                      // 구름 속도
    public List<EmotionInfo> mFinalEmotions;    // 변환할 감정값 리스트

    [Header("구름 이동 및 사용 정보")]
    public bool isAlive = false;                //
    public GameObject Target;                   // 목표 손님
    public float DelayToUse;                    // 구름 사용시간

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
        DelayToUse = 20.0f;

        // 테스트를 위한 고정 값 넣기
        targetChairPos = SOWManager.mChairPos[0].transform;
    }


    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            if (Mathf.Abs(this.transform.position.x - targetChairPos.position.x) < 0.1f)
            {
                isAlive = true;
            }
            else
            {
                Vector3 temp;
                temp = new Vector3(-0.4f, 1.4f, 0f);

                // TODO : 의자를 앉는 방향에 따라서 targetChairPos에 대한 값을 변환시켜준다.
                transform.position = Vector2.MoveTowards(transform.position, targetChairPos.position + temp, cloudSpeed * Time.deltaTime);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopToUse();

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Guest" && isAlive)
        {
            // 구름을 받은 손님을 사용상태로 변경
            GuestManager.mGuestInfo[mGuestNum].isUsing = true;

            Target = collision.gameObject.transform.root.gameObject;

            // 구름을 사용하는 손님의 머리 위치로 이동시킨다.
            this.transform.position = collision.gameObject.transform.position;
            this.transform.Translate(new Vector3(0.0f, 0.8f, 0.0f));
            
            // 구름을 사용중인 모션을 띄운다. (애니메이션 변경)


            // 구름을 사용중인 상태로만든다.
            StartCoroutine("WaitForSetEmotion");

            // 구름의 감정값을 적용된 경우의 결과값의 만족도 변화량에 따라서 손님 애니메이션 변경

            int prevSat = GuestManager.mGuestInfo[mGuestNum].mSatatisfaction;

            // 임의 적용 -> 구름의 감정값을 더한 후, 만족도와 상하한선 근접값을 구하여 업데이트한다.
            for (int i = 0; i < mFinalEmotions.Count; ++i)
            {
                GuestManager.SetEmotion(mGuestNum, (int)mFinalEmotions[i].Key, mFinalEmotions[i].Value);               
            }
            GuestManager.RenewakSat(mGuestNum);

            int currSat = GuestManager.mGuestInfo[mGuestNum].mSatatisfaction;

            // 감정표현에 사용될 변수들을 갱신한다.
            collision.gameObject.transform.root.gameObject.GetComponent<GuestObject>().faceValue = GuestManager.SpeakEmotionEffect(mGuestNum);
            collision.gameObject.transform.root.gameObject.GetComponent<GuestObject>().dialogEmotion = GuestManager.SpeakEmotionDialog(mGuestNum);

            // 임의 적용 해제 -> 실제 적용되는 시간에 적용하여야 하기 때문에 다시 더해준 값을 빼준다.
            for (int i = 0; i < mFinalEmotions.Count; ++i)
            {
                GuestManager.SetEmotion(mGuestNum, (int)mFinalEmotions[i].Key, mFinalEmotions[i].Value * -1);
            }
            GuestManager.RenewakSat(mGuestNum);

            collision.gameObject.transform.root.gameObject.GetComponent<GuestObject>().mGuestAnim.SetInteger("increase", currSat - prevSat);

            // 더이상 구름에 대한 충돌체크가 필요 없으므로 비활성화
            this.GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    // 구름을 사용하는 중 기다리는 함수이다.
    IEnumerator WaitForSetEmotion()
    {
        Debug.Log("코루틴 시작");
        yield return new WaitForSeconds(DelayToUse);

        bool result = UsingCloud();

        if(!result)
        {
            Debug.Log("구름 정보값을 손님에게 전달하는 과정에서 오류가 발생하였습니다.");
        }
        else
        {
            Target.GetComponent<GuestObject>().isEndUsingCloud = true;
            Debug.Log("코루틴 종료");

            // 모든 과정을 마친 후, 제거
            Destroy(this.gameObject);
        }
    }

    public void StopToUse()
    {
        if(Target != null)
            Target.GetComponent<GuestObject>().isEndUsingCloud = true;
        Debug.Log("코루틴 강제 중단");

        Destroy(this.gameObject);
    }
        

    bool UsingCloud()
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
        {
            GuestManager.mGuestInfo[mGuestNum].isDisSat = true;
        }
        else
            Debug.Log("상하한선을 침범하지 않았습니다.");

        // 만족도 값 갱신
        GuestManager.RenewakSat(mGuestNum);

        return true;
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
