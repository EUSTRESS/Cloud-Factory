using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Change_image : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public Sprite return_Sprite;
    public Sprite Change_Sprite; // 마우스를 갖다대었을 때 보여줄 스프라이트
    public Sprite Click_change_Sprite;

    public GameObject basic_image_obj;

    private Image originalSprite;

    private void Start()
    {
        originalSprite = basic_image_obj.GetComponent<Image>();
        return_Sprite = originalSprite.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        originalSprite.sprite = Change_Sprite; 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        originalSprite.sprite = Click_change_Sprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        originalSprite.sprite = return_Sprite;
    }
}