using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour, IHealth
{
    public int maxHealth = 100;
    private float currentHealth;
    public Animator animator;
    public bool dead;
    public float dieAnimationLength = 2.0f; // Set this to the length of your "die" animation
    public DetectionZone zone;
    private Vector2 startPosition;
    private  Rigidbody2D rb; // Adding 'new' keyword to hide inherited member
    private new Collider2D collider; // Adding 'new' keyword to hide inherited member

    void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        // Make sure the animator is assigned correctly
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on Boss!");
        }
    }

    public void ResetBoss()
    {
        transform.position = startPosition;
        currentHealth = maxHealth;
        collider.enabled = true;
        rb.velocity = Vector2.zero; // Reset velocity to ensure no movement
        rb.gravityScale = 1; // Restore gravity scale if altered
    }

    public void TakeDamage(int damage) // Implementing IHealth interface
    {
        if (dead) return; // If already dead, don't take damage again
        currentHealth -= damage;
        if (currentHealth > 0)
        {
            // Trigger the "hurt" animation if still alive
            animator.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                dead = true;
                animator.SetTrigger("die");
                collider.enabled = false;
                rb.velocity = Vector2.zero; // Stop any movement
                rb.gravityScale = 0; // Freeze the boss in place by setting gravity scale to zero
                // Destroy the DetectionZone when the boss dies
                if (zone != null)
                {
                    Destroy(zone.gameObject); // Destroy the entire zone GameObject if applicable
                }
                // Start the coroutine to disable the animator and destroy the boss
                StartCoroutine(DisableAnimatorAndDestroy());
            }
        }
    }

    private IEnumerator DisableAnimatorAndDestroy()
    {
        // Wait for the die animation to finish
        yield return new WaitForSeconds(dieAnimationLength);
        // Destroy the GameObject after the animation finishes
        Destroy(gameObject);
    }
}
