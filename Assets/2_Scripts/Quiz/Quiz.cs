using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("질문")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI hintText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("보기")]
    [SerializeField] GameObject[] answerButtons;

    [Header("버튼 색깔")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("타이머")]
    [SerializeField] Image timerImge;
    [SerializeField] Sprite problemTimerSprite;
    [SerializeField] Sprite solutionTimerSprite;
    Timer timer;
    bool chooseAnswer = false;

    [Header("점수")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoerKeeper scoreKeeper;

    [Header("바")]
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
        string[] topics = new string[] { "최신 IT 기술", "인기 있는 게임", "최근 개봉한 영화", "유명한 역사적 사건", "세계적인 스포츠 이벤트" };
        int randomIndex = Random.Range(0, topics.Length); // Fixed typo: changed 'randamidax' to 'randomIndex' and corrected syntax for Random.Range
        return topics[randomIndex];
    }

    void QuizGeneratedHandler(List<QuestionSO> generateQuestions)
    {
        Debug.Log($"QuizGeneratedHandler : {generateQuestions.Count} questions recived.");
        isGeneratingQuestions = false;

        if (generateQuestions == null || generateQuestions.Count == 0)
        {
            Debug.LogError("질문이 생성되지 않았습니다. ");
            LoadingText.text = "질문 생성에 실패했습니다. 다시 시도해주세요.";
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
        // Timer 이미지 업데이트
        timerImge.fillAmount = timer.fillAmount;

        if (timer.isProblemTime)
        {
            timerImge.sprite = problemTimerSprite;
        }
        else
        {
            timerImge.sprite = solutionTimerSprite;
        }

        // 다음 문제 불러오기
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

        // SolutionTime인데 답을 선택하지 않았을 때
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
            Debug.Log("더이상 질문이 없습니다.");
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
        hintText.text = "힌트: " + currentQuestion.hint;

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
            questionText.text = "정답입니다.";
            answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
            scoreKeeper.IncrementCurrectAnswer();
        }
        else
        {
            questionText.text = "오답입니다. 아쉬워라 정답은 " + currentQuestion.GetCorrectAnswer();
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
