using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    [System.Serializable]
    public class StageButton
    {
        public Button button;
        public string stageName;   // �� �̸�
        public string clearKey;    // PlayerPrefs ���� Ű
    }

    public StageButton[] stageButtons;

    void Start()
    {
        for (int i = 0; i < stageButtons.Length; i++)
        {
            bool cleared = PlayerPrefs.GetInt(stageButtons[i].clearKey, 0) == 1;

            // Ŭ������ ���������� ��ư ��Ȱ��ȭ �� �� ���ϰ� ó��
            if (cleared)
            {
                stageButtons[i].button.interactable = false;
                var colors = stageButtons[i].button.colors;
                colors.normalColor = Color.gray;  // ���� ȸ�� ������ ����
                stageButtons[i].button.colors = colors;
            }
            else
            {
                // Ŭ���� ���� ���������� �⺻ ��
                var colors = stageButtons[i].button.colors;
                colors.normalColor = Color.white;
                stageButtons[i].button.colors = colors;

                // Stage1�� Ȱ��ȭ, ������ ��Ȱ��ȭ
                if (stageButtons[i].stageName == "Stage1")
                    stageButtons[i].button.interactable = true;
                else
                    stageButtons[i].button.interactable = false;
            }
        }
    }

    public void EnterStage(string stageName)
    {
        // �� �ε�
        SceneManager.LoadScene(stageName);
    }
}
