using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;

public class BossHealth : MonoBehaviour, IHealth
{
    public int maxHealth;
    private float currentHealth;
    public bool dead;

    [SerializeField] private SimpleFlash flash; // Reference to SimpleFlash script
    private Animator animator; // Reference to Animator
    private Collider2D bossCollider; // Reference to Collider

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource; // Audio source for playing sounds
    [SerializeField] private AudioClip damageSound;   // Sound for taking damage
    [SerializeField] private AudioClip deathSound;    // Sound for death

    void Start()
    {
        currentHealth = maxHealth;

        // Initialize SimpleFlash component if not assigned via the Inspector
        if (flash == null)
        {
            flash = GetComponent<SimpleFlash>();
            if (flash == null)
            {
                Debug.LogError("SimpleFlash not found on Boss!");
            }
        }

        // Get Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on Boss!");
        }

        // Get Collider component
        bossCollider = GetComponent<Collider2D>();
        if (bossCollider == null)
        {
            Debug.LogError("Collider2D not found on Boss!");
        }

        // Get AudioSource component
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource not found on Boss!");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        // Decrease health
        currentHealth -= damage;
        Debug.Log("Boss takes damage: " + damage + ". Current health: " + currentHealth);

        // Play damage sound
        if (damageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        // Flash when taking damage
        if (currentHealth > 0)
        {
            flash.Flash();
        }
        else
        {
            if (!dead)
            {
                dead = true;
                HandleDeath();
            }
        }
    }

    private void HandleDeath()
    {
        Debug.Log("Boss is dead!");

        // Play death sound
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        //// Disable collider to prevent interaction
        //if (bossCollider != null)
        //{
        //    bossCollider.enabled = false;
        //}
    }

    // Call this method using Animation Event at the end of death animation
    public void OnDeathAnimationEnd()
    {
        // Destroy the boss object after animation ends
        Destroy(gameObject);
    }
}