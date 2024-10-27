using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    public LayerMask enemyLayers;
    public Animator animator;
    public bool isAttacking = false;
    public static PlayerAttack instance;
    public Transform attackPoint;
    public float attackRange = 0.5f;  // Tầm đánh trên mặt đất
    public float airAttackRange = 1.0f;  // Tầm đánh khi trên không
    public int groundDamage = 20;  // Sát thương khi trên mặt đất
    public int airDamage = 15;  // Sát thương khi trên không
    public float attackCooldown = 0.5f;  // Thời gian giữa các lần tấn công
    private float nextAttackTime = 0f;

    private PlayerMovement playerMovement;
    private Coroutine attackCoroutine; // Biến để lưu Coroutine của đòn tấn công

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();  // Lấy tham chiếu tới PlayerMovement
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

        if (playerMovement.IsGrounded())
        {
            animator.SetTrigger("GroundAttack");
            attackCoroutine = StartCoroutine(HandleGroundAttack());
        }
        else
        {
            animator.SetTrigger("AirAttack");
            attackCoroutine = StartCoroutine(HandleAirAttack());
        }
    }

    // Xử lý tấn công trên mặt đất
    private IEnumerator HandleGroundAttack()
    {
        yield return new WaitForSeconds(0.1f);  // Chờ đồng bộ với hoạt ảnh

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            IHealth enemyHealth = enemy.GetComponent<IHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(groundDamage);
                Debug.Log("Ground attack hit " + enemy.name);
            }
        }

        yield return new WaitForSeconds(0.3f);  // Chờ hoàn thành hành động tấn công
        isAttacking = false;
    }

    // Xử lý tấn công trên không
    private IEnumerator HandleAirAttack()
    {
        yield return new WaitForSeconds(0.1f);  // Chờ đồng bộ với hoạt ảnh

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, airAttackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            IHealth enemyHealth = enemy.GetComponent<IHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(airDamage);
                Debug.Log("Air attack hit " + enemy.name);
            }
        }

        yield return new WaitForSeconds(0.3f);  // Chờ hoàn thành hành động tấn công
        isAttacking = false;
    }

    // Hàm ngắt đòn tấn công
    public void InterruptAttack()
    {
        if (isAttacking)
        {
            isAttacking = false; // Đặt lại trạng thái
            animator.ResetTrigger("GroundAttack"); // Ngắt hoạt ảnh
            animator.ResetTrigger("AirAttack");

            // Nếu có coroutine đang chạy thì dừng nó
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }

            Debug.Log("Attack interrupted!");
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, airAttackRange);
    }
}
