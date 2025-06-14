using TMPro;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [Header("기본 정보")]
    public string enemyName = "Enemy";
    public int maxHP = 10;
    public int CurrentHP { get; private set; }

    [Header("주사위 설정")]
    public Transform[] diceSlots;
    public int diceCount = 3;
    public GameObject dicePrefab; // ★ Enemy별 주사위 프리팹 설정

    public Transform[] attackSlots; // 적 공격 보드
    public Transform[] defenseSlots; // 적 방어 보드


    [Header("UI 연결")]
    public TextMeshProUGUI hpText;

    public bool IsDead => CurrentHP <= 0;

    private void Awake()
    {
        CurrentHP = maxHP;
        UpdateHPUI();
    }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP < 0) CurrentHP = 0;

        UpdateHPUI();

        if (IsDead)
        {
            Debug.Log($"{enemyName} 처치됨");
            gameObject.SetActive(false);
        }
    }

    private void UpdateHPUI()
    {
        if (hpText != null)
            hpText.text = $"{CurrentHP} / {maxHP}";
    }
}
