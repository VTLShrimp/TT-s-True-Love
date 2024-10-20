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
    }

    void Update()
    {
        if (Time.time >= nextAttackTime && Input.GetMouseButtonDown(0) && !isAttacking)
        {
            PerformAttack();
            nextAttackTime = Time.time + attackCooldown;
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
        yield return new WaitForSeconds(0.1f); // Chờ để đồng bộ hóa với hoạt ảnh
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy != null)
            {
                IHealth enemyHealth = enemy.GetComponent<IHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                    Debug.Log("We attacked " + enemy.name);
                }
            }
        }
        yield return new WaitForSeconds(0.3f); // Chờ để hoàn thành hành động tấn công
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}
