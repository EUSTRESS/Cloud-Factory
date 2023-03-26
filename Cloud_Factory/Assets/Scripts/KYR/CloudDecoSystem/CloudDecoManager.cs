using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
public class CloudDecoManager : MonoBehaviour
{
    //파츠 증감소 클래스
    [System.Serializable]
    public class PartsMenu
    {
        private GameObject I_Image;
        private GameObject B_Pos;
        private GameObject B_Neg;

        public bool state; //감소 혹은 증가?
        public bool isInit;
        public PartsMenu(GameObject _B_decoParts, GameObject _I_PartsMenu, GameObject _B_PosNeg, int _idx)
        {
            I_Image = _I_PartsMenu.transform.GetChild(_idx).GetChild(0).gameObject;
            B_Pos = _B_PosNeg.transform.GetChild(_idx).GetChild(0).gameObject;
            B_Neg = _B_PosNeg.transform.GetChild(_idx).GetChild(1).gameObject;
            isInit = true;

            I_Image.transform.GetComponent<Image>().sprite = _B_decoParts.transform.GetChild(_idx).GetChild(0).GetChild(0).GetComponent<Image>().sprite;
        }

        public void btnClicked(Sprite[] _I_SelectedSticker, Sprite[] _I_UnSelectedSticker)
        {
            if (!state)
            {
                B_Pos.GetComponent<Image>().sprite = _I_SelectedSticker[0];
                B_Neg.GetComponent<Image>().sprite = _I_UnSelectedSticker[1];
                state = true;
            }
            else
            {
                B_Pos.GetComponent<Image>().sprite = _I_UnSelectedSticker[0];
                B_Neg.GetComponent<Image>().sprite = _I_SelectedSticker[1];
                state = false;
            }
        }

        public bool getPartsNPState()
        {
            return state;
        }

    }
    public GameObject CaptureZone;
    //Sprite Merger

    public List<PartsMenu> mLPartsMenu;
    //데코용 버튼 그룹
    public GameObject B_decoParts;
    public GameObject B_PosNeg;
    public GameObject[] B_Edits; //frame : 이동, Rotate: 회전. 외부에서 대입.

    public GameObject P_FinSBook;
    //Text
    public GameObject[] T_CountInfo; //파츠개수

    //Image
    public GameObject I_targetCloud; //스케치북 위에 올리는 구름 이미지 게임오브젝트
    private GameObject I_BasicDecoCloud;
    public GameObject I_PartsMenu; //증감소 파츠 메뉴

    public Sprite[] I_SelectedSticker; //선택됨을 알려주는 테이프 이미지
    public Sprite[] I_UnSelectedSticker; //default img

    public CloudData mBaseCloudDt;// 구름공장에서 구름 데이터 제공.

    private bool cursorChasing;
    private bool isDecoDone;


    private DecoParts selectedParts;
    private List<List<GameObject>> mUIDecoParts;
    private int mCloudPieceDecoMax;
    private InventoryManager inventoryManager;
    private List<GameObject> LDecoParts;
    //스케치북 기즈모
    public Vector2 top_right_corner;
    public Vector2 bottom_left_corner;



    private void Start()
    {
        mUIDecoParts = new List<List<GameObject>>();
        LDecoParts = new List<GameObject>();
        //(씬 이동시에만 가능)
        inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();


        initParam();
        init();
    }

