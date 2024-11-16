using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button optionsButton;
    public void OnNewGameClick()
    {
        DisableButtons();
        Debug.Log("New Game Clicked");
        DataPresistenceManager.Instance.NewGame();
        SceneManager.LoadSceneAsync("Home");
    }
    public void OnLoadGameClick()
    {
        DisableButtons();
        SceneManager.LoadSceneAsync("Home");
    }
    public void OnOptionsClick()
    {
        DisableButtons();
        Debug.Log("Options Clicked");
    }
    public void OnQuitClick()
    {
        Application.Quit();
    }
    private void DisableButtons()
    {
        newGameButton.interactable = false;
        loadGameButton.interactable = false;
        optionsButton.interactable = false;
    }
}
