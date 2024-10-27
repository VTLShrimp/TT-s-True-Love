using System.Collections;
using UnityEngine;

public class EnemyPatrolAndAttack : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Transform[] waypoints;        // Waypoints to patrol between
    private int currentWaypointIndex = 0;

    [Header("Melee Attack Settings")]
    [SerializeField] float meleeAttackRange = 1.5f; // Range for melee attack
    [SerializeField] int meleeDamage = 10;          // Damage dealt by melee attack
    [SerializeField] float meleeAttackCooldown = 1f; // Time between melee attacks
    private float lastMeleeAttackTime;

    [Header("Bomb Throwing Settings")]
    [SerializeField] GameObject bombPrefab;          // Reference to the bomb prefab
    [SerializeField] float bombThrowRange = 5f;      // Range for throwing bombs
    [SerializeField] float bombThrowCooldown = 3f;    // Time between bomb throws
    private float lastBombThrowTime;

    [Header("Other")]
    [SerializeField] Transform player;                // Reference to the player
    private Rigidbody2D rb;
    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking)
        {
            // If attacking, do not patrol
            return;
        }

        Patrol();

        if (PlayerInRange(meleeAttackRange))
        {
            // Try to perform a melee attack
            if (Time.time - lastMeleeAttackTime >= meleeAttackCooldown)
            {
                StartCoroutine(MeleeAttack());
            }
        }
        else if (PlayerInRange(bombThrowRange))
        {
            // Try to throw a bomb
            if (Time.time - lastBombThrowTime >= bombThrowCooldown)
            {
                ThrowBomb();
            }
        }
    }

    void Patrol()
    {
        // Move toward the next waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector2 direction = (targetWaypoint.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        // Check if we’ve reached the waypoint
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Cycle through waypoints
        }

        // Flip the sprite depending on movement direction
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Facing right
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Facing left
        }
    }

    bool PlayerInRange(float range)
    {
        // Check if the player is within a specified range
        return Vector2.Distance(transform.position, player.position) <= range;
    }

    IEnumerator MeleeAttack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero; // Stop movement during attack
        animator.SetTrigger("Attack1");

        // Wait for attack animation to finish (adjust this to match your animation timing)
        yield return new WaitForSeconds(0.5f);

        if (PlayerInRange(meleeAttackRange))
        {
            // Deal damage to the player
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(meleeDamage);
                Debug.Log("Player hit by melee attack!");
            }
        }

        lastMeleeAttackTime = Time.time; // Reset melee attack cooldown
        isAttacking = false;
    }

    void ThrowBomb()
    {
        // Start throwing animation
        animator.SetTrigger("Attack2");

        // This assumes your throwing animation has a duration that matches the bomb throw timing
        StartCoroutine(ThrowBombCoroutine());
    }

    IEnumerator ThrowBombCoroutine()
    {
        // Wait for the animation to complete (adjust this duration based on your animation)
        yield return new WaitForSeconds(0.5f); // Adjust this to match your animation length

        // Instantiate and throw the bomb
        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);

        // Add logic to throw the bomb towards the player
        Vector2 throwDirection = (player.position - transform.position).normalized;
        bomb.GetComponent<Rigidbody2D>().velocity = throwDirection * 5f; // Set bomb velocity

        lastBombThrowTime = Time.time; // Reset bomb throw cooldown
        Debug.Log("Bomb thrown at player!");
    }

}
