using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("UI 연결")]
    public TextMeshProUGUI gameOverMessageText;

    [Header("랜덤 메시지 목록")]
    [TextArea(2, 5)]
    public string[] messages = {
        "저런 운 없으시군요",
        "유감",
        "운이 조금만 더 좋았더라면...",
        "끝까지 가는게 중요하긴해",
        "조금만 더 노력했다면!",
        "다음엔 주사위가 당신 편일지도 몰라요.",
        "상대가 너무 강했군요!",
        "그거 그렇게 하는거 아닌데"
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
