using UnityEngine;

public class DiceRollManager : MonoBehaviour
{
    public Transform[] playerAttackSlots;
    public Transform[] playerDefenseSlots;

    public void RollAllPlayerDice()
    {
        RollDiceInSlots(playerAttackSlots);
        RollDiceInSlots(playerDefenseSlots);
    }

    void RollDiceInSlots(Transform[] slots)
    {
        foreach (Transform slot in slots)
        {
            if (slot.childCount > 0)
            {
                Dice dice = slot.GetChild(0).GetComponent<Dice>();
                if (dice != null)
                {
                    dice.RollDice();
                }
            }
        }
    }
}
