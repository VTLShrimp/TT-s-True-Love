using System.Collections;
using UnityEngine;

public class LightningTrap2D : MonoBehaviour
{
    public float activationDelay = 5.0f;  // Time between trap activations
    public Animator animator;  // Reference to the trap's animator
    public int damage = 20;  // Damage amount

    private Collider2D playerCollider;  // To track the player in the trap's trigger

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

    // This method will be triggered by the animation event
    public void TriggerDamage()
    {
        // Check if the player is inside the collider when the trap is activated
        if (playerCollider != null)
        {
            playerCollider.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }
}
