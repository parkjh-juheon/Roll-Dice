using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] ScoerKeeper scoreKeeper;

    // ���� ���� ���
    public void ShowFinalScore()
    {
        int score = scoreKeeper.CalculateScore();
        string message;

        if (score == 100)
        {
            message = "���� �����, �Ǹ��ѵ�?";
        }
        else if (score >= 80)
        {
            message = "���� �Ϻ���, ���߾�!";
        }
        else if (score >= 50)
        {
            message = "�ƽ���? �� �� ����غ�!";
        }
        else
        {
            message = "���� Ʋ�ȳ�... ������ �� ���� �� �����ž�!";
        }

        finalScoreText.text = message;
    }

}
