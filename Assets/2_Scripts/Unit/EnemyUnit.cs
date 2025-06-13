using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EnemyUnit : MonoBehaviour
{
    [Header("기본 정보")]
    public string enemyName = "Goblin";
    public int maxHP = 10;
    public int currentHP;

    [Header("UI 연결")]
    public TextMeshProUGUI hpText;

    [Header("주사위 설정")]
    public int diceCount = 2;
    public GameObject dicePrefab;
    public Transform[] attackSlots; // 슬롯 여러 개 연결 가능

    private List<Dice> spawnedDice = new List<Dice>();

    private void Start()
    {
        currentHP = maxHP;
        UpdateHPUI();

        SpawnDiceOnly(); // Start에서는 굴리지 않고 생성만
    }

    // 🔹 주사위를 슬롯에 생성만 하고 굴리진 않음
    public void SpawnDiceOnly()
    {
        ClearAllDice(); // 기존 주사위 제거

        List<int> randomIndices = GetShuffledIndices(attackSlots.Length);

        for (int i = 0; i < diceCount && i < attackSlots.Length; i++)
        {
            GameObject diceObj = Instantiate(dicePrefab, attackSlots[randomIndices[i]]);
            diceObj.transform.localPosition = Vector3.zero;
            diceObj.transform.localScale = Vector3.one;

            Dice dice = diceObj.GetComponent<Dice>();
            if (dice != null)
                spawnedDice.Add(dice);
        }
    }

    // 🔹 외부에서 호출: Roll 버튼 클릭 시 실행
    public void RollAllDice()
    {
        foreach (Dice dice in spawnedDice)
        {
            if (dice != null)
                dice.RollDice();
        }
    }

    public void ResetDicePositions()
    {
        SpawnDiceOnly();
    }

    private void ClearAllDice()
    {
        foreach (Dice dice in spawnedDice)
        {
            if (dice != null)
                Destroy(dice.gameObject);
        }
        spawnedDice.Clear();
    }

    private List<int> GetShuffledIndices(int length)
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < length; i++) indices.Add(i);

        for (int i = 0; i < indices.Count; i++)
        {
            int j = Random.Range(i, indices.Count);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }
        return indices;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;

        UpdateHPUI();

        if (currentHP <= 0)
        {
            Debug.Log($"{enemyName} 처치됨");
            Destroy(gameObject);
        }
    }

    public void UpdateHPUI()
    {
        if (hpText != null)
            hpText.text = $"{currentHP}/{maxHP}";
    }
}
