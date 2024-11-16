using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionsPanelUI;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button saveButton;

    private const float PausedTimeScale = 0f;
    private const float RunningTimeScale = 1f;

    private void Awake()
    {
        // Thiết lập sự kiện cho các nút
        resumeButton.onClick.AddListener(Resume);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
        saveButton.onClick.AddListener(SaveGame);
    }

    private void Start()
    {
        // Ẩn các menu khi bắt đầu game
        pauseMenuUI.SetActive(false);
        optionsPanelUI.SetActive(false);
    }

    private void Update()
    {
        // Nhấn phím Escape để dừng hoặc tiếp tục game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    private void Resume()
    {
        // Tắt tất cả các UI và tiếp tục game
        pauseMenuUI.SetActive(false);
        optionsPanelUI.SetActive(false);
        SetGamePaused(false);
    }

    private void Pause()
    {
        // Hiển thị menu tạm dừng và dừng game
        pauseMenuUI.SetActive(true);
        optionsPanelUI.SetActive(false);
        SetGamePaused(true);
    }

    private void OpenOptions()
    {
        optionsPanelUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        GameIsPaused = true;
        Debug.Log("Options menu opened.");
    }

    private void SaveGame()
    {
        DataPresistenceManager.Instance.SavePlayerData();
        Debug.Log("Game saved.");
    }

    private void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    private void SetGamePaused(bool isPaused)
    {
        Time.timeScale = isPaused ? PausedTimeScale : RunningTimeScale;
        GameIsPaused = isPaused;
    }
}
