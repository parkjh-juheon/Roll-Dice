using UnityEngine;
using UnityEngine.UI; // UI 관련 기능을 사용하기 위해 추가

public class GameManager : MonoBehaviour
{
    // 종료 버튼에 할당할 함수
    public void QuitGame()
    {
        // 에디터에서 실행 중일 때와 빌드된 애플리케이션에서 실행 중일 때를 구분
#if UNITY_EDITOR
        // 에디터에서 플레이 모드를 중지
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 애플리케이션 종료
        Application.Quit();
#endif

        Debug.Log("게임 종료!"); // 디버그 용도
    }
}