using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] ScoerKeeper scoreKeeper;

    // ���� ���� ���
    public void ShowFinalScore()
    {
        finalScoreText.text = $"�����մϴ�!\n\n����� ������" + $"{scoreKeeper.CalculateScore()}% �Դϴ�.";
    }
}
