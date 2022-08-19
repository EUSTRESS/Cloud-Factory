using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Pathfinding;
public class GuestObject : MonoBehaviour
{
    // ������Ʈ ������ �ʿ��� ����
    public float        mLimitTime;         // �մ��� ����ϴ� �ð��� �Ѱ谪
    public int          mGuestNum;          // �ش� ������Ʈ�� �մԹ�ȣ
    public bool         isSit;              // �ڸ��� �ɾ��ִ°�?
    public bool         isUsing;            // ���� ġ�Ḧ �޴����ΰ�?
    private Transform   mTransform;         // ��ġ���� ���ϴ��� Ȯ���ϱ� ���� ����
    private Guest       mGuestManager;
    public GameObject   mTargetChair;       // ��ǥ�� �ϴ� ������ ��ġ
    public int          mTargetChiarIndex;
    public bool         isMove;             // �̵����ΰ�?   
    public bool         isUse;              // ����� �Ϸ� �ߴ°�?

    public SOWManager mSOWManager;

    // �ִϸ��̼� ��� ���� ����
    public bool isRight;        // ������ �������� �ȴ� ���ΰ�?

    // �մ� ��ȣ�� �������ش�.
    public void setGuestNum(int guestNum = 0)
    {
        mGuestNum = guestNum;
    }

    private void Awake()
    {

        DontDestroyOnLoad(this.gameObject);

        // ���ð� �ʱ�ȭ
        mLimitTime = 0.0f;
        isSit = false;
        isUsing = false;
        isMove = false;
        isUse =  false;
        mTransform = this.transform;
        mTargetChiarIndex = -1;
        mTargetChair = null;
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

        // �մԿ� ���� �̹���(�ִϸ��̼�)�� �����Ų��.
        InitAnimationClip();


    }

    // �ȴ� �ִϸ��̼� ���
    // �ȴ� �ִϸ��̼��� ����Ʈ �ִϸ��̼����� ����

    private void Update()
    {
        // �Ҵ�޴� ���� ����
        if (mTargetChiarIndex != -1)
        {
            mTargetChair = mSOWManager.mChairPos[mTargetChiarIndex];
            this.GetComponent<AIDestinationSetter>().enabled = true;
            this.GetComponent<AIDestinationSetter>().target = mTargetChair.transform;

            // ���ڿ� �������� �ʾҴٸ� AIPATH�� Ȱ��ȭ�Ѵ�.
            if (this.transform != mTargetChair.transform)
            {
                this.GetComponent<WayPoint>().isMove = false;
                this.GetComponent<AIPath>().enabled = true;
            }
            else
            {
                this.GetComponent<AIPath>().enabled = false;
            }
        }

        // ���ð��� ���Ž�Ų��.
        mLimitTime += Time.deltaTime;

        // ���ð��� �����ų� �Ҹ���Ƽ�� �� ��쿡
        if (mLimitTime > 50.0f || mGuestManager.mGuestInfos[mGuestNum].isDisSat == true)
        {
            // ����� ����Ʈ���� ���ְ�, �ش� ���ڸ� �ٽ� true�� �ٲ��־�� �Ѵ�.
            mSOWManager.mCheckChairEmpty[mTargetChiarIndex] = true;
            mTargetChair = null;

            MoveToEntrance();
        }

        // �Ա��� ������ ���
        if(isUse == true && this.transform.position.x - mSOWManager.mWayPoint[0].transform.position.x <= 0.2f)
        {
            Destroy(this.gameObject);
        }

        // ���ڿ� ������ ���'
        if (mTargetChiarIndex != -1)
        {
            if (isUse == false && this.transform.position.x - mTargetChair.transform.position.x <= 0.2f)
            {
                // ���� ��ġ�� �̵�
                //this.transform.position = mSOWManager.mWayPoint[0].transform.position;
                isSit = true;
            }
        }

        // ���¿� ���� �ִϸ��̼� ����
        if (isSit)
        {
            // ������ �����ޱ� ���� ���ڿ� ��ġ�ϱ� ������ �ɾ��ִ� ����� ����

            // ġ�� ���� ��� ġ��ȿ���� ���� �ֱ������� �ִϸ��̼��� ����
            if (isUsing)
            {

            }
        }
        else
        {
            // ��å�θ� ������ ������̱� ������ �ȴ� ����� �����Ѵ�.

            // ������Ʈ�� ��ġ���� ������ �ʾҴٸ� ���ִ� �ִϸ��̼��� ������ش�.
            if (mTransform == this.transform)
            {

            }
            // �ٽ� ���ϴ� ��쿡�� �ȴ� �ִϸ��̼��� ����Ѵ�.
            else
            {
                // �ȴ� ���⿡ ���� �ִϸ��̼��� ������ �ٸ��� �����Ѵ�.

            }

        }

    }

    // Ŭ�� �� ��ȣ�ۿ�
    public void OpenCloudWindow()
    {
        // ���� ���� ȭ���� Ȱ��ȭ�Ѵ�.
        Debug.Log("���� ȭ���� �����մϴ�");
    }

    public void SpeakEmotion()
    {
        Debug.Log("���� ����� ����մϴ�");
        // ���� ����, ���� ������ ���� ����� ������ ���� ��Ʈ(����Ʈ)
        // ������ �ݿ� �������� ���� �� ������ �˷��ִ� ���



    }

    // �ִϸ��̼� Ŭ������ �մԿ� �°� �ʱ�ȭ�Ѵ�.
    private void InitAnimationClip()
    {

    }

    // �Ա��� �����ϴ� �Լ��̴�.
    private void MoveToEntrance()
    {
        this.GetComponent<AIDestinationSetter>().target = mSOWManager.mWayPoint[0].transform;
        isUse = true;
    }

}
