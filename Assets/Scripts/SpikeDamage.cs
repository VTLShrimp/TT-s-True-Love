using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    // Số lượng sát thương mà bẫy gai gây ra
    public int damageAmount;

    // Hàm xử lý va chạm khi nhân vật đi vào vùng gai
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem đối tượng va chạm có phải là nhân vật không (bằng tag "Player")
        if (collision.CompareTag("Player"))
        {
            // Lấy script điều khiển nhân vật, ví dụ là PlayerHealth, để trừ máu
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            // Nếu nhân vật có thành phần PlayerHealth, gây sát thương
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
