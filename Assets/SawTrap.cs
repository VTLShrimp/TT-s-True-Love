using UnityEngine;

public class SawTrap : MonoBehaviour
{
    public int damage = 10; // Damage dealt by the trap
    public float knockbackForce = 10f; // Knockback force

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered the trap's trigger.");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trap's trigger.");
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Calculate knockback direction
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;

                // Apply damage and knockback
                playerHealth.TakeDamage(damage);
                Debug.Log("Player took " + damage + " damage from the saw trap!");
            }
            else
            {
                Debug.Log("PlayerHealth component not found on the player.");
            }
        }
    }
}
