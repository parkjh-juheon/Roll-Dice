using System.Collections;
using TMPro;
    using UnityEngine;
using UnityEngine.UI;

public class Event : MonoBehaviour
{
    public GameObject eventPanel;
    public Button rollButton;
    public Button nextButton;
    public TextMeshProUGUI resultText;

    private bool hasEventEnded = false;

    void Start()
    {
        eventPanel.SetActive(false);
        nextButton.interactable = false;
        rollButton.onClick.AddListener(RollDiceAndApplyEvent);
        nextButton.onClick.AddListener(ShowEventPanel);
    }

    void Update()
    {
        
    }

    public void RollDiceAndApplyEvent()
    {
        if (hasEventEnded) return;

        int diceValue = GetRandomEventValue();
        StartCoroutine(ApplyEventResult(diceValue));
    }

    int GetRandomEventValue()
    {
        // -6~-1 또는 1~6 중 하나를 무작위로 선택 (0은 절대 안 나옴)
        bool isHeal = Random.value > 0.5f;
        if (isHeal)
            return Random.Range(1, 7);   // 1~6
        else
            return Random.Range(-6, 0);  // -6~-1
    }

    IEnumerator ApplyEventResult(int value)
    {
        // 연출용 대기 (필요 없으면 바로 진행)
        yield return new WaitForSeconds(0.5f);

        if (value > 0)
        {
            PlayerData.Instance.currentHP += value;
            if (PlayerData.Instance.currentHP > PlayerData.Instance.maxHP)
                PlayerData.Instance.currentHP = PlayerData.Instance.maxHP;
            resultText.text = $"luck! +{value}\nHP: {PlayerData.Instance.currentHP}/{PlayerData.Instance.maxHP}";
        }
        else if (value < 0)
        {
            PlayerData.Instance.currentHP += value; // value가 음수이므로 감소
            if (PlayerData.Instance.currentHP < 0)
                PlayerData.Instance.currentHP = 0;
            resultText.text = $"fool! {value}\nHP: {PlayerData.Instance.currentHP}/{PlayerData.Instance.maxHP}";
        }

        PlayerData.Instance.SaveHP();

        hasEventEnded = true;
        rollButton.interactable = false;
        nextButton.interactable = true;
    }

    void ShowEventPanel()
    {
        if (hasEventEnded)
        {
            eventPanel.SetActive(true);
            resultText.gameObject.SetActive(false);
            nextButton.interactable = false;
        }
    }
}
