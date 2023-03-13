using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class WeatherUIManager : MonoBehaviour
{
    private SeasonDateCalc mSeason; // 계절 계산 스크립트
    private TutorialManager mTutorialManager;
    private SOWManager mSOWManager;

	[Header("Gather")]
    public GameObject mGuideGather; // 채집할건지 안할건지 알려주는 UI
    public GameObject mGathering;   // 채집 중 출력하는 UI
    public GameObject mGatherResult;// 채집 결과를 출력하는 UI

    public GameObject[] mSeasonObj = new GameObject[4]; // 4계절 오브젝트
    public GameObject[] mSeasonUIObj = new GameObject[4]; // 4계절 UI오브젝트

    public Animator mGatheringAnim; // 채집 애니메이션

    public Text tGatheringText;      // 채집 중... 텍스트
    private int mGatheringTextCount; // 채집 중 '.' 재귀 제한

    public RectTransform mGatherImageRect; // 채집 이미지 Rect Transform

    public RectTransform[] mFxShine = new RectTransform[5]; // 5개의 채집 결과 회전 효과
    public RectTransform[] mGatherRect = new RectTransform[5]; // 5개의 채집 결과 UI 이동
    public GameObject[] mGatherObj = new GameObject[5]; // 5개의 채집 게임 오브젝트

    public int mRandomGather; // 재료 채집 랜덤 개수

    [Header("BackGround")]
    public GameObject iMainBG; // 메인 배경 이미지 
    public Sprite[] mBackground = new Sprite[4]; // 계절별로 달라지는 배경

    // 영상을 찍기위해 임시로 가져오는 마당 오브젝트들
    private GameObject[] mGardens = new GameObject[4];
    public Sprite[] mSpringGardenSprites = new Sprite[2];
	public Sprite[] mSummerGardenSprites = new Sprite[2];
	public Sprite[] mFallGardenSprites = new Sprite[2];
	public Sprite[] mWinterGardenSprites = new Sprite[2];
    private Sprite[] mSwitchGardenSprites = new Sprite[2];

	//예람
	private GameObject selectedYard;
    private void Awake()
    {
        mSeason = GameObject.Find("Season Date Calc").GetComponent<SeasonDateCalc>();
		mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
		mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
	}

    void Update()
    {
        if (mGatherResult.activeSelf)
        {
            // 채집 결과 효과
            mFxShine[0].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[1].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[2].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[3].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[4].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
        }

        switch (mSeason.mSeason)
        {
            case 1:
                UpdateSeasonBg(0);// 봄
                UpdateSeasonGarden(0);
				UpdateSeasonGardenSprites(1);
				break;
            case 2:
                UpdateSeasonBg(1);// 여름
				UpdateSeasonGarden(1);
				UpdateSeasonGardenSprites(2);
				break;
            case 3:
                UpdateSeasonBg(2);// 가을
				UpdateSeasonGarden(2);
				UpdateSeasonGardenSprites(3);
				break;
            case 4:
                UpdateSeasonBg(3); // 겨울
				UpdateSeasonGarden(3);
				UpdateSeasonGardenSprites(4);
				break;
            default:
                break;
        }
    }

    void UpdateSeasonBg(int _iCurrent)
    {
        iMainBG.GetComponent<SpriteRenderer>().sprite = mBackground[_iCurrent];
        for (int i = 0; i < 4; i++)
        {
            if (_iCurrent == i) continue;

            mSeasonObj[i].SetActive(false);
            mSeasonUIObj[i].SetActive(false);
            // 산책로 오브젝트 추가

        }

        mSeasonObj[_iCurrent].SetActive(true);
        mSeasonUIObj[_iCurrent].SetActive(true);
        // 산책로 오브젝트 추가

    }

    void UpdateSeasonGarden(int season)
    {
        GameObject seasonObj = mSeasonObj[season];
        
        for(int num = 0; num < 4; num++){
            mGardens[num] = seasonObj.transform.Find("Garden" + (num + 1)).gameObject;
			if (mSOWManager.yardGatherCount[num] <= 0) { mGardens[num].GetComponent<SpriteRenderer>().sprite = mSwitchGardenSprites[0]; }
			else { mGardens[num].GetComponent<SpriteRenderer>().sprite = mSwitchGardenSprites[1]; }
		}
	}

    void UpdateSeasonGardenSprites(int season)
    {
		mSwitchGardenSprites = new Sprite[2];
        switch (season - 1)
        {
            case 0:
                mSwitchGardenSprites = mSpringGardenSprites;
                break;
            case 1:
                mSwitchGardenSprites = mSummerGardenSprites;
                break;
            case 2:
                mSwitchGardenSprites = mFallGardenSprites;
                break;
            case 3:
                mSwitchGardenSprites = mWinterGardenSprites;
                break;
            default:
                break;
        }
    }


    // 마당 버튼 클릭 시, 채집하시겠씁니까? 오브젝트 활성화    
    public void OpenGuideGather()
    {
        selectedYard = EventSystem.current.currentSelectedGameObject;

		// 채집 횟수가 모두 차감되었으면 채집하지 않고 창을 띄우지 않는다.
		YardHandleSystem system = selectedYard.GetComponentInParent<YardHandleSystem>();
        system.UpdateYardGatherCount();
		if (system.CanBeGathered(selectedYard) == false)
		{
			Debug.Log("채집 가능 횟수가 모두 차감되었습니다.");
			return;
		}
        if (mTutorialManager.isFinishedTutorial[2] == false)
        { mTutorialManager.SetActiveFadeOutScreen(false); }
		mGuideGather.SetActive(true);
	}
    // 나가기, 채집하시겠씁니까? 오브젝트 비활성화    
    public void CloseGuideGather()
    {
		if (mTutorialManager.isFinishedTutorial[2] == false) { return; }
		mGuideGather.SetActive(false);
    }
    // 채집하기
    public void GoingToGather()
    {
		mGuideGather.SetActive(false);
        mGathering.SetActive(true);
        mGatheringTextCount = 0; // 초기화
        tGatheringText.text = "재료 채집 중"; // 초기화

        if (SeasonDateCalc.Instance) // null check
        {                            // 각 해당하는 애니메이션 출력
            Invoke("PrintGatheringText", 0.5f); // 0.5초 딜레이마다 . 추가
            if (SeasonDateCalc.Instance.mSeason == 1) // 봄이라면
                UpdateGatherAnim(1090, 590, true, false, false, false);
            else if (SeasonDateCalc.Instance.mSeason == 2) // 여름이라면
                UpdateGatherAnim(1090, 590, false, true, false, false);
            else if (SeasonDateCalc.Instance.mSeason == 3) // 가을이라면
                UpdateGatherAnim(735, 420, false, false, true, false);
            else if (SeasonDateCalc.Instance.mSeason == 4) // 겨울이라면
                UpdateGatherAnim(560, 570, false, false, false, true);
        }
        // 5초 동안 채집 후 결과 출력
        Invoke("Gathering", 5.0f);

        mSOWManager.yardGatherCount[selectedYard.transform.GetSiblingIndex()]--;
	}
    
    void UpdateGatherAnim(int _iX, int _iY, bool _bSpring, bool _bSummer, bool _bFall, bool _bWinter)
    {
        mGatherImageRect.sizeDelta = new Vector2(_iX, _iY); // 이미지 사이즈 맞추기
        mGatheringAnim.SetBool("Spring", _bSpring);
        mGatheringAnim.SetBool("Summer", _bSummer);
        mGatheringAnim.SetBool("Fall", _bFall);
        mGatheringAnim.SetBool("Winter", _bWinter);
    }

  
    void Gathering()
    {
        YardHandleSystem system = selectedYard.GetComponentInParent<YardHandleSystem>();

        mRandomGather = Random.Range(0, 5); // 0~4
        if (mTutorialManager.isFinishedTutorial[2] == false) { mRandomGather = 1; }

        // Result Image match
        GatherResultMatchWithUI(system.Gathered(selectedYard, mRandomGather));

        if (!system.CanBeGathered(selectedYard)) { mGardens[selectedYard.transform.GetSiblingIndex()].GetComponent<SpriteRenderer>().sprite = mSwitchGardenSprites[0]; }
        else { mGardens[selectedYard.transform.GetSiblingIndex()].GetComponent<SpriteRenderer>().sprite = mSwitchGardenSprites[1]; }

        if (mRandomGather % 2 == 1) // 홀수
        {
            mGatherRect[0].anchoredPosition = new Vector3(125.0f, 0.0f, 0.0f);
            mGatherRect[1].anchoredPosition = new Vector3(-125.0f, 0.0f, 0.0f);
            mGatherRect[2].anchoredPosition = new Vector3(375.0f, 0.0f, 0.0f);
            mGatherRect[3].anchoredPosition = new Vector3(-375.0f, 0.0f, 0.0f);
        }
        else
        {
            mGatherRect[0].anchoredPosition = new Vector3(0, 0.0f, 0.0f);
            mGatherRect[1].anchoredPosition = new Vector3(-225.0f, 0.0f, 0.0f);
            mGatherRect[2].anchoredPosition = new Vector3(225.0f, 0.0f, 0.0f);
            mGatherRect[3].anchoredPosition = new Vector3(-450.0f, 0.0f, 0.0f);
        }

        switch (mRandomGather) // active 관리
        {
            case 0:
                ActiveRandGather(true, false, false, false, false);
                break;
            case 1:
                ActiveRandGather(true, true, false, false, false);
                break;
            case 2:
                ActiveRandGather(true, true, true, false, false);
                break;
            case 3:
                ActiveRandGather(true, true, true, true, false);
                break;
            case 4:
                ActiveRandGather(true, true, true, true, true);
                break;
            default:
                break;
        }


        mGathering.SetActive(false);
        mGatherResult.SetActive(true);

        CancelInvoke(); // 인보크 충돌 방지를 위해서 출력 결과가 나오면 모든 인보크 꺼버림
    }

    private void GatherResultMatchWithUI(Dictionary<IngredientData, int> results)
    {
        if (results == null) { return; }

        int i = 0;
        foreach (KeyValuePair<IngredientData, int> data in results)
        {
            GameObject targetUI = mGatherObj[i];
            Image image = targetUI.transform.GetChild(1).GetComponent<Image>();
            Text text = targetUI.transform.GetChild(1).GetChild(0).GetComponent<Text>();

            image.sprite = data.Key.image;
            text.text = data.Value.ToString();

            i++;

        }
    }

    void ActiveRandGather(bool _bOne, bool _bTwo, bool _bThree, bool _bFour, bool _bFive)
    {
        mGatherObj[0].SetActive(_bOne);
        mGatherObj[1].SetActive(_bTwo);
        mGatherObj[2].SetActive(_bThree);
        mGatherObj[3].SetActive(_bFour);
        mGatherObj[4].SetActive(_bFive);
    }

    // 재귀함수로 마침표를 재귀적으로 출력한다
    void PrintGatheringText()
    {
        mGatheringTextCount++;
        tGatheringText.text = tGatheringText.text + ".";

        if (mGatheringTextCount <= 3)
        {
            Invoke("PrintGatheringText", 0.25f); // 0.25초 딜레이마다 . 추가
        }
        else // 초기화
        {
            mGatheringTextCount = 0;
            tGatheringText.text = "재료 채집 중";
            Invoke("PrintGatheringText", 0.25f); // 0.25초 딜레이마다 . 추가
        }
    }
    // 채집 끝!
    public void CloseResultGather()
    {
        if (mTutorialManager.isFinishedTutorial[2] == false)
        { 
            mTutorialManager.SetActiveGuideSpeechBubble(true);
            GameObject.Find("B_GardenSpring").transform.SetParent(GameObject.Find("Canvas").transform);
            GameObject.Find("B_GardenSpring").transform.SetSiblingIndex(5);
		}

		mGatherResult.SetActive(false);        
    }
}
