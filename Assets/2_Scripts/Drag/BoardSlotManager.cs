using UnityEngine;

public class BoardSlotManager : MonoBehaviour
{
    public Transform[] slots;

    public Transform GetFirstEmptySlot()
    {
        // ���� �迭�� ���ų� ���̰� 0�̸� null ��ȯ
        if (slots == null || slots.Length == 0)
        {
            Debug.LogWarning($"[BoardSlotManager] ���� �迭�� ��� ����! (������Ʈ: {gameObject.name})");
            return null;
        }

        foreach (Transform slot in slots)
        {
            if (slot == null)
                continue;

            if (slot.childCount == 0)
                return slot;
        }

        // �� ���� ����
        return null;
    }
}
