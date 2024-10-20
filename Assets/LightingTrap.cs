using System.Collections;
using UnityEngine;

public class LightningTrap2D : MonoBehaviour
{
    public float activationDelay = 5.0f;  // Time between trap activations
    public Animator animator;  // Reference to the trap's animator
    public int damage = 20;  // Damage amount
    public float damageInterval = 0.5f;  // Interval between damage ticks (if player stays in trap)

    private Collider2D playerCollider;  // To track the player in the trap's trigger
    private bool isDealingDamage = false;  // To track if the trap should be dealing damage
    private Coroutine damageCoroutine;  // To handle repeated damage

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Start the trap cycle with a delay
        StartCoroutine(TrapCycle());
    }

    IEnumerator TrapCycle()
    {
        while (true)  // Repeat the trap activation indefinitely
        {
            yield return new WaitForSeconds(activationDelay);  // Wait before activating the trap
            animator.SetTrigger("Activate");  // Trigger the animation to play the charge + activate phases
        }
    }

    // This function gets called when the player enters the trap's trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerCollider = collision;  // Save reference to the player when they enter the collider
        }
    }

    // This function gets called when the player exits the trap's trigger collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerCollider = null;  // Clear the reference when the player leaves the collider
        }
    }

    // Animation Event: Start dealing continuous damage
    public void StartDamage()
    {
        if (!isDealingDamage && playerCollider != null)
        {
            isDealingDamage = true;
            damageCoroutine = StartCoroutine(DealDamageOverTime());
        }
    }

    // Animation Event: Stop dealing damage
    public void StopDamage()
    {
        if (isDealingDamage)
        {
            isDealingDamage = false;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
        }
    }

    // Coroutine to deal damage over time during the activation phase
    IEnumerator DealDamageOverTime()
    {
        while (isDealingDamage)
        {
            if (playerCollider != null)  // Ensure player is still in the trigger
            {
                playerCollider.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            }
            yield return new WaitForSeconds(damageInterval);  // Wait before applying damage again
        }
    }
}
