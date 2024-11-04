using UnityEngine;

public class VirtualCameraController : MonoBehaviour
{
    private static VirtualCameraController instance;

    private void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent the Virtual Camera object from being destroyed on scene load
        }
        else
        {
            Destroy(gameObject); // Destroy the new instance if one already exists
        }
    }
}