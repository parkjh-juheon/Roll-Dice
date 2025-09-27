using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] ScoerKeeper scoreKeeper;

    // 최종 점수 출력
    public void ShowFinalScore()
    {
        finalScoreText.text = $"축하합니다!\n\n당신의 점수는" + $"{scoreKeeper.CalculateScore()}% 입니다.";
    }
}
