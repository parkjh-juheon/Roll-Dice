using UnityEngine.SceneManagement;

public static class StageLoader
{
    public static void LoadStage(int index)
    {
        // 예: Stage 씬 이름은 "Stage1", "Stage2" 이런 식으로 되어 있다고 가정
        SceneManager.LoadScene($"Stage{index + 1}");
    }
}
