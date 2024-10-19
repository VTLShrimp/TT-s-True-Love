using UnityEngine;
using System.Collections;

public class CeilingTrap : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float damageInterval = 0.5f; // Time between damage applications
    [SerializeField] private float damageDuration = 2f;   // Total time the trap can deal damage (e.g., 2 seconds)

    [Header("Trap Timing")]
    [SerializeField] private float activationDelay = 1f; // Delay before trap starts
    [SerializeField] private float activeTime = 2f;      // Total length of the activated animation

    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered;  // When the trap is triggered
    private Coroutine damageCoroutine; // Reference to the damage coroutine
    private bool hasDamaged; // Flag to track if the player has been damaged

    private PlayerHealth player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerHealth>();

            if (!triggered)
            {
                StartCoroutine(ActivateFiretrap());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null; // Reset player reference when they leave
            StopDamage();  // Stop applying damage when the player exits
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        hasDamaged = false; // Reset the damage flag when activating the trap

        // Delay before the trap starts
        yield return new WaitForSeconds(activationDelay);
        anim.SetBool("activated", true);  // Start the activation animation

        // Start dealing damage to the player over time
        damageCoroutine = StartCoroutine(DealDamageOverTime());

        // Wait for the rest of the animation to finish
        yield return new WaitForSeconds(activeTime);

        // Reset the trap after the animation is complete
        ResetTrap();
    }

    private IEnumerator DealDamageOverTime()
    {
        float elapsedTime = 0f; // Track how long the trap has been dealing damage

        // Apply damage only once when the trap is activated
        if (player != null && !hasDamaged)
        {
            player.TakeDamage(damage);
            hasDamaged = true; // Set the flag to true after dealing damage
            Debug.Log("Player took damage: " + damage);
        }

        // Wait for the damage interval
        yield return new WaitForSeconds(damageInterval);

        // Continue applying damage until the total damage duration is reached
        elapsedTime += damageInterval;

        while (elapsedTime < damageDuration)
        {
            // After the first hit, we do not apply damage again
            yield return new WaitForSeconds(damageInterval); // Wait for the next interval
            elapsedTime += damageInterval; // Increase elapsed time
        }
    }

    private void StopDamage()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine); // Stop the damage coroutine if the player exits
            damageCoroutine = null;
        }
    }

    private void ResetTrap()
    {
        triggered = false;
        StopDamage(); // Ensure damage application is stopped when resetting
        anim.SetBool("activated", false); // Reset animation to idle
        spriteRend.color = Color.white;   // Reset color to normal
    }
}
