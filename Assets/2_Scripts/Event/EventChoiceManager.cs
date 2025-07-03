using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventChoiceManager : MonoBehaviour
{
    [Header("초기 질문")]
    public TextMeshProUGUI questionText;
    public Button yesButton;
    public Button noButton;

    [Header("반응 및 다음")]
    public TextMeshProUGUI responseText;
    public Button nextButton;

    [TextArea]
    public string responseIfYes = "동상에서 목소리가 들린다 \n \"폰트를 바꾸는게 좋을려나?\" ";
    [TextArea]
    public string responseIfNo = "당신은 그냥 지나쳤다. 아무일도 일어나지 않았다";

    void Start()
    {
        // 초기 상태 설정
        responseText.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);

        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    void OnYesClicked()
    {
        ShowResponse(responseIfYes);
    }

    void OnNoClicked()
    {
        ShowResponse(responseIfNo);
    }

    void ShowResponse(string message)
    {
        // 질문 비활성화
        questionText.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        // 응답 표시
        responseText.text = message;
        responseText.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
    }
}
