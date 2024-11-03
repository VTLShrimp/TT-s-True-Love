using BarthaSzabolcs.Tutorial_SpriteFlash;
using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour, IHealth
{
    public int maxHealth;
    private float currentHealth;
    public bool dead;
    public float dieAnimationLength = 2.0f; // Set this to the length of your "die" animation
    public DetectionZone zone;
    private Vector2 startPosition;
    public bool isInvulnerable = false;

    // Use SerializeField to expose these variables in the Inspector
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D collider;
    [SerializeField] private SimpleFlash flash; // Reference to SimpleFlash script
    public Transform player; // Tham chiếu đến người chơi
    public float aggroRange = 5.0f;

    private bool isEnraged; // Variable to track enrage status

    void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;
        isEnraged = false; // Initialize enrage status

        // Initialize components if not assigned via the Inspector
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator not found on Boss!");
            }
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (collider == null)
        {
            collider = GetComponent<Collider2D>();
        }

        if (flash == null)
        {
            flash = GetComponent<SimpleFlash>();
            if (flash == null)
            {
                Debug.LogError("SimpleFlash not found on Boss!");
            }
        }
    }

    public void ResetBoss()
    {
        transform.position = startPosition;
        currentHealth = maxHealth;
        collider.enabled = true;
        rb.velocity = Vector2.zero; // Reset velocity to ensure no movement
        rb.gravityScale = 1; // Restore gravity scale if altered
        isEnraged = false; // Reset enrage status
        animator.SetBool("IsEnrage", false); // Reset animator enrage status
    }
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Debug.Log("Distance to player: " + distanceToPlayer); // Kiểm tra khoảng cách

        if (distanceToPlayer < aggroRange)
        {
            if (currentHealth < 500 && !isEnraged)
            {
                isEnraged = true;
                animator.SetTrigger("IsEnraged");
                Debug.Log("Boss is enraged due to proximity to player!");
            }
        }
    }


    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
        {
            Debug.Log("Boss is invulnerable, no damage taken!");
            return;
        }

        currentHealth -= damage;

        Debug.Log("Boss takes damage: " + damage + ". Current health: " + currentHealth);

        if (currentHealth < 500 && !isEnraged)
        {
            isEnraged = true; // Đánh dấu boss là làm giận
            Debug.Log("Triggering enrage state!"); // Thông báo trước khi gọi trigger
            animator.SetTrigger("IsEnraged");
        }


        if (currentHealth > 0)
        {
            flash.Flash();
        }
        else
        {
            if (!dead)
            {
                dead = true;
                animator.SetTrigger("die");
                collider.enabled = false;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
                if (zone != null)
                {
                    Destroy(zone.gameObject);
                }
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
