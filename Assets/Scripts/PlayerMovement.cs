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
<<<<<<< HEAD

    // Reference to the HealthBar script
    public HealthBar healthBar;
=======
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    private float dashTime;
    private bool isDashing;
    public float dashCooldown = 1f;
    private float dashCooldownTimer;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);
>>>>>>> LamLmao2

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    void Start()
    {
        // Initialize health and health bar
        currenthealth = maxhealth;
<<<<<<< HEAD
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxhealth); // Set the health bar max health
        }
        else
        {
            Debug.LogError("HealthBar not assigned in the Inspector!");
        }

        // Optional: Set frame rate
=======
>>>>>>> LamLmao2
        Application.targetFrameRate = 60;
    }

    void Update()
    {
<<<<<<< HEAD
        // Get horizontal input
        horizontal = Input.GetAxisRaw("Horizontal");

        // Jump logic
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                jumpcount = 0; // Reset jump count when on the ground
            }
            else if (jumpcount < 1) // Allow one mid-air jump
=======
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0)
        {
            isDashing = true;
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;
        }
        if (!isDashing)
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
>>>>>>> LamLmao2
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
<<<<<<< HEAD

        // Animator ground state
        animator.SetBool("IsGround", IsGrounded());

        // Handle flipping the character based on movement direction
        Flip();

        // Reset jump count if grounded
        if (IsGrounded())
        {
            jumpcount = 0;
        }

        // Update animator speed (used for walking/running animation)
        animator.SetInteger("Speed", (int)Mathf.Abs(horizontal));

        // Sword usage with "G" key
        if (Input.GetKeyDown(KeyCode.G))
        {
            animator.SetBool("IsUsingSword", false); // Example logic for sword usage
        }

        // Example damage taken for testing health bar
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20); // Reduce health by 20 for testing
        }
=======
        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }

>>>>>>> LamLmao2
    }

    private void FixedUpdate()
    {
<<<<<<< HEAD
        // Move the player based on input
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

=======
        if (isDashing)
        {
            rb.velocity = new Vector2(transform.localScale.x * dashSpeed, rb.velocity.y);
            dashTime -= Time.fixedDeltaTime;
            if (dashTime <= 0)
            {
                EndDash();
            }
        }
        else
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }
>>>>>>> LamLmao2
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
<<<<<<< HEAD

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword") && Input.GetKey(KeyCode.E))
        {
            animator.SetBool("IsUsingSword", true);
            Destroy(collision.gameObject); // Pickup and destroy sword
        }
    }

    public void TakeDamage(int damage)
    {
        // Reduce health and clamp between 0 and maxhealth
        currenthealth -= damage;
        currenthealth = Mathf.Clamp(currenthealth, 0, maxhealth);

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.TakeDamage(damage);
        }

        // Check if current health is 0 and destroy the player
        if (currenthealth <= 0)
        {
            Destroy(gameObject); // Destroy this GameObject (the player)
        }
    }
}
=======
    private void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        dashCooldownTimer = dashCooldown;
        // animator.SetTrigger("Dash");
    }
    private void EndDash()
    {
        isDashing = false;
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

}
>>>>>>> LamLmao2
