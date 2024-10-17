using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchTrap : MonoBehaviour
{
    public float punchInterval = 2f; // Time between punches
    public int punchDamage = 10; // Damage dealt by the punch
    public float punchForce = 5f; // Force of the punch
    public Transform punchPoint; // Point where the punch originates
    public float punchRange = 1f; // Range of the punch
    public LayerMask playerLayer; // Layer of the player

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(PunchRoutine());
    }

    IEnumerator PunchRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(punchInterval);
            Punch();
        }
    }

    void Punch()
    {
        // Play punch animation
        if (animator != null)
        {
            animator.SetTrigger("Punch");
        }

        // Detect player in range
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(punchPoint.position, punchRange, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            // Apply damage to the player
            player.GetComponent<PlayerHealth>().TakeDamage(punchDamage);

            // Apply force to the player
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = player.transform.position - punchPoint.position;
                rb.AddForce(direction.normalized * punchForce, ForceMode2D.Impulse);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (punchPoint == null)
            return;

        Gizmos.DrawWireSphere(punchPoint.position, punchRange);
    }
}