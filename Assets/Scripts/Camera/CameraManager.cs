using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; } // Singleton instance

    private Transform playerTransform;

    private void Awake()
    {
        // Kiểm tra nếu đã có một Instance tồn tại
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Hủy đối tượng nếu đã tồn tại một Instance khác
            return;
        }

        // Gán Instance và không hủy đối tượng khi chuyển scene
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindPlayer();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        // Tìm đối tượng Player trong scene
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("Player found!");
        }
        else
        {
            Debug.LogWarning("Player not found in scene!");
        }
    }

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            // Cập nhật vị trí camera để theo dõi Player (chỉ cập nhật trục X và Y cho game 2D)
            Vector3 targetPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f); // Làm mượt với Lerp
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
