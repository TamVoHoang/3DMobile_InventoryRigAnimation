using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FPSCounter : MonoBehaviour
{
    private static FPSCounter instance;
    public TextMeshProUGUI fpsText; // Thêm một Text UI để hiển thị FPS

    private int frameCount = 0;
    private float deltaTime = 0.0f;
    private float fps = 0.0f;
    private float updateInterval = 0.5f; // Cập nhật FPS mỗi 0.5 giây
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (!fpsText)
        {
            Debug.LogError("FPSCounter: Text component is not assigned.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        frameCount++;
        deltaTime += Time.unscaledDeltaTime;

        if (deltaTime > updateInterval)
        {
            fps = frameCount / deltaTime;
            fpsText.text = string.Format("FPS: {0:F2}", fps);

            frameCount = 0;
            deltaTime -= updateInterval;
        }
    }
}
