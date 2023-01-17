using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_movement : MonoBehaviour
{

    private float x = 0;
    private float y = 0;
    private float x2 = 0;
    private float y2 = 0;
    private float x3 = 0;
    private float y3 = 0;
    private float x4 = 0;
    private float y4 = 0;


    public int num = 0;
    //public int result = 0;
    public float limintMin = -5.0f;
    public GameObject cloud;
    public GameObject parts;

    Parts_fly parts_Fly;
    




    // Start is called before the first frame update
    void Start()
    {
        //parts_Fly = GameObject.Find<"Part">.GetComponent<Parts_fly>().fly_speed;
        num = Random.Range(1, 4);

        switch (num)
        {
            case 1:
                x = -0.5f;
                y = 0.23f;

                cloud.transform.localPosition = new Vector2(x, y);
                break;

            case 2:
                x2 = 0.57f;
                y2 = 0.31f;

                cloud.transform.localPosition = new Vector2(x2, y2);
                break;

            case 3:
                x3 = -0.29f;
                y3 = -0.29f;

                cloud.transform.localPosition = new Vector2(x3, y3);
                break;

            case 4:
                x4 = 0.34f;
                y4 = -0.1f;

                cloud.transform.localPosition = new Vector2(x4, y4);
                break;







        }



        //cloud.transform.localPosition = new Vector2(x, y);



    }


    // Update is called once per frame
    void Update()
{
        //x = Random.Range(-0.5f, 0.5f);
        //y = Random.Range(0.2f, 0.5f);
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    cloud.transform.localPosition = new Vector2(x, y);

        
        //}
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject go = Instantiate(parts);
            go.transform.position = cloud.transform.position;

        }
        //x = Random.Range(-0.5f, 0.5f);
        //y = Random.Range(0.2f, 0.5f);
        //while (true)
        //{
        //    cloud.transform.localPosition = new Vector2(x, y);

        //    GameObject go = Instantiate(parts);
        //    go.transform.position = cloud.transform.position;
        //    num++;
        //    if (num == 1)
        //    {
        //        break;
        //    }


        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    cloud.transform.localPosition = new Vector2(x, y);


        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    GameObject go = Instantiate(parts);
        //    go.transform.position = cloud.transform.position;

        //}




    }
}
