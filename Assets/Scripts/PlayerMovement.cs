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

    // Reference to the HealthBar script
    public HealthBar healthBar;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        // Initialize health and health bar
        currenthealth = maxhealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxhealth); // Set the health bar max health
        }
        else
        {
            Debug.LogError("HealthBar not assigned in the Inspector!");
        }

        // Optional: Set frame rate
        Application.targetFrameRate = 60;
    }

    void Update()
    {
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
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                jumpcount++;
            }
        }

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
    }

    private void FixedUpdate()
    {
        // Move the player based on input
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
