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
            IngredientData data = mtrlDATA.mItemList.Find(item => name == item.ingredientName);
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
                    EmotionInfo emoDt = new EmotionInfo();
                    emoDt.init((Emotion)emo.Key, emo.Value);
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

    private void d_readCSV(string name)
    {
        //���� ���չ� ������ �׶� ��ũ��Ʈ �ۼ�.
        Debug.Log("������Ḧ Ȯ���մϴ�.");

        //��� �� ���� ����Ʈ ����. => Base Emotion List
        List<EmotionInfo> emotionList = slct_mtrl.mGetTotalEmoList(mtrlDATA);

        
        //�ߺ��Ǵ� ������ ����Ʈ�� ���� �� ���� ��� �۾��� �ݺ��Ѵ�.
        while (true)
        {
            List<EmotionInfo> overlapList = mIsOverlap(emotionList);
            if (overlapList == null) break;

            //�ߺ����� ó��.
            EmotionInfo target = new EmotionInfo();
            //1. ���տ� ���� ���� �����ϱ�.
            if (overlapList[0].Value >= overlapList[1].Value) target = overlapList[1]; //���� �� ����. ���ٸ� �ڿ��� ����.
            else target = overlapList[0];

            //2. ����� ������ �����ϱ�.
              //(1) �켱����<1> : �������� ���Ǵ� �������� �տ� ��ġ ��.

              //(2) �켱����<2> : �������� ���Ǵ� �������� �ڿ� ��ġ��.

              //(3) ����� �� ������ ������ �Ұ����ϴٸ� ������ �߻����� �ʴ´�.
        }
    }

    private List<EmotionInfo> mIsOverlap(List<EmotionInfo> emotionList)
    {
        //1. �ߺ��Ǵ� ������ �����ϴ��� �˻�.
        List<Emotion> overlapsK = new List<Emotion>();
        List<int> overlapsV = new List<int>();
        foreach (EmotionInfo info in emotionList)
        {
            //2. ���� ������ �̹� ����Ʈ�� ������ Ž���� ��� ���߰�, list(�ߺ��Ǵ� �ΰ��� ��������� ���� ����Ʈ)�� �����Ѵ�.
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
        return null; //�ߺ� ��ᰡ ������ false�� return�Ѵ�.
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
        init();
    }

}
