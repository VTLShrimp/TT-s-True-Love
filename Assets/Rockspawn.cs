using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject rockPrefab; // Prefab của đá
    public BoxCollider2D spawnArea; // BoxCollider2D xác định vùng spawn
    public float spawnInterval = 3f; // Khoảng thời gian giữa mỗi lần spawn
    public int numberOfRocks = 5; // Số lượng viên đá cần spawn mỗi lần

    private void Start()
    {
        // Kiểm tra xem prefab đã được gán chưa
        if (rockPrefab == null)
        {
            Debug.LogError("Rock prefab chưa được gán!");
            return;
        }

        // Bắt đầu spawn đá theo chu kỳ
        InvokeRepeating(nameof(SpawnRocks), 0f, spawnInterval);
    }

    private void SpawnRocks()
    {
        if (spawnArea == null)
        {
            Debug.LogError("Spawn area chưa được gán!");
            return;
        }

        // Lấy thông tin vùng spawn từ BoxCollider2D
        Bounds bounds = spawnArea.bounds;

        // Spawn nhiều viên đá cùng một lúc
        for (int i = 0; i < numberOfRocks; i++)
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            Vector2 spawnPosition = new Vector2(randomX, randomY);

            // Kiểm tra nếu vị trí nằm trong giới hạn của màn hình
            if (spawnPosition.x < bounds.min.x || spawnPosition.x > bounds.max.x ||
                spawnPosition.y < bounds.min.y || spawnPosition.y > bounds.max.y)
            {
                Debug.LogWarning("Vị trí spawn ngoài vùng cho phép: " + spawnPosition);
            }

            // Tạo viên đá tại vị trí ngẫu nhiên và giữ nguyên rotation từ prefab
            Instantiate(rockPrefab, spawnPosition, rockPrefab.transform.rotation);
            Debug.Log("Rock được spawn tại: " + spawnPosition + " với góc xoay: " + rockPrefab.transform.rotation.eulerAngles.z);
        }
    }


    private void OnDrawGizmos()
    {
        if (spawnArea != null)
        {
            // Vẽ vùng spawn trong Scene để dễ kiểm tra
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spawnArea.bounds.center, spawnArea.bounds.size);
        }
    }
}
