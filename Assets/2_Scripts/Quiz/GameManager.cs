using UnityEngine;
using UnityEngine.SceneManagement; // �� �ε带 ���� �ʿ�

public class GameManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static GameManager Instance { get; private set; }

    [SerializeField] private Quiz quiz;
    [SerializeField] private EndScreen endScreen;
    [SerializeField] private GameObject loadingCanvas;

    private void Awake()
    {
        // �ν��Ͻ��� ������ �ڽ��� �Ҵ�, ������ �ߺ� ����
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //ShowQuizScene();
    }

    /// <summary>
    /// ���� ȭ���� �����ִ� �Լ�
    /// </summary>
    public void ShowQuizScene()
    {
        quiz.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(false);
    }

    /// <summary>
    /// ���� ȭ���� �����ִ� �Լ�
    /// </summary>
    public void ShowEndScreen()
    {
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(true);
        endScreen.ShowFinalScore();
        loadingCanvas.SetActive(false);
    }

    public void ShowLoadingScreen()
    {
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(true);
    }

    /// <summary>
    /// ���� ���� �ٽ� �ҷ����� �Լ� (Replay ��ư�� ����)
    /// </summary>
    public void OnReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
