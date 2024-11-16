using UnityEngine;

public class BossDamageZone : MonoBehaviour
{
    public float damage = 25f; // Lượng sát thương mà boss gây ra

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Vào vùng chạm: " + collision.name); // Kiểm tra xem vùng chạm có hoạt động không

        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Gây sát thương: " + damage);
            }
            else
            {
                Debug.Log("Không tìm thấy PlayerHealth trên đối tượng Player.");
            }
        }
        else
        {
            Debug.Log("Đối tượng không phải Player.");
        }
    }
}
