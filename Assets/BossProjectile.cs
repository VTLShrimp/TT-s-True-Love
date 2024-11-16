using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;          // Tốc độ của viên đạn
    [SerializeField] private float resetTime = 2f;      // Thời gian để reset đạn
    [SerializeField] private int damage = 10;           // Sát thương gây ra
    private float lifetime;                             // Thời gian tồn tại của đạn

    private Vector2 direction;                          // Hướng di chuyển của đạn

    // Kích hoạt đạn và đặt lại thời gian sống
    public void ActivateProjectile(Transform player)
    {
        lifetime = 0f;
        gameObject.SetActive(true);

        // Xác định hướng di chuyển theo vị trí của người chơi
        // Nếu người chơi đang nhìn sang phải, đạn sẽ bay sang phải; nếu nhìn sang trái, đạn sẽ bay sang trái
        direction = new Vector2(player.localScale.x, 0).normalized; // Chỉ theo chiều X
    }

    private void Update()
    {
        MoveProjectile();      // Di chuyển đạn
        CheckLifetime();       // Kiểm tra thời gian sống của đạn
    }

    // Hàm di chuyển đạn theo hướng và tốc độ đã định
    private void MoveProjectile()
    {
        transform.Translate(direction.x * speed * Time.deltaTime, 0, 0);  // Di chuyển chỉ theo trục X
    }

    // Kiểm tra nếu đạn đã vượt qua thời gian sống, sẽ reset lại
    private void CheckLifetime()
    {
        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        {
            gameObject.SetActive(false);
        }
    }

    // Xử lý va chạm với các đối tượng
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu đạn va chạm với Player (thêm điều kiện nếu cần)
        if (collision.CompareTag("Player"))
        {
            // Gọi phương thức TakeDamage trên PlayerHealth
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Đạn không bị xóa khi xuyên qua các đối tượng
            // Không cần gọi gameObject.SetActive(false) nếu bạn muốn đạn tiếp tục bay xuyên qua
        }
    }
}
