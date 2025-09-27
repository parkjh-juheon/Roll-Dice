using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]

public class QuestionSO : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] string question = "여기에 질문을 적어주세요";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIndex = 0;
    [SerializeField] public string hint = "힌트"; 


    public string GetQuestion()
    {
        return question;
    }
    
    public string GetHint()
    {
        return hint;
    }

    public string GetCorrectAnswer()
    {
        return answers[correctAnswerIndex];
    }

    public string GetAnswers(int index)
    {
        return answers[index];
    }
    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }

    public void SetData(string q, string[] a, int correctIndex, string h)
    {
        SetData(q, a, correctIndex);
        hint = h;
    }
    public void SetData(string q, string[] a, int correctIndex)
    {
        question = q;
        answers = a;
        correctAnswerIndex = correctIndex;
    }
}
