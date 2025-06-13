using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Unit : MonoBehaviour
{
    [Header("�⺻ ���� ����")]
    public string unitName = "Player";
    public int maxHP = 20;
    public int currentHP;

    [Header("UI ���")]
    public TextMeshProUGUI hpText;

    private void Awake()
    {
        LoadHP();
        UpdateHPUI();
    }

    // HP UI ������Ʈ
    public void UpdateHPUI()
    {
        if (hpText != null)
            hpText.text = $"{currentHP}/{maxHP}";
    }

    // ���� ó��
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;

        UpdateHPUI();

        if (unitName == "Player" && currentHP <= 0)
        {
            Debug.Log("Game Over!");
            SceneManager.LoadScene("GameOver"); // GameOver �� �ε�
        }
    }

    // ȸ�� ó��
    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;

        UpdateHPUI();
    }

    // HP ���� (�� �̵� �� ����)
    public void SaveHP()
    {
        PlayerPrefs.SetInt("PlayerHP", currentHP);
    }

    // HP �ҷ�����
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
