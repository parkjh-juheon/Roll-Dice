using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("UI ����")]
    public TextMeshProUGUI gameOverMessageText;

    [Header("���� �޽��� ���")]
    [TextArea(2, 5)]
    public string[] messages = {
        "���� �� �����ñ���",
        "����",
        "���� ���ݸ� �� ���Ҵ����...",
        "������ ���°� �߿��ϱ���",
        "���ݸ� �� ����ߴٸ�!",
        "������ �ֻ����� ��� �������� �����.",
        "��밡 �ʹ� ���߱���!",
        "�װ� �׷��� �ϴ°� �ƴѵ�"
    };

    void Start()
    {
        ShowRandomMessage();
    }

    void ShowRandomMessage()
    {
        if (messages.Length == 0 || gameOverMessageText == null) return;

        int rand = Random.Range(0, messages.Length);
        gameOverMessageText.text = messages[rand];
    }
}
