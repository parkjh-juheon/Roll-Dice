using TMPro;
using UnityEngine;

public class BossUnit : EnemyUnit
{
    private bool hasRevived = false;

    [Header("��Ȱ ����")]
    [Tooltip("��Ȱ �� ȸ���� ü�� (�⺻��: 40)")]
    public int reviveHP = 40;

    [Tooltip("��Ȱ �� �ֻ��� ���� (�⺻��: 6)")]
    public int reviveDiceCount = 6;

    public TextMeshProUGUI bossHPText;

    public override void TakeDamage(int damage)
    {
        int prevHP = CurrentHP;
        CurrentHP -= damage;
        if (CurrentHP < 0) CurrentHP = 0;

        UpdateHPUI();

        // �ǰ� ȿ�� �� �ִϸ��̼�
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

            // �ǰ� �ִϸ��̼� Ʈ����
            if (animator != null)
                animator.SetTrigger("TakeHit");
        }

        // ��Ȱ üũ ����!
        if (IsDead && !hasRevived)
        {
            hasRevived = true;

            CurrentHP = Mathf.Max(reviveHP, maxHP / 2); // �� ������ �κ�
            UpdateHPUI();

            diceCount = reviveDiceCount;

            if (animator != null)
                animator.SetTrigger("Revive");

            Debug.Log($"{enemyName} (Boss) ��Ȱ! ���� HP: {CurrentHP}, �ֻ��� ����: {diceCount}");

            return; // ���� �ʰ� ��Ȱ
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
                Destroy(particle, 3.5f);
            }

            // ��� �ִϸ��̼� Ʈ����
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
            base.UpdateHPUI(); // �Ϲ� hpText�� ������ ��츦 ���
    }
}
