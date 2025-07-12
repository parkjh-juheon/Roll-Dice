using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour
{
    [Header("기본 정보")]
    public string enemyName = "Enemy";
    public int maxHP = 10;
    public int CurrentHP { get; protected set; }

    [Header("주사위 설정")]
    public int diceCount = 3;
    public GameObject dicePrefab;

    [Header("전투 보드 슬롯")]
    public Transform[] attackSlots;
    public Transform[] defenseSlots;
    public Transform[] attackReceiveSlots;

    [Header("UI 연결")]
    public TextMeshProUGUI hpText;

    [Header("체력바")]
    public Image hpBar; // 인스펙터에서 체력바 이미지 연결

    [Header("스프라이트")]
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    public float hitColorDuration = 0.15f;

    [Header("파티클")]
    public GameObject dieParticlePrefab;
    public GameObject hitEffectPrefab;             
    public Transform hitEffectPoint;              

    [Header("애니메이션")]
    public Animator animator; // 인스펙터에서 할당

    [Header("효과음")]
    public AudioClip hitSound;
    public AudioSource audioSource;

    [Header("카메라 흔들림")]
    public bool shakeCameraOnHit = true;

    public bool IsDead => CurrentHP <= 0;

    private List<GameObject> attackDiceObjects = new List<GameObject>();
    private List<GameObject> defenseDiceObjects = new List<GameObject>();

    private bool hasRevived = false;

    private void Awake()
    {
        CurrentHP = maxHP;
        UpdateHPUI();
    }
    private void Start()
    {
        UpdateHPUI(); // 씬이 완전히 로드되고 난 뒤 다시 보장
    }

    public virtual void TakeDamage(int damage)
    {
        int prevHP = CurrentHP;
        CurrentHP -= damage;
        if (CurrentHP < 0) CurrentHP = 0;

        UpdateHPUI();

        // 피격 효과
        if (damage > 0 && CurrentHP < prevHP)
        {
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

                // 피격 사운드
                if (audioSource != null && hitSound != null)
                    audioSource.PlayOneShot(hitSound);

                // 카메라 흔들림
                if (shakeCameraOnHit && CameraShake.Instance != null)
                    CameraShake.Instance.ShakeCamera();
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

    // EnemyUnit.cs 안에 이렇게 함수 분리
    public void TakeDamageCore(int damage)
    {
        int prevHP = CurrentHP;
        CurrentHP -= damage;
        if (CurrentHP < 0) CurrentHP = 0;

        UpdateHPUI();

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

            if (audioSource != null && hitSound != null)
                audioSource.PlayOneShot(hitSound);

            if (shakeCameraOnHit && CameraShake.Instance != null)
                CameraShake.Instance.ShakeCamera();

            if (animator != null)
                animator.SetTrigger("TakeHit");
        }
    }



    protected IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    protected IEnumerator HitColorEffect()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(hitColorDuration);
        spriteRenderer.color = originalColor;
    }

    protected virtual void UpdateHPUI()
    {
        if (hpText != null)
            hpText.text = $"{CurrentHP} / {maxHP}";

        if (hpBar != null)
            hpBar.fillAmount = (float)CurrentHP / maxHP;
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

    public virtual void ResetHP()
    {
        CurrentHP = maxHP;
        UpdateHPUI();
    }

}
