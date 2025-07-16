using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventChoiceManager : MonoBehaviour
{
    public enum ChoiceEffectType { None, Heal, Damage }

    [Header("초기 질문")]
    public TextMeshProUGUI questionText;
    public Button yesButton;
    public Button noButton;

    [Header("반응 및 다음")]
    public TextMeshProUGUI responseText;
    public Button nextButton;

    [TextArea]
    public string responseIfYes = "동상에서 목소리가 들린다 \n \"폰트를 바꾸는게 좋을려나?\" ";
    [TextArea]
    public string responseIfNo = "당신은 그냥 지나쳤다. 아무일도 일어나지 않았다";

    [Header("플레이어 유닛")]
    public Unit playerUnit; // 인스펙터에서 Player Unit 할당

    [Header("Yes 버튼 효과")]
    public ChoiceEffectType yesEffect = ChoiceEffectType.Heal;
    public int yesValue = 10;

    [Header("No 버튼 효과")]
    public ChoiceEffectType noEffect = ChoiceEffectType.Damage;
    public int noValue = 5;

    void Start()
    {
        responseText.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);

        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    void OnYesClicked()
    {
        ApplyEffect(yesEffect, yesValue);
        ShowResponse(responseIfYes);
    }

    void OnNoClicked()
    {   
        ApplyEffect(noEffect, noValue);
        ShowResponse(responseIfNo);
    }

    void ApplyEffect(ChoiceEffectType effectType, int value)
    {
        if (playerUnit == null) return;

        switch (effectType)
        {
            case ChoiceEffectType.Heal:
                playerUnit.Heal(value);
                break;
            case ChoiceEffectType.Damage:
                playerUnit.TakeDamage(value);
                break;
            case ChoiceEffectType.None:
            default:
                break;
        }
    }

    void ShowResponse(string message)
    {
        questionText.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        responseText.text = message;
        responseText.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
    }
}
