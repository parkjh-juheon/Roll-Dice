using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMap : MonoBehaviour
{
    public void GoToMap()
    {
        SceneManager.LoadScene("Map"); // "Map" �� �̸��� �°�
    }
}
