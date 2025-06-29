using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour
{
    public Sprite[] diceFaces; // ��������Ʈ (0~5�� ������ ��)
    public SpriteRenderer spriteRenderer;

    public float rollDuration = 1.0f;
    public float rollInterval = 0.1f;

    [Header("�� ���� ����")]
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
            int randSprite = Random.Range(0, diceFaces.Length); // �ð� ȿ����
            spriteRenderer.sprite = diceFaces[randSprite];
            elapsed += rollInterval;
            yield return new WaitForSeconds(rollInterval);
        }

        // ���� ����� ����
        CurrentValue = Random.Range(minValue, maxValue + 1);

        // ��������Ʈ�� ���� �������� ǥ��
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