    private void initParam()
    {
        cursorChasing = false;
        isDecoDone = false;
        //UI 세팅
        for (int i = 0; i < 3; i++)
        {
            Transform tmp = B_decoParts.transform.GetChild(i);
            List<GameObject> Ltmp = new List<GameObject>();
            for (int j = 0; j < 3; j++)
                Ltmp.Add(tmp.GetChild(j).gameObject);


            mUIDecoParts.Add(Ltmp);
        }
        mLPartsMenu = new List<PartsMenu>();

    }
    private void init()
    {
        //Cloud data 가져오기.(씬이동시에만가능)
        mBaseCloudDt = getTargetCloudData();


        //클라우드 데이터에 따라 UI에 이미지 삽입.
        //Set Base Cloud Image on Sketchbook.
        I_targetCloud.transform.GetComponent<Image>().sprite = mBaseCloudDt.getForDecoCloudSprite();

        //Set Deco Parts as Dt cnt: mUIDecoParts[1],mUIDecoParts[2]
        for (int i = 0; i < mBaseCloudDt.getDecoPartsCount(); i++)
        {
            List<GameObject> tmpList = mUIDecoParts[i];
            for (int j = 0; j < 3; j++)
            {
                if (i == 0)        //Set CloudPiece        
                    mUIDecoParts[i][j].transform.GetChild(0).GetComponent<Image>().sprite = mBaseCloudDt.getCloudParts()[j];


                mUIDecoParts[i + 1][j].transform.GetChild(0).GetComponent<Image>().sprite = mBaseCloudDt.getSizeDifferParts(i)[j];
            }

        }

        for (int i = 0; i < mBaseCloudDt.getDecoPartsCount() + 1; i++)
        {
            mLPartsMenu.Add(new PartsMenu(B_decoParts, I_PartsMenu, B_PosNeg, i));

        }

        //파츠 개수 제한.
        mCloudPieceDecoMax = 10; //구름조각은 10개로 한정.
        List<int> cntList = mBaseCloudDt.getMaxDecoPartsCount();
        for (int i = 0; i < mBaseCloudDt.getDecoPartsCount(); i++)
        {
            T_CountInfo[i].GetComponent<TMP_Text>().text = cntList[i].ToString();
        }
    }

    private CloudData getTargetCloudData()
    {
        return inventoryManager.createdCloudData;
    }

    //UI Button Functions
    public void cloudDecoDoneBtn() //마지막 스케치북 결과 ㅣ OK 버튼
    {
		TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
		if (mTutorialManager.isFinishedTutorial[6] == false) { mTutorialManager.isFinishedTutorial[6] = true; }
		List<int> mEmoValues = new List<int>();
        //감정계산.
        for (int i = 0; i < mLPartsMenu.Count; i++)
        {
            if (mLPartsMenu[i].getPartsNPState())
                mEmoValues.Add(1);
            else
                mEmoValues.Add(-1);
        }
        mBaseCloudDt.addFinalEmotion(mEmoValues);

        inventoryManager.addStock(I_targetCloud);
        //LoadScene
        SceneManager.LoadScene("Cloud Storage");
    }

    public void cloudDecoBackBtn() // 스케치북 결과 | Reset 버튼
    {
        TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        if (mTutorialManager.isFinishedTutorial[6] == false) { return; }
        Destroy(P_FinSBook.transform.GetChild(0).GetChild(0).gameObject); //삭제
        I_targetCloud = I_BasicDecoCloud;
        initParam();
        init();

        for (int i = 0; i < B_PosNeg.transform.childCount; i++)
        {
            B_PosNeg.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = I_UnSelectedSticker[0];
            B_PosNeg.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = I_UnSelectedSticker[1];
        }

        P_FinSBook.SetActive(false);
    }

