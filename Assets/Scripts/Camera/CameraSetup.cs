using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CameraSetup : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignPlayerToCamera();
    }

    private void AssignPlayerToCamera()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null && virtualCamera != null)
        {
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform;
            Debug.Log("Player assigned to Virtual Camera");
        }
        else
        {
            Debug.LogWarning("Player or Virtual Camera not found in scene.");
        }
    }
}
