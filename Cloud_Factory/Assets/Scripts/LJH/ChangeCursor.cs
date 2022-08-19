using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 싱글톤 활용해서 어디서든 호출할 수 있는 클래스
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
    // 커서 이미지
    [SerializeField] Texture2D mCursorDef;
    [SerializeField] Texture2D mCursorHold;
    [SerializeField] Texture2D mCursorPoint;
    [SerializeField] Texture2D mCursorType;

    void Start()
    {
        SetDefCursor(); // 기본 커서
    }

    public void SetDefCursor() // 기본 커서
    {
        Cursor.SetCursor(mCursorDef, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void SetHoldCursor() // 집을 때 커서
    {
        Cursor.SetCursor(mCursorHold, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void SetPointCursor() // 오브젝트 활성화 커서
    {
        Cursor.SetCursor(mCursorPoint, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void SetTypeCursor() // 타이핑 칠 때 커서
    {
        Cursor.SetCursor(mCursorType, Vector2.zero, CursorMode.ForceSoftware);
    }
}
