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
        resultText.text = "�ֻ����� ���� ü���� ȸ���ϼ���!";
        nextButton.interactable = false;
    }

    public void RollDice()
    {
        if (hasRolled) return; // �ߺ� ���� ����

        int diceResult = Random.Range(1, 7); // 1~6
        PlayerData.Instance.currentHP += diceResult;

        // ü�� �ִ�ġ ����
        if (PlayerData.Instance.currentHP > PlayerData.Instance.maxHP)
            PlayerData.Instance.currentHP = PlayerData.Instance.maxHP;

        resultText.text = $"�ֻ��� ��: {diceResult}\nü���� {diceResult}��ŭ ȸ���Ǿ����ϴ�!\n���� HP: {PlayerData.Instance.currentHP}/{PlayerData.Instance.maxHP}";
        PlayerData.Instance.SaveHP();

        hasRolled = true;
        nextButton.interactable = true; // �������� �̵� ����
    }

    public void GoToNextStage(string nextSceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }
}
