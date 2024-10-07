using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Points")]
    public GameObject pointA;
    public GameObject pointB;

    [Header("Movement Settings")]
    public float moveSpeed;
    public float pointReachThreshold;

    [Header("Health Settings")]
    public int maxHealth = 100;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform currentTarget;
    private int currentHealth;
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentTarget = pointB.transform;
        currentHealth = maxHealth;

        // Ensure the enemy starts moving.
        animator.SetBool("isRunning", true);
    }

    void FixedUpdate()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        // Calculate the movement direction and move only on the X-axis
        Vector2 direction = (currentTarget.position - transform.position).normalized;

        // Set velocity in the direction of the target, ensure only x direction is applied
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        // Check if the enemy is close enough to the target point
        if (Vector2.Distance(transform.position, currentTarget.position) <= pointReachThreshold)
        {
            SwitchTarget();
        }
    }

    void SwitchTarget()
    {
        // Flip the target
        if (currentTarget == pointB.transform)
        {
            currentTarget = pointA.transform;
        }
        else
        {
            currentTarget = pointB.transform;
        }

        // Flip the sprite direction when switching target
        Flip();
    }



    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Stop all movement and disable the object
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Die");
        gameObject.SetActive(false);
    }

    private void Flip()
    {
        // Flip the sprite direction by inverting the local scale on the X-axis
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Flip the X scale to change direction
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        // Visualize the points and path in the editor
        if (pointA && pointB)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pointA.transform.position, pointReachThreshold);
            Gizmos.DrawWireSphere(pointB.transform.position, pointReachThreshold);
            Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        }
    }
}
