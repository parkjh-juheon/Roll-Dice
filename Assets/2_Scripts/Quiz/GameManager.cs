using UnityEngine;
using UnityEngine.SceneManagement; // 씬 로드를 위해 필요

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    [SerializeField] private Quiz quiz;
    [SerializeField] private EndScreen endScreen;
    [SerializeField] private GameObject loadingCanvas;

    private void Awake()
    {
        // 인스턴스가 없으면 자신을 할당, 있으면 중복 방지
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
    /// 퀴즈 화면을 보여주는 함수
    /// </summary>
    public void ShowQuizScene()
    {
        quiz.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(false);
    }

    /// <summary>
    /// 엔딩 화면을 보여주는 함수
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
    /// 현재 씬을 다시 불러오는 함수 (Replay 버튼에 연결)
    /// </summary>
    public void OnReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
