using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f; // Sát thương của đạn
    public GameObject explosionPrefab; // Prefab vụ nổ

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if it collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Gây sát thương
                Debug.Log("Projectile gây sát thương: " + damage);
            }
        }

        // Check if it collided with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Projectile chạm đất và bị hủy.");
        }

        // Instantiate the explosion effect at the collision point
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // If the explosion prefab has a particle system, destroy it after the duration
            ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                Destroy(explosion, particleSystem.main.duration); // Destroy the explosion after the duration of the particle system
            }
            else
            {
                // If no particle system is found, destroy immediately after instantiation
                Destroy(explosion, 0.5f); // You can adjust the time here as needed
            }
        }

        // Destroy the projectile after the explosion effect is instantiated
        Destroy(gameObject); // Destroy the projectile on hit
    }
}