    public void clickedAutoSettingBtn() //자동 배치
    {
        float width_max_range = I_targetCloud.GetComponent<RectTransform>().rect.width/2;                                  // 파츠가 구름 밖으로 벗어나지 않도록 범위 조정
        float Height_max_range = I_targetCloud.GetComponent<RectTransform>().rect.height/2;                                
        Vector2 I_targetCloudPosition = new Vector2(I_targetCloud.transform.position.x, I_targetCloud.transform.position.y);    // 구름 이미지의 중심 벡터를 받아온다.

        GameObject[] cloudParts = new GameObject[mBaseCloudDt.getDecoPartsCount()];      // 구름 파츠들의 위치와 크기 비교를 위해 지역 변수로 선언
        int partsIdx = 0;

        // Vector2 top_right_corner = I_targetCloud.Rect
        for (int i = 0; i < mBaseCloudDt.getDecoPartsCount(); i++)
        {
			for (int j = 0; j < int.Parse(T_CountInfo[i].GetComponent<TMP_Text>().text); j++)
            {
                GameObject partsObj = B_decoParts.transform.GetChild(i + 1).GetChild(Random.Range(0,3)).GetChild(0).gameObject; //Image GameObject
                float x = Random.Range(-width_max_range, width_max_range);
				float y = Random.Range(-Height_max_range, Height_max_range);

                // 범위를 줄여도 구름 밖으로 파츠가 튀어나오는 부분이 존재. 그 부분에 파츠가 부착되면, 위치를 다시 설정하도록
                while (!IsDecoPartInCloud(x, y))
                {
					x = Random.Range(-width_max_range, width_max_range);
					y = Random.Range(-Height_max_range, Height_max_range);
				}

                cloudParts[partsIdx] = Instantiate(partsObj, new Vector2(0, 0), transform.rotation);
				cloudParts[partsIdx].transform.SetParent(transform);
				cloudParts[partsIdx].transform.position = new Vector2(x, y) + I_targetCloudPosition;        // 랜덤으로 생성한 좌표를 기준점(구름 이미지의 중심 벡터) 기준으로 위치를 선정해준다.

                //  파츠 범위에 다른 파츠가 없는지 검색 필요 transform.localScale비교 
                if (partsIdx > 0) {   // 두 번째 파츠를 부착 할 때부터 겹쳤는지 확인
                    for(int num = 0; num < partsIdx;)
                    {
                        if(IsObjectOverlapped(cloudParts[num], cloudParts[partsIdx]))   // 파츠가 겹쳤으면, 새로운 파츠 위치 재선정
                        {
							x = Random.Range(-width_max_range, width_max_range);
							y = Random.Range(-Height_max_range, Height_max_range);
							cloudParts[partsIdx].transform.position = new Vector2(x, y) + I_targetCloudPosition;
						}
                        else if(!IsDecoPartInCloud(x, y))
                        {
							x = Random.Range(-width_max_range, width_max_range);
							y = Random.Range(-Height_max_range, Height_max_range);
							cloudParts[partsIdx].transform.position = new Vector2(x, y) + I_targetCloudPosition;
						}
                        else { num++; }
                    }
                }
            }

            T_CountInfo[i].GetComponent<TMP_Text>().text = "0";
        }
    }

