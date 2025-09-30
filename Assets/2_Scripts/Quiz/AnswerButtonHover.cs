using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnswerButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite hoverSprite;       // ���콺 ���� �� �̹���
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

    // �⺻ ��������Ʈ�� �ܺο��� �ʱ�ȭ�� �� �ֵ���
    public void ResetSprite(Sprite sprite)
    {
        defaultSprite = sprite;
        image.sprite = sprite;
    }
}
