using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Unit : MonoBehaviour
{
    [Header("기본 유닛 정보")]
    public string unitName = "Player";
    public int maxHP = 20;
    public bool IsDead => CurrentHP <= 0;

    [Header("UI 요소")]
    public TextMeshProUGUI hpText;

    [Header("주사위 보드 슬롯")]
    public Transform[] attackSlots;
    public Transform[] defenseSlots;
    public Transform[] hitSlots;

    [Header("애니메이션")]
    public Animator animator; // 인스펙터에 연결 필요

    private void Awake()
    {
        LoadHP();
        UpdateHPUI();
    }

    public int CurrentHP
    {
        get => PlayerData.Instance.currentHP;
        set
        {
            PlayerData.Instance.currentHP = Mathf.Clamp(value, 0, maxHP);
            UpdateHPUI();
        }
    }

    public void UpdateHPUI()
    {
        if (hpText != null)
            hpText.text = $"{CurrentHP}/{maxHP}";
    }

    public void TakeDamage(int damage)
    {
        int prevHP = CurrentHP;
        CurrentHP -= damage;

        // 실제로 체력이 깎였을 때만 실행
        if (damage > 0 && CurrentHP < prevHP)
        {
            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }

            if (CameraShake.Instance != null)
                CameraShake.Instance.ShakeCamera();
        }

        // 사망 처리
        if (unitName == "Player" && CurrentHP <= 0)
        {
            Debug.Log("Game Over!");
            SceneManager.LoadScene("GameOver");
        }

        if (IsDead)
        {
            Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        CurrentHP += amount;
    }

    public void SaveHP()
    {
        PlayerData.Instance.SaveHP();
    }

    public void LoadHP()
    {
        if (unitName == "Player")
        {
            PlayerData.Instance.LoadHP(); // 이미 구현됨!
        }
        else
        {
            PlayerData.Instance.currentHP = maxHP; // 적은 항상 풀피
        }
    }

}
