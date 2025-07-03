using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventChoiceManager : MonoBehaviour
{
    [Header("�ʱ� ����")]
    public TextMeshProUGUI questionText;
    public Button yesButton;
    public Button noButton;

    [Header("���� �� ����")]
    public TextMeshProUGUI responseText;
    public Button nextButton;

    [TextArea]
    public string responseIfYes = "���󿡼� ��Ҹ��� �鸰�� \n \"��Ʈ�� �ٲٴ°� ��������?\" ";
    [TextArea]
    public string responseIfNo = "����� �׳� �����ƴ�. �ƹ��ϵ� �Ͼ�� �ʾҴ�";

    void Start()
    {
        // �ʱ� ���� ����
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
        // ���� ��Ȱ��ȭ
        questionText.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        // ���� ǥ��
        responseText.text = message;
        responseText.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
    }
}
