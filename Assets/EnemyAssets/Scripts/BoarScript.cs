using UnityEngine;

public class BoarScript : MonoBehaviour
{
    public float speed = 5f;
    public Transform PointA;
    public Transform PointB;
    private Transform currentTarget;
    private Rigidbody2D rb;
    private Animator animator;

    // Health variable
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentTarget = PointB;
        animator.SetBool("IsRunning", true);

        // Initialize health
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Calculate direction towards the current target
        Vector2 direction = (currentTarget.position - transform.position).normalized;

        // Set the velocity to move towards the target
        rb.velocity = direction * speed;

        // Check the distance to the current target
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            // Switch target
            currentTarget = currentTarget == PointA ? PointB : PointA;
        }
    }

    // Method to handle taking damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle death
    private void Die()
    {
        // animator.SetTrigger("Die");
        rb.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}