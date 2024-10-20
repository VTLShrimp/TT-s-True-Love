using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public int damageAmount; // Damage amount dealt by the spike trap
    public float knockbackForce = 10f; // Knockback force

    // Handle collision when the character enters the spike trap area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player (by tag "Player")
        if (collision.CompareTag("Player"))
        {
            // Get the PlayerHealth script to apply damage
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Calculate knockback direction
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;

                // Apply damage and knockback
                playerHealth.TakeDamage(damageAmount, knockbackDirection);
            }
        }
    }
}
