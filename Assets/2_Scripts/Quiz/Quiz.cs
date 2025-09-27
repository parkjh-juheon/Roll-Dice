using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("����")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI hintText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("����")]
    [SerializeField] GameObject[] answerButtons;

    [Header("��ư ����")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Ÿ�̸�")]
    [SerializeField] Image timerImge;
    [SerializeField] Sprite problemTimerSprite;
    [SerializeField] Sprite solutionTimerSprite;
    Timer timer;
    bool chooseAnswer = false;

    [Header("����")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoerKeeper scoreKeeper;

    [Header("��")]
    [SerializeField] Slider progressBar;

    [Header("ChteatGPT")]
    [SerializeField] ChatGPTClient ChatGPTClient;
    [SerializeField] int questionCount = 3;
    [SerializeField] TextMeshProUGUI LoadingText;

    bool isGeneratingQuestions = false;

    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<ScoerKeeper>();
        ChatGPTClient.quizGenerateHandler += QuizGeneratedHandler;

        if (questions.Count == 0)
        {
            GenerateQuestionsIfNeeded();
        }
        else
        {
            InitializeProgressBar();
        }
    }

    private void GenerateQuestionsIfNeeded()
    {
        if (isGeneratingQuestions) return;

        isGeneratingQuestions = true;
        GameManager.Instance.ShowLoadingScreen();
        string toltipcUse = GetTrendingTooltip();
        ChatGPTClient.GenerateQuizQuestions(questionCount, toltipcUse);
        Debug.Log($"GenerateQuestionsIfNeeded : {toltipcUse}");
    }

    private static string GetTrendingTooltip()
    {
        string[] topics = new string[] { "�ֽ� IT ���", "�α� �ִ� ����", "�ֱ� ������ ��ȭ", "������ ������ ���", "�������� ������ �̺�Ʈ" };
        int randomIndex = Random.Range(0, topics.Length); // Fixed typo: changed 'randamidax' to 'randomIndex' and corrected syntax for Random.Range
        return topics[randomIndex];
    }

    void QuizGeneratedHandler(List<QuestionSO> generateQuestions)
    {
        Debug.Log($"QuizGeneratedHandler : {generateQuestions.Count} questions recived.");
        isGeneratingQuestions = false;

        if (generateQuestions == null || generateQuestions.Count == 0)
        {
            Debug.LogError("������ �������� �ʾҽ��ϴ�. ");
            LoadingText.text = "���� ������ �����߽��ϴ�. �ٽ� �õ����ּ���.";
            return;
        }
        this.questions.AddRange(generateQuestions);
        progressBar.maxValue += generateQuestions.Count;

        GetNextQuestion();
    }

    private void InitializeProgressBar()
    {
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    private void Update()
    {
        // Timer �̹��� ������Ʈ
        timerImge.fillAmount = timer.fillAmount;

        if (timer.isProblemTime)
        {
            timerImge.sprite = problemTimerSprite;
        }
        else
        {
            timerImge.sprite = solutionTimerSprite;
        }

        // ���� ���� �ҷ�����
        if (timer.loadNextQuestion)
        {
            if (questions.Count == 0)
            {
                GenerateQuestionsIfNeeded();
                //GameManager.Instance.ShowEndScreen();
            }
            else
            {
                //timer.loadNextQuestion = false;
                GetNextQuestion();
            }
        }

        // SolutionTime�ε� ���� �������� �ʾ��� ��
        if (timer.isProblemTime == false && chooseAnswer == false)
        {
            DisplaySolution(-1);
        }
    }

    private void GetNextQuestion()
    {
        timer.loadNextQuestion = false;

        if (questions.Count <= 0)
        {
            Debug.Log("���̻� ������ �����ϴ�.");
            return;
        }

        GameManager.Instance.ShowQuizScene();
        chooseAnswer = false;
        SetButtonState(true);
        SetDefaultButtonSprites();
        GetRandomQuestion();
        OnDisplayQuestion();
        scoreKeeper.IncrementquestionSeen();
        progressBar.value++;
    }

    private void GetRandomQuestion()
    {
        int randomIndex = Random.Range(0, questions.Count);
        currentQuestion = questions[randomIndex];
        questions.RemoveAt(randomIndex);
    }

    private void SetDefaultButtonSprites()
    {
        foreach (GameObject obj in answerButtons)
        {
            obj.GetComponent<Image>().sprite = defaultAnswerSprite;
        }
    }

    private void OnDisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();
        hintText.text = "��Ʈ: " + currentQuestion.hint;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.GetAnswers(i);
        }
    }

    public void OnAnswerButtonClicked(int index)
    {
        chooseAnswer = true;
        DisplaySolution(index);

        timer.CancelTimer();
        scoreText.text = $"Score: {scoreKeeper.CalculateScore()} %";
    }

    private void DisplaySolution(int index)
    {
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "�����Դϴ�.";
            answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
            scoreKeeper.IncrementCurrectAnswer();
        }
        else
        {
            questionText.text = "�����Դϴ�. �ƽ����� ������ " + currentQuestion.GetCorrectAnswer();
        }
        SetButtonState(false);
    }

    private void SetButtonState(bool state)
    {
        foreach (GameObject obj in answerButtons)
        {
            obj.GetComponent<Button>().interactable = state;
        }
    }
}
