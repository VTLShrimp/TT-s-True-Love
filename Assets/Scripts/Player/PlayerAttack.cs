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
        // Wait for a moment before checking for collision (to sync with animation)
        yield return new WaitForSeconds(0.1f);

        // Get all enemies in the attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy == null)
            {
                Debug.LogError("Invalid enemy hit");
                continue; // Skip this iteration if the enemy is null
            }

            // Check if the enemy has a BossHealth component
            BossHealth bossHealth = enemy.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                // Apply damage if the BossHealth component exists
                bossHealth.TakeDamage(damage);
                Debug.Log("We attacked " + enemy.name);
            }
            else
            {
                Debug.LogWarning(enemy.name + " does not have a BossHealth component");
            }
        }

        // Reset attack trigger to avoid re-triggering
        animator.ResetTrigger("Attack");

        // Wait for a short duration to finish attack action before allowing another attack
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}
