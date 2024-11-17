using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask enemyLayers;
    public Animator animator;
    public Image manabar;
    public bool isAttacking = false;
    public static PlayerAttack instance;
    public Transform attackPoint;
    public float attackRange = 0.5f;  // Ground attack range
    public float airAttackRange = 1.0f;  // Air attack range
    public int groundDamage = 20;  // Ground attack damage
    public int airDamage = 15;  // Air attack damage
    public float attackCooldown = 0.5f;  // Time between attacks
    private float nextAttackTime = 0f;
    public int maxMana = 100;
    public int currentMana;
    public GameObject forgemenu;
    public GameObject priestessmenu;
    private PlayerMovement playerMovement;
    private Coroutine attackCoroutine; // Variable to hold the attack coroutine
    private bool canAttack = true; // Track if the player can attack

    public GameObject swordWavePrefab; // Prefab for the sword wave
    public Transform spawnPoint;       // Spawn point for the sword wave
    public int swordWaveDamage = 20;   // Damage of the sword wave

    // Sound Effects
    public AudioClip groundAttackSound;
    public AudioClip airAttackSound;
    public AudioClip swordWaveSound;

    private AudioSource audioSource;
    public void InterruptAttack()
    {
        if (isAttacking)
        {
            isAttacking = false; // Reset the attacking state
            animator.ResetTrigger("GroundAttack"); // Stop ground attack animation
            animator.ResetTrigger("AirAttack");   // Stop air attack animation

            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine); // Stop the attack coroutine
                attackCoroutine = null;
            }

            Debug.Log("Attack interrupted!");
        }
    }


    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        currentMana = maxMana;
        playerMovement = GetComponent<PlayerMovement>();  // Get reference to PlayerMovement
    }

    void Update()
    {
        if (Time.time >= nextAttackTime && Input.GetMouseButtonDown(0) && canAttack && !isAttacking)
        {
            PerformAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
        if (Input.GetMouseButtonDown(1) && currentMana >=20)  // Right mouse button
        {
            LaunchSwordWave();
            currentMana -= 20;
            UpdateManaBar();
        }
    }

    private void PerformAttack()
    {
        isAttacking = true;

        if (playerMovement.IsGrounded())
        {
            animator.SetTrigger("GroundAttack");
            PlaySound(groundAttackSound);
            attackCoroutine = StartCoroutine(HandleGroundAttack());
        }
        else
        {
            animator.SetTrigger("AirAttack");
            PlaySound(airAttackSound);
            attackCoroutine = StartCoroutine(HandleAirAttack());
        }
    }

    private void LaunchSwordWave()
    {
        // Spawn sword wave prefab at spawnPoint position
        GameObject swordWave = Instantiate(swordWavePrefab, spawnPoint.position, Quaternion.identity);

        // Get SwordWave component from the spawned sword wave
        SwordWave wave = swordWave.GetComponent<SwordWave>();

        // Set damage for the sword wave
        wave.damage = swordWaveDamage;

        // Determine direction based on character's facing direction
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        wave.SetDirection(direction);

        // Play sword wave sound
        PlaySound(swordWaveSound);
    }

    private IEnumerator HandleGroundAttack()
    {
        yield return new WaitForSeconds(0.1f);  // Sync with animation

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            IHealth enemyHealth = enemy.GetComponent<IHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(groundDamage);
                Debug.Log("Ground attack hit " + enemy.name);
            }
        }

        yield return new WaitForSeconds(0.3f);  // Wait for attack to complete
        isAttacking = false;
    }

    private IEnumerator HandleAirAttack()
    {
        yield return new WaitForSeconds(0.1f);  // Sync with animation

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, airAttackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            IHealth enemyHealth = enemy.GetComponent<IHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(airDamage);
                Debug.Log("Air attack hit " + enemy.name);
            }
        }

        yield return new WaitForSeconds(0.3f);  // Wait for attack to complete
        isAttacking = false;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void UpdateManaBar()
    {
        if (manabar != null)
        {
            manabar.fillAmount = (float)currentMana / maxMana;
        }
    }
}
