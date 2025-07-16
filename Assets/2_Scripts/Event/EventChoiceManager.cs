using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventChoiceManager : MonoBehaviour
{
    public enum ChoiceEffectType { None, Heal, Damage }

    [Header("�ʱ� ����")]
    public TextMeshProUGUI questionText;
    public Button yesButton;
    public Button noButton;

    [Header("���� �� ����")]
    public TextMeshProUGUI responseText;
    public Button nextButton;

    [TextArea]
    public string responseIfYes = "���󿡼� ��Ҹ��� �鸰�� \n \"��Ʈ�� �ٲٴ°� ��������?\" ";
    [TextArea]
    public string responseIfNo = "����� �׳� �����ƴ�. �ƹ��ϵ� �Ͼ�� �ʾҴ�";

    [Header("�÷��̾� ����")]
    public Unit playerUnit; // �ν����Ϳ��� Player Unit �Ҵ�

    [Header("Yes ��ư ȿ��")]
    public ChoiceEffectType yesEffect = ChoiceEffectType.Heal;
    public int yesValue = 10;

    [Header("No ��ư ȿ��")]
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
