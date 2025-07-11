using UnityEngine;
using UnityEngine.UI;

public class DiceButtonManager : MonoBehaviour
{
    public Button rollButton;
    public Button resetButton;
    public DiceRollManager diceRollManager;

    [Header("드래그 가능한 주사위 연결")]
    public DiceDrag[] diceList; // 인스펙터에서 연결

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
        if (hasRolled || !diceRollManager.AllDiceStopped()) return;

        hasRolled = true;

        // 버튼 잠금
        rollButton.interactable = false;
        resetButton.interactable = false;

        // 주사위 굴리고, 주사위 다 멈춘 뒤 버튼 다시 켜기
        StartCoroutine(HandleRollAndEnableReset());
    }

    private System.Collections.IEnumerator HandleRollAndEnableReset()
    {
        diceRollManager.RollAllPlayerDice();

        // 주사위가 멈출 때까지 대기
        yield return new WaitUntil(() => diceRollManager.AllDiceStopped());

        // Reset만 다시 켜줌
        resetButton.interactable = true;
    }

    void OnResetClicked()
    {
        if (!diceRollManager.AllDiceStopped()) return;

        diceRollManager.ResetAllDice();
        ResetAllDice();

        hasRolled = false;
        rollButton.interactable = true;
        resetButton.interactable = false;
    }

    public void ResetAllDice()
    {
        foreach (DiceDrag dice in diceList)
        {
            if (dice != null)
                dice.ResetPosition();
        }
    }
}
