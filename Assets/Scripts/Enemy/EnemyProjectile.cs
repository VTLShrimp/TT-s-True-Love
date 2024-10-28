using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed = 5f;          // Tốc độ của viên đạn
    [SerializeField] private float resetTime = 2f;      // Thời gian để reset đạn
    private float lifetime;                             // Thời gian tồn tại của đạn

    // Kích hoạt đạn và đặt lại thời gian sống
    public void ActivateProjectile()
    {
        lifetime = 0f;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        MoveProjectile();      // Di chuyển đạn
        CheckLifetime();       // Kiểm tra thời gian sống của đạn
    }

    // Hàm di chuyển đạn theo hướng và tốc độ đã định
    private void MoveProjectile()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
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

    // Khi va chạm, gọi logic từ lớp cha và vô hiệu hóa đạn
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);   // Thực thi logic từ lớp cha
        gameObject.SetActive(false);        // Vô hiệu hóa đạn sau va chạm
    }
}
