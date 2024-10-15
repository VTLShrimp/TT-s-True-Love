using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private float currentHealth;
    public Animator animator;
    public bool dead;

    // Add a reference to the die animation length
    public float dieAnimationLength = 2.0f; // Set this to the actual length of your "die" animation

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth > 0)
        {
            animator.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                dead = true;
                animator.SetTrigger("die");
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
