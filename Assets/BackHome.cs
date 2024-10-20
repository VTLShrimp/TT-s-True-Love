using UnityEngine;

public class FirstSceneRespawnManager : MonoBehaviour
{
    public GameObject playerPrefab;  // The player object or prefab

    void Start()
    {
        // Find the player in the scene (assuming the player is already in the scene)
        GameObject player = GameObject.FindWithTag("Player");

        // Check if a respawn position was saved
        if (PlayerPrefs.HasKey("RespawnX") && PlayerPrefs.HasKey("RespawnY"))
        {
            // Get the respawn position from PlayerPrefs
            float respawnX = PlayerPrefs.GetFloat("RespawnX");
            float respawnY = PlayerPrefs.GetFloat("RespawnY");

            // Move the player to the respawn position
            player.transform.position = new Vector2(respawnX, respawnY);

            // Clear the saved respawn position
            PlayerPrefs.DeleteKey("RespawnX");
            PlayerPrefs.DeleteKey("RespawnY");
        }
        else
        {
            // Optionally: set a default spawn position if no respawn data is found
            Debug.Log("No respawn position found, using default spawn point.");
        }
    }
}