using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageNode[] stageNodes;

    private void Start()
    {
        UpdateStageStates();
    }

    public void UpdateStageStates()
    {
        for (int i = 0; i < stageNodes.Length; i++)
        {
            // 스테이지0은 항상 오픈
            if (i == 0)
            {
                stageNodes[i].SetState(StageNode.StageState.Unlocked);
            }
            else
            {
                bool prevClear = PlayerPrefs.GetInt($"Stage{i - 1}Clear", 0) == 1;

                if (prevClear)
                    stageNodes[i].SetState(StageNode.StageState.Unlocked);
                else
                    stageNodes[i].SetState(StageNode.StageState.Locked);
            }
        }
    }

    public void ClearStage(int index)
    {
        PlayerPrefs.SetInt($"Stage{index}Clear", 1);
        PlayerPrefs.Save();
    }
}
