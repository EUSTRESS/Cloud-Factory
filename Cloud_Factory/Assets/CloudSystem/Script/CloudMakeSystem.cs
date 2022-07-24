using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CloudSystem
{
    public delegate void EventHandler(string name);//재료이름 혹은 키를 인자로 받아 넘김.

    //selected ingredients list
    [System.Serializable]
    class S_list
    {
        public List<GameObject> UI_slct_mtrl;
        public int UI_mtrl_count;

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

        private List<IngredientData> mGetingredientDatas(IngredientList mtrlDATA) //확정된 리스트를 IngredientData를 갖고있는 리스트로 변환하여 제공.
        {
            List<IngredientData> results = new List<IngredientData>();
            foreach(GameObject stock in UI_slct_mtrl)
            {
                string targetImgNm = stock.GetComponent<Image>().sprite.name;
                if (targetImgNm == default_sprite.name) continue;
                IngredientData data = mtrlDATA.mItemList.Find(item => targetImgNm == item.image.name);
                results.Add(data);
            }

            return results;
        }

        public List<EmotionInfo> mGetTotalEmoList(IngredientList mtrlDATA)
        {
            List<IngredientData> raw = mGetingredientDatas(mtrlDATA);
            List<EmotionInfo> results = new List<EmotionInfo>();

            //재료들의 감정들을 차례로 리스트에 추가한다.
            foreach(IngredientData data in raw)
            {
                foreach (KeyValuePair<int, int> emo in data.iEmotion)
                {
                    EmotionInfo emoDt = new EmotionInfo();
                    emoDt.init((Emotion)emo.Key, emo.Value);
                    results.Add(emoDt);
                }
                    
            }

            return results;

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

    //Total Emotion Input List 
    private List<EmotionInfo> mEmotions;

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

        //계산 들어갈 감정 리스트 추출. => Base Emotion List
        List<EmotionInfo> emotionList = slct_mtrl.mGetTotalEmoList(mtrlDATA);

        
        //중복되는 감정이 리스트에 없을 떄 까지 계속 작업을 반복한다.
        while (true)
        {
            List<EmotionInfo> overlapList = mIsOverlap(emotionList);
            if (overlapList == null) break;

            //중복감정 처리.
            EmotionInfo target = new EmotionInfo();
            //1. 조합에 사용될 감정 추출하기.
            if (overlapList[0].Value >= overlapList[1].Value) target = overlapList[1]; //작은 것 선택. 같다면 뒤에꺼 선택.
            else target = overlapList[0];

            //2. 가까운 감정과 조합하기.
              //(1) 우선순위<1> : 조합재료로 사용되는 감정보다 앞에 위치 함.

              //(2) 우선순위<2> : 조합재료로 사용되는 감정보다 뒤에 위치함.

              //(3) 가까운 두 감정과 조합이 불가능하다면 조합은 발생하지 않는다.
        }
    }

    private List<EmotionInfo> mIsOverlap(List<EmotionInfo> emotionList)
    {
        //1. 중복되는 감정이 존재하는지 검사.
        List<Emotion> overlapsK = new List<Emotion>();
        List<int> overlapsV = new List<int>();
        foreach (EmotionInfo info in emotionList)
        {
            //2. 위의 감정이 이미 리스트에 있으면 탐색을 즉시 멈추고, list(중복되는 두가지 재료정보를 갖는 리스트)를 리턴한다.
            if (overlapsK.Contains(info.Key))
            { 
                List<EmotionInfo> results = new List<EmotionInfo>();

                int idx = overlapsK.IndexOf(info.Key);
                EmotionInfo preInfo = new EmotionInfo();
                preInfo.init(info.Key, overlapsV[idx]);

                results.Add(preInfo);
                results.Add(info);

                return results;
            }
            overlapsK.Add(info.Key);
            overlapsV.Add(info.Value);
        }
        return null; //중복 재료가 없으면 false를 return한다.
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

    IEnumerator isMaking(float time) //UI 처리
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
        slct_mtrl.init(this.transform.Find("Contents").transform);
        slct_mtrl.default_sprite = default_sprite;

        UI_btn_txt = this.transform.Find("B_CloudGIve").GetComponentInChildren<Text>();
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
        mEmotions = new List<EmotionInfo>(); //감정계산할 떄 쓰이는 Emotion List
        init();
    }

}
