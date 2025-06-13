using System.Collections.Generic;
using UnityEngine;

public class EnemyDiceSpawner : MonoBehaviour
{
    [Header("���� ���")]
    public GameObject dicePrefab;                 // ������ �ֻ��� ������
    public Transform[] diceSlots;                 // ���� �迭
    private List<GameObject> spawnedDice = new(); // ������ �ֻ��� ����

    void Start()
    {
        SpawnAllDice();
    }

    // �ֻ����� ���Կ� �������� ��ġ
    public void SpawnAllDice()
    {
        ClearAllDice(); // ���� �ֻ��� ����

        List<Transform> availableSlots = new List<Transform>(diceSlots);
        for (int i = 0; i < diceSlots.Length && availableSlots.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableSlots.Count);
            Transform slot = availableSlots[randomIndex];
            availableSlots.RemoveAt(randomIndex);

            GameObject dice = Instantiate(dicePrefab, slot.position, Quaternion.identity, slot);
            spawnedDice.Add(dice);

            // RollDice�� ��ư ���� �� ����ǵ��� �� ���⼱ ȣ�� X
        }
    }

    // Roll ��ư�� ������ �� ȣ��
    public void RollAll()
    {
        foreach (var diceObj in spawnedDice)
        {
            Dice dice = diceObj.GetComponent<Dice>();
            if (dice != null)
                dice.RollDice();
        }
    }

    // Reset ��ư�� ������ �� ȣ��
    public void RespawnAll()
    {
        SpawnAllDice();
    }

    // ������ �ֻ��� ����
    private void ClearAllDice()
    {
        foreach (var dice in spawnedDice)
        {
            if (dice != null)
                Destroy(dice);
        }
        spawnedDice.Clear();
    }
}
