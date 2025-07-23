using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D clickCursor;
    public Vector2 hotspot = Vector2.zero;

    private bool isMouseDown = false;

    private static CursorManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }


    void Start()
    {
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            Cursor.SetCursor(clickCursor, hotspot, CursorMode.Auto);
            isMouseDown = true;
        }
        else if (Input.GetMouseButtonUp(0) && isMouseDown)
        {
            Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
            isMouseDown = false;
        }
    }
}