    private bool IsObjectOverlapped(GameObject existing_object, GameObject new_object)
    {
        float existing_width_range = existing_object.GetComponent<RectTransform>().rect.width / 2;                                  // 기존 오브젝트의 가로 길이 / 2
        float existing_height_range = existing_object.GetComponent<RectTransform>().rect.height / 2;                                // 기존 오브젝트의 세로 길이 / 2
        Vector2 existing_position = new Vector2(existing_object.transform.position.x, existing_object.transform.position.y);        // 기존 오브젝트의 위치

        float new_width_range = new_object.GetComponent<RectTransform>().rect.width / 2;                                            // 비교할 오브젝트의 가로 길이 / 2
        float new_height_range = new_object.GetComponent<RectTransform>().rect.height / 2;                                         // 비교할 오브젝트의 세로 길이 / 2
        Vector2 new_position = new Vector2(new_object.transform.position.x, new_object.transform.position.y);                       // 비교할 오브젝트의 위치

        // 비교할 오브젝트가 기존 오브젝트보다 우측에 위치한 경우
        if ((new_position.x >= existing_position.x)
            && (new_position.x - existing_position.x >= new_width_range + existing_width_range))                                    // 두 오브젝트의 x축 차이가 크므로 겹칠 수 없다
        { return false; }
        else if((new_position.x >= existing_position.x)
			&& (new_position.x - existing_position.x < new_width_range + existing_width_range))                                     // 두 오브젝트의 x축 차이가 충분히 크지 않으므로 y축 비교 시작
        {
            if((new_position.y >= existing_position.y)                                                                              // 비교할 오브젝트가 기존 오브젝트보다 상단일 때,
                && (new_position.y - existing_position.y >= new_height_range + existing_height_range))                              // y축의 차이가 크므로 겹치지 않는다
            { return false; }
            else if((new_position.y < existing_position.y)
                && (existing_position.y - new_position.y >= new_height_range + existing_height_range))
            { return false; }
            else { return true; }                                                                                                   // x축과 y축 둘 다 차이가 충분히 크지 않으므로 겹친다고 판단
        }

        //비교할 오브젝트가 기존 오브젝트보다 좌측에 위치한 경우
        if ((new_position.x < existing_position.x)
            && (existing_position.x - new_position.x >= new_width_range + existing_width_range))
        { return false; }
        else if ((new_position.x < existing_position.x)
            && (existing_position.x - new_position.x >= new_width_range + existing_width_range))
        {
            if ((new_position.y >= existing_position.y)
                && (new_position.y - existing_position.y >= new_height_range + existing_height_range))
            { return false; }
            else if ((new_position.y < existing_position.y)
                && (existing_position.y - new_position.y >= new_height_range + existing_height_range))
            { return false; }
            else { return true; }
        }
        return true;
	}

    private bool IsDecoPartInCloud(float width_range, float height_range)
    {
		//□□□□□□■□□□
		//□□□■■■■■□□
		//□□■■■■■■□□
		//■■■■■■■■■■
		//□□■■■■■■■□
		//□□□□■■■□□□
        if((width_range <= 100 && height_range >= 200)
            || (width_range <= -200 && height_range >= 100)
            || (width_range <= -300 && height_range >= 0)
            || (width_range <= -300 && height_range <= -100)
            || (width_range <= -100 && height_range <= -200)
            || (width_range >= 200 && height_range >= 200)
            || (width_range >= 300 && height_range >= 0)
            || (width_range >= 400 && height_range <= -100)
            || (width_range >= 200 && height_range <= -200))
        { Debug.Log("Out of Range"); return false; }
		return true;
	}

	public void clickedPosNegButton()
    {
		TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();

		GameObject target = EventSystem.current.currentSelectedGameObject.transform.gameObject;
        int idx = target.transform.parent.GetSiblingIndex();// 부모가 몇번째 sibling인지.

        if (idx > mBaseCloudDt.getDecoPartsCount()) return;

        Debug.Log("clicked_" + target.name);
        if(mLPartsMenu[idx].isInit) //처음이면 둘다 체크가 안되어있음.
        {
            
            if (mTutorialManager.isFinishedTutorial[6] == false && mLPartsMenu[0].isInit == true)
            {
                if(idx != 0 || target.transform.GetSiblingIndex() != 0) { return; }
                else { 
                    mTutorialManager.SetActiveGuideSpeechBubble(true);
                    mTutorialManager.SetActiveFadeOutScreen(false);
                    mTutorialManager.SetActiveArrowUIObject(false);
                }
            }

            target.transform.GetComponent<Image>().sprite = target.transform.GetSiblingIndex() == 0 ? I_SelectedSticker[0] : I_SelectedSticker[1];
            mLPartsMenu[idx].state = target.transform.GetSiblingIndex() == 0 ? true : false;

            mLPartsMenu[idx].isInit = false;
        }
        else
        {
            if (mTutorialManager.isFinishedTutorial[6] == false && idx == 0) { return; }
            mLPartsMenu[idx].btnClicked(I_SelectedSticker,I_UnSelectedSticker);
        }

    }
    public void EClickedDecoParts()
    {
        GameObject target = EventSystem.current.currentSelectedGameObject;
        int partsIdx = target.transform.parent.GetSiblingIndex(); //parent = type(?)
        int cnt = partsIdx >= 1 ? int.Parse(T_CountInfo[partsIdx - 1].GetComponent<TMP_Text>().text) : 0 ;

            
        //개수 감소시키기, 제한개수를 소진하면 클릭 할 수 없다.
        switch (partsIdx)
        {
            case 0:
                if (mCloudPieceDecoMax == 0) return;
                mCloudPieceDecoMax--;
                break;
            case 1:
                if (cnt == 0) return;
                cnt--;
                T_CountInfo[partsIdx - 1].GetComponent<TMP_Text>().text = cnt.ToString();
                break;
            case 2:
                if (cnt == 0) return;
                cnt--;
                T_CountInfo[partsIdx - 1].GetComponent<TMP_Text>().text = cnt.ToString();
                break;
        }

        GameObject newParts = Instantiate(target.transform.GetChild(0).gameObject, Input.mousePosition, target.transform.rotation);
        newParts.AddComponent<DecoParts>();
        selectedParts = newParts.GetComponent<DecoParts>();
        selectedParts.init(top_right_corner, bottom_left_corner);
        newParts.AddComponent<Button>();
        selectedParts.transform.SetParent(this.transform, true);

        newParts.GetComponent<Button>().onClick.AddListener(EPartsClickedInArea);

        LDecoParts.Add(newParts);//파츠 관리하는 리스트에 추가.

        cursorChasing = true;
    }

