using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecoParts : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 mouseWorld;
    public bool canAttached;
    private Vector2 top_right_corner;
    private Vector2 bottom_left_corner;
    public void init(Vector2 _top_right_corner, Vector2 _bottom_left_corner)
    {
        canAttached = false;
        top_right_corner = _top_right_corner;
        bottom_left_corner = _bottom_left_corner;
        gameObject.AddComponent<PolygonCollider2D>();
        gameObject.AddComponent<Rigidbody2D>();

    }

    private void Update()
    {
        mouseWorld = Camera.main.ScreenToWorldPoint(gameObject.transform.position);
        Collider2D _collider = Physics2D.OverlapArea(bottom_left_corner, top_right_corner,LayerMask.GetMask());
        if (_collider != null)
        {
            Debug.Log(_collider.name);
            canAttached = true;
        }
        else
            canAttached = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            Debug.Log(other.name);
            canAttached = true;
        }
        else
            canAttached = false;
    }




}
