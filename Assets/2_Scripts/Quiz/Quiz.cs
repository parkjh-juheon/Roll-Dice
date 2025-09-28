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
    [SerializeField] Slider problemSlider;   // ���� �ð���
    [SerializeField] Slider solutionSlider;  // �ش� �ð���

    Timer timer;
    bool chooseAnswer = false;

    [Header("����")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoerKeeper scoreKeeper;

    [Header("���൵ UI (������ ���)")]
    [SerializeField] Image[] progressIcons;   // ���� ����ŭ �Ҵ�
    [SerializeField] Sprite defaultIcon;      // �⺻ ������ (��: ȸ��)
    [SerializeField] Sprite correctIcon;      // ���� ������ (��: �ʷϻ�)
    [SerializeField] Sprite wrongIcon;        // ���� ������ (��: ������)

    private int currentQuestionIndex = 0;     // ���� ���� ���� ���� ��ȣ


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

        // ����/�ش� �����̴� �ʱ� ����
        if (problemSlider != null)
        {
            problemSlider.minValue = 0f;
            problemSlider.maxValue = 1f;
            problemSlider.wholeNumbers = false;
        }
        if (solutionSlider != null)
        {
            solutionSlider.minValue = 0f;
            solutionSlider.maxValue = 1f;
            solutionSlider.wholeNumbers = false;
        }

        // ���൵ ������ �ʱ�ȭ
        for (int i = 0; i < progressIcons.Length; i++)
        {
            progressIcons[i].sprite = defaultIcon;
        }

        if (questions.Count == 0)
            GenerateQuestionsIfNeeded();
    }



    private void GenerateQuestionsIfNeeded()
    {
        if (isGeneratingQuestions) return;

        isGeneratingQuestions = true;
        QuizGameManager.Instance.ShowLoadingScreen();
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

        GetNextQuestion();
    }

    private void Update()
    {
        if (timer == null) return;

        if (timer.isProblemTime)
        {
            // ���� �ð�: ���� �����̴��� Ȱ��ȭ
            if (problemSlider != null) problemSlider.gameObject.SetActive(true);
            if (solutionSlider != null) solutionSlider.gameObject.SetActive(false);

            if (problemSlider != null)
                problemSlider.value = timer.fillAmount;
        }
        else
        {
            // �ش� �ð�: �ش� �����̴��� Ȱ��ȭ
            if (problemSlider != null) problemSlider.gameObject.SetActive(false);
            if (solutionSlider != null) solutionSlider.gameObject.SetActive(true);

            if (solutionSlider != null)
                solutionSlider.value = timer.fillAmount;
        }

        // ���� ���� �ҷ�����
        if (timer.loadNextQuestion)
        {
            if (questions.Count == 0)
            {
                GenerateQuestionsIfNeeded();
            }
            else
            {
                GetNextQuestion();
            }
        }

        // SolutionTime�ε� ���� �������� �ʾ��� ��
        if (!timer.isProblemTime && !chooseAnswer)
        {
            DisplaySolution(-1);
        }

        // ��� ������ �� Ǯ���� ��� ���� ȭ������ �̵�
        if (currentQuestionIndex >= progressIcons.Length)
        {
            QuizGameManager.Instance.ShowEndScreen();
            enabled = false; // Quiz ��ũ��Ʈ ��Ȱ��ȭ
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

        QuizGameManager.Instance.ShowQuizScene();
        chooseAnswer = false;
        SetButtonState(true);
        SetDefaultButtonSprites();
        GetRandomQuestion();
        OnDisplayQuestion();
        scoreKeeper.IncrementquestionSeen();
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

            // ���൵ ������ ǥ�� (����)
            if (currentQuestionIndex < progressIcons.Length)
                progressIcons[currentQuestionIndex].sprite = correctIcon;
        }
        else
        {
            questionText.text = "�����Դϴ�. ������ " + currentQuestion.GetCorrectAnswer();

            // ���൵ ������ ǥ�� (����)
            if (currentQuestionIndex < progressIcons.Length)
                progressIcons[currentQuestionIndex].sprite = wrongIcon;
        }

        currentQuestionIndex++; // ���� �ϳ� �Ҹ��
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
