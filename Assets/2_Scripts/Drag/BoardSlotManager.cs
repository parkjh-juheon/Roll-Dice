using UnityEngine;

public class BoardSlotManager : MonoBehaviour
{
    public Transform[] slots;

    public Transform GetFirstEmptySlot()
    {
        // 슬롯 배열이 없거나 길이가 0이면 null 반환
        if (slots == null || slots.Length == 0)
        {
            Debug.LogWarning($"[BoardSlotManager] 슬롯 배열이 비어 있음! (오브젝트: {gameObject.name})");
            return null;
        }

        foreach (Transform slot in slots)
        {
            if (slot == null)
                continue;

            if (slot.childCount == 0)
                return slot;
        }

        // 빈 슬롯 없음
        return null;
    }
}
