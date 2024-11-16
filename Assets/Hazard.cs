using UnityEngine;

public class Hazard : MonoBehaviour
{
    public int damage = 20;  // Damage value for the boss's hazard

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other.gameObject);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        CheckCollision(other.gameObject);
    }

    private void CheckCollision(GameObject collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Assuming the Player has a script named PlayerHealth or similar
            var player = collider.GetComponent<PlayerHealth>();  // Get PlayerHealth component

            if (player == null) return;  // Early exit if no PlayerHealth component is found

            // Apply recoil force to the player (to knock them back)
            var recoilDirection = (collider.transform.position - transform.position).normalized;
            float multiplier = recoilDirection.y < 0 ? 1.0f : 500.0f;
            Vector2 recoilForce = recoilDirection * multiplier;

            // Apply damage and knockback to the player
            player.TakeDamage(damage); // Call TakeDamage to apply damage (recoil handled separately)
            ApplyKnockback(recoilForce, player); // Apply knockback separately
        }
    }

    // Method to apply knockback to the player
    private void ApplyKnockback(Vector2 recoilForce, PlayerHealth player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>(); // Assuming the player has a Rigidbody2D component
        if (rb != null)
        {
            rb.AddForce(recoilForce, ForceMode2D.Impulse);  // Apply the recoil force to the player
        }
    }
}
