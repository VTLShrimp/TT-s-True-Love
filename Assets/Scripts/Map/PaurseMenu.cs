using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [Header("UI References")]
    public GameObject pauseMenuUI;
    public GameObject optionsPanelUI;
    public Button resumeButton;
    public Button optionsButton;
    public Button quitButton;
    public Button saveButton;

    void Start()
    {
        // Ẩn các menu khi bắt đầu game
        pauseMenuUI.SetActive(false);
        optionsPanelUI.SetActive(false);

        // Thêm các sự kiện cho các nút
        resumeButton.onClick.AddListener(Resume);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
        saveButton.onClick.AddListener(SaveGame);
    }

    void Update()
    {
        // Nhấn phím Escape để dừng hoặc tiếp tục game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused) Resume();
            else Pause();
        }
    }

    void Resume()
    {
        // Tắt tất cả các UI và tiếp tục game
        pauseMenuUI.SetActive(false);
        optionsPanelUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        // Hiển thị menu tạm dừng và dừng game
        pauseMenuUI.SetActive(true);
        optionsPanelUI.SetActive(false); // Đảm bảo Options Panel tắt khi vào Pause Menu
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    void OpenOptions()
    {
        // Hiển thị Options Panel và ẩn Pause Menu UI
        optionsPanelUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Debug.Log("Options menu opened."); // Thêm logic tuỳ chọn tại đây
    }

    void SaveGame()
    {
    }

    void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
