using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogIndex : MonoBehaviour
{

    private static DialogIndex  instance = null;                    // 싱글톤 기법을 위함 instance 생성

    public int                  mDialogIndex;                       // 씬 이동중에도 대화 진행도를 저장하기 위함

    // Start is called before the first frame update
    void Awake()
    {
        // 싱글톤 기법 사용
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        mDialogIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
