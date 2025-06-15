using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Unit : MonoBehaviour
{
    [Header("기본 유닛 정보")]
    public string unitName = "Player";
    public int maxHP = 20;
    public int currentHP;
    public bool IsDead => currentHP <= 0;

    [Header("UI 요소")]
    public TextMeshProUGUI hpText;

    [Header("주사위 보드 슬롯")]
    public Transform[] attackSlots; // ★ 추가: 공격 보드
    public Transform[] defenseSlots; // ★ 추가: 방어 보드
    public Transform[] hitSlots;     // ★ 추가: 피격 보드 (적의 공격 주사위가 올려짐)

    private void Awake()
    {
        LoadHP();
        UpdateHPUI();
    }

    // HP UI 업데이트
    public void UpdateHPUI()
    {
        if (hpText != null)
            hpText.text = $"{currentHP}/{maxHP}";
    }

    // 피해 처리
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;

        UpdateHPUI();

        if (unitName == "Player" && currentHP <= 0)
        {
            Debug.Log("Game Over!");
            SceneManager.LoadScene("GameOver"); // GameOver 씬 로드
        }

        if (IsDead)
        {
            Destroy(gameObject); // 필요 시 제거 방식 변경 가능
        }
    }

    // 회복 처리
    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;

        UpdateHPUI();
    }

    // HP 저장 (씬 이동 시 유지)
    public void SaveHP()
    {
        PlayerPrefs.SetInt("PlayerHP", currentHP);
    }

    // HP 불러오기
    public void LoadHP()
    {
        if (unitName == "Player")
        {
            currentHP = PlayerPrefs.GetInt("PlayerHP", maxHP);
        }
        else
        {
            currentHP = maxHP;
        }
    }
}