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
    [SerializeField] Slider problemSlider;   // 문제 시간용
    [SerializeField] Slider solutionSlider;  // 해답 시간용

    Timer timer;
    bool chooseAnswer = false;

    [Header("점수")]
    ScoerKeeper scoreKeeper;

    [Header("진행도 UI (아이콘 방식)")]
    [SerializeField] Image[] progressIcons;   // 문제 수만큼 할당
    [SerializeField] Sprite defaultIcon;      // 기본 아이콘 (예: 회색)
    [SerializeField] Sprite correctIcon;      // 정답 아이콘 (예: 초록색)
    [SerializeField] Sprite wrongIcon;        // 오답 아이콘 (예: 빨강색)

    private int currentQuestionIndex = 0;     // 현재 진행 중인 문제 번호

    [Header("플레이어 유닛")]
    [SerializeField] Unit playerUnit;
    [SerializeField] int healAmount = 5;   // 정답시 회복량
    [SerializeField] int damageAmount = 5; // 오답시 피해량

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

        // 문제/해답 슬라이더 초기 세팅
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

        // 진행도 아이콘 초기화
        for (int i = 0; i < progressIcons.Length; i++)
        {
            progressIcons[i].sprite = defaultIcon;
        }
    }

    // 외부에서 호출될 메서드
    public void StartQuiz()
    {
        if (questions.Count == 0)
            GenerateQuestionsIfNeeded();
        else
        {
            // 아이콘 초기화
            for (int i = 0; i < progressIcons.Length; i++)
            {
                progressIcons[i].sprite = defaultIcon;
            }
        }

        // progressIcons 개수를 questionCount에 맞춰 검증
        if (progressIcons.Length != questionCount)
        {
            Debug.LogWarning($"progressIcons 개수({progressIcons.Length})와 questionCount({questionCount})가 다릅니다. 자동 조정 필요!");
            // → 여기서 UI를 생성하거나 questionCount를 progressIcons.Length로 강제 맞추기
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

        GetNextQuestion();
    }

    private void Update()
    {
        if (timer == null) return;

        if (timer.isProblemTime)
        {
            // 문제 시간: 문제 슬라이더만 활성화
            if (problemSlider != null) problemSlider.gameObject.SetActive(true);
            if (solutionSlider != null) solutionSlider.gameObject.SetActive(false);

            if (problemSlider != null)
                problemSlider.value = timer.fillAmount;
        }
        else
        {
            // 해답 시간: 해답 슬라이더만 활성화
            if (problemSlider != null) problemSlider.gameObject.SetActive(false);
            if (solutionSlider != null) solutionSlider.gameObject.SetActive(true);

            if (solutionSlider != null)
                solutionSlider.value = timer.fillAmount;
        }

        // 다음 문제 불러오기
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
            chooseAnswer = true;   // 오답 처리 후 더 이상 중복 호출 안 되게 플래그 세팅
            DisplaySolution(-1);
        }

        if (timer.isProblemTime && problemSlider != null)
        {
            float threshold = 0.2f;   // 남은 시간 비율 20% 이하일 때
            float blinkSpeed = 5f;    // 깜빡이는 속도

            Image sliderFill = problemSlider.fillRect.GetComponent<Image>();
            if (timer.fillAmount <= threshold)
            {
                float t = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
                sliderFill.color = Color.Lerp(Color.white, Color.red, t);

                //  힌트 표시 (처음 한번만)
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



        // 모든 문제를 다 풀었을 경우 엔딩 화면으로 이동
        if (currentQuestionIndex >= progressIcons.Length)
        {
            QuizGameManager.Instance.ShowEndScreen();
            enabled = false; // Quiz 스크립트 비활성화
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
            Debug.Log("더이상 질문이 없습니다.");
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
            Debug.LogError("currentQuestion이 null입니다!");
            return;
        }

        // 문제 텍스트 설정
        questionText.text = currentQuestion.GetQuestion();

        // 힌트 초기화
        hintText.text = "힌트: " + currentQuestion.hint;
        hintText.gameObject.SetActive(false);

        // 버튼 텍스트 설정
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
        // 남은 시간 비율
        float timeFactor = timer != null ? timer.fillAmount : 1f;

        // 정답/오답에 따른 실제 회복/피해량 계산
        int actualHeal = Mathf.CeilToInt(healAmount * timeFactor);        // 시간이 많으면 더 회복
        int actualDamage = Mathf.CeilToInt(damageAmount * (1f - timeFactor)); // 시간이 적으면 더 피해

        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "정답입니다.";
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
            questionText.text = "오답입니다. 정답은 " + currentQuestion.GetCorrectAnswer();

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
        public int amount;      // 변화량 (+ 회복, - 피해)
        public string question; // 어떤 문제에서 발생했는지 (선택사항)

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
