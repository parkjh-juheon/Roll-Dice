using UnityEngine.SceneManagement;

public static class StageLoader
{
    public static void LoadStage(int index)
    {
        // ��: Stage �� �̸��� "Stage1", "Stage2" �̷� ������ �Ǿ� �ִٰ� ����
        SceneManager.LoadScene($"Stage{index + 1}");
    }
}
