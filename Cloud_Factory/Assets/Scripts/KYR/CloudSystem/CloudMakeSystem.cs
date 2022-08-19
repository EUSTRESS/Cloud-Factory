using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CloudSystem
{
    public delegate void EventHandler(string name);//����̸� Ȥ�� Ű�� ���ڷ� �޾� �ѱ�.

    //selected ingredients list
    class S_list
    {
        private List<GameObject> UI_slct_mtrl;
        private int UI_mtrl_count;

        public Sprite default_sprite;//private���� �ٲ� ����.
        public Sprite cloud_sprite;//Data structure�� �ٲ� ����.

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

        private List<IngredientData> mGetingredientDatas(IngredientList mtrlDATA) //Ȯ���� ����Ʈ�� IngredientData�� �����ִ� ����Ʈ�� ��ȯ�Ͽ� ����.
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

            //������ �������� ���ʷ� ����Ʈ�� �߰��Ѵ�.
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
            GameObject ERSD = UI_slct_mtrl.Find(item => name == item.name); //������ ��ĭ ã��.
            ERSD.GetComponent<Image>().sprite = default_sprite;

            return ERSD;
        }

        public void m_sort(GameObject ERSD,int total) //����ƮUI ����
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
                //�� ���
                for (int i = 0; i < total; i++)
                {
                    UI_slct_mtrl[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
            else
            {
                //�� ��Ӱ�
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
                Debug.Log("�̹��� �ʱ�ȭ");
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
    public CloudSystem.EventHandler E_Selected;  //�κ��丮���� ��ἱ�ý� �̺�Ʈ ȣ��_ �Լ� �� �����ϰ� ����...
    public CloudSystem.EventHandler E_UnSelected;  // ���õ� ��� ��� 
    public CloudSystem.EventHandler E_createCloud; //�������� ��ư


    //�����κ��丮 ����Ʈ_�ӽ�
    public List<GameObject> L_cloudsInven;//�ܺο��� 5���� �����س���
    //�ӽ�
    public Sprite default_sprite;//private���� �ٲ� ����.
    public Sprite cloud_sprite;//Data structure�� �ٲ� ����.

    ////////////////////////////////////////////////////////
    ///////            private Value                 ///////
    ////////////////////////////////////////////////////////
    private Transform T_mtrl;//parent transform of mtrl gameObject

    //Data
    [SerializeField]
    public IngredientList mtrlDATA; // ��� ��� ������ ���� �ִ� ����Ʈ scriptable data

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

    private void d_selectMtrl(string name)
    {
        if (total >= 5) return; //�ִ� 5������ ���� ����

        total++; //update total count

        slct_mtrl.add(mtrlDATA, total, name);


    }

    private void d_deselectMtrl(string name)
    {
        total--; //update total count
        slct_mtrl.m_sort(slct_mtrl.getErsdobj(name),total); //�������忡���� �̹��� ����
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
    private void d_readCSV(string name)//���� ���չ� �˰���
    {
        Debug.Log("������Ḧ Ȯ���մϴ�.");

        //1.��� �� ���� ����Ʈ ����. => Base Emotion List
        List<EmotionInfo> emotionList = slct_mtrl.mGetTotalEmoList(mtrlDATA);

        //2.�ߺ����� ����Ʈ ����
        //(1) �ߺ������� 3�� �̻��� ��� ���ʿ� ��ġ�� 2���� ������ �ߺ�����Ʈ�� �ִ´�.
        Dictionary<Emotion, KeyValuePair<int, int>> overlapsEmosList = new Dictionary<Emotion, KeyValuePair<int, int>>(); //�ߺ����� Key : {percent, percent}

        Dictionary<Emotion, int> tmpList = new Dictionary<Emotion, int>();

        while (isOverlapState(emotionList))
        {

            for (int i = 0; i < emotionList.Count; i++)
            {
                EmotionInfo content = emotionList[i];

                if (tmpList.ContainsKey(content.Key))//-1- content�� �ߺ�����Ʈ�� ���ԵǾ�������
                {
                    //overlapsEmosList.Add(content.Key, new KeyValuePair<int, int>(tmpList.Value, content.Value));
                    //-2- �ߺ� ���� �� ���� ��ġ�� ���� ������ ���տ� ���ȴ�.
                    int mergedV = tmpList[content.Key] <= content.Value ? tmpList[content.Key] : content.Value;//���׿����ڸ� �̿��Ͽ� �� ���� �� ����.
                    Debug.Log("[(1)������ġ���]" + tmpList[content.Key] + "|" + content.Value + "����" + mergedV + "����!");
                    tmpList.Remove(content.Key);//-2-1 ���տ� ���� ������ ó�������� �ߺ����� ����Ʈ���� �����Ѵ�.

                    Debug.Log("[(2)�ߺ��߰�]�ߺ������� ����Ʈ���� ����");
                    Debug_PrintState("[�����ߺ�����Ʈ ���� ���]", tmpList);
                    //-3- "emotionList" ���� ���� ���� ���� ������ ������ ������ ���� ������ �Ͼ��.
                    //-3-1 ������ ���� ������ �켱������ ���� �����Ѵ�.
                    EmotionInfo fndItm = new EmotionInfo(content.Key, mergedV);
                    int targetIdx = emotionList.FindIndex(a => (a.Key == content.Key && a.Value == mergedV)); //����� ��������Ʈ������ ���մ�� ������ idx
                    int subTargetIdx = targetIdx - 1;
                    Debug.Log("[(3)���մ��]" + "{" + targetIdx + "}");
                    Debug.Log("[(3)���մ��]" + "{" + targetIdx + "}" + emotionList[targetIdx].Key);
                    Debug.Log("[(3)���մ��]" + "{" + subTargetIdx + "}");


                    bool commend;
                    //�տ� ä�� ���: outOfBound
                    //�ڿ� ä�� ���: outOfBound, �տ��� none ��� ���� ���Ա� ����.
                    if (subTargetIdx >= 0 ? true : false)//�켱����(1): ����� ���� ������ �����Ѵ�.
                        commend = emoTableDATA.getCombineResult(emotionList[targetIdx].Key, emotionList[subTargetIdx].Key) != Emotion.NONE ? true : false;//index�� OutOfBound�� �ƴϰ� ������ ����� �ִٸ�! command = false
                    else
                    {
                        commend = false;
                        subTargetIdx = targetIdx + 1;
                        Debug.Log("[�켱����2]ä��");
                    }


                    //�켱����(2): ������ command�� true�� ���� �켱���� (2)�� �Ѿ��.   subTargetIdx = targetIdx + 1
                    if (!commend && subTargetIdx < emotionList.Count ? true : false)
                        commend = emoTableDATA.getCombineResult(emotionList[targetIdx].Key, emotionList[subTargetIdx].Key) != Emotion.NONE ? true : false;

                    //�켱����(1)�Ǵ� (2)���� ���������� ����� ���������� ���� ���.
                    if (commend)
                    {
                        //(1) ���տ� ���� �� ���� �� ���� ��ġ�� �����´�.
                        int CEmoV = emotionList[targetIdx].Value <= emotionList[subTargetIdx].Value ? emotionList[targetIdx].Value : emotionList[subTargetIdx].Value;
                        EmotionInfo finalEmo = new EmotionInfo(emoTableDATA.getCombineResult(emotionList[targetIdx].Key, emotionList[subTargetIdx].Key), CEmoV);

                        emotionList[targetIdx] = finalEmo;//(2)targetEmotion�� ���յ� �� �������� �ٲ۴�.
                        Debug.Log("[���հ��]" + finalEmo.Key);
                        //(2)���տ� ��� �� ������ ����Ʈ���� �����Ѵ�.(for�� ���̱� ������ index ���� ����� �Ѵ�.)
                        emotionList.RemoveAt(subTargetIdx);
                        // ������ ���� index�� ���� Ÿ�� ����  index���� ���� ���(��� ����. ���� Ÿ������ ���� ������ �и��� ������, index�� -1 ���־�� �Ѵ�.
                        if (subTargetIdx < i) i--;
                        // ������ ���� index�� ���� Ÿ�� ���� index���� Ŭ ���.(��� ����: ����Ʈ�� ���̸� �޶�����.)
                    }
                }
                else
                {
                    if (Emotion.PLEASURE <= content.Key && content.Key <= Emotion.INTEXPEC)
                        tmpList.Add(content.Key, content.Value);
                }
                Debug_PrintState("[���簨������Ʈ]", emotionList);
                Debug_PrintState("[�����ߺ�����Ʈ]", tmpList);
            }
        }

        //���� ���� ����Ʈ ����.
    }

    
    //Debug Function
    private void Debug_PrintState(string sTitle,List<EmotionInfo> emotionList)
    {
        string result = sTitle;
        foreach(EmotionInfo info in emotionList)
        {
            result += (info.Key.ToString()+"|");
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
            Debug.Log("������ �����մϴ�.");
            return;
        }

        float time = 5f;
        //�ڷ�ƾ
        UI_btn_txt.text = "����� ��";
        StartCoroutine(isMaking(time));        
    }

    IEnumerator isMaking(float time) //UI ó��
    {
        this.transform.Find("Button").GetComponent<Button>().enabled = false;

        //�� ��Ӱ�
        slct_mtrl.u_setUIbright(total, false);

        yield return new WaitForSeconds(time);

        this.transform.Find("Button").GetComponent<Button>().enabled = true;

        yield return new WaitForSeconds(1);
        //�� ���
        slct_mtrl.u_setUIbright(total);

        //UI �ʱ�ȭ
        slct_mtrl.u_init(total);

        total = 0;
        UI_btn_txt.text = "�����ϱ�";

        Debug.Log("������ ����������ϴ�.");

        m_saveCloud();

        yield break;
    }

    //���� ���� �� �κ��丮�� ����
    private void m_saveCloud()
    {
        //���� �κ��丮 ����Ʈ �����ͼ�
        int cnt = 0;

        //�� �κ��丮 ��ô�ؼ� ���� �ֱ�.
        while(true)
        {
            if (cnt >= 5) break; //�κ��丮 ũ�� �ʰ��ϸ� ����X
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
        Debug.Log("������ ����Ǿ����ϴ�.");
    }
   
    //�ʱ�ȭ �Լ�
    private void init()
    {     
        total = 0;
      
        slct_mtrl = new CloudSystem.S_list();
        slct_mtrl.init(this.transform.Find("selectedIngredient").transform);
        slct_mtrl.default_sprite = default_sprite;

        UI_btn_txt = this.transform.Find("Button").GetComponentInChildren<Text>();
        UI_btn_txt.text = "�����ϱ�";

        
        //���� �κ��丮 ����Ʈ�� ���۷����� ������ ���°� ���� �� ����.

        //event
        m_setEvent();
    }

    //eventmethod ���� �Լ�
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
        mEmotions = new List<EmotionInfo>(); //��������� �� ���̴� Emotion List
        mtrlDATA = new IngredientList();

        InventoryManager inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        mtrlDATA = inventoryManager.mIngredientDatas[inventoryManager.minvenLevel - 1];
        init();
    }

}
