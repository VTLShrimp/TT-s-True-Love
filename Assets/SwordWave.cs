using UnityEngine;

public class SwordWave : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 20;
    public float duration = 2f; // Time the sword wave exists after being launched
    public Vector2 direction = Vector2.right; // Direction of the sword wave
    private bool isFlipped = false;
    private void Start()
    {
        // Destroy the sword wave after the specified duration
        Invoke("DestroySwordWave", duration);
    }


    private void Update()
    {
        // Move the sword wave in the specified direction
        transform.Translate(direction * speed * Time.deltaTime);

        // Flip the sword wave based on the player's facing direction
        if (direction.x < 0 && !isFlipped)
        {
            Flip();
        }
        else if (direction.x > 0 && isFlipped)
        {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("SwordWave collided with: " + collision.gameObject.name); // See which object collided
        if (collision.CompareTag("Enemy"))
        {
            IHealth enemyHealth = collision.GetComponent<IHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // Deal damage to the enemy
                Destroy(gameObject); // Destroy the sword wave after collision
            }
        }
        else if (collision.CompareTag("Boss"))
        {
            BossHealth bossHealth = collision.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage); // Deal damage to the boss
                Destroy(gameObject); // Destroy the sword wave after collision
            }
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); // Destroy sword wave if it hits a wall
        }
    }
    private void Flip()
    {
        // Flip the sprite's scale (scale.x is multiplied by -1 to flip the object horizontally)
        isFlipped = !isFlipped;
        Vector3 localScale = transform.localScale;
        localScale.x = -localScale.x;
        transform.localScale = localScale;
    }
    // Function to set the movement direction of the sword wave
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    // Function to destroy the sword wave
    private void DestroySwordWave()
    {
        Destroy(gameObject);
    }
}
