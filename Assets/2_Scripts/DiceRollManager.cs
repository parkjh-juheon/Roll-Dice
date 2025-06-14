using UnityEngine;

public class DiceRollManager : MonoBehaviour
{
    public Transform[] playerAttackSlots;
    public Transform[] playerDefenseSlots;

    public GameObject enemyDicePrefab;
    public EnemyUnit[] enemyUnits; // 씬에서 모든 적을 배열로 수동 연결

    private void Start()
    {
        SpawnEnemyDice(); // 게임 시작 시 적 주사위 생성
    }

    public void RollAllPlayerDice()
    {
        // Player 주사위 굴리기
        RollDiceInSlots(playerAttackSlots);
        RollDiceInSlots(playerDefenseSlots);

        // Enemy 주사위 굴리기
        foreach (EnemyUnit enemy in enemyUnits)
        {
            if (!enemy.IsDead)
            {
                RollDiceInSlots(enemy.diceSlots);
            }
        }
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

    public void SpawnEnemyDice()
    {
        foreach (EnemyUnit enemy in enemyUnits)
        {
            if (!enemy.IsDead)
            {
                SpawnDiceInSlots(enemy.diceSlots, enemy.diceCount, enemy.dicePrefab);
            }
        }
    }

    void SpawnDiceInSlots(Transform[] slots, int diceCount, GameObject dicePrefab)
    {
        int maxDicePossible = Mathf.Min(diceCount, slots.Length);
        int placedCount = 0;
        int attemptCount = 0;
        int maxAttempts = 100;

        while (placedCount < maxDicePossible && attemptCount < maxAttempts)
        {
            int randIndex = Random.Range(0, slots.Length);
            Transform slot = slots[randIndex];

            if (slot.childCount == 0)
            {
                GameObject diceObj = Instantiate(dicePrefab, slot.position, Quaternion.identity);
                diceObj.transform.SetParent(slot);
                placedCount++;
            }

            attemptCount++;
        }

        if (attemptCount >= maxAttempts)
        {
            Debug.LogWarning("SpawnDiceInSlots: 최대 시도 초과.");
        }
    }


    void ClearDiceInSlots(Transform[] slots)
    {
        foreach (Transform slot in slots)
        {
            if (slot.childCount > 0)
            {
                Destroy(slot.GetChild(0).gameObject);
            }
        }
    }
    public void ResetAllDice()
    {
        // Player 주사위 Reset
        foreach (Dice dice in FindObjectsOfType<Dice>())
        {
            var drag = dice.GetComponent<DiceDrag>();
            if (drag != null)
            {
                dice.transform.SetParent(null);
                dice.transform.position = drag.InitialPosition;
            }
        }

        // Enemy 주사위 전부 삭제
        foreach (EnemyUnit enemy in enemyUnits)
        {
            ClearDiceInSlots(enemy.diceSlots);
        }

        // Enemy 주사위 재생성 (원래 diceCount 만큼만)
        SpawnEnemyDice();
    }

}
