using UnityEngine;

public class GuidePanelManager : MonoBehaviour
{
    public GameObject guidePanel; // 설명 패널 (UI 이미지 등)

    public void ShowGuide()
    {
        guidePanel.SetActive(true);
    }

    public void HideGuide()
    {
        guidePanel.SetActive(false);
    }
}
