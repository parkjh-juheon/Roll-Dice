using UnityEngine;
using UnityEngine.UI;

public class DiceButtonManager : MonoBehaviour
{
    public Button rollButton;
    public Button resetButton;
    public DiceRollManager diceRollManager;

    [Header("�巡�� ������ �ֻ��� ����")]
    public DiceDrag[] diceList; // �ν����Ϳ��� ����

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

        // ��ư ���
        rollButton.interactable = false;
        resetButton.interactable = false;

        // �ֻ��� ������, �ֻ��� �� ���� �� ��ư �ٽ� �ѱ�
        StartCoroutine(HandleRollAndEnableReset());
    }

    private System.Collections.IEnumerator HandleRollAndEnableReset()
    {
        diceRollManager.RollAllPlayerDice();

        // �ֻ����� ���� ������ ���
        yield return new WaitUntil(() => diceRollManager.AllDiceStopped());

        // Reset�� �ٽ� ����
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
