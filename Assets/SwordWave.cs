using UnityEngine;

public class SwordWave : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 20;
    public float duration = 2f; // Thời gian tồn tại của kiếm khí sau khi phóng ra
    public Vector2 direction = Vector2.right; // Hướng di chuyển của kiếm khí

    private void Start()
    {
        // Hủy đối tượng sau khoảng thời gian tồn tại
        Invoke("DestroySwordWave", duration);
    }

    private void Update()
    {
        // Di chuyển kiếm khí theo hướng đã chỉ định
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    Debug.Log("SwordWave collided with: " + collision.gameObject.name); // Xem đối tượng va chạm
    if (collision.CompareTag("Enemy"))
    {
        IHealth enemyHealth = collision.GetComponent<IHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject); // Hủy kiếm khí sau khi va chạm
        }
    }
    else if (collision.CompareTag("Wall"))
    {
        Destroy(gameObject);
    }
}


    // Hàm để thiết lập hướng di chuyển cho kiếm khí
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    // Hàm để hủy đối tượng
    private void DestroySwordWave()
    {
        Destroy(gameObject);
    }
}
