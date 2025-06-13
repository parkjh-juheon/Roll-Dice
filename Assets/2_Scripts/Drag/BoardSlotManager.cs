using UnityEngine;

public class BoardSlotManager : MonoBehaviour
{
    public Transform[] slots;

    public Transform GetFirstEmptySlot()
    {
        foreach (Transform slot in slots)
        {
            if (slot.childCount == 0)
                return slot;
        }
        return null;
    }
}
