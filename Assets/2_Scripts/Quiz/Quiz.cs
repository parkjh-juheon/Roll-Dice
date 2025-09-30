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
    ScoerKeeper scoreKeeper;

    [Header("���൵ UI (������ ���)")]
    [SerializeField] Image[] progressIcons;   // ���� ����ŭ �Ҵ�
    [SerializeField] Sprite defaultIcon;      // �⺻ ������ (��: ȸ��)
    [SerializeField] Sprite correctIcon;      // ���� ������ (��: �ʷϻ�)
    [SerializeField] Sprite wrongIcon;        // ���� ������ (��: ������)

    private int currentQuestionIndex = 0;     // ���� ���� ���� ���� ��ȣ

    [Header("�÷��̾� ����")]
    [SerializeField] Unit playerUnit;
    [SerializeField] int healAmount = 5;   // ����� ȸ����
    [SerializeField] int damageAmount = 5; // ����� ���ط�

    public List<HPLog> hpLogs = new List<HPLog>();

    [Header("ChteatGPT")]
    [SerializeField] ChatGPTClient ChatGPTClient;
    [SerializeField] int questionCount = 4;
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
    }

    // �ܺο��� ȣ��� �޼���
    public void StartQuiz()
    {
        if (questions.Count == 0)
            GenerateQuestionsIfNeeded();
        else
        {
            // ������ �ʱ�ȭ
            for (int i = 0; i < progressIcons.Length; i++)
            {
                progressIcons[i].sprite = defaultIcon;
            }
        }

        // progressIcons ������ questionCount�� ���� ����
        if (progressIcons.Length != questionCount)
        {
            Debug.LogWarning($"progressIcons ����({progressIcons.Length})�� questionCount({questionCount})�� �ٸ��ϴ�. �ڵ� ���� �ʿ�!");
            // �� ���⼭ UI�� �����ϰų� questionCount�� progressIcons.Length�� ���� ���߱�
            questionCount = progressIcons.Length;
        }

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

        // Update()
        if (!timer.isProblemTime && !chooseAnswer)
        {
            chooseAnswer = true;   // ���� ó�� �� �� �̻� �ߺ� ȣ�� �� �ǰ� �÷��� ����
            DisplaySolution(-1);
        }

        if (timer.isProblemTime && problemSlider != null)
        {
            float threshold = 0.2f;   // ���� �ð� ���� 20% ������ ��
            float blinkSpeed = 5f;    // �����̴� �ӵ�

            Image sliderFill = problemSlider.fillRect.GetComponent<Image>();
            if (timer.fillAmount <= threshold)
            {
                float t = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
                sliderFill.color = Color.Lerp(Color.white, Color.red, t);

                //  ��Ʈ ǥ�� (ó�� �ѹ���)
                if (!hintText.gameObject.activeSelf)
                {
                    hintText.gameObject.SetActive(true);
                }
            }
            else
            {
                sliderFill.color = Color.white;
            }
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

        if (currentQuestionIndex >= progressIcons.Length)
        {
            QuizGameManager.Instance.ShowEndScreen();
            enabled = false;
            return;
        }

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
        if (currentQuestion == null)
        {
            Debug.LogError("currentQuestion�� null�Դϴ�!");
            return;
        }

        // ���� �ؽ�Ʈ ����
        questionText.text = currentQuestion.GetQuestion();

        // ��Ʈ �ʱ�ȭ
        hintText.text = "��Ʈ: " + currentQuestion.hint;
        hintText.gameObject.SetActive(false);

        // ��ư �ؽ�Ʈ ����
        for (int i = 0; i < answerButtons.Length; i++)
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.GetAnswers(i);
    }


    public void OnAnswerButtonClicked(int index)
    {
        chooseAnswer = true;
        DisplaySolution(index);

        timer.CancelTimer();
    }

    private void DisplaySolution(int index)
    {
        // ���� �ð� ����
        float timeFactor = timer != null ? timer.fillAmount : 1f;

        // ����/���信 ���� ���� ȸ��/���ط� ���
        int actualHeal = Mathf.CeilToInt(healAmount * timeFactor);        // �ð��� ������ �� ȸ��
        int actualDamage = Mathf.CeilToInt(damageAmount * (1f - timeFactor)); // �ð��� ������ �� ����

        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "�����Դϴ�.";
            if (index >= 0)
                answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
            scoreKeeper.IncrementCurrectAnswer();

            if (currentQuestionIndex < progressIcons.Length)
                progressIcons[currentQuestionIndex].sprite = correctIcon;

            if (playerUnit != null)
            {
                playerUnit.Heal(actualHeal);
                hpLogs.Add(new HPLog(+actualHeal, currentQuestion.GetQuestion()));
            }
        }
        else
        {
            questionText.text = "�����Դϴ�. ������ " + currentQuestion.GetCorrectAnswer();

            if (currentQuestionIndex < progressIcons.Length)
                progressIcons[currentQuestionIndex].sprite = wrongIcon;

            if (playerUnit != null)
            {
                playerUnit.TakeDamage(actualDamage);
                hpLogs.Add(new HPLog(-actualDamage, currentQuestion.GetQuestion()));
            }
        }

        SetButtonState(false);
        currentQuestionIndex++;
    }

    public class HPLog
    {
        public int amount;      // ��ȭ�� (+ ȸ��, - ����)
        public string question; // � �������� �߻��ߴ��� (���û���)

        public HPLog(int amount, string question = "")
        {
            this.amount = amount;
            this.question = question;
        }
    }

    private void SetButtonState(bool state)
    {
        foreach (GameObject obj in answerButtons)
        {
            obj.GetComponent<Button>().interactable = state;
        }
    }
}
