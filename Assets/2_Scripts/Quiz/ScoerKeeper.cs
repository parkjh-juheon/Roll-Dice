using UnityEngine;
public class ScoerKeeper : MonoBehaviour
{
    int correctAnswer = 0;
    int questionSeen = 0;
    public int GetCurrectAnswer()
    {
        return correctAnswer;
    }
    public void IncrementCurrectAnswer()
    {
        correctAnswer++;
    }
    public int GetquestionSeen()
    {
        return questionSeen;
    }
    public void IncrementquestionSeen()
    {
        questionSeen++;
    }
    public int CalculateScore()
    {
        return Mathf.RoundToInt((float)correctAnswer / questionSeen * 100);
    }
}