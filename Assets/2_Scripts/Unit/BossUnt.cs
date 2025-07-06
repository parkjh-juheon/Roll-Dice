using UnityEngine;

public class BossUnit : EnemyUnit
{
    private bool hasRevived = false;

    public override void TakeDamage(int damage)
    {
        int prevHP = CurrentHP;
        CurrentHP -= damage;
        if (CurrentHP < 0) CurrentHP = 0;

        UpdateHPUI();

        // 피격 효과만 실행 (기본 애니메이션, 이펙트 등)
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

        // 부활 체크 먼저!
        if (IsDead && !hasRevived)
        {
            hasRevived = true;

            CurrentHP = Mathf.Max(40, maxHP / 2);
            UpdateHPUI();

            diceCount = 6;

            if (animator != null)
                animator.SetTrigger("Revive");

            Debug.Log($"{enemyName} (Boss) 부활! 현재 HP: {CurrentHP}, 주사위 개수: {diceCount}");

            return; //  죽지 않고 부활
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
                Destroy(particle, 2f);
            }

            if (animator != null)
                animator.SetTrigger("Death");

            StartCoroutine(DeactivateAfterDelay(0.5f));
        }
    }
}
