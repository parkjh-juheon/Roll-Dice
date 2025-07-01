using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [Header("기본 정보")]
    public string enemyName = "Enemy";
    public int maxHP = 10;
    public int CurrentHP { get; private set; }

    [Header("주사위 설정")]
    public int diceCount = 3;
    public GameObject dicePrefab;

    [Header("전투 보드 슬롯")]
    public Transform[] attackSlots;
    public Transform[] defenseSlots;
    public Transform[] attackReceiveSlots;

    [Header("UI 연결")]
    public TextMeshProUGUI hpText;

    [Header("스프라이트")]
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    public float hitColorDuration = 0.15f;

    [Header("파티클")]
    public GameObject dieParticlePrefab;
    public GameObject hitEffectPrefab;             // 🔽 추가: 피격 이펙트 프리팹
    public Transform hitEffectPoint;               // 🔽 추가: 피격 이펙트 위치

    public bool IsDead => CurrentHP <= 0;

    private List<GameObject> attackDiceObjects = new List<GameObject>();
    private List<GameObject> defenseDiceObjects = new List<GameObject>();

    private void Awake()
    {
        CurrentHP = maxHP;
        UpdateHPUI();
    }

    public void TakeDamage(int damage)
    {
        int prevHP = CurrentHP;
        CurrentHP -= damage;
        if (CurrentHP < 0) CurrentHP = 0;

        UpdateHPUI();

        // 피격 효과
        if (damage > 0 && CurrentHP < prevHP)
        {
            if (spriteRenderer != null)
                StartCoroutine(HitColorEffect());

            if (hitEffectPrefab != null && hitEffectPoint != null)
            {
                GameObject effect = Instantiate(hitEffectPrefab, hitEffectPoint.position, Quaternion.identity);

                var ps = effect.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();

                Destroy(effect, 1.5f);
            }
        }

        // 사망 처리
        if (IsDead)
        {
            Debug.Log($"{enemyName} 처치됨");

            if (dieParticlePrefab != null)
            {
                GameObject particle = Instantiate(dieParticlePrefab, transform.position, Quaternion.identity);
                particle.transform.SetParent(null);

                var ps = particle.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();

                Destroy(particle, 2f);
            }

            StartCoroutine(DeactivateAfterDelay(0.5f));
        }
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    private IEnumerator HitColorEffect()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(hitColorDuration);
        spriteRenderer.color = originalColor;
    }

    private void UpdateHPUI()
    {
        if (hpText != null)
            hpText.text = $"{CurrentHP} / {maxHP}";
    }

    public void RollAttackDice()
    {
        foreach (GameObject dice in attackDiceObjects) Destroy(dice);
        attackDiceObjects.Clear();

        int created = 0;
        for (int i = 0; i < attackSlots.Length && created < diceCount; i++)
        {
            if (attackSlots[i].childCount == 0)
            {
                GameObject dice = Instantiate(dicePrefab, attackSlots[i]);
                dice.transform.localPosition = Vector3.zero;
                attackDiceObjects.Add(dice);
                created++;
            }
        }
    }

    public void RollDefenseDice()
    {
        foreach (GameObject dice in defenseDiceObjects) Destroy(dice);
        defenseDiceObjects.Clear();

        int created = 0;
        for (int i = 0; i < defenseSlots.Length && created < diceCount; i++)
        {
            if (defenseSlots[i].childCount == 0)
            {
                GameObject dice = Instantiate(dicePrefab, defenseSlots[i]);
                dice.transform.localPosition = Vector3.zero;
                defenseDiceObjects.Add(dice);
                created++;
            }
        }
    }
}
