using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Change_image : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite return_Sprite;
    public Sprite Change_Sprite; // ���콺�� ���ٴ���� �� ������ ��������Ʈ

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

    public void OnPointerExit(PointerEventData eventData)
    {
        originalSprite.sprite = return_Sprite;
    }
}