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
    public Animator animator;

    [Header("피격 이펙트")]
    public GameObject hitEffectPrefab;
    public Transform hitEffectPoint;

    [Header("회복 이펙트")]
    public GameObject healEffectPrefab;
    public Transform healEffectPoint; 


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

        if (damage > 0 && CurrentHP < prevHP)
        {
            if (animator != null)
                animator.SetTrigger("Hit");

            if (CameraShake.Instance != null)
                CameraShake.Instance.ShakeCamera();

            if (hitEffectPrefab != null && hitEffectPoint != null)
            {
                GameObject effect = Instantiate(hitEffectPrefab, hitEffectPoint.position, Quaternion.identity);
                var ps = effect.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();
                Destroy(effect, 2f);
            }
        }

        if (unitName == "Player" && CurrentHP <= 0)
        {
            Debug.Log("Game Over!");
            if (animator != null)
                animator.SetTrigger("Death");

            StartCoroutine(LoadGameOverSceneWithDelay(1.5f));
        }
    }

    public void Heal(int amount)
    {
        int prevHP = CurrentHP;
        CurrentHP += amount;

        if (healEffectPrefab != null && healEffectPoint != null)
        {
            GameObject effect = Instantiate(healEffectPrefab, healEffectPoint.position, Quaternion.identity);
            ParticleSystem ps = effect.GetComponentInChildren<ParticleSystem>();
            if (ps != null)
            {
                Debug.Log(" 회복 이펙트: ParticleSystem 찾음, 재생 시작");
                ps.Play();
            }
            else
            {
                Debug.LogWarning(" 회복 이펙트: ParticleSystem 찾지 못함");
            }
            Destroy(effect, 2f);
        }

    }


    public void SaveHP()
    {
        PlayerData.Instance.SaveHP();
    }

    public void LoadHP()
    {
        if (unitName == "Player")
        {
            PlayerData.Instance.LoadHP();
        }
        else
        {
            PlayerData.Instance.currentHP = maxHP;
        }
    }

    private System.Collections.IEnumerator LoadGameOverSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("GameOver");
    }
}
