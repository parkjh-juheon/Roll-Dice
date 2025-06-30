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
    public GameObject dicePrefab; // 적별 주사위 프리팹

    [Header("전투 보드 슬롯")]
    public Transform[] attackSlots;        // 적 공격용 슬롯
    public Transform[] defenseSlots;       // 적 방어용 슬롯
    public Transform[] attackReceiveSlots; // 플레이어가 이 적을 공격할 때 사용하는 슬롯

    [Header("UI 연결")]
    public TextMeshProUGUI hpText;

    [Header("스프라이트")]
    public SpriteRenderer spriteRenderer; // 인스펙터에서 연결

    public Color hitColor = Color.red;    // 데미지 시 색상
    public float hitColorDuration = 0.15f; // 색상 지속 시간

    [Header("파티클")]
    public GameObject dieParticlePrefab; // 인스펙터에서 할당

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

        if (spriteRenderer != null && damage > 0 && CurrentHP < prevHP)
            StartCoroutine(HitColorEffect());

        if (IsDead)
        {
            Debug.Log($"{enemyName} 처치됨");

            // 파티클 생성 (부모 없이 생성)
            if (dieParticlePrefab != null)
            {
                GameObject particle = Instantiate(dieParticlePrefab, transform.position, Quaternion.identity);
                particle.transform.SetParent(null); // 부모 관계 제거
                Destroy(particle, 2f); // 파티클 수명 후 제거
            }


            // 일정 시간 후 오브젝트 비활성화
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

    // 공격 보드용 주사위 굴리기
    public void RollAttackDice()
    {
        // 기존 주사위 삭제
        foreach (GameObject dice in attackDiceObjects)
        {
            Destroy(dice);
        }
        attackDiceObjects.Clear();

        // 새 주사위 생성
        int created = 0;
        for (int i = 0; i < attackSlots.Length && created < diceCount; i++)
        {
            if (attackSlots[i].childCount == 0)
            {
                GameObject dice = Instantiate(dicePrefab, attackSlots[i]);
                dice.transform.localPosition = Vector3.zero; // 위치 정렬
                attackDiceObjects.Add(dice);
                created++;
            }
        }

    }

    // 방어 보드용 주사위 굴리기
    public void RollDefenseDice()
    {
        // 기존 주사위 삭제
        foreach (GameObject dice in defenseDiceObjects)
        {
            Destroy(dice);
        }
        defenseDiceObjects.Clear();

        // 새 주사위 생성
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
