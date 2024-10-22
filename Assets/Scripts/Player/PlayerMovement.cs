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
    private Vector2 wallJumpingPower = new(8f, 16f);
    private bool isCrouching = false;
    private bool isDropping = false;
    public Collider2D standingCollider;
    public Collider2D crouchingCollider;
    private bool isDodging = false;
    public float dodgecooldown = 1f;
    private float dodgecooldownTimer;
    public float dodgespeed = 20f;
    public float dodgeduration = 0.9f;
    private float dodgetime;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform platfromCheck;
    [SerializeField] private LayerMask platfromLayer;
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
        if (dodgecooldownTimer > 0)
        {
            dodgecooldownTimer -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0 && !isCrouching)
        {
            animator.SetBool("IsDash",true);    
            isDashing = true;
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;
            rb.gravityScale = 0f;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && dodgecooldownTimer <= 0)
        {
            isDodging = true;
            dodgetime = dodgeduration;
            dodgecooldownTimer = dodgecooldown;
            standingCollider.enabled = false;
            crouchingCollider.enabled = true;
        }
        if (!isDashing)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump") && IsGrounded() && !isCrouching && !isWallSliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                animator.SetBool("IsGround",false);

                if (jumpcount < 1)
                {
                    jumpcount++;
                }
            }
            else if (Input.GetButtonDown("Jump") && jumpcount < 1 && !IsGrounded() && !isCrouching && !isWallSliding ) 
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                animator.SetBool("IsGround", false);
                jumpcount++;
            }
            if (Input.GetKey(KeyCode.S) && !isDropping)
            {
                Crouch();
            }
            else if (!Input.GetKey(KeyCode.S) && isCrouching)
            {
                StandUp();
            }

            // Drop-down logic
            if (Input.GetKeyDown(KeyCode.S) && isCrouching && !isDropping && IsPlatfrom())
            {
                DropDown();
            }

            animator.SetBool("IsGround", IsGrounded());
            Flip();
            if (IsGrounded())
            {
                jumpcount = 0;
                animator.SetBool("IsGround",true);
               // animator.SetBool("IswallSide",false) ;

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
            rb.velocity = new Vector2(transform.localScale.x * dashSpeed,0f );
            dashTime -= Time.fixedDeltaTime;
            if (dashTime <= 0)
            {
                EndDash();
            }
        }
        else if (isDodging)
        {
            rb.velocity = new Vector2(transform.localScale.x * dodgespeed, rb.velocity.y);
            dodgetime -= Time.fixedDeltaTime;
            if (dodgetime <= 0)
            {
                EndDodge();
            }
        }
        else
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

  
    private bool IsPlatfrom()
    {
        return Physics2D.OverlapCircle(platfromCheck.position, 0.2f, platfromLayer);
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
        animator.SetBool("IsDash", false);
        rb.gravityScale = 5;
    }

    private void EndDodge()
    {
        isDodging = false;
        standingCollider.enabled = true;
        crouchingCollider.enabled = false;
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            animator.SetBool("IswallSide",true);
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
            animator.SetBool("IswallSide",false);
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

        // Allow wall jump only if the counter is active and the player isn't already jumping or sliding
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f && !isWallSliding && !isWallJumping)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    void Crouch()
    {
        if (!isCrouching)
        {
            isCrouching = true;
            standingCollider.enabled = false;
            crouchingCollider.enabled = true;
            // animator.SetBool("IsCrouching", true);
        }
    }
    void StandUp()
    {
        if (isCrouching)
        {
            isCrouching = false;
            standingCollider.enabled = true;
            crouchingCollider.enabled = false;
            // animator.SetBool("IsCrouching", false);
        }
    }
    void DropDown()
    {
        if (!isDropping)
        {
            isDropping = true;
            standingCollider.enabled = false;
            crouchingCollider.enabled = false;
            Invoke(nameof(EnableColliders), 0.2f);
        }
    }
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }


    void EnableColliders()
    {
        standingCollider.enabled = true;
        isDropping = false;
        isCrouching = false;
    }
}