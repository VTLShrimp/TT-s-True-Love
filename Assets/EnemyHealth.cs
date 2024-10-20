using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    public int maxHealth = 100; // Máu tối đa của kẻ thù
    private int currentHealth; // Máu hiện tại của kẻ thù

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
        Destroy(gameObject); // Xóa đối tượng kẻ thù khỏi game
    }
}
