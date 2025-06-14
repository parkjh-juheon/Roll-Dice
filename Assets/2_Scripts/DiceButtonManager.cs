using UnityEngine;
using UnityEngine.UI;

public class DiceButtonManager : MonoBehaviour
{
    public Button rollButton;
    public Button resetButton;
    public DiceRollManager diceRollManager;

    private bool hasRolled = false;

    void Start()
    {
        rollButton.onClick.AddListener(OnRollClicked);
        resetButton.onClick.AddListener(OnResetClicked);

        rollButton.interactable = true;
        resetButton.interactable = false;
    }

    void OnRollClicked()
    {
        if (hasRolled) return;

        diceRollManager.RollAllPlayerDice();

        hasRolled = true;
        rollButton.interactable = false;
        resetButton.interactable = true;
    }

    void OnResetClicked()
    {
        diceRollManager.ResetAllDice(); // 새로 만든 Reset 함수 호출

        hasRolled = false;
        rollButton.interactable = true;
        resetButton.interactable = false;
    }


}
