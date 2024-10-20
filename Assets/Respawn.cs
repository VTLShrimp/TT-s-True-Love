using UnityEngine;
using UnityEngine.SceneManagement;  // Required to load scenes

public class PlayerRespawn : MonoBehaviour
{
    public string firstSceneName = "FirstScene";  // Name of the first scene
    public Vector2 respawnPositionInFirstScene;   // Coordinates for respawn position in the first scene

    // Call this function when the player dies
    public void RespawnPlayer()
    {
        // Save the respawn position so it can be set when the first scene loads
        PlayerPrefs.SetFloat("RespawnX", respawnPositionInFirstScene.x);
        PlayerPrefs.SetFloat("RespawnY", respawnPositionInFirstScene.y);
        PlayerPrefs.Save();  // Save the data

        // Load the first scene
        SceneManager.LoadScene(firstSceneName);
    }
}