using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController instance;

    private void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent the camera object from being destroyed on scene load
        }
        else
        {
            Destroy(gameObject); // Destroy the new instance if one already exists
        }
    }
}