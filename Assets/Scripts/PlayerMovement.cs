using System;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 10f;
    public float jumpingPower = 16f;
    private bool isFacingRight = true;
    public Animator animator;
    private int jumpcount = 0;
    public int maxhealth = 100;
    public int currenthealth;
    public int damage = 20;
    private bool isAttacking = false;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public Transform attackPoint;
    private float attackCooldownTimer = 0f;
    public float attackCooldownDuration = 1f;

    private RangeWeapon rangeWeapon;
    public float bowcooldown = 1f;

    //yes testing//
    // public HealthBar healthBar;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        currenthealth = maxhealth;
        Application.targetFrameRate = 60;
        if (rangeWeapon == null)
        {
            rangeWeapon = GetComponent<RangeWeapon>();
        }

    }


    void Update()
    {
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        if (bowcooldown > 0)
        {
            bowcooldown -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0)
        {
            Attack();
        }

        if (Input.GetMouseButtonDown(1) && bowcooldown <= 0)
        {
            Bow();
        }
        if (!isAttacking)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                if (jumpcount < 1)
                {
                    jumpcount++;
                }
            }
            else if (Input.GetButtonDown("Jump") && jumpcount < 1 && IsGrounded() == false)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                jumpcount++;
            }
            animator.SetBool("IsGround", IsGrounded());
            Flip();
            if (IsGrounded())
            {
                jumpcount = 0;
            }
            animator.SetInteger("Speed", (int)Mathf.Abs(horizontal));
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

    }


    private void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            // enemy.GetComponent<BoarScript>().TakeDamage(damage);    
            Debug.Log("We hit " + enemy.name);
        }
        attackCooldownTimer = attackCooldownDuration;
    }

    private void Bow()
    {
        animator.SetTrigger("Bow");
        isAttacking = true;
        rangeWeapon.Shoot();
        Invoke("EndAttack", 1f);
        attackCooldownTimer = bowcooldown;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}