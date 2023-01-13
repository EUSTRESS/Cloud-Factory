using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_movement : MonoBehaviour
{
    public float x = 0;
    public float y = 0;
    public float limintMin = -5.0f;
    public GameObject cloud;
    public GameObject parts;





    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        x = Random.Range(-0.5f, 0.5f);
        y = Random.Range(0.1f, 0.5f);
        if (Input.GetKeyDown(KeyCode.A))
        {
            cloud.transform.localPosition = new Vector2(x, y);

           
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject go = Instantiate(parts);
            go.transform.position = cloud.transform.position;
            
        }
        



    }
}
