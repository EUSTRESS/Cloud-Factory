using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �̱��� Ȱ���ؼ� ��𼭵� ȣ���� �� �ִ� Ŭ����
public class ChangeCursor : MonoBehaviour
{
    private static ChangeCursor instance = null;
    public static ChangeCursor Instance
    {
        get
        {
            if (null == instance) return null;

            return instance;
        }
    }
    // Ŀ�� �̹���
    [SerializeField] Texture2D mCursorDef;
    [SerializeField] Texture2D mCursorHold;
    [SerializeField] Texture2D mCursorPoint;
    [SerializeField] Texture2D mCursorType;

    void Start()
    {
        SetDefCursor(); // �⺻ Ŀ��
    }

    public void SetDefCursor() // �⺻ Ŀ��
    {
        Cursor.SetCursor(mCursorDef, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void SetHoldCursor() // ���� �� Ŀ��
    {
        Cursor.SetCursor(mCursorHold, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void SetPointCursor() // ������Ʈ Ȱ��ȭ Ŀ��
    {
        Cursor.SetCursor(mCursorPoint, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void SetTypeCursor() // Ÿ���� ĥ �� Ŀ��
    {
        Cursor.SetCursor(mCursorType, Vector2.zero, CursorMode.ForceSoftware);
    }
}
