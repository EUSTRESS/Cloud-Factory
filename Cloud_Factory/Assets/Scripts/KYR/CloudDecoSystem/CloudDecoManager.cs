using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CloudDecoManager : MonoBehaviour
{
    //파츠 증감소 클래스
    [System.Serializable]
    public class PartsMenu
    {
        private GameObject I_Image;
        private GameObject B_Pos;
        private GameObject B_Neg;

        private bool state; //감소 혹은 증가?
        public bool isInit;
        public PartsMenu(GameObject _B_decoParts,GameObject _I_PartsMenu, GameObject _B_PosNeg, int _idx)
        {
            I_Image = _I_PartsMenu.transform.GetChild(_idx).GetChild(0).gameObject;
            B_Pos = _B_PosNeg.transform.GetChild(_idx).GetChild(0).gameObject;
            B_Neg = _B_PosNeg.transform.GetChild(_idx).GetChild(1).gameObject;
            isInit = true;

            I_Image.transform.GetComponent<Image>().sprite = _B_decoParts.transform.GetChild(_idx).GetChild(0).GetChild(0).GetComponent<Image>().sprite;
        }
    
        public void btnClicked(Sprite[] _I_SelectedSticker, Sprite[] _I_UnSelectedSticker)
        {
            if (state)
            {
                B_Pos.GetComponent<Image>().sprite = _I_SelectedSticker[0];
                B_Neg.GetComponent<Image>().sprite = _I_UnSelectedSticker[1];
                state = false;
            }
            else
            {
                B_Pos.GetComponent<Image>().sprite = _I_UnSelectedSticker[0];
                B_Neg.GetComponent<Image>().sprite = _I_SelectedSticker[1];   
                state = true;
            }
        }

    }

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
        //inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();


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
            for(int j = 0; j<3;j++)
            {
                if(i == 0)        //Set CloudPiece        
                    mUIDecoParts[i][j].transform.GetChild(0).GetComponent<Image>().sprite = mBaseCloudDt.getCloudParts()[j];
                

                mUIDecoParts[i+1][j].transform.GetChild(0).GetComponent<Image>().sprite = mBaseCloudDt.getSizeDifferParts(i)[j];
            }

        }

        for (int i = 0; i < mBaseCloudDt.getDecoPartsCount()+1; i++)
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
        return GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().createdCloudData;
    }


    //UI Button Function
    public void clickedPosNegButton()
    {
        GameObject target = EventSystem.current.currentSelectedGameObject.transform.gameObject;
        int idx = target.transform.parent.GetSiblingIndex();// 부모가 몇번째 sibling인지.

        if (idx > mBaseCloudDt.getDecoPartsCount()) return;

        Debug.Log("clicked_" + target.name);
        if(mLPartsMenu[idx].isInit) //처음이면 둘다 체크가 안되어있음.
        {
            target.transform.GetComponent<Image>().sprite = target.transform.GetSiblingIndex() == 0 ? I_SelectedSticker[0] : I_SelectedSticker[1];
            mLPartsMenu[idx].isInit = false;
        }
        else
        {
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

        while(transform.childCount != 0)
        {
            transform.GetChild(0).SetParent(I_targetCloud.transform);
        }

        yield return new WaitForSeconds(1.0f);

       

        I_targetCloud.transform.SetParent(P_FinSBook.transform.GetChild(0).transform);
        I_targetCloud.transform.localPosition = new Vector3(0, 0, 0);
        P_FinSBook.SetActive(true);

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
