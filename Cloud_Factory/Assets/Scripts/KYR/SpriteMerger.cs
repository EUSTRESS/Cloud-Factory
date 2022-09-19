using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteMerger : MonoBehaviour
{
    [SerializeField]
    private List<Transform> mRawList; //스프라이트 분리 안된, 게임오브젝트 리스트

    [SerializeField]
    private List<Sprite> mspriteToMerge;
    // Start is called before the first frame update
    void Start()
    {
        mRawList = new List<Transform>();
    }

    public void initMergeList(GameObject _Fincloud)
    {
        //List 초기화
        mRawList.Add(_Fincloud.transform);

        for(int i = 0; i <_Fincloud.transform.childCount; i++)
        {
            mRawList.Add(_Fincloud.transform.GetChild(i));
        }

        for(int i = 0; i < mRawList.Count; i++)
        {
            mspriteToMerge.Add(mRawList[i].GetComponent<Image>().sprite);
        }
    }
   
    private void Merge()
    {
       // var newText = new Texture2D();
    }
}
