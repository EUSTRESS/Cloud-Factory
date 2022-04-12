using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloudSystem
{
    public delegate void EventHandler(string name);//재료이름 혹은 키를 인자로 받아 넘김.
}

public class CloudMakeSystem : MonoBehaviour
{
    ////////////////////////////////////////////////////////
    ///////            Interface Value               ///////
    ////////////////////////////////////////////////////////
    public CloudSystem.EventHandler event_Selected;  //인벤토리에서 재료선택시 이벤트 호출_ 함수 명 변경하고 시픔...
    public CloudSystem.EventHandler event_UnSelected;  // 선택된 재료 취소 
    public CloudSystem.EventHandler event_createBtn; //구름제작 버튼

    public Sprite default_sprite;//private으로 바꿀 예정.

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


    //Debug
    private List<string> L_mtrls;// 나중에 지워도 될듯? 약간 디버깅 용임..ㅎ

    //count
    private int total;

    private void selectMtrl(string name)
    {
        if (total > 5) return; //최대 5개까지 선택 가능

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

    private void deselectMtrl(string name)
    {
        GameObject ERSD = UI_slct_mtrl.Find(item => name == item.name); //삭제된 빈칸 찾기.
        ERSD.GetComponent<SpriteRenderer>().sprite =default_sprite;

        
        listReOrder(UI_slct_mtrl.FindIndex(item => ERSD == item));
    }


    private void listReOrder(int idx) //리스트UI 정렬
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

    private void Del_ReadCSV(string name)
    {
        //구름 조합법 나오면 그때 스크립트 작성.
        Debug.Log("조합재료를 확인합니다.");
    }
    private void Del_CreateCloud(string name)
    {
        if (total < 5)
        {
            Debug.Log("재료수가 부족합니다.");
            return;
        }//5개 안채우면 제작 안됨

        //UI 초기화
        for(int i = 0; i < total;i++)
        {
            UI_slct_mtrl[i].GetComponent<SpriteRenderer>().sprite = default_sprite;
        }
        total = 0;
        Debug.Log("구름이 만들어졌습니다.");
    }

    private void init()
    {
        T_mtrl = this.transform.Find("selectedIngredient").transform;
        total = 0;
        UI_mtrl_count = T_mtrl.childCount;
        UI_slct_mtrl = new List<GameObject>(); //구름제작 UI창의 선택된 재료.
        L_mtrls = new List<string>(); //구름제작 UI창의 선택된 재료.
        
        for (int i = 0; i < UI_mtrl_count; i++)
        {
            UI_slct_mtrl.Add(T_mtrl.GetChild(i).gameObject);
        }

        //event
        event_Selected = selectMtrl;
        

        event_UnSelected = deselectMtrl;

        event_createBtn = Del_ReadCSV;
        event_createBtn += Del_CreateCloud;
    }

    ////////////////////////////////////////////////////////
    ///////                Pipeline                  ///////
    ////////////////////////////////////////////////////////
    void Start()
    {
        init();
    }

}
