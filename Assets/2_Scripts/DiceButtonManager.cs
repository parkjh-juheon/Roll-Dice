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
        foreach (Dice dice in FindObjectsOfType<Dice>())
        {
            var drag = dice.GetComponent<DiceDrag>();
            if (drag != null)
            {
                dice.transform.SetParent(null);
                dice.transform.position = drag.InitialPosition;
            }

        }

        hasRolled = false;
        rollButton.interactable = true;
        resetButton.interactable = false;
    }
}
