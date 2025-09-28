using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] ScoerKeeper scoreKeeper;
    [SerializeField] TextMeshProUGUI hpLogText;


    // ���� ���� ���
    public void ShowFinalScore()
    {
        int score = scoreKeeper.CalculateScore();
        string message;

        if (score == 100)
        {
            message = "�Ǹ��ѵ�?";
        }
        else if (score >= 80)
        {
            message = "�Ʊ����, " +
                "�׳� �Ʊ��ٰ�";
        }
        else if (score >= 50)
        {
            message = "�ƽ���? " +
                "������������";
        }
        else
        {
            message = "��";
        }

        finalScoreText.text = message;

        Quiz quiz = FindFirstObjectByType<Quiz>();
        if (quiz != null && quiz.hpLogs.Count > 0 && hpLogText != null)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<b>ü�� ��ȭ ���</b>");

            foreach (var log in quiz.hpLogs)
            {
                string sign = log.amount > 0 ? "+" : "";
                sb.AppendLine($"{log.question}: {sign}{log.amount}");
            }

            hpLogText.text = sb.ToString();
        }
    }
}

