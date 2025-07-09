using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject handStep1; // ȭ�� Ŭ�� ���� �ִϸ��̼�
    public GameObject handStep2; // Roll ��ư Ŭ�� ����
    public GameObject handStep3; // Reset ��ư Ŭ�� ����

    public GameObject dimBackground; // ������ ��� (���� �߰�)

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
            Debug.Log("Step 1 �Ϸ�: ȭ�� Ŭ�� �� Step 2 ����");
        }
    }

    public void OnRollClicked()
    {
        if (step == 2)
        {
            ShowStep(3);
            Debug.Log("Step 2 �Ϸ�: Roll ��ư �� Step 3 ����");
        }
    }

    public void OnResetClicked()
    {
        if (step == 3)
        {
            ShowStep(0); // Ʃ�丮�� ����
            Debug.Log("Step 3 �Ϸ�: Reset ��ư �� Ʃ�丮�� ����");

            // ������ ��� ����
            if (dimBackground != null)
                dimBackground.SetActive(false);
        }
    }

    void ShowStep(int newStep)
    {
        step = newStep;

        handStep1.SetActive(step == 1);
        handStep2.SetActive(step == 2);
        handStep3.SetActive(step == 3);

        // Ʃ�丮�� ���� �� ��� �ѱ�
        if (dimBackground != null)
            dimBackground.SetActive(step != 0);
    }
}
