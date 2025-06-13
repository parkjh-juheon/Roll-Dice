using System.Collections.Generic;
using UnityEngine;

public class EnemyDiceSpawner : MonoBehaviour
{
    [Header("연결 대상")]
    public GameObject dicePrefab;                 // 생성할 주사위 프리팹
    public Transform[] diceSlots;                 // 슬롯 배열
    private List<GameObject> spawnedDice = new(); // 생성된 주사위 저장

    void Start()
    {
        SpawnAllDice();
    }

    // 주사위를 슬롯에 무작위로 배치
    public void SpawnAllDice()
    {
        ClearAllDice(); // 기존 주사위 제거

        List<Transform> availableSlots = new List<Transform>(diceSlots);
        for (int i = 0; i < diceSlots.Length && availableSlots.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableSlots.Count);
            Transform slot = availableSlots[randomIndex];
            availableSlots.RemoveAt(randomIndex);

            GameObject dice = Instantiate(dicePrefab, slot.position, Quaternion.identity, slot);
            spawnedDice.Add(dice);

            // RollDice는 버튼 누를 때 실행되도록 → 여기선 호출 X
        }
    }

    // Roll 버튼을 눌렀을 때 호출
    public void RollAll()
    {
        foreach (var diceObj in spawnedDice)
        {
            Dice dice = diceObj.GetComponent<Dice>();
            if (dice != null)
                dice.RollDice();
        }
    }

    // Reset 버튼을 눌렀을 때 호출
    public void RespawnAll()
    {
        SpawnAllDice();
    }

    // 생성된 주사위 제거
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
