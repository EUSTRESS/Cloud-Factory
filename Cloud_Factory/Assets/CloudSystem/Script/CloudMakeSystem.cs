using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CloudSystem
{
    public delegate void EventHandler(string name);//재료이름 혹은 키를 인자로 받아 넘김.

    //selected ingredients list
    class S_list
    {
        private List<GameObject> UI_slct_mtrl;
        private int UI_mtrl_count;

        public Sprite default_sprite;//private으로 바꿀 예정.
        public Sprite cloud_sprite;//Data structure로 바꿀 예정.

        public void init(Transform T_mtrl)
        {
            UI_slct_mtrl = new List<GameObject>();

            UI_mtrl_count = T_mtrl.childCount;
            for (int i = 0; i < UI_mtrl_count; i++)
            {
                UI_slct_mtrl.Add(T_mtrl.GetChild(i).gameObject);
            }
        }

        public void add(IngredientList mtrlDATA,int total,string name)
        {
            IngredientData data = mtrlDATA.mItemList.Find(item => name == item.ingredientName);
            UI_slct_mtrl[total - 1].GetComponent<Image>().sprite = data.image;
        }

        public GameObject getErsdobj(string name)
        {
            GameObject ERSD = UI_slct_mtrl.Find(item => name == item.name); //삭제된 빈칸 찾기.
            ERSD.GetComponent<Image>().sprite = default_sprite;

            return ERSD;
        }

        public void m_sort(GameObject ERSD,int total) //리스트UI 정렬
        {
            int idx = UI_slct_mtrl.FindIndex(item => ERSD == item);
            if (total <= 0) return;

            //list reorder Algorithm
            for (int i = idx; i < total; i++)
            {
                GameObject curr = UI_slct_mtrl[i];
                GameObject next = UI_slct_mtrl[i + 1];
                curr.GetComponent<Image>().sprite = next.GetComponent<Image>().sprite;
            }

            //exception handling
            if (total == 0) return;
            UI_slct_mtrl[total].GetComponent<Image>().sprite = default_sprite;
        }
        public void u_setUIbright(int total,bool isBright = true)
        {
            if(isBright)
            {
                //색 밝게
                for (int i = 0; i < total; i++)
                {
                    UI_slct_mtrl[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
            else
            {
                //색 어둡게
                for (int i = 0; i < total; i++)
                {
                    UI_slct_mtrl[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                }
            }
        }
        public void u_init(int _total)
        {
            for (int i = 0; i < _total; i++)
            {
                Debug.Log("이미지 초기화");
                UI_slct_mtrl[i].GetComponent<Image>().sprite = default_sprite;
            }
        }
    }
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
   
    //UI
    [SerializeField]
    private CloudSystem.S_list slct_mtrl; //selected_material data class
    [SerializeField]
    private Text UI_btn_txt;

    //count
    private int total;

    private void d_selectMtrl(string name)
    {
        if (total >= 5) return; //최대 5개까지 선택 가능

        total++; //update total count

        slct_mtrl.add(mtrlDATA, total, name);


    }

    private void d_deselectMtrl(string name)
    {
        total--; //update total count
        slct_mtrl.m_sort(slct_mtrl.getErsdobj(name),total); //구름공장에서의 이미지 정렬
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
        }

        float time = 5f;
        //코루틴
        UI_btn_txt.text = "만드는 중";
        StartCoroutine(isMaking(time));
        
    }

    IEnumerator isMaking(float time)
    {
        this.transform.Find("Button").GetComponent<Button>().enabled = false;

        //색 어둡게
        slct_mtrl.u_setUIbright(total, false);

        yield return new WaitForSeconds(time);

        this.transform.Find("Button").GetComponent<Button>().enabled = true;

        yield return new WaitForSeconds(1);
        //색 밝게
        slct_mtrl.u_setUIbright(total);

        //UI 초기화
        slct_mtrl.u_init(total);

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
   
    //초기화 함수
    private void init()
    {     
        total = 0;
      
        slct_mtrl = new CloudSystem.S_list();
        slct_mtrl.init(this.transform.Find("selectedIngredient").transform);
        slct_mtrl.default_sprite = default_sprite;

        UI_btn_txt = this.transform.Find("Button").GetComponentInChildren<Text>();
        UI_btn_txt.text = "제작하기";

        
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
