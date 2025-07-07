using TMPro;
using UnityEngine;

public class BossUnit : EnemyUnit
{
    private bool hasRevived = false;

    [Header("부활 설정")]
    [Tooltip("부활 시 회복할 체력 (기본값: 40)")]
    public int reviveHP = 40;

    [Tooltip("부활 후 주사위 개수 (기본값: 6)")]
    public int reviveDiceCount = 6;

    public TextMeshProUGUI bossHPText;

    public override void TakeDamage(int damage)
    {
        int prevHP = CurrentHP;
        CurrentHP -= damage;
        if (CurrentHP < 0) CurrentHP = 0;

        UpdateHPUI();

        // 피격 효과 및 애니메이션
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

            // 피격 애니메이션 트리거
            if (animator != null)
                animator.SetTrigger("TakeHit");
        }

        // 부활 체크 먼저!
        if (IsDead && !hasRevived)
        {
            hasRevived = true;

            CurrentHP = Mathf.Max(reviveHP, maxHP / 2); // ← 수정된 부분
            UpdateHPUI();

            diceCount = reviveDiceCount;

            if (animator != null)
                animator.SetTrigger("Revive");

            Debug.Log($"{enemyName} (Boss) 부활! 현재 HP: {CurrentHP}, 주사위 개수: {diceCount}");

            return; // 죽지 않고 부활
        }


        // 진짜 사망 처리
        if (IsDead)
        {
            Debug.Log($"{enemyName} (Boss) 완전히 처치됨");

            if (dieParticlePrefab != null)
            {
                GameObject particle = Instantiate(dieParticlePrefab, transform.position, Quaternion.identity);
                var ps = particle.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();
                Destroy(particle, 3.5f);
            }

            // 사망 애니메이션 트리거
            if (animator != null)
                animator.SetTrigger("Death");

            StartCoroutine(DeactivateAfterDelay(2.3f));
        }
    }

    protected override void UpdateHPUI()
    {
        if (bossHPText != null)
            bossHPText.text = $"{CurrentHP} / {maxHP}";
        else
            base.UpdateHPUI(); // 일반 hpText가 설정된 경우를 대비
    }
}
