using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private float currentHealth;
    public Animator animator;
    public bool dead;
    public ReSpawn respawnScript;
    public float knockbackForce = 10f; // Ensure this is set

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        respawnScript = GetComponent<ReSpawn>();
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
                StartCoroutine(RespawnPlayer());
            }
        }
    }


    private void Knockback(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }
    }


    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        respawnScript.Respawn();
        currentHealth = maxHealth;
        dead = false;
        animator.ResetTrigger("die");
     
        animator.SetTrigger("idle" );
        // Find the boss and reset it
        BossHealth boss = FindObjectOfType<BossHealth>();
            if (boss != null)
            {
                boss.ResetBoss();
            }
        }


    }
