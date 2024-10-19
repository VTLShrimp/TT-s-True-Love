using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private float currentHealth;
    public Animator animator;
    public bool dead;
    public float dieAnimationLength = 2.0f; // Set this to the length of your "die" animation
    public DetectionZone zone;

    void Start()
    {
        currentHealth = maxHealth;

        // Make sure the animator is assigned correctly
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on Boss!");
        }
    }

    public void TakeDamage(float damage)
    {
        if (dead)
            return; // If already dead, don't take damage again

        currentHealth -= damage;

        if (currentHealth > 0)
        {
            // Trigger the "hurt" animation if still alive
            animator.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                dead = true;
                animator.SetTrigger("die");

                // Destroy the DetectionZone when the boss dies
                if (zone != null)
                {
                    Destroy(zone.gameObject); // Destroy the entire zone GameObject if applicable
                }

                // Start the coroutine to disable the animator and destroy the boss
                StartCoroutine(DisableAnimatorAndDestroy());
            }
        }
    }

    private IEnumerator DisableAnimatorAndDestroy()
    {
        // Wait for the die animation to finish
        yield return new WaitForSeconds(dieAnimationLength);

        // Destroy the GameObject after the animation finishes
        Destroy(gameObject);
    }
}
