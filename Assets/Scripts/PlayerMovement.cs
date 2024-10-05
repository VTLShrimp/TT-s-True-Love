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

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    void Start()
    {
        currenthealth = maxhealth;
        Application.targetFrameRate = 60;
    }


    void Update()
    {
    
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
        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }

    }

    private void FixedUpdate()
    {
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