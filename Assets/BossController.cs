using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Animator animator; // Boss animator
    public Transform player; // Reference to player position
    public float attackRange = 3f;
    public float regularAttackCooldown = 2f;
    public float groundTremorCooldown = 5f;
    public float meteorFallCooldown = 10f;

    private bool isAttacking = false;

    void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                StartCoroutine(SwingSword());
                yield return new WaitForSeconds(regularAttackCooldown);
            }
            else
            {
                int skillChoice = Random.Range(0, 3);
                if (skillChoice == 0)
                {
                    yield return StartCoroutine(SwingSword());
                }
                else if (skillChoice == 1)
                {
                    yield return StartCoroutine(GroundTremor());
                }
                else if (skillChoice == 2)
                {
                    yield return StartCoroutine(MeteorFall());
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    // Regular Attack: Swing Sword
    IEnumerator SwingSword()
    {
        isAttacking = true;
        animator.SetTrigger("SwingSword");
        yield return new WaitForSeconds(0.5f); // Delay for animation to play

        // Check if player is in range and deal damage
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            // Add damage logic to player here
            player.GetComponent<PlayerHealth>().TakeDamage(10);
        }

        yield return new WaitForSeconds(regularAttackCooldown);
        isAttacking = false;
    }

    // Special Skill 1: Ground Tremor
    IEnumerator GroundTremor()
    {
        isAttacking = true;
        animator.SetTrigger("Spit");
        yield return new WaitForSeconds(0.5f); // Delay for animation to play

        // Instantiate projectile toward the player
        Vector2 direction = (player.position - transform.position).normalized;
        GameObject projectile = Instantiate(spitProjectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;

        // Wait for cooldown
        yield return new WaitForSeconds(groundTremorCooldown);
        isAttacking = false;
    }

    // Special Skill 2: Meteor Fall
    IEnumerator MeteorFall()
    {
        isAttacking = true;
        animator.SetTrigger("Roar");
        yield return new WaitForSeconds(1f); // Delay for roar animation

        // Boss stands still
        int meteorCount = 5;
        for (int i = 0; i < meteorCount; i++)
        {
            Vector2 randomPosition = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            Instantiate(meteorPrefab, randomPosition, Quaternion.identity);

            yield return new WaitForSeconds(0.3f); // Delay between each meteor
        }

        yield return new WaitForSeconds(meteorFallCooldown);
        isAttacking = false;
    }
}
