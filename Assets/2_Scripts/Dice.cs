using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour
{
    public Sprite[] diceFaces; // 스프라이트 (0~5만 있으면 됨)
    public SpriteRenderer spriteRenderer;

    public float rollDuration = 1.0f;
    public float rollInterval = 0.1f;

    [Header("값 범위 설정")]
    public int minValue = -6;
    public int maxValue = 6;

    public int CurrentValue { get; private set; } = 1;
    private bool isRolling = false;

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
            int randSprite = Random.Range(0, diceFaces.Length); // 시각 효과용
            spriteRenderer.sprite = diceFaces[randSprite];
            elapsed += rollInterval;
            yield return new WaitForSeconds(rollInterval);
        }

        // 실제 결과값 결정
        CurrentValue = Random.Range(minValue, maxValue + 1);

        // 스프라이트는 절댓값 기준으로 표시
        int faceIndex = Mathf.Clamp(Mathf.Abs(CurrentValue) - 1, 0, diceFaces.Length - 1);
        spriteRenderer.sprite = diceFaces[faceIndex];

        isRolling = false;
    }

    public bool IsRolling()
    {
        return isRolling;
    }

    public void ResetDice()
    {
        CurrentValue = 1;
        spriteRenderer.sprite = diceFaces[0];
    }
}
