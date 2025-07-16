using UnityEngine;

public class PanelController : MonoBehaviour
{
    public GameObject targetPanel; // 활성화할 패널

    public void ShowPanel()
    {
        targetPanel.SetActive(true); // 패널을 활성화
    }

    public void HidePanel()
    {
        targetPanel.SetActive(false); // 패널 비활성화
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 현재 활성화 상태를 반전시켜서 SetActive
            bool isActive = targetPanel.activeSelf;
            targetPanel.SetActive(!isActive);

        }
    }
}