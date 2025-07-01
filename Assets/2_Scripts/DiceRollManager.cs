using System.Collections;
using UnityEngine;


public class DiceRollManager : MonoBehaviour
{
    [Header("Player Dice Slots")]
    public Transform[] playerAttackSlots;   // 플레이어 공격 슬롯 (적별)
    public Transform[] playerDefenseSlots;  // 플레이어 방어 슬롯 (공용)

    [Header("References")]
    public EnemyUnit[] enemyUnits;
    public Unit playerUnit; // 플레이어 유닛
    
    [Header("UI Panel")]
    public GameObject victoryPanel;

    public AudioClip diceRollClip;      // 인스펙터에서 주사위 소리 연결
    private AudioSource audioSource;

    [Header("씬 이동 설정")]
    public string nextSceneName = "Clear"; // 인스펙터에서 조정 가능

    private void Start()
    {
        SpawnEnemyDice();
        audioSource = GetComponent<AudioSource>();
    }

    // 플레이어 및 적 모든 주사위 굴리기
    public void RollAllPlayerDice()
    {
        StartCoroutine(RollAllDiceAndCalculate());
        PlayDiceSound();
    }

    void PlayDiceSound()
    {
        if (audioSource != null && diceRollClip != null)
        {
            audioSource.PlayOneShot(diceRollClip);
        }
    }

    private IEnumerator RollAllDiceAndCalculate()
    {
        // 플레이어 공격/방어 주사위 굴림
        RollDiceInSlots(playerAttackSlots);
        RollDiceInSlots(playerDefenseSlots);

        // 적 공격/방어 주사위 굴림
        foreach (EnemyUnit enemy in enemyUnits)
        {
            if (!enemy.IsDead)
            {
                RollDiceInSlots(enemy.attackSlots);
                RollDiceInSlots(enemy.defenseSlots);
            }
        }

        yield return new WaitForSeconds(1.1f);

        CalculateBattle();
    }

    // 주어진 슬롯 배열의 주사위 모두 굴리기
    void RollDiceInSlots(Transform[] slots)
    {
        foreach (Transform slot in slots)
        {
            if (slot.childCount > 0)
            {
                Dice dice = slot.GetChild(0).GetComponent<Dice>();
                if (dice != null)
                    dice.RollDice();
            }
        }
    }

    // 적 유닛 각각의 공격/방어 주사위 생성
    public void SpawnEnemyDice()
    {
        foreach (EnemyUnit enemy in enemyUnits)
        {
            if (!enemy.IsDead)
            {
                SpawnDiceInSlots(enemy.attackSlots, enemy.diceCount, enemy.dicePrefab);
                SpawnDiceInSlots(enemy.defenseSlots, enemy.diceCount, enemy.dicePrefab);
            }
            else
            {
                Debug.Log($"{enemy.enemyName}은(는) 죽어 주사위 생성 안 함");
            }
        }
    }

    void SpawnDiceInSlots(Transform[] slots, int diceCount, GameObject dicePrefab)
    {
        int maxDice = Mathf.Min(diceCount, slots.Length);
        int placed = 0, attempts = 0, maxAttempts = 100;

        while (placed < maxDice && attempts < maxAttempts)
        {
            int rand = Random.Range(0, slots.Length);
            Transform slot = slots[rand];

            if (slot.childCount == 0)
            {
                GameObject diceObj = Instantiate(dicePrefab, slot.position, Quaternion.identity);
                diceObj.transform.SetParent(slot);
                diceObj.transform.localPosition = Vector3.zero;
                placed++;
            }
            attempts++;
        }

        if (attempts >= maxAttempts)
            Debug.LogWarning("SpawnDiceInSlots: 시도 초과");
    }

    public void ResetAllDice()
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

        foreach (EnemyUnit enemy in enemyUnits)
        {
            ClearDiceInSlots(enemy.attackSlots);
            ClearDiceInSlots(enemy.defenseSlots);
        }

