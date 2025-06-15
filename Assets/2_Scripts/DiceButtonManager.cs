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

        diceRollManager.RollAllPlayerDice(); // 주사위 굴리기
        //diceRollManager.CalculateBattle();   // 바로 전투 계산

        hasRolled = true;
        rollButton.interactable = false;
        resetButton.interactable = true;
    }

    void OnResetClicked()
    {
        diceRollManager.ResetAllDice();

        hasRolled = false;
        rollButton.interactable = true;
        resetButton.interactable = false;
    }
}
