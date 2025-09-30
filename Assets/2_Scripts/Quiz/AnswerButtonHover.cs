using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnswerButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite hoverSprite;       // 마우스 오버 시 이미지
    private Image image;
    private Sprite defaultSprite;

    void Awake()
    {
        image = GetComponent<Image>();
        defaultSprite = image.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image != null)
            image.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (image != null)
            image.sprite = defaultSprite;
    }

    // 기본 스프라이트를 외부에서 초기화할 수 있도록
    public void ResetSprite(Sprite sprite)
    {
        defaultSprite = sprite;
        image.sprite = sprite;
    }
}