        SpawnEnemyDice();
    }

    private void CalculateBattle()
    {
        // 1. 플레이어 → 적 개별 방어
        foreach (EnemyUnit enemy in enemyUnits)
        {
            if (!enemy.IsDead)
            {
                string playerAttackDetail = GetDiceValuesDetailed(enemy.attackReceiveSlots, out int playerAttack);
                string enemyDefenseDetail = GetDiceValuesDetailed(enemy.defenseSlots, out int enemyDefense);

                int damageToEnemy = Mathf.Max(0, playerAttack - enemyDefense);

                Debug.Log($"[전투] {enemy.enemyName} 공격받음: ({playerAttackDetail}) - ({enemyDefenseDetail}) = {damageToEnemy}");
                enemy.TakeDamage(damageToEnemy);
            }
        }

        // 2. 적 전체 공격 → 플레이어 방어 (평균 공격력 사용)
        int totalEnemyAttackBeforeDivide = 0;
        int aliveEnemyCount = 0;
        string totalEnemyAttackDetail = "";

        foreach (EnemyUnit enemy in enemyUnits)
        {
            if (!enemy.IsDead)
            {
                string attackDetail = GetDiceValuesDetailed(enemy.attackSlots, out int attackValue);
                totalEnemyAttackBeforeDivide += attackValue;
                totalEnemyAttackDetail += $"{enemy.enemyName}({attackDetail}) ";
                aliveEnemyCount++;
            }
        }

        // 평균 공격력 계산 (정수 나눗셈)
        int totalEnemyAttack = aliveEnemyCount > 0 ? totalEnemyAttackBeforeDivide / aliveEnemyCount : 0;

        string playerDefenseDetail = GetDiceValuesDetailed(playerDefenseSlots, out int playerDefense);
        int damageToPlayer = Mathf.Max(0, totalEnemyAttack - playerDefense);

        Debug.Log($"[전투] 적 전체 공격 합계: {totalEnemyAttackDetail}= {totalEnemyAttackBeforeDivide}");
        Debug.Log($"[전투] 살아 있는 적 수: {aliveEnemyCount}, 평균 공격력: {totalEnemyAttack}");
        Debug.Log($"[전투] 플레이어 방어: {playerDefenseDetail}= {playerDefense}");
        Debug.Log($"[전투] 플레이어 피해량: {totalEnemyAttack} - {playerDefense} = {damageToPlayer}");

        playerUnit.TakeDamage(damageToPlayer);



        bool allEnemiesDead = true;
        foreach (EnemyUnit enemy in enemyUnits)
        {
            if (!enemy.IsDead)
            {
                allEnemiesDead = false;
                break;
            }
        }

        if (allEnemiesDead)
        {
            Debug.Log("[전투] 모든 적 사망. 클리어 씬으로 이동!");
            StartCoroutine(DelayAndChangeScene(2f)); // 2초 딜레이 후 씬 전환 (원하는 시간으로 조절)
        }
    }

    private IEnumerator DelayAndChangeScene(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ChangeScene 컴포넌트 통해서 nextSceneName 씬으로 이동
        ChangeScene changer = FindObjectOfType<ChangeScene>();
        if (changer != null)
        {
            changer.ChangeToScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("ChangeScene 컴포넌트를 찾을 수 없습니다.");
        }
    }

    private string GetDiceValuesDetailed(Transform[] slots, out int sum)
    {
        sum = 0;
        System.Text.StringBuilder sb = new System.Text.StringBuilder("(");
        bool first = true;

        foreach (Transform slot in slots)
        {
            if (slot.childCount > 0)
            {
                Dice dice = slot.GetChild(0).GetComponent<Dice>();
                if (dice != null)
                {
                    if (!first) sb.Append(", ");
                    sb.Append(dice.CurrentValue);
                    sum += dice.CurrentValue;
                    first = false;
                }
            }
        }
        sb.Append(")");
        return sb.ToString();
    }

    void ClearDiceInSlots(Transform[] slots)
    {
        foreach (Transform slot in slots)
        {
            if (slot.childCount > 0)
                Destroy(slot.GetChild(0).gameObject);
        }
    }
}
