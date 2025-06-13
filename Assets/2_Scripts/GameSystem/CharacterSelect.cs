using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    // 캐릭터 ID 또는 이름을 저장하는 함수
    public void SelectCharacter(int characterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        PlayerPrefs.Save();

        // 게임 씬으로 이동
        SceneManager.LoadScene("GameScene");
    }
}
