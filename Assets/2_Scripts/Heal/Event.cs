using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Event : MonoBehaviour
{
    public Dice eventDice;
    public Button rollButton;
    public Button nextButton;
    public GameObject healPanel;
    public TextMeshProUGUI healResultText;

    private bool hasProcessed = false;

    void Start()
    {
        healPanel.SetActive(false);
        healResultText.text = "";

        // 주사위 범위를 설정
        //eventDice.minValue = -6;
        //eventDice.maxValue = 6;

        rollButton.onClick.AddListener(RollDiceAndProcessHP);
        nextButton.onClick.AddListener(ShowHealPanel);
    }

    public void RollDiceAndProcessHP()
    {
        if (hasProcessed) return;

        eventDice.RollDice();
        StartCoroutine(WaitForResult());
    }

    IEnumerator WaitForResult()
    {
        while (eventDice.IsRolling())
        {
            yield return null;
        }

        int result = eventDice.CurrentValue;
        int oldHP = PlayerData.Instance.currentHP;

        if (result > 0)
        {
            PlayerData.Instance.currentHP += result;
            if (PlayerData.Instance.currentHP > PlayerData.Instance.maxHP)
                PlayerData.Instance.currentHP = PlayerData.Instance.maxHP;

            healResultText.text = $"Heal +{result}\nHP: {oldHP} → {PlayerData.Instance.currentHP}";
        }
        else if (result < 0)
        {
            PlayerData.Instance.currentHP += result; // result가 음수이므로 감소됨
            if (PlayerData.Instance.currentHP < 0)
                PlayerData.Instance.currentHP = 0;

            healResultText.text = $"Damage {result}\nHP: {oldHP} → {PlayerData.Instance.currentHP}";
        }
        else
        {
            healResultText.text = $" 변화 없음 (0)";
        }

        PlayerData.Instance.SaveHP();
        hasProcessed = true;

        rollButton.interactable = false;
        nextButton.interactable = true;
    }

    void ShowHealPanel()
    {
        if (hasProcessed)
        {
            healPanel.SetActive(true);
            healResultText.gameObject.SetActive(false);
            nextButton.interactable = false;
        }
    }
}
