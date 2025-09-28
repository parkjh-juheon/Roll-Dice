using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] ScoerKeeper scoreKeeper;

    // 최종 점수 출력
    public void ShowFinalScore()
    {
        int score = scoreKeeper.CalculateScore();
        string message;

        if (score == 100)
        {
            message = "전부 맞췄어, 훌륭한데?";
        }
        else if (score >= 80)
        {
            message = "거의 완벽해, 잘했어!";
        }
        else if (score >= 50)
        {
            message = "아쉽네? 좀 더 노력해봐!";
        }
        else
        {
            message = "많이 틀렸네... 다음엔 더 잘할 수 있을거야!";
        }

        finalScoreText.text = message;
    }

}
