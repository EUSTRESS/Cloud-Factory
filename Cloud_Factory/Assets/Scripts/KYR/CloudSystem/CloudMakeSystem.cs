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
            IngredientData data = mtrlDATA.mItemList.Find(item => name == item.dataName);
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
                    EmotionInfo emoDt = new EmotionInfo((Emotion)emo.Key, emo.Value);
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

    public EmotionsTable emoTableDATA;

    //Total Emotion Input List 
    private List<EmotionInfo> mEmotions;

    //UI
    [SerializeField]
    private CloudSystem.S_list slct_mtrl; //selected_material data class
    [SerializeField]
    private Text UI_btn_txt;

    //count
    private int total;

    //bool
    [HideInInspector]
    public bool isMakingCloud;   // 구름 제작 중, InventoryContainer.cs에서 선택된 재료들을 클릭할 수 없게 제한하기 위한 변수 

    private void d_selectMtrl(string name)
    {
        if (total >= 5) return; //최대 5개까지 선택 가능

        total++; //update total count

        slct_mtrl.add(mtrlDATA, total, name);
    }

    public bool d_selectMtrlListFull()
    {
        if(total >= 5) { return true; }
        else { return false; }
    }

    public bool d_selectMtrlListEmpty()
    {
        if(total <= 0) { return true; }
        else { return false; }
    }

    private void d_deselectMtrl(string name)
    {
        total--; //update total count
        slct_mtrl.m_sort(slct_mtrl.getErsdobj(name),total); //구름공장에서의 이미지 정렬
    }

    private bool isOverlapState(List<EmotionInfo> emotionList)
    {
        List<Emotion> tmpL = new List<Emotion>();

        foreach(EmotionInfo info in emotionList)
        {
            if (info.Key > Emotion.INTEXPEC) continue;

            if (tmpL.Contains(info.Key)) return true;
            else
                tmpL.Add(info.Key);

        }

        return false;
    }
    private void d_readCSV(string name)//구름 조합법 알고리즘
    {
        Debug.Log("조합재료를 확인합니다.");

        //1.계산 들어갈 감정 리스트 추출. => Base Emotion List
        List<EmotionInfo> emotionList = slct_mtrl.mGetTotalEmoList(mtrlDATA);

        //2.중복감정 리스트 추출
        //(1) 중복감정이 3개 이상일 경우 앞쪽에 위치한 2개의 종류만 중복리스트에 넣는다.
        Dictionary<Emotion, KeyValuePair<int, int>> overlapsEmosList = new Dictionary<Emotion, KeyValuePair<int, int>>(); //중복감정 Key : {percent, percent}

        Dictionary<Emotion, int> tmpList = new Dictionary<Emotion, int>();

        while (isOverlapState(emotionList))
        {
            bool possibility = false;
            for (int i = 0; i < emotionList.Count; i++)
            {
                EmotionInfo content = emotionList[i];

                if (tmpList.ContainsKey(content.Key))//-1- content가 중복리스트에 포함되어있으면
                {
                    //overlapsEmosList.Add(content.Key, new KeyValuePair<int, int>(tmpList.Value, content.Value));
                    //-2- 중복 감정 중 낮은 수치를 가진 감정이 조합에 사용된다.
                    int mergedV = tmpList[content.Key] <= content.Value ? tmpList[content.Key] : content.Value;//삼항연산자를 이용하여 더 작은 값 차용.
                    Debug.Log("[(1)낮은수치사용]" + tmpList[content.Key] + "|" + content.Value + "에서" + mergedV + "차용!");
                    tmpList.Remove(content.Key);//-2-1 조합에 사용될 감정을 처리했으면 중복감정 리스트에서 제외한다.

                    Debug.Log("[(2)중복발견]중복아이템 리스트에서 삭제");
                    Debug_PrintState("[현재중복리스트 삭제 결과]", tmpList);
                    //-3- "emotionList" 에서 작은 값을 가진 감정과 인접한 감정과 감정 조합이 일어난다.
                    //-3-1 조합할 인접 감정을 우선순위에 따라 선정한다.
                    EmotionInfo fndItm = new EmotionInfo(content.Key, mergedV);
                    int targetIdx = emotionList.FindIndex(a => (a.Key == content.Key && a.Value == mergedV)); //추출된 감정리스트에서의 조합대상 감정의 idx
                    int subTargetIdx = targetIdx - 1;
                    Debug.Log("[(3)조합대상]" + "{" + targetIdx + "}");
                    Debug.Log("[(3)조합대상]" + "{" + targetIdx + "}" + emotionList[targetIdx].Key);
                    Debug.Log("[(3)조합대상]" + "{" + subTargetIdx + "}");


                    bool commend;
                    //앞에 채택 경우: outOfBound
                    //뒤에 채택 경우: outOfBound, 앞에서 none 결과 값이 나왔기 떄문.
                    if (subTargetIdx >= 0 ? true : false)//우선순위(1): 대상의 앞의 감정과 조합한다.
                        commend = emoTableDATA.getCombineResult(emotionList[targetIdx].Key, emotionList[subTargetIdx].Key) != Emotion.NONE ? false : true;//index가 OutOfBound가 아니고 조합의 결과가 있다면! command = false
                    else
                    {
                        commend = false;
                        subTargetIdx = targetIdx + 1;
                        Debug.Log("[우선순위2]채택");
                    }


                    //우선순위(2): 위에서 command가 true가 나면 우선순위 (2)로 넘어간다.   subTargetIdx = targetIdx + 1
                    if (!commend && subTargetIdx < emotionList.Count ? true : false)
                        commend = emoTableDATA.getCombineResult(emotionList[targetIdx].Key, emotionList[subTargetIdx].Key) != Emotion.NONE ? true : false;

                    //우선순위(1)또는 (2)에서 감정조합의 결과가 정상적으로 나온 경우.
                    if (commend)
                    {
                        //(1) 조합에 사용된 두 감정 중 낮은 수치를 가져온다.
                        int CEmoV = emotionList[targetIdx].Value <= emotionList[subTargetIdx].Value ? emotionList[targetIdx].Value : emotionList[subTargetIdx].Value;
                        EmotionInfo finalEmo = new EmotionInfo(emoTableDATA.getCombineResult(emotionList[targetIdx].Key, emotionList[subTargetIdx].Key), CEmoV);

                        emotionList[targetIdx] = finalEmo;//(2)targetEmotion을 조합된 새 감정으로 바꾼다.
                        Debug.Log("[조합결과]" + finalEmo.Key);
                        //(2)조합에 사용 된 감정은 리스트에서 제외한다.(for문 안이기 떄문에 index 관리 해줘야 한다.)
                        if (finalEmo.Key == Emotion.NONE) continue;

                        emotionList.RemoveAt(subTargetIdx);
                        // 삭제할 값의 index가 현재 타겟 중인  index보다 작은 경우(상관 있음. 현재 타겟중인 값이 앞으로 밀리기 때문에, index를 -1 해주어야 한다.
                        if (subTargetIdx < i) i--;
                        // 삭제할 값의 index가 현재 타겟 중인 index보다 클 경우.(상관 없음: 리스트의 길이만 달라진다.)

                        possibility = true;
                    }
  
                }
                else
                {
                    if (Emotion.PLEASURE <= content.Key && content.Key <= Emotion.INTEXPEC)
                        tmpList.Add(content.Key, content.Value);
                }
                Debug_PrintState("[현재감정리스트]", emotionList);
                Debug_PrintState("[현재중복리스트]", tmpList);
            }

            if (!possibility)
                break;

        }

        //최종 감정 리스트 저장.
        //중복이 있다면 그중 가장 큰 감정 채용
        Dictionary<Emotion, int> LoverlapsEmo = new Dictionary<Emotion, int>();
        
        foreach (EmotionInfo emotion in emotionList)
        {
            if(LoverlapsEmo.Count == 0)
            {
                LoverlapsEmo.Add(emotion.Key, emotion.Value);
                continue;
            }

            if(LoverlapsEmo.ContainsKey(emotion.Key))
            {
                if (LoverlapsEmo[emotion.Key] < emotion.Value)
                    LoverlapsEmo[emotion.Key] = emotion.Value;
            }
            else
                LoverlapsEmo.Add(emotion.Key, emotion.Value);

        }
        Debug_PrintState("[중복 감정 중 큰감정 채용]", LoverlapsEmo);

        List<EmotionInfo> LfinalEmo = new List<EmotionInfo>();
        foreach (KeyValuePair<Emotion,int> overlap in LoverlapsEmo)
        {
            EmotionInfo tmp = new EmotionInfo(overlap.Key, overlap.Value);
            LfinalEmo.Add(tmp);
        }

        emotionList = LfinalEmo;
        Debug_PrintState("[최종감정리스트(1)]", emotionList);

        LfinalEmo = new List<EmotionInfo>();

        //2가지 감정 선택(제일 큰 감정 + 두번째로 큰 감정)
        int roopCnt = 2;
        while(roopCnt!=0)
        {
            EmotionInfo maxValue = new EmotionInfo(emotionList[0].Key, emotionList[0].Value);
            foreach (EmotionInfo emotion in emotionList)
            {
                if (maxValue.Value < emotion.Value)
                    maxValue = emotion;
                else if(maxValue.Value == emotion.Value) //같다면 둘 중 랜덤으로 선택.
                {
                    int i = Random.Range(0, 2);
                    maxValue = (i == 0 ? maxValue : emotion);

                }

            }
            LfinalEmo.Add(maxValue);
            emotionList.Remove(maxValue);
            roopCnt--;
        }

        emotionList = LfinalEmo;
        Debug_PrintState("[최종감정리스트]", emotionList);
        mEmotions = emotionList;
    }

    
    //Debug Function
    private void Debug_PrintState(string sTitle,List<EmotionInfo> emotionList)
    {
        string result = sTitle;
        foreach(EmotionInfo info in emotionList)
        {
            result += (info.Key.ToString()+":" + info.Value + "|");
        }

        Debug.Log(result);
    }

    private void Debug_PrintState(string sTitle, Dictionary<Emotion, int> emotionList)
    {
        string result = sTitle;
        foreach (KeyValuePair<Emotion,int> info in emotionList)
        {
            result += (info.Key.ToString() + "|");
        }

        Debug.Log(result);
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

        isMakingCloud = true;

        //making UI 처리
        StartCoroutine(isMaking(time));        
    }

    IEnumerator isMaking(float time) //UI 처리
    {

        //색 어둡게
        slct_mtrl.u_setUIbright(total, false);

        yield return new WaitForSeconds(time);


        yield return new WaitForSeconds(1);
        //색 밝게
        slct_mtrl.u_setUIbright(total);

        //UI 초기화
        slct_mtrl.u_init(total);

        total = 0;
        UI_btn_txt.text = "제작하기";

        isMakingCloud = false;

        int emotionCnt = mEmotions.Count;

        InventoryManager inventoryManager = GameObject.FindWithTag("InventoryManager").transform.GetComponent<InventoryManager>();
        inventoryManager.createdCloudData = new CloudData(mEmotions); //createdCloudData 갱신.
        //큰 수치 = 구름색
        //다음 수치 = 구름의 장식
        
        transform.Find("I_Result").gameObject.GetComponent<Image>().sprite = inventoryManager.createdCloudData.getBaseCloudSprite();
        Debug.Log("구름이 만들어졌습니다.");

        m_sendCloud();

        yield break;
    }

    //구름 생성 후 인벤토리에 저장
    private void m_sendCloud()
    {
        //해당 감정에 맞는 구름 이미지 생성

        GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().createdCloudData = new CloudData(mEmotions);

        ////////////////////아래는 변경될 예정. 
        //구름 인벤토리 리스트 가져와서
        int cnt = 0;

        //빈 인벤토리 추척해서 구름 넣기.
        //while(true)
        //{
        //    if (cnt >= 5) break; //인벤토리 크기 초과하면 실행X
        //    if (L_cloudsInven[cnt].GetComponent<Image>().sprite != default_sprite)
        //    {
        //        cnt++;
        //        continue;
        //    }
        //    L_cloudsInven[cnt].GetComponent<Image>().sprite = cloud_sprite;
        //    L_cloudsInven[cnt].GetComponent<Button>().onClick.AddListener(DEMOcreateCloud);
        //    break;
        //}
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

        isMakingCloud = false;

        
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
        mtrlDATA = new IngredientList();

        InventoryManager inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        mtrlDATA = inventoryManager.mIngredientDatas[inventoryManager.minvenLevel - 1];
        init();
    }

}
