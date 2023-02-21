using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
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

    [Header("���� ����")]
    public int mGuestNum;                       // Ÿ�� �մ� ��ȣ
    public float cloudSpeed;                      // ���� �ӵ�
    public List<EmotionInfo> mFinalEmotions;    // ��ȯ�� ������ ����Ʈ
    private StoragedCloudData virtualCloudData;

    [Header("���� �̵� �� ��� ����")]
    public bool isAlive = false;                //
    public GameObject Target;                   // ��ǥ �մ�
    public float DelayToUse;                    // ���� ���ð�

    // ó�� �޾ƿ;� �ϴ� ��
    // 1) Cloud Spawner�κ��� �������� �޾ƿ´�.

    // ���ο��� �����ؾ� �� ���
    // 1) �մ԰��� �浹 ó�� (�浹 �� ����Ѵٴ� ����)
    // 2) �浹 �� �ش� �մ��� �������� ��ȭ�ϰԲ� ����

    public RuntimeAnimatorController[] animValue2;
    

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        GuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        animValue2 = new RuntimeAnimatorController[8];
        DelayToUse = 20.0f;
        cloudSpeed = 0f;

        // TODO : MoveCloud Animator�� ������ �°� ����
        
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

                // TODO : ���ڸ� �ɴ� ���⿡ ���� targetChairPos�� ���� ���� ��ȯ�����ش�.
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
            // ������ ���� �մ��� �����·� ����
            GuestManager.mGuestInfo[mGuestNum].isUsing = true;

            Target = collision.gameObject.transform.root.gameObject;

            // ������ ����ϴ� �մ��� �Ӹ� ��ġ�� �̵���Ų��.
            this.transform.position = collision.gameObject.transform.position;
            this.transform.Translate(new Vector3(0.0f, 1.2f, 0.0f));

            // ������ ������� ����� ����. (�ִϸ��̼� ����)
            {
                // moveCloud�� Using���·� �ٲ۴�.
                //this.transform.GetChild(0).gameObject.SetActive(false);
                this.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Using");

                // VirtualObjectManager�� ���ؼ� ������Ʈ�� ������. (������ ��Ÿ���� ���� ���)
                VirtualObjectManager vObjectManager = GameObject.Find("VirtualObjectManager").GetComponent<VirtualObjectManager>();
                GameObject tempObject = vObjectManager.InstantiateVirtualObjectToSceneToSprite(virtualCloudData, this.transform.position);
                tempObject.transform.SetParent(this.transform);
            }

            // ������ ������� ���·θ����.
            StartCoroutine("WaitForSetEmotion");

            // ������ �������� ����� ����� ������� ������ ��ȭ���� ���� �մ� �ִϸ��̼� ����
            int prevSat = GuestManager.mGuestInfo[mGuestNum].mSatatisfaction;

            // ���� ���� -> ������ �������� ���� ��, �������� �����Ѽ� �������� ���Ͽ� ������Ʈ�Ѵ�.
            for (int i = 0; i < mFinalEmotions.Count; ++i)
            {
                GuestManager.SetEmotion(mGuestNum, (int)mFinalEmotions[i].Key, mFinalEmotions[i].Value);               
            }
            GuestManager.RenewakSat(mGuestNum);

            int currSat = GuestManager.mGuestInfo[mGuestNum].mSatatisfaction;

            // ����ǥ���� ���� �������� �����Ѵ�.
            collision.gameObject.transform.root.gameObject.GetComponent<GuestObject>().faceValue = GuestManager.SpeakEmotionEffect(mGuestNum);
            collision.gameObject.transform.root.gameObject.GetComponent<GuestObject>().dialogEmotion = GuestManager.SpeakEmotionDialog(mGuestNum);

            // ���� ���� ���� -> ���� ����Ǵ� �ð��� �����Ͽ��� �ϱ� ������ �ٽ� ������ ���� ���ش�.
            for (int i = 0; i < mFinalEmotions.Count; ++i)
            {
                GuestManager.SetEmotion(mGuestNum, (int)mFinalEmotions[i].Key, mFinalEmotions[i].Value * -1);
            }
            GuestManager.RenewakSat(mGuestNum);

            collision.gameObject.transform.root.gameObject.GetComponent<GuestObject>().mGuestAnim.SetInteger("increase", currSat - prevSat);

            // ���̻� ������ ���� �浹üũ�� �ʿ� �����Ƿ� ��Ȱ��ȭ
            this.GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    // ������ ����ϴ� �� ��ٸ��� �Լ��̴�.
    IEnumerator WaitForSetEmotion()
    {
        Debug.Log("�ڷ�ƾ ����");
        yield return new WaitForSeconds(DelayToUse);

        bool result = UsingCloud();

        if(!result)
        {
            Debug.Log("���� �������� �մԿ��� �����ϴ� �������� ������ �߻��Ͽ����ϴ�.");
        }
        else
        {
            Target.GetComponent<GuestObject>().isEndUsingCloud = true;
            Debug.Log("�ڷ�ƾ ����");

            // ��� ������ ��ģ ��, ����
            // TODO -> ���� �Ҹ� �ִϸ��̼��� ����ϴ� ������ ���� (���� �Ҹ� �ִϸ��̼ǿ� ���� ������Ʈ�� �����ϴ� ����� �߰�)
            this.transform.GetChild(0).GetComponent<Animator>().SetTrigger("End");
            GuestManager.mGuestInfo[mGuestNum].isUsing = false;
        }
    }

    public void StopToUse()
    {
        if(Target != null)
            Target.GetComponent<GuestObject>().isEndUsingCloud = true;
        Debug.Log("�ڷ�ƾ ���� �ߴ�");
        GuestManager.mGuestInfo[mGuestNum].isUsing = false;
        this.transform.GetChild(0).GetComponent<Animator>().SetTrigger("End");
    }
        

    bool UsingCloud()
    {
        // ���� �� ��ȯ�Լ� ȣ��
        for (int i = 0; i < mFinalEmotions.Count; ++i)
        {
            GuestManager.SetEmotion(mGuestNum, (int)mFinalEmotions[i].Key, mFinalEmotions[i].Value);
            Debug.Log(mGuestNum + "�� �մ� ������ȯ �Լ� ȣ��" + (int)mFinalEmotions[i].Key + " " + mFinalEmotions[i].Value);
        }

        // TODO : ������ ����� �մ��� ������ ���� (LIST : ���� ��, �����Ѽ� ���, ������ ����)

        // ���� �����Ѽ� �˻�
        if (GuestManager.IsExcessLine(mGuestNum) != -1)
        {
            GuestManager.mGuestInfo[mGuestNum].isDisSat = true;
        }
        else
            Debug.Log("�����Ѽ��� ħ������ �ʾҽ��ϴ�.");

        // ������ �� ����
        GuestManager.RenewakSat(mGuestNum);

        //����� ���� ���� ������Ʈ
        GuestManager.mGuestInfo[mGuestNum].mUsedCloud.Add(virtualCloudData);

        return true;
    }


    // �Ʒ��� 4���� �Լ��� ������ ������ �� �����ʿ��� ����ϴ� �Լ��̴�.
    public void SetTargetChair(int guestNum)
    {
        targetChairPos = SOWManager.mChairPos[GuestManager.mGuestInfo[guestNum].mSitChairIndex].transform;
    }

    public void SetValue(StoragedCloudData CloudData)
    {
        virtualCloudData = CloudData;
        mFinalEmotions = CloudData.mFinalEmotions;
    }

    public void SetGuestNum(int _guestNum)
    {
        mGuestNum = _guestNum;
    }

    public void SetSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }


    // ���� -> �̵����� �Ѿ �� �ִϸ��̼� �̺�Ʈ�� �ߵ��ȴ�.
    public void SetSpeed()
    {
        //���� ��ġ���� ��ǥ ������ ��ġ���� �޾ƿͼ� ���� ����Ѵ�.
        // distance = ���� ��ġ�� ���� ��ġ�� �Ÿ�
        float distance = Mathf.Sqrt(Mathf.Pow(this.transform.position.x - targetChairPos.position.x, 2f) 
            + Mathf.Pow(this.transform.position.y - targetChairPos.position.y, 2f));

        // �Ÿ����� �⺻���� ���ϴ� ������ �̿��Ͽ� �������� ������ �����.
        cloudSpeed = 0.1f * distance;

        Debug.Log(cloudSpeed);
    }

}
