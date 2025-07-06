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

        // �ǰ� ȿ���� ���� (�⺻ �ִϸ��̼�, ����Ʈ ��)
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

        // ��Ȱ üũ ����!
        if (IsDead && !hasRevived)
        {
            hasRevived = true;

            CurrentHP = Mathf.Max(40, maxHP / 2);
            UpdateHPUI();

            diceCount = 6;

            if (animator != null)
                animator.SetTrigger("Revive");

            Debug.Log($"{enemyName} (Boss) ��Ȱ! ���� HP: {CurrentHP}, �ֻ��� ����: {diceCount}");

            return; //  ���� �ʰ� ��Ȱ
        }

        // ��¥ ��� ó��
        if (IsDead)
        {
            Debug.Log($"{enemyName} (Boss) ������ óġ��");

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
