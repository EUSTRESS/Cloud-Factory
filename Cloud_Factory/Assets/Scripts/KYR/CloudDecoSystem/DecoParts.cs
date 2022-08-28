using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecoParts : MonoBehaviour
{
    // Start is called before the first frame update
    
    public bool canAttached; //부착할 수 있는가.
    public bool isEditActive; //이동 회전 모드가 활성화 되었는가.
    public bool canEdit; //이동 및 회전이 가능한가.UI On(프레임과 버튼)


    public Vector2 mouseWorld;
    private Vector2 top_right_corner;
    private Vector2 bottom_left_corner;
    public void init(Vector2 _top_right_corner, Vector2 _bottom_left_corner)
    {
        canAttached = true;// 나중에 false로 바꿔줘야함.
        isEditActive = false;
        canEdit = false;
        top_right_corner = _top_right_corner;
        bottom_left_corner = _bottom_left_corner;
    }

    //private void Update()
    //{
    //    mouseWorld = Camera.main.ScreenToWorldPoint(gameObject.transform.position);
    //    Collider2D _collider = Physics2D.OverlapArea(bottom_left_corner, top_right_corner,LayerMask.GetMask());
    //    if (_collider != null)
    //    {
    //        Debug.Log(_collider.name);
    //        canAttached = true;
    //    }
    //    else
    //        canAttached = false;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other != null)
    //    {
    //        Debug.Log(other.name);
    //        canAttached = true;
    //    }
    //    else
    //        canAttached = false;
    //}

    public void ReSettingDecoParts()
    {
        if (canAttached)
            isEditActive = true;
        else
            isEditActive = false;
    }

    public void ActiveCanEdit()
    {
        canEdit = true;
    }

    public void UnactiveCanEdit()
    {
        canEdit = false;
    }
}
