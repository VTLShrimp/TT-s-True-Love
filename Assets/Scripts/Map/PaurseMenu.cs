using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    
    [Header("UI References")]
    public GameObject pauseMenuUI;
    public GameObject optionsPanelUI;
    public GameObject healthBarCanvas;
    public GameObject nutCanvas;
    public GameObject pauseMenuCanvas;
    public GameObject pauseGamePanel;

    [Header("Buttons")]
    public Button resumeButton;
    public Button optionsButton;
    public Button quitButton;

    void Start()
    {
        optionsPanelUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        
        resumeButton.onClick.AddListener(Resume);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        optionsPanelUI.SetActive(false);
        SetGameplayUI(true);

        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        SetGameplayUI(false);

        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void OpenOptions()
    {
        optionsPanelUI.SetActive(true);
        SetGameplayUI(false);
    }

    void SetGameplayUI(bool isActive)
    {
        // Keep healthBarCanvas always active and control other elements only
        nutCanvas.SetActive(isActive);
        pauseMenuCanvas.SetActive(isActive);
        pauseGamePanel.SetActive(isActive);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
