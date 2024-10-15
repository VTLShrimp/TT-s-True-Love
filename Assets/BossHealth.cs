using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int health = 500;
    public GameObject deathEffect; // Gán prefab hiệu ứng chết
    public bool isInvulnerable = false;

    private void Awake()
    {
        // Kiểm tra xem deathEffect có được gán không
        if (deathEffect == null)
        {
            Debug.LogError("deathEffect chưa được gán!");
        }
    }

    public bool isDead = false;

    public void TakeDamage(int damage)
    {
        if (isInvulnerable || isDead)
            return;

        health -= damage;

        Debug.Log("Boss nhận sát thương. Máu còn lại: " + health); // Kiểm tra sát thương

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true; // Đánh dấu boss là đã chết
        isInvulnerable = true; // Đặt boss thành miễn dịch khi chết
        Debug.Log("Boss đã chết!");
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity); // Tạo hiệu ứng chết
        Destroy(effect, 1.5f); // Xóa hiệu ứng chết sau 2 giây
        Destroy(gameObject); // Xóa đối tượng boss
    }
}