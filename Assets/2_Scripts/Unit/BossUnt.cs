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
        int tempHP = CurrentHP - damage;

        // 부활 조건 먼저 확인
        if (tempHP <= 0 && !hasRevived)
        {
            hasRevived = true;

            CurrentHP = Mathf.Max(reviveHP, maxHP / 2);
            UpdateHPUI();
            diceCount = reviveDiceCount;

            if (animator != null)
                animator.SetTrigger("Revive");

            Debug.Log("Boss 부활!");
            return;
        }

        // 부활이 아니면 일반 피격 처리
        TakeDamageCore(damage);

        // 일반 사망 처리
        if (IsDead)
        {
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
            base.UpdateHPUI();
    }
}
