using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour
{
    [Header("주사위 스프라이트 설정")]
    public Sprite[] diceFaces; // 0 ~ 5: 1~6 스프라이트
    public SpriteRenderer spriteRenderer;

    [Header("애니메이션 설정")]
    public float rollDuration = 1.0f; // 굴리는 전체 시간
    public float rollInterval = 0.1f; // 눈이 바뀌는 속도

    public int CurrentValue { get; private set; } = 1;
    private bool isRolling = false;

    // 외부에서 호출하여 굴림 시작
    public void RollDice()
    {
        if (!isRolling)
            StartCoroutine(RollDiceRoutine());
    }

    private IEnumerator RollDiceRoutine()
    {
        isRolling = true;

        float elapsed = 0f;
        while (elapsed < rollDuration)
        {
            int randIndex = Random.Range(0, diceFaces.Length);
            spriteRenderer.sprite = diceFaces[randIndex];
            elapsed += rollInterval;
            yield return new WaitForSeconds(rollInterval);
        }

        // 최종 결과 결정
        CurrentValue = Random.Range(1, 7);
        spriteRenderer.sprite = diceFaces[CurrentValue - 1];

        isRolling = false;
    }

    public bool IsRolling()
    {
        return isRolling;
    }

    public void ResetDice()
    {
        // 리셋 시 초기 눈(1)로 설정
        CurrentValue = 1;
        spriteRenderer.sprite = diceFaces[0];
    }
}


