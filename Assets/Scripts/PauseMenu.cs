using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false; // Static variable to track pause state
    public GameObject pauseMenuUI; // Reference to the pause menu UI
    public GameObject optionsPanelUI; // Reference to the options panel UI
    public GameObject healthBarCanvas; // Reference to the HealthBar canvas
    public GameObject nutCanvas; // Reference to the 4Nut canvas
    public GameObject inventoryHealthCanvas; // Reference to the InventoryHealth canvas
    public GameObject pauseMenuCanvas; // Reference to the PauseMenu canvas
    public Button resumeButton; // Button to resume the game
    public Button optionsButton; // Button to open options
    public Button quitButton; // Button to quit the game

    public GameObject PaurseGamePanel;
    void Start()
    {

        // Make sure the options panel is hidden at the start
        optionsPanelUI.SetActive(false);
    
        pauseMenuUI.SetActive(false); // Hide the pause menu at the start
        // Attach the button listeners here
        resumeButton.onClick.AddListener(Resume);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        // Check if the "Escape" key is pressed to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume(); // If game is paused, resume
            }
            else
            {
                inventoryHealthCanvas.SetActive(false);
                Pause(); // If game is running, pause
            }
        }
    }

    public void Resume()
    {
        // Hide pause menu
        pauseMenuUI.SetActive(false);
        
        // Ensure options panel is hidden when resuming
        optionsPanelUI.SetActive(false);

        // Restore game time
        Time.timeScale = 1f;

        // Set the pause state to false
        GameIsPaused = false;

        // Show all relevant canvases when resuming
        healthBarCanvas.SetActive(true);
        nutCanvas.SetActive(true);
        inventoryHealthCanvas.SetActive(true);
        pauseMenuCanvas.SetActive(true);
    }

    void Pause()
    {
        // Show pause menu
        pauseMenuUI.SetActive(true);

        // Freeze game time
        Time.timeScale = 0f;

        // Set the pause state to true
        GameIsPaused = true;
    }

    public void OpenOptions()
    {
        // Show the options panel
        optionsPanelUI.SetActive(true);

        // Hide other canvases when the options panel is active
        healthBarCanvas.SetActive(false);
        nutCanvas.SetActive(false);
        inventoryHealthCanvas.SetActive(false);
        PaurseGamePanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        // Resume time before loading the main menu scene
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game..."); // For debugging
        Application.Quit(); // Quit the application
    }
}
