using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask enemyLayers;
    public Animator animator;
    public bool isAttacking = false;
    public static PlayerAttack instance;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int damage = 20;
    public float attackCooldown = 0.5f; // Thời gian giữa các lần tấn công
    private float nextAttackTime = 0f;

    private void Awake()
    {
        instance = this;

        // Kiểm tra xem attackPoint có được gán không
        if (attackPoint == null)
        {
            Debug.LogError("AttackPoint chưa được gán!");
        }

        // Kiểm tra xem animator có được gán không
        if (animator == null)
        {
            Debug.LogError("Animator chưa được gán!");
        }
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0) && !isAttacking) // Nhấn chuột trái
            {
                PerformAttack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    private void PerformAttack()
    {
        isAttacking = true;

        // Kích hoạt hoạt ảnh tấn công
        animator.SetTrigger("Attack");

        // Bắt đầu kiểm tra va chạm với kẻ địch
        StartCoroutine(HandleAttack());
    }


    private IEnumerator HandleAttack()
    {
        // Đợi một chút trước khi thực hiện kiểm tra va chạm (phù hợp với thời gian hoạt ảnh)
        yield return new WaitForSeconds(0.1f);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy == null)
            {
                Debug.LogError("Kẻ địch bị trúng không hợp lệ");
                continue; // Bỏ qua vòng lặp nếu kẻ địch là null
            }

            BossHealth bossHealth = enemy.GetComponent<BossHealth>();
            if (bossHealth == null)
            {
                Debug.LogError($"Không tìm thấy BossHealth trên {enemy.name}");
                continue; // Bỏ qua vòng lặp nếu không tìm thấy BossHealth
            }

            bossHealth.TakeDamage(damage);
            Debug.Log("Chúng ta đã tấn công " + enemy.name);
        }

        // Reset trigger để ngăn chặn kích hoạt lại
        animator.ResetTrigger("Attack");

        // Đợi để kết thúc hành động tấn công và cho phép tiếp tục tấn công
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}
