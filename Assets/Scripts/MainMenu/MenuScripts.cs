using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{
    public void NewGameClicked()
    {
        DataPersistenceManager.instance.NewGame();
        Debug.Log("New Game");
        SceneManager.LoadScene("Home");
    }

    public void LoadGameClicked()
    {
        DataPersistenceManager.instance.LoadGame();
        Debug.Log("Load Game button clicked.");

        // Always load the Home scene when loading a game
        SceneManager.LoadScene("Home");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}