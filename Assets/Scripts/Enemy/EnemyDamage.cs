using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float knockbackForce = 10f; // Knockback force

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Calculate knockback direction
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;

                // Apply damage and knockback
                playerHealth.TakeDamage(damage, knockbackDirection);
            }
        }
    }
}
