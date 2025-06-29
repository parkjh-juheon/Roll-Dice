using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Unit : MonoBehaviour
{
    [Header("�⺻ ���� ����")]
    public string unitName = "Player";
    public int maxHP = 20;
    public bool IsDead => CurrentHP <= 0;

    [Header("UI ���")]
    public TextMeshProUGUI hpText;

    [Header("�ֻ��� ���� ����")]
    public Transform[] attackSlots;
    public Transform[] defenseSlots;
    public Transform[] hitSlots;

    [Header("�ִϸ��̼�")]
    public Animator animator; // �ν����Ϳ� ���� �ʿ�

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

        // ������ ü���� ���� ���� ����
        if (damage > 0 && CurrentHP < prevHP)
        {
            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }

            if (CameraShake.Instance != null)
                CameraShake.Instance.ShakeCamera();
        }

        // ��� ó��
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
            PlayerData.Instance.LoadHP(); // �̹� ������!
        }
        else
        {
            PlayerData.Instance.currentHP = maxHP; // ���� �׻� Ǯ��
        }
    }

}
