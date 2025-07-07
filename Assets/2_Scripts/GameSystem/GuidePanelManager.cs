using UnityEngine;

public class GuidePanelManager : MonoBehaviour
{
    public GameObject guidePanel; // ���� �г� (UI �̹��� ��)

    public void ShowGuide()
    {
        guidePanel.SetActive(true);
    }

    public void HideGuide()
    {
        guidePanel.SetActive(false);
    }
}
