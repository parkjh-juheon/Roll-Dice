using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventSceneManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public Button rollButton;
    public Button nextButton;

    private bool hasRolled = false;

    private void Start()
    {
        resultText.text = "주사위를 굴려 체력을 회복하세요!";
        nextButton.interactable = false;
    }

    public void RollDice()
    {
        if (hasRolled) return; // 중복 굴림 방지

        int diceResult = Random.Range(1, 7); // 1~6
        PlayerData.Instance.currentHP += diceResult;

        // 체력 최대치 제한
        if (PlayerData.Instance.currentHP > PlayerData.Instance.maxHP)
            PlayerData.Instance.currentHP = PlayerData.Instance.maxHP;

        resultText.text = $"주사위 값: {diceResult}\n체력이 {diceResult}만큼 회복되었습니다!\n현재 HP: {PlayerData.Instance.currentHP}/{PlayerData.Instance.maxHP}";
        PlayerData.Instance.SaveHP();

        hasRolled = true;
        nextButton.interactable = true; // 다음으로 이동 가능
    }

    public void GoToNextStage(string nextSceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }
}
