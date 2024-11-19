using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    public int maxHealth = 100; // Máu tối đa của kẻ thù
    private int currentHealth; // Máu hiện tại của kẻ thù
    public GameObject coinPrefab; // Prefab của đồng xu sẽ rớt khi kẻ thù chết
    public float spawnDelay = 0.1f;
    public int minCoins = 1;
    public int maxCoins = 3;

    void Start()
    {
        currentHealth = maxHealth; // Khởi tạo máu hiện tại bằng máu tối đa
    }

    // Hàm để kẻ thù nhận sát thương
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Trừ máu hiện tại bằng lượng sát thương nhận được
        if (currentHealth <= 0)
        {
            Die(); // Gọi hàm Die nếu máu hiện tại bằng hoặc nhỏ hơn 0
        }
    }

    // Hàm xử lý khi kẻ thù chết
    void Die()
    {
        // Thêm logic xử lý khi kẻ thù chết, ví dụ như phát hoạt ảnh chết, vô hiệu hóa kẻ thù, v.v.
        Debug.Log("Enemy died!");

        // Instantiate a random number of coins at the enemy's position
        StartCoroutine(SpawnCoinsWithDelay());

        // Xóa đối tượng kẻ thù khỏi game
    }
    private IEnumerator SpawnCoinsWithDelay()
    {
        if (coinPrefab != null)
        {
            int coinCount = Random.Range(minCoins, maxCoins + 1);
            for (int i = 0; i < coinCount; i++)
            {
                GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Apply a random force to the coin to make it "jump" in different directions
                    float randomX = Random.Range(-1f, 1f);
                    float randomY = Random.Range(1f, 2f);
                    rb.AddForce(new Vector2(randomX, randomY), ForceMode2D.Impulse);
                }
                yield return new WaitForSeconds(spawnDelay);
            }
        }
        Destroy(gameObject);
    }
}
