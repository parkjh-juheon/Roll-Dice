using UnityEngine;
using UnityEngine.UI;

public class StageNode : MonoBehaviour
{
    public enum StageState { Locked, Unlocked, Cleared }
    public StageState currentState;

    public Button stageButton;
    public Image stageImage;

    public Color lockedColor;
    public Color unlockedColor;

    public int stageIndex;

    public void SetState(StageState state)
    {
        currentState = state;
        switch (state)
        {
            case StageState.Locked:
                stageButton.interactable = false;
                stageImage.color = lockedColor;
                break;
            case StageState.Unlocked:
                stageButton.interactable = true;
                stageImage.color = unlockedColor;
                break;
        }
    }

    public void OnClickStage()
    {
        PlayerData.Instance.SaveHP(); // HP 저장
        StageLoader.LoadStage(stageIndex); // 스테이지 이동
    }
}
