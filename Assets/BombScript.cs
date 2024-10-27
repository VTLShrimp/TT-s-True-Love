using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float waitBeforeBeep = 1f;      // Time to wait before starting the explosion animation (beeping)
    [SerializeField] float explosionRadius = 2f;     // Radius of explosion damage
    [SerializeField] int damage = 50;                // Damage value
    [SerializeField] float explosionDuration = 3f;   // Length of the explosion animation (including beeping)

    private bool hasHitGround = false; // Track whether the bomb has hit the ground
    private Rigidbody2D rb;            // Reference to the Rigidbody2D component
    private Collider2D bombCollider;   // Reference to the bomb's Collider2D
    private Animator animator;         // Reference to the Animator component

    void Start()
    {
        animator = GetComponent<Animator>();    // Get the Animator component attached to the bomb
        rb = GetComponent<Rigidbody2D>();       // Get the Rigidbody2D component attached to the bomb
        bombCollider = GetComponent<Collider2D>(); // Get the Collider2D component of the bomb

        // Ignore collision with the player when the bomb is flying
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            Collider2D playerCollider = playerObject.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(bombCollider, playerCollider);
            }
            else
            {
                Debug.LogError("Player found, but no Collider2D attached!");
            }
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player is tagged as 'Player'.");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bomb hits the ground to start the process
        if (!hasHitGround && collision.collider.CompareTag("Ground")) // Ensure the ground has the correct tag
        {
            hasHitGround = true; // Bomb hit the ground
            Debug.Log("Bomb hit the ground! Waiting before starting beeping and explosion...");

            // Stop the bomb from moving by making its Rigidbody2D static (disable physics interaction)
            rb.velocity = Vector2.zero;
            rb.isKinematic = true; // Bomb will no longer be affected by physics or forces

            StartCoroutine(WaitBeforeBeeping());
        }
    }

    IEnumerator WaitBeforeBeeping()
    {
        // Wait for 1 second before triggering the explosion animation (with beeping)
        yield return new WaitForSeconds(waitBeforeBeep);
        Debug.Log("Wait complete! Starting explosion animation (with beeping)...");
        TriggerExplosion();
    }

    void TriggerExplosion()
    {
        // Start the explosion animation (which includes beeping)
        animator.SetTrigger("Explode");
        Debug.Log("Explosion animation triggered!");

        // Bomb will be destroyed after the animation completes
        Invoke(nameof(DestroyBomb), explosionDuration); // Destroy the bomb after the animation
    }

    // This method will be triggered by the animation event at the correct frame
    public void ApplyExplosionDamage()
    {
        Debug.Log("Applying explosion damage!");

        // Detect objects in the explosion radius and apply damage
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D obj in hitObjects)
        {
            // Apply damage to objects that have a health component
            PlayerHealth player = obj.GetComponent<PlayerHealth>();
            if (player != null)
            {
                Debug.Log("Player hit by explosion! Applying damage.");
                player.TakeDamage(damage);
            }
        }
    }

    void DestroyBomb()
    {
        // Destroy the bomb after the explosion animation completes
        Debug.Log("Explosion complete, destroying bomb.");
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the explosion radius in the Unity Editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
