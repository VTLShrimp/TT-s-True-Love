using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellingTrap : MonoBehaviour
{
    public float resetDelay = 2f; // Delay before the trap resets
    public int damage = 20; // Damage dealt by the trap

    private Animator animator;
    private bool isTriggered = false;
    private bool isResetting = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            // Apply damage to the player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Debug.Log("Trap triggered!");
            // Trigger the animation
            animator.SetTrigger("Activate");
            StartCoroutine(ResetTrap());
        }
    }

    IEnumerator ResetTrap()
    {
        isResetting = true;
        yield return new WaitForSeconds(resetDelay);
        isTriggered = false;
        isResetting = false;
    }
}