    public void EPartsClickedInArea()
    {
        //꾸미기가 완료된 상태라면 사용자 조작에 반응하지 않는다.
        if (isDecoDone)
            return;
        //클릭된 객체로 변경해줘야함
        if(LDecoParts.Count>1 && EventSystem.current.currentSelectedGameObject.transform.parent != selectedParts.transform.parent)
                return;
        
        selectedParts = EventSystem.current.currentSelectedGameObject.GetComponent<DecoParts>();

        if (!selectedParts.canAttached) return; 
        if(!selectedParts.isEditActive)//처음 파츠를 선택한 상태. 부착가능한 공간에 부착 가능.
        {
            cursorChasing = false; //커서 따라다니지 않게 설정.
            selectedParts.ReSettingDecoParts(); //CanEdit = true로 만듦.

            //새로운 버튼 만들어서 덮어 씌움.
            GameObject B_Frame = Instantiate(B_Edits[0], Vector2.zero, selectedParts.transform.rotation);
            B_Frame.transform.SetParent(selectedParts.transform,false);
            B_Frame.AddComponent<MouseDragMove>();
            B_Frame.GetComponent<Button>().onClick.AddListener(EEditPartsPos);
            B_Frame.GetComponent<RectTransform>().sizeDelta = selectedParts.GetComponent<RectTransform>().sizeDelta;

            GameObject B_Rotate = Instantiate(B_Edits[1], Vector2.zero, selectedParts.transform.rotation);
            B_Rotate.transform.SetParent(selectedParts.transform, false);
            B_Rotate.AddComponent<MouseDragRotate>();


            //Rotation Button Frame 조정.
            float rotateImg_H = B_Rotate.GetComponent<RectTransform>().sizeDelta.y/2.0f;
            float PartsImg_H = selectedParts.gameObject.GetComponent<RectTransform>().sizeDelta.y/2.0f;
            float correctionPos = PartsImg_H - rotateImg_H + rotateImg_H * 2;
            B_Rotate.transform.position = new Vector2(B_Rotate.transform.position.x, B_Rotate.transform.position.y+correctionPos);

            B_Frame.SetActive(false);
            B_Rotate.SetActive(false);

            selectedParts.isEditActive = true;
            return;
        }

        //부착이 한번 되면 canEdit상태는 언제나 true이다.
        if (!selectedParts.isEditActive) return;
        //스케치북에 부착된 상태에서만 아래코드 접근 가능.

        if (!selectedParts.canEdit)
        {
            selectedParts.canEdit = true;
            selectedParts.transform.GetChild(0).gameObject.SetActive(true);
            selectedParts.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void EEditPartsPos()
    {
        if (!selectedParts.canEdit)
        {
            selectedParts.canEdit = true;
            selectedParts.transform.GetChild(0).gameObject.SetActive(true);
            selectedParts.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            selectedParts.canEdit = false;
            selectedParts.transform.GetChild(0).gameObject.SetActive(false);
            selectedParts.transform.GetChild(1).gameObject.SetActive(false);
        }

    }


    private void Update_PartsMoving()
    {
        if (!cursorChasing) return;
        selectedParts.transform.position = Input.mousePosition;
    }

    private bool checkIsWorkComplete()
    {
        //증감 선택을 완료했는지 체크
        for(int i = 0; i < mLPartsMenu.Count; i++)
        {
            if (mLPartsMenu[i].isInit == true) return false;
            if (int.Parse(T_CountInfo[i].GetComponent<TMP_Text>().text) != 0) return false;
        }
        if (cursorChasing == true) return false;

        return true;
    }

    private void createFinCloud() //구름위에 파츠 붙은 게임오브젝트 생성.
    {
        if (isDecoDone) return;

        StartCoroutine(popUpFinSBook());
    }

    IEnumerator popUpFinSBook()
    {
        isDecoDone = true;
        GameObject FinCloud = Instantiate(I_targetCloud, I_targetCloud.transform.position, I_targetCloud.transform.rotation);    

        yield return new WaitForSeconds(1.0f);

		//스케치북에 붙여진 파츠들은 <CloudDecoManager>아래에 저장되는데, 이를 저장할때는 구름베이스 하위로 바꾼다.
		while (transform.childCount != 0)
		{
			transform.GetChild(0).SetParent(FinCloud.transform);
		}
		GameObject Capture = Instantiate(FinCloud, FinCloud.transform.position, FinCloud.transform.rotation);
		Capture.transform.SetParent(CaptureZone.transform);

		FinCloud.transform.SetParent(P_FinSBook.transform.GetChild(0).transform);
        FinCloud.transform.localPosition = new Vector3(0, 0, 0);

        I_BasicDecoCloud = I_targetCloud;
        I_targetCloud = FinCloud;


        FinCloud.GetComponent<Transform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
        P_FinSBook.SetActive(true);

        TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        if (mTutorialManager.isFinishedTutorial[6] == false) { mTutorialManager.InstantiateArrowUIObject(GameObject.Find("B_Complete").transform.position, 150f); }

        yield break;

    }
   
    
    private void FixedUpdate()
    {        
        Update_PartsMoving();

        if (checkIsWorkComplete())
            createFinCloud();
    }

    
    //Gizmo//
    private void DrawRectange(Vector2 top_right_corner, Vector2 bottom_left_corner)
    {
        Vector2 center_offset = (top_right_corner + bottom_left_corner) * 0.5f;
        Vector2 displacement_vector = top_right_corner - bottom_left_corner;
        float x_projection = Vector2.Dot(displacement_vector, Vector2.right);
        float y_projection = Vector2.Dot(displacement_vector, Vector2.up);

        Vector2 top_left_corner = new Vector2(-x_projection * 0.5f, y_projection * 0.5f) + center_offset;
        Vector2 bottom_right_corner = new Vector2(x_projection * 0.5f, -y_projection * 0.5f) + center_offset;

        Gizmos.DrawLine(top_right_corner, top_left_corner);
        Gizmos.DrawLine(top_left_corner, bottom_left_corner);
        Gizmos.DrawLine(bottom_left_corner, bottom_right_corner);
        Gizmos.DrawLine(bottom_right_corner, top_right_corner);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        DrawRectange(top_right_corner, bottom_left_corner);

    }


}
