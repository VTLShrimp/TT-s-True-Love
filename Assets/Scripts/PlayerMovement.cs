using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 9f;
    public float jumpingPower = 16f;
    private bool isFacingRight = true;
    public Animator animator;
    private int jumpcount = 0;
    public int maxhealth = 100;
    public int currenthealth;
    public HealthBar healthBar;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        currenthealth = maxhealth;
        healthBar.SetMaxHealth(maxhealth);
        Application.targetFrameRate = 60;

    }


    void Update()
    {
        

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
           if(jumpcount < 1)
            {
                jumpcount++;
            }
        }
        else if (Input.GetButtonDown("Jump") && jumpcount < 1 && IsGrounded()==false)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpcount++;
        }
        animator.SetBool("IsGround", IsGrounded());
        Flip();
        if(IsGrounded())
        {
            jumpcount = 0;
        }
        animator.SetInteger("Speed", (int)Mathf.Abs(horizontal));

        if (Input.GetKey(KeyCode.G))
        {
            animator.SetBool("IsUsingSword", false);
        }
       

    }

    private void FixedUpdate()
    {
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
            animator.SetBool("IsUsingSword",true);
            Destroy(collision.gameObject);
        }
    }
} 