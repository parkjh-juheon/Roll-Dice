using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public Unit playerUnit; // 인스펙터 연결 or Start()에서 자동 연결됨

    private void Start()
    {
        // Start에서 PlayerData 및 Unit 자동 연결
        if (playerUnit == null)
        {
            playerUnit = FindObjectOfType<Unit>();
        }
    }

    public void ChangeToScene(string sceneName)
    {
        if (playerUnit != null)
        {
            // Restart 용으로 Title 씬으로 갈 때 초기화
            if (sceneName == "Title") // Restart 버튼 클릭 시
            {
                PlayerData.Instance.currentHP = PlayerData.Instance.maxHP;
                PlayerData.Instance.SaveHP();
            }
            else
            {
                playerUnit.SaveHP(); // 일반적인 씬 이동은 현재 체력 저장
            }
        }

        SceneManager.LoadScene(sceneName);
    }

    // Option UI의 Restart 버튼에 연결할 함수 (더 명확한 방법)
    public void RestartGame()
    {
        if (playerUnit != null)
        {
            PlayerData.Instance.currentHP = PlayerData.Instance.maxHP;
            PlayerData.Instance.SaveHP();
        }

        SceneManager.LoadScene("Title"); // Restart 시 Title로 이동
    }
}
