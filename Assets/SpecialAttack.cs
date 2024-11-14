using UnityEngine;
using System.Collections;
public class BossAttack : MonoBehaviour
{
    public Transform player; // Vị trí của người chơi
    public float attackRange = 10f; // Phạm vi tấn công của boss
    public float jumpDistance = 5f; // Khoảng cách nhảy của boss
    public float attackDamage = 20f; // Sát thương của boss
    public float attackCooldown = 5f; // Thời gian chờ giữa các lần tấn công

    private Rigidbody2D rb;
    private float nextAttackTime = 0f;
    private enum BossState { Idle, Attacking }
    private BossState currentState = BossState.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Time.time >= nextAttackTime && currentState == BossState.Idle)
        {
            if (ShouldJumpAttack())
            {
                StartCoroutine(JumpAttack());
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    bool ShouldJumpAttack()
    {
        // Sử dụng giá trị ngẫu nhiên để quyết định liệu boss có nên nhảy tấn công không
        float randomValue = Random.Range(0f, 1f);
        return randomValue > 0.5f; // 50% cơ hội tấn công
    }

    IEnumerator JumpAttack()
    {
        currentState = BossState.Attacking;

        for (int i = 0; i < 3; i++)
        {
            // Tính toán hướng và khoảng cách tới người chơi
            Vector2 direction = (player.position - transform.position).normalized;

            // Tạo một lực để nhảy tới người chơi
            rb.AddForce(direction * jumpDistance, ForceMode2D.Impulse);

            // Chờ một khoảng thời gian cho đến khi nhảy tiếp theo
            yield return new WaitForSeconds(0.5f);
        }

        // Sau khi hoàn tất 3 lần nhảy, quay lại trạng thái ban đầu
        currentState = BossState.Idle;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Gây sát thương cho người chơi
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }
}
