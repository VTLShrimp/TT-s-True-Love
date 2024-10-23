using System.Collections;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Transform[] waypoints; // Waypoints to patrol between
    private int currentWaypointIndex = 0;

    [Header("Melee Attack Settings")]
    [SerializeField] float meleeAttackRange = 1.5f; // Range for melee attack
    [SerializeField] int meleeDamage = 10;          // Damage dealt by melee attack
    [SerializeField] float meleeAttackCooldown = 1f; // Time between melee attacks
    private float lastMeleeAttackTime;

    [Header("Bomb Throwing Settings")]
    [SerializeField] GameObject bombPrefab;          // Reference to the bomb prefab
    [SerializeField] Transform throwPoint;           // Point where the bomb will be thrown from
    [SerializeField] float throwForce = 10f;         // Force applied to throw the bomb
    [SerializeField] float throwInterval = 3f;       // Time between bomb throws
    [SerializeField] float bombThrowRange = 5f;      // Range for throwing bombs
    private float lastBombThrowTime;

    [Header("Detection Settings")]
    [SerializeField] float detectionRange = 5f;      // Range to detect player
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
        // If attacking, do not patrol
        if (isAttacking)
        {
            return;
        }

        Patrol();

        // Check for player in range for melee attack
        if (PlayerInRange(meleeAttackRange))
        {
            if (Time.time - lastMeleeAttackTime >= meleeAttackCooldown)
            {
                StartCoroutine(MeleeAttack());
            }
        }
        // Check for player in range for bomb throwing
        else if (PlayerInRange(bombThrowRange))
        {
            // Only throw a bomb if the cooldown period has passed
            if (Time.time - lastBombThrowTime >= throwInterval)
            {
                StartCoroutine(ThrowBomb());
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
            animator.SetBool("isMoving", true); // Trigger moving animation
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Facing left
            animator.SetBool("isMoving", true); // Trigger moving animation
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop movement when idle
            animator.SetBool("isMoving", false); // Stop moving animation
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
        animator.SetTrigger("Attack"); // Trigger melee attack animation

        // Wait for attack animation to finish (adjust this to match your animation timing)
        yield return new WaitForSeconds(0.5f); // Adjust based on your melee attack animation length

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
        isAttacking = false; // Allow the enemy to patrol again
    }

    IEnumerator ThrowBomb()
    {
        isAttacking = true; // Set the attacking state to true
        rb.velocity = Vector2.zero; // Stop movement during bomb throw
        animator.SetTrigger("ThrowBomb"); // Trigger bomb throw animation

        // Wait for the animation to complete (adjust based on your animation length)
        yield return new WaitForSeconds(0.5f); // Adjust this duration based on your animation length

        // Instantiate the bomb at the throw point
        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, throwPoint.rotation);

        // Get Rigidbody2D component to apply force
        Rigidbody2D bombRb = bomb.GetComponent<Rigidbody2D>();
        if (bombRb != null)
        {
            // Calculate direction toward the player and apply force to throw the bomb
            Vector2 throwDirection = (player.position - throwPoint.position).normalized;
            bombRb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        }

        lastBombThrowTime = Time.time; // Update the last throw time
        isAttacking = false; // Allow the enemy to patrol again
        Debug.Log("Bomb thrown at player!");
    }
}
