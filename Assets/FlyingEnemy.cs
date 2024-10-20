using System.Collections;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed;
    public float detectionRadius = 5f;
    public float attackRange = 1.5f; // Adjust the attack range to a suitable value
    public int attackDamage = 10;
    public float attackInterval = 1.5f;
    private float nextAttackTime;

    private GameObject player;
    public bool chase = false;
    public Transform startPos;
    public Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        chase = distanceToPlayer <= detectionRadius;

        if (chase)
            Chase();
        else
            ReturnToStartPos();

        Flip();
    }

    private void ReturnToStartPos()
    {
        transform.position = Vector2.MoveTowards(transform.position, startPos.position, speed * Time.deltaTime);

        // Flip the enemy to face the starting position
        if (transform.position.x > startPos.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void Chase()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // Stop moving closer once within attack range
        if (distanceToPlayer > attackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }

        // Initiate attack if within attack range and cooldown allows
        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackInterval;
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");

        // Apply damage to the player
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("FlyingEnemy attacked the player!");
        }
    }

    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
