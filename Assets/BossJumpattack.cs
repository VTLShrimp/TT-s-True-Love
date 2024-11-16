using System.Collections;
using UnityEngine;
using Cinemachine; // Nhớ thêm namespace của Cinemachine

public class BossJumpAttack : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public float jumpSpeed = 15f; // Jump speed
    public float jumpHeight = 5f; // Jump height
    public float jumpCooldown = 2f; // Jump cooldown time
    public float stopTimeAfterLand = 1.5f; // Time to stop after landing

    private bool isJumping = false; // Check if boss is jumping
    private bool canJump = true; // Check if boss can jump again
    private Vector3 targetPosition; // Target position for the boss to jump to
    private bool isFacingRight = true; // Check if boss is facing right or left
    private Animator animator; // Animator to control animations

    private CinemachineImpulseSource impulseSource; // Reference to Cinemachine Impulse Source

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; // Find player if not assigned
        }

        animator = GetComponent<Animator>(); // Get reference to the Animator

        // Get the Cinemachine Impulse Source component from the boss
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        // Check if the boss is not jumping and can jump
        if (!isJumping && canJump)
        {
            float jumpChance = Random.Range(0f, 1f); // Random chance for jumping

            if (jumpChance <= 0.2f) // 20% chance to jump
            {
                StartCoroutine(JumpToPlayer());
            }
        }
    }

    IEnumerator JumpToPlayer()
    {
        isJumping = true;
        canJump = false; // Prevent further jumping until cooldown is over

        // Trigger jump animation immediately when the boss starts preparing to jump
        animator.SetTrigger("Jump");

        // Calculate the target position to jump to
        targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);

        // Save the starting position for the jump
        Vector3 startPosition = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < jumpCooldown)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the jump height using a sine function for a smooth arc
            float height = Mathf.Sin(Mathf.PI * (elapsedTime / jumpCooldown)) * jumpHeight;

            // Move the boss horizontally and adjust its height
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / jumpCooldown);
            transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);

            yield return null;
        }

        // Ensure the boss reaches the correct target position
        transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        // Trigger landing animation once the boss reaches the ground
        animator.SetTrigger("Land");

        // Trigger the camera shake effect when the boss lands
        impulseSource.GenerateImpulse(); // Generate an impulse to trigger camera shake

        // Wait for the specified time after landing
        yield return new WaitForSeconds(stopTimeAfterLand);

        // Flip the boss to face the player after landing
        Flip();

        // Trigger the idle (Wait) animation after stopping
        animator.SetTrigger("Wait");

        // Allow the boss to jump again
        isJumping = false;
        canJump = true;
    }

    private void Flip()
    {
        // Check if the boss needs to flip direction
        if ((isFacingRight && player.position.x < transform.position.x) ||
            (!isFacingRight && player.position.x > transform.position.x))
        {
            isFacingRight = !isFacingRight; // Reverse the facing direction
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f; // Flip the boss on the X axis
            transform.localScale = localScale; // Apply the flip
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // If boss touches the ground
        {
            canJump = true; // Allow the boss to jump again
        }
    }
}
