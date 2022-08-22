using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cloud
{
    public int mShelfLife;
    public List<EmotionInfo> mEmotions;

    private bool mState; //0 = 폐기 1 = 가능
    private Image cloudImage;
    private List<Image> decoImages;
    
    public Cloud(List<EmotionInfo> Emotions)
    {
        mEmotions = Emotions;

        //계산식함수로 자동으로 데이터 세팅
        setShelfLife(mEmotions);
        setCloudImage(mEmotions);
        setDecoImage(mEmotions);
    }

    private void setShelfLife(List<EmotionInfo> Emotions)
    {
        //감정에 따라 맞는 보관기간
    }
    private void setCloudImage(List<EmotionInfo> Emotions)
    {
        //감정에 따라 맞는 base 구름이미지
    }

    private void setDecoImage(List<EmotionInfo> Emotions)
    {
        //감정에 따라 맞는 데코 이미지
    }
}
public class CloudDecoManager : MonoBehaviour
{

    //데코용 버튼 그룹
    public GameObject B_decoParts;
    public GameObject B_PosNeg;
    public GameObject[] B_Edits; //frame : 이동, Rotate: 회전. 외부에서 대입.

    public Cloud mtargetCloud;// 구름공장에서 구름 데이터 제공.

    private bool cursorChasing;
    
    private DecoParts selectedParts;
    private List<List<GameObject>> mUIDecoParts;
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
        //UI 세팅
        for (int i = 0; i < 3; i++)
        {
            Transform tmp = B_decoParts.transform.GetChild(i);
            List<GameObject> Ltmp = new List<GameObject>();
            for (int j = 0; j < 3; j++)
                Ltmp.Add(tmp.GetChild(j).gameObject);


            mUIDecoParts.Add(Ltmp);
        }

    }
    private void init()
    {
        //Cloud data 가져오기.(씬이동시에만가능)
       // mtargetCloud = inventoryManager.createdCloudData;

       
        //클라우드 데이터에 따라 UI에 이미지 삽입.

    }

    private Cloud getTargetCloudData()
    {
        return GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().createdCloudData;
    }


    //UI Button Function
    public void EClickedDecoParts()
    {
        GameObject target = EventSystem.current.currentSelectedGameObject;
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
        //클릭된 객체로 변경해줘야함.
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

    private void FixedUpdate()
    {
        
        Update_PartsMoving();
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
