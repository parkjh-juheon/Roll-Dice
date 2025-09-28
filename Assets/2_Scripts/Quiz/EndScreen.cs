using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] ScoerKeeper scoreKeeper;
    [SerializeField] TextMeshProUGUI hpLogText;


    // 최종 점수 출력
    public void ShowFinalScore()
    {
        int score = scoreKeeper.CalculateScore();
        string message;

        if (score == 100)
        {
            message = "훌륭한데?";
        }
        else if (score >= 80)
        {
            message = "아까워라, " +
                "그냥 아깝다고";
        }
        else if (score >= 50)
        {
            message = "아쉽네? " +
                "유감스러워라";
        }
        else
        {
            message = "ㅋ";
        }

        finalScoreText.text = message;

        Quiz quiz = FindFirstObjectByType<Quiz>();
        if (quiz != null && quiz.hpLogs.Count > 0 && hpLogText != null)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<b>체력 변화 기록</b>");

            foreach (var log in quiz.hpLogs)
            {
                string sign = log.amount > 0 ? "+" : "";
                sb.AppendLine($"{log.question}: {sign}{log.amount}");
            }

            hpLogText.text = sb.ToString();
        }
    }
}

