using UnityEngine;

public class StageClearHandler : MonoBehaviour
{
    public int stageIndex; // �� �������� ��ȣ
    public ReturnToMap returnToMap; // ReturnToMap ������Ʈ ���� �ʿ�

    public void ClearStageAndGoToMap()
    {
        StageManager stageManager = FindObjectOfType<StageManager>();
        stageManager.ClearStage(stageIndex); // Ŭ���� ���� ����
        returnToMap.GoToMap(); // �� ������ �̵�
    }
}
