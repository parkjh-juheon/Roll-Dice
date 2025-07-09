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
        int tempHP = CurrentHP - damage;

        // ��Ȱ ���� ���� Ȯ��
        if (tempHP <= 0 && !hasRevived)
        {
            hasRevived = true;

            CurrentHP = Mathf.Max(reviveHP, maxHP / 2);
            UpdateHPUI();
            diceCount = reviveDiceCount;

            if (animator != null)
                animator.SetTrigger("Revive");

            Debug.Log("Boss ��Ȱ!");
            return;
        }

        // ��Ȱ�� �ƴϸ� �Ϲ� �ǰ� ó��
        TakeDamageCore(damage);

        // �Ϲ� ��� ó��
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
