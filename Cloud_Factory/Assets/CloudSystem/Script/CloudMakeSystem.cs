using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CloudSystem
{
    public delegate void EventHandler(string name);//재료이름 혹은 키를 인자로 받아 넘김.
}

public class CloudMakeSystem : MonoBehaviour
{
    ////////////////////////////////////////////////////////
    ///////            Interface Value               ///////
    ////////////////////////////////////////////////////////
    public CloudSystem.EventHandler E_Selected;  //인벤토리에서 재료선택시 이벤트 호출_ 함수 명 변경하고 시픔...
    public CloudSystem.EventHandler E_UnSelected;  // 선택된 재료 취소 
    public CloudSystem.EventHandler E_createCloud; //구름제작 버튼


    //구름인벤토리 리스트_임시
    public List<GameObject> L_cloudsInven;//외부에서 5개로 지정해놓음
    //임시
    public Sprite default_sprite;//private으로 바꿀 예정.
    public Sprite cloud_sprite;//Data structure로 바꿀 예정.

    ////////////////////////////////////////////////////////
    ///////            private Value                 ///////
    ////////////////////////////////////////////////////////
    private Transform T_mtrl;//parent transform of mtrl gameObject

    //Data
    [SerializeField]
    public IngredientList mtrlDATA; // 모든 재료 정보를 갖고 있는 리스트 scriptable data
    //나중에 private으로 바꿀거임./

    //UI
    [SerializeField]
    private int UI_mtrl_count; //선택할 수 있는 재료 개수
    [SerializeField]
    private List<GameObject> UI_slct_mtrl; //UI_selected_material
    [SerializeField]
    private Text UI_btn_txt;

    //Debug
    private List<string> L_mtrls;// 나중에 지워도 될듯? 약간 디버깅 용임..ㅎ

    //count
    private int total;

    private void d_selectMtrl(string name)
    {
        if (total >= 5) return; //최대 5개까지 선택 가능

        //
        Debug.Log(name + "가 선택되었습니다.");
        L_mtrls.Add(name);
        //

        total++; //update total count

        IngredientData data = mtrlDATA.itemList.Find(item => name == item.ingredientName);
        UI_slct_mtrl[total - 1].GetComponent<SpriteRenderer>().sprite = data.image;


        //
        Debug.Log(name + "가 구름제작 리스트에 추가됩니다.");
        for (int i = 0; i < L_mtrls.Count; i++)
        {
            Debug.Log("현재 " + L_mtrls[i] + "있음.");
        }
        //
    }

    private void d_deselectMtrl(string name)
    {
        GameObject ERSD = UI_slct_mtrl.Find(item => name == item.name); //삭제된 빈칸 찾기.
        ERSD.GetComponent<SpriteRenderer>().sprite =default_sprite;

        
        listreOrder(UI_slct_mtrl.FindIndex(item => ERSD == item));
    }


    private void listreOrder(int idx) //리스트UI 정렬
    {
        if (total <= 0) return;

        //
        L_mtrls.RemoveAt(idx); 
        //

        total--; //update total count

        //list reorder Algorithm
        for (int i = idx; i < total; i ++ )
        {
            GameObject curr = UI_slct_mtrl[i];
            GameObject next = UI_slct_mtrl[i + 1];
            curr.GetComponent<SpriteRenderer>().sprite = next.GetComponent<SpriteRenderer>().sprite;
        }

        //exception handling
        if (total == 0) return;
        UI_slct_mtrl[total].GetComponent<SpriteRenderer>().sprite = default_sprite;
    }

    private void d_readCSV(string name)
    {
        //구름 조합법 나오면 그때 스크립트 작성.
        Debug.Log("조합재료를 확인합니다.");
    }
    private void d_createCloud(string name = null)
    {
        if (total < 1)
        {
            Debug.Log("재료수가 부족합니다.");
            return;
        }//5개 안채우면 제작 안됨

        float time = 5f;
        //코루틴
        UI_btn_txt.text = "만드는 중";
        StartCoroutine(isMaking(time));
        
    }

 

    IEnumerator isMaking(float time)
    {
        this.transform.Find("Button").GetComponent<Button>().enabled = false;

        //색 어둡게
        for (int i = 0; i < total; i++)
        {
            UI_slct_mtrl[i].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }

        yield return new WaitForSeconds(time);

        this.transform.Find("Button").GetComponent<Button>().enabled = true;

        yield return new WaitForSeconds(1);
        //색 밝게
        for (int i = 0; i < total; i++)
        {
            UI_slct_mtrl[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }


        //UI 초기화
        init_UI();

        total = 0;
        UI_btn_txt.text = "제작하기";

        Debug.Log("구름이 만들어졌습니다.");
        m_saveCloud();

        yield break;
    }

    //구름 생성 후 인벤토리에 저장
    private void m_saveCloud()
    {
        //구름 인벤토리 리스트 가져와서
        int cnt = 0;

        //빈 인벤토리 추척해서 구름 넣기.
        while(true)
        {
            if (cnt >= 5) break; //인벤토리 크기 초과하면 실행X
            if (L_cloudsInven[cnt].GetComponent<Image>().sprite != default_sprite)
            {
                cnt++;
                continue;
            }
            L_cloudsInven[cnt].GetComponent<Image>().sprite = cloud_sprite;
            L_cloudsInven[cnt].GetComponent<Button>().onClick.AddListener(DEMOcreateCloud);
            break;
        }
    }

    public void DEMOcreateCloud()
    {
        Debug.Log("구름이 저장되었습니다.");
    }
    private void init_UI()
    {
        for (int i = 0; i < total; i++)
        {
            UI_slct_mtrl[i].GetComponent<SpriteRenderer>().sprite = default_sprite;
        }
    }


    //초기화 함수
    private void init()
    {
        T_mtrl = this.transform.Find("selectedIngredient").transform;
        total = 0;
        UI_mtrl_count = T_mtrl.childCount;
        UI_slct_mtrl = new List<GameObject>(); //구름제작 UI창의 선택된 재료.
        L_mtrls = new List<string>(); //구름제작 UI창의 선택된 재료.
        UI_btn_txt = this.transform.Find("Button").GetComponentInChildren<Text>();
        UI_btn_txt.text = "제작하기";

        for (int i = 0; i < UI_mtrl_count; i++)
        {
            UI_slct_mtrl.Add(T_mtrl.GetChild(i).gameObject);
        }
        //구름 인벤토리 리스트도 레퍼런스로 가지고 오는게 좋을 것 같다.


        //event
        m_setEvent();
    }

    //eventmethod 로직 함수
    private void m_setEvent()
    {
        E_Selected = d_selectMtrl;

        E_UnSelected = d_deselectMtrl;

        E_createCloud = d_readCSV;
        E_createCloud += d_createCloud;
    }


    ////////////////////////////////////////////////////////
    ///////                Pipeline                  ///////
    ////////////////////////////////////////////////////////
    void Start()
    {
        init();
    }

}
