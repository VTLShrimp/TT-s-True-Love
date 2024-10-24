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
    [SerializeField] float bombThrowCooldown = 2f;   // Cooldown time between bomb throws
    private float lastBombThrowTime;                 // Last time a bomb was thrown

    [Header("Detection Settings")]
    [SerializeField] float detectionRange = 5f;      // Range to detect player
    [SerializeField] float bombThrowRange = 5f;      // Range for throwing bombs
    [SerializeField] Transform player;                // Reference to the player

    private Rigidbody2D rb;
    private Animator animator;
    private bool isAttacking = false;
    private bool isThrowingBomb = false; // Flag to track if the Goblin is throwing a bomb

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking) return; // If attacking, skip all actions

        if (PlayerInRange(meleeAttackRange))
        {
            if (Time.time - lastMeleeAttackTime >= meleeAttackCooldown)
            {
                StartCoroutine(MeleeAttack());
            }
        }
        else if (PlayerInRange(bombThrowRange))
        {
            if (Time.time - lastBombThrowTime >= bombThrowCooldown && !isThrowingBomb) // Check the flag
            {
                StartCoroutine(ThrowBombCoroutine());
            }
        }
        else if (PlayerInRange(detectionRange))
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
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
        UpdateSpriteDirection(direction.x);
        animator.SetBool("isMoving", rb.velocity.x != 0); // Trigger moving animation
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

    IEnumerator ThrowBombCoroutine()
    {
        isThrowingBomb = true; // Set flag to indicate a bomb is being thrown
        isAttacking = true; // Prevent patrol

        rb.velocity = Vector2.zero; // Stop movement during bomb throw
        animator.SetBool("isMoving", false); // Ensure the moving animation is stopped
        animator.SetTrigger("ThrowBomb"); // Trigger bomb throw animation

        // Wait for a portion of the animation, if needed
        yield return new WaitForSeconds(0.5f); // Adjust based on timing of the throwing animation

        // This is optional if you are using an animation event to call ThrowBomb
        // ThrowBomb(); // Call the bomb throwing function here if not using animation event

        lastBombThrowTime = Time.time; // Update the last throw time
        isThrowingBomb = false; // Reset the flag
        isAttacking = false; // Allow the enemy to patrol again
    }

    public void ThrowBomb()
    {
        // This function can now be called via an animation event.
        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, throwPoint.rotation);

        Rigidbody2D bombRb = bomb.GetComponent<Rigidbody2D>();
        if (bombRb != null)
        {
            // Calculate direction toward the player and apply force to throw the bomb
            Vector2 throwDirection = (player.position - throwPoint.position).normalized;
            bombRb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        }

        lastBombThrowTime = Time.time; // Update the last throw time
        Debug.Log("Bomb thrown at player!");
    }

    void ChasePlayer()
    {
        // Move towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        UpdateSpriteDirection(direction.x); // Update sprite direction based on chase
        animator.SetBool("isMoving", true); // Trigger moving animation
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
}
