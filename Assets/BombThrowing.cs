using System.Collections;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Transform[] waypoints;
    private int currentWaypointIndex = 0;

    [Header("Melee Attack Settings")]
    [SerializeField] float meleeAttackRange = 1.5f;
    [SerializeField] int meleeDamage = 10;
    [SerializeField] float meleeAttackCooldown = 1f;
    private float lastMeleeAttackTime;

    [Header("Bomb Throwing Settings")]
    [SerializeField] GameObject bombPrefab;
    [SerializeField] Transform throwPoint;
    [SerializeField] float throwForce = 10f;
    [SerializeField] float bombThrowCooldown = 2f;
    [SerializeField] float bombThrowRange = 5f;
    private float lastBombThrowTime;

    [Header("Detection Settings")]
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float chaseRange = 7f;
    [SerializeField] float chaseTimeout = 2f;
    [SerializeField] Transform player;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isAttacking = false;
    private bool isThrowingBomb = false;
    private float lastPlayerSeenTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking) return;

        if (PlayerInRange(meleeAttackRange))
        {
            if (Time.time - lastMeleeAttackTime >= meleeAttackCooldown)
            {
                StartCoroutine(MeleeAttack());
            }
        }
        else if (PlayerInRange(bombThrowRange))
        {
            if (Time.time - lastBombThrowTime >= bombThrowCooldown && !isThrowingBomb)
            {
                StartCoroutine(ThrowBombCoroutine());
            }
        }
        else if (PlayerInRange(detectionRange) || Time.time - lastPlayerSeenTime <= chaseTimeout)
        {
            if (PlayerInRange(chaseRange))
            {
                ChasePlayer();
                lastPlayerSeenTime = Time.time;
            }
            else if (Time.time - lastPlayerSeenTime > chaseTimeout)
            {
                Patrol();
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector2 direction = (targetWaypoint.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        UpdateSpriteDirection(direction.x);
        animator.SetBool("isMoving", rb.velocity.x != 0);
    }

    bool PlayerInRange(float range)
    {
        return Vector2.Distance(transform.position, player.position) <= range;
    }

    IEnumerator MeleeAttack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f);

        if (PlayerInRange(meleeAttackRange))
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(meleeDamage);
                Debug.Log("Player hit by melee attack!");
            }
        }

        lastMeleeAttackTime = Time.time;
        isAttacking = false;
    }

    IEnumerator ThrowBombCoroutine()
    {
        isThrowingBomb = true;
        isAttacking = true;

        rb.velocity = Vector2.zero;
        animator.SetBool("isMoving", false);

        // Ensure the goblin faces the player before throwing the bomb
        FacePlayer();

        animator.SetTrigger("ThrowBomb");

        yield return new WaitForSeconds(0.5f); // Wait for the bomb throw animation to progress

        lastBombThrowTime = Time.time;
        isThrowingBomb = false;
        isAttacking = false;
    }

    public void ThrowBomb()
    {
        // Ensure the goblin is facing the player when the bomb is thrown
        FacePlayer();

        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, throwPoint.rotation);
        Rigidbody2D bombRb = bomb.GetComponent<Rigidbody2D>();

        if (bombRb != null)
        {
            Vector2 throwDirection = (player.position - throwPoint.position).normalized;
            bombRb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        }

        lastBombThrowTime = Time.time;
        Debug.Log("Bomb thrown at player!");
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        UpdateSpriteDirection(direction.x);
        animator.SetBool("isMoving", true);
    }

    void UpdateSpriteDirection(float horizontalInput)
    {
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Facing right
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Facing left
        }
    }

    void FacePlayer()
    {
        // Face the player based on the player's position relative to the goblin
        Vector3 playerDirection = player.position - transform.position;

        if (playerDirection.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Face right
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); // Face left
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, bombThrowRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
