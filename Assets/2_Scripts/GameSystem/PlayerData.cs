using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    public int maxHP = 20;
    public int currentHP;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHP();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveHP()
    {
        PlayerPrefs.SetInt("PlayerHP", currentHP);
    }

    public void LoadHP()
    {
        currentHP = PlayerPrefs.GetInt("PlayerHP", maxHP);
    }
}
