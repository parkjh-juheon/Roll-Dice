// GameInitializer.cs 라는 별도 스크립트 생성
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public GameObject playerDataPrefab; // PlayerData 프리팹 연결

    void Awake()
    {
        if (PlayerData.Instance == null)
        {
            Instantiate(playerDataPrefab);
        }
    }
}
