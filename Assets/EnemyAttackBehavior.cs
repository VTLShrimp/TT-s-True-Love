using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float attackRange = 1.5f; // Tầm đánh
    [SerializeField] int attackDamage = 10; // Sát thương
    [SerializeField] float attackInterval = 1.5f; // Thời gian giữa các lần tấn công
    [SerializeField] float chargeSpeed = 5f; // Tốc độ khi tấn công lao vào
    [SerializeField] float retreatDistance = 2f; // Khoảng cách lùi lại sau khi tấn công
    [SerializeField] LayerMask playerLayer; // Lớp để phát hiện người chơi

    private float nextAttackTime;
    private GameObject player;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isCharging = false;
    private bool isRetreating = false;
    private Vector2 originalPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player != null && Time.time >= nextAttackTime)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (!isCharging && !isRetreating && distanceToPlayer <= attackRange)
            {
                Charge();
            }
        }
    }

    private void Charge()
    {
        isCharging = true;
        originalPosition = transform.position;

        // Tăng tốc độ tạm thời và di chuyển về phía người chơi
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * chargeSpeed;

        // Kích hoạt hoạt ảnh tấn công
        animator.SetTrigger("Attack");

        // Kiểm tra va chạm với người chơi sau một khoảng thời gian ngắn
        StartCoroutine(CheckForPlayerCollision(0.2f)); // Thời gian để kiểm tra va chạm
    }

    private IEnumerator CheckForPlayerCollision(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Kiểm tra va chạm với người chơi
        Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(transform.position + new Vector3(transform.localScale.x * attackRange / 2, 0),
                                                          new Vector2(attackRange, transform.localScale.y),
                                                          0,
                                                          playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Enemy attacked the player!");
            }
        }

        // Bắt đầu lùi lại sau khi tấn công
        Retreat();
    }

    private void Retreat()
    {
        isCharging = false;
        isRetreating = true;

        Vector2 direction = (originalPosition - rb.position).normalized;
        rb.velocity = direction * chargeSpeed;

        // Dừng lại sau khi lùi về vị trí ban đầu
        StartCoroutine(StopRetreat(0.5f)); // Thời gian để lùi lại
    }

    private IEnumerator StopRetreat(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector2.zero;
        isRetreating = false;
        nextAttackTime = Time.time + attackInterval;
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ hình vuông tấn công trong trình chỉnh sửa để trực quan hóa
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(transform.localScale.x * attackRange / 2, 0),
                            new Vector3(attackRange, transform.localScale.y));
    }
}
