using UnityEngine;

public class StageClearHandler : MonoBehaviour
{
    public int stageIndex; // 이 스테이지 번호
    public ReturnToMap returnToMap; // ReturnToMap 컴포넌트 참조 필요

    public void ClearStageAndGoToMap()
    {
        StageManager stageManager = FindObjectOfType<StageManager>();
        stageManager.ClearStage(stageIndex); // 클리어 정보 저장
        returnToMap.GoToMap(); // 맵 씬으로 이동
    }
}
