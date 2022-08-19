using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogIndex : MonoBehaviour
{

    private static DialogIndex  instance = null;                    // �̱��� ����� ���� instance ����

    public int                  mDialogIndex;                       // �� �̵��߿��� ��ȭ ���൵�� �����ϱ� ����

    // Start is called before the first frame update
    void Awake()
    {
        // �̱��� ��� ���
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
