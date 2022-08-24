using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Aflip_next_report : MonoBehaviour
{
    [SerializeField]
    private Animation anim;
    public AnimationClip a_mv_Next;
    public AnimationClip a_mv_Origin;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetComponent<Animation>();
    }

    public void moveNextPage()
    {
        StartCoroutine(Anim_moveNext_Page());
    }

    IEnumerator Anim_moveNext_Page()
    {
        anim.clip = a_mv_Next; //넘기는 모션 할당
        anim.Play();//play
        yield return new WaitForSeconds(0.3f);
        MoveInHierarchy(-2);
        yield return new WaitForSeconds(0.7f);
        anim.Stop();
        transform.localPosition = new Vector3(-48.0f, -49.0f, 0);
        MoveInHierarchy(2);
        yield break;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveInHierarchy(int delta)
    {
        int index = transform.GetSiblingIndex();
        transform.SetSiblingIndex(index + delta);
    }
}
