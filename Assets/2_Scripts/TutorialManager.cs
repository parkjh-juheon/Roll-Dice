using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject handStep1; // 화면 클릭 유도 애니메이션
    public GameObject handStep2; // Roll 버튼 클릭 유도
    public GameObject handStep3; // Reset 버튼 클릭 유도

    private int step = 1;

    void Start()
    {
        ShowStep(1);
    }

    void Update()
    {
        // Step 1: 화면 클릭 → Step 2로 전환
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
            ShowStep(0); // 튜토리얼 종료
            Debug.Log("Step 3 완료: Reset 버튼 → 튜토리얼 종료");
        }
    }

    void ShowStep(int newStep)
    {
        step = newStep;

        handStep1.SetActive(step == 1);
        handStep2.SetActive(step == 2);
        handStep3.SetActive(step == 3);
    }
}
