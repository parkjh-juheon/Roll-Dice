using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject handStep1; // 화면 클릭 유도 애니메이션
    public GameObject handStep2; // Roll 버튼 클릭 유도
    public GameObject handStep3; // Reset 버튼 클릭 유도

    public GameObject dimBackground; // 불투명 배경 (새로 추가)

    private int step = 1;

    void Start()
    {
        ShowStep(1);
    }

    void Update()
    {
        if (step == 1 && Input.GetMouseButtonDown(0))
        {
            ShowStep(2);
            Debug.Log("Step 1 완료: 화면 클릭 → Step 2 시작");
        }
    }

    public void OnRollClicked()
    {
        if (step == 2)
        {
            ShowStep(3);
            Debug.Log("Step 2 완료: Roll 버튼 → Step 3 시작");
        }
    }

    public void OnResetClicked()
    {
        if (step == 3)
        {
            ShowStep(0);
            Debug.Log("Step 3 완료: Reset 버튼 → 튜토리얼 종료");

            // 배경 제거
            if (dimBackground != null)
                dimBackground.SetActive(false);

            // 씬 내 모든 유닛 HP 초기화
            ResetAllUnitsHP();
        }
    }

    private void ResetAllUnitsHP()
    {
        // Player Unit
        Unit player = FindObjectOfType<Unit>();
        if (player != null)
            player.ResetHP();

        // Enemy Units
        EnemyUnit[] enemies = FindObjectsOfType<EnemyUnit>();
        foreach (EnemyUnit enemy in enemies)
        {
            enemy.ResetHP();
        }

        Debug.Log("튜토리얼 종료 후 모든 유닛 HP 초기화 완료");
    }


    void ShowStep(int newStep)
    {
        step = newStep;

        handStep1.SetActive(step == 1);
        handStep2.SetActive(step == 2);
        handStep3.SetActive(step == 3);

        // 튜토리얼 시작 시 배경 켜기
        if (dimBackground != null)
            dimBackground.SetActive(step != 0);
    }
}
