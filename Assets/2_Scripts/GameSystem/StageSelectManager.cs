using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    [System.Serializable]
    public class StageButton
    {
        public Button button;
        public string stageName;   // 씬 이름
        public string clearKey;    // PlayerPrefs 저장 키
    }

    public StageButton[] stageButtons;

    void Start()
    {
        for (int i = 0; i < stageButtons.Length; i++)
        {
            bool cleared = PlayerPrefs.GetInt(stageButtons[i].clearKey, 0) == 1;

            // 클리어한 스테이지는 버튼 비활성화 및 색 진하게 처리
            if (cleared)
            {
                stageButtons[i].button.interactable = false;
                var colors = stageButtons[i].button.colors;
                colors.normalColor = Color.gray;  // 진한 회색 등으로 변경
                stageButtons[i].button.colors = colors;
            }
            else
            {
                // 클리어 안한 스테이지는 기본 색
                var colors = stageButtons[i].button.colors;
                colors.normalColor = Color.white;
                stageButtons[i].button.colors = colors;

                // Stage1만 활성화, 나머지 비활성화
                if (stageButtons[i].stageName == "Stage1")
                    stageButtons[i].button.interactable = true;
                else
                    stageButtons[i].button.interactable = false;
            }
        }
    }

    public void EnterStage(string stageName)
    {
        // 씬 로드
        SceneManager.LoadScene(stageName);
    }
}
