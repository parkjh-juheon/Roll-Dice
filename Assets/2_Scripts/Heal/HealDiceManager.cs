using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventDiceManager : MonoBehaviour
{
    public Dice greenDice;
    public GameObject healPanel;         //  나중에 수동으로 활성화할 패널
    public Button rollButton;            // Roll 버튼
    public Button nextButton;            //  Next 버튼
    public TextMeshProUGUI healResultText;

    private bool hasHealed = false;

    void Start()
    {
        healPanel.SetActive(false);
        nextButton.interactable = true; // Next 버튼을 항상 활성화
        rollButton.onClick.AddListener(RollDiceAndHeal);
        nextButton.onClick.AddListener(ShowHealPanel); //  버튼 클릭 연결
    }

    public void RollDiceAndHeal()
    {
        if (hasHealed) return;

        greenDice.RollDice();
        StartCoroutine(WaitForResult());
        rollButton.interactable = false;
    }

    IEnumerator WaitForResult()
    {
        while (greenDice.IsRolling())
        {
            yield return null;
        }

        int result = greenDice.CurrentValue;

        PlayerData.Instance.currentHP += result;
        if (PlayerData.Instance.currentHP > PlayerData.Instance.maxHP)
            PlayerData.Instance.currentHP = PlayerData.Instance.maxHP;

        PlayerData.Instance.SaveHP();
        Debug.Log($"[회복 이벤트] 주사위: {result}, 회복 후 체력: {PlayerData.Instance.currentHP}");

        hasHealed = true;
        
        nextButton.interactable = true; // 회복 후에도 Next 버튼 사용 가능

        healResultText.text = $"Heal: {result}\nHeal After HP\n{PlayerData.Instance.currentHP}/{PlayerData.Instance.maxHP}";

        FindObjectOfType<Unit>()?.UpdateHPUI();
        var player = FindObjectOfType<Unit>();
        if (player != null && player.healEffectPrefab != null && player.hitEffectPoint != null)
        {
            GameObject effect = Instantiate(player.healEffectPrefab, player.hitEffectPoint.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }

    void ShowHealPanel()
    {
        // 조건 없이 항상 동작
        healPanel.SetActive(true);                 // 패널 활성화
        healResultText.gameObject.SetActive(false); // healResultText 비활성화
        nextButton.interactable = false;
    }
}
