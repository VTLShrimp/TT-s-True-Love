using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
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

    private bool canAttack = true; // Track if the player can attack

    public GameObject swordWavePrefab; // Prefab kiếm khí
    public Transform spawnPoint;       // Điểm xuất phát của kiếm khí
    public int swordWaveDamage = 20;   // Sát thương của kiếm khí

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
        if (Time.time >= nextAttackTime && Input.GetMouseButtonDown(0) && canAttack && !isAttacking)
        {
            PerformAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
        if (Input.GetMouseButtonDown(1))  // 1 là chuột phải
        {
            LaunchSwordWave();
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
    private void LaunchSwordWave()
    {

        // Tạo kiếm khí từ prefab tại vị trí spawnPoint
        GameObject swordWave = Instantiate(swordWavePrefab, spawnPoint.position, Quaternion.identity);

        // Lấy component SwordWave từ kiếm khí đã tạo
        SwordWave wave = swordWave.GetComponent<SwordWave>();

        // Thiết lập sát thương cho kiếm khí
        wave.damage = swordWaveDamage;

        // Xác định hướng di chuyển của kiếm khí dựa vào hướng của nhân vật
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        wave.SetDirection(direction); // Gọi hàm SetDirection để kiếm khí bay theo hướng của nhân vật
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

    // Method to disable attacking temporarily
    public void DisableAttacking(float duration)
    {
        canAttack = false; // Disable attacking
        StartCoroutine(EnableAttackingAfterDelay(duration));
    }

    // Coroutine to re-enable attacking after a delay
    private IEnumerator EnableAttackingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canAttack = true; // Re-enable attacking
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
