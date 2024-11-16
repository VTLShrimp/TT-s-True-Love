using UnityEngine;
using TheKiwiCoder;

public class FacePlayer : ActionNode
{
    private float baseScaleX;
    private Transform player;
    private bool isGrounded;  // Track whether the boss is on the ground
    private Rigidbody2D rb;

    protected override void OnStart()
    {
        var agent = context.gameObject;
        baseScaleX = agent.transform.localScale.x;  // Get the base scale for flipping

        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }

        // Get the Rigidbody2D component to check ground status
        rb = agent.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on agent.");
        }
    }

    protected override void OnStop()
    {
        // Optional: Reset any flags if needed
    }

    protected override State OnUpdate()
    {
        if (player != null && rb != null)
        {
            // Check if the boss is grounded (using a simple check for now)
            isGrounded = Mathf.Abs(rb.velocity.y) < 0.1f;  // Boss is grounded if the vertical speed is near zero

            // Only flip the boss when it is grounded (not jumping)
            if (isGrounded)
            {
                // Get the current position of the boss and the player
                Vector3 directionToPlayer = player.position - context.transform.position;

                // Flip the boss based on the player's position
                if (directionToPlayer.x > 0) // Player is to the right
                {
                    // Ensure the boss faces right
                    context.transform.localScale = new Vector3(baseScaleX, context.transform.localScale.y, context.transform.localScale.z);
                }
                else if (directionToPlayer.x < 0) // Player is to the left
                {
                    // Ensure the boss faces left
                    context.transform.localScale = new Vector3(-baseScaleX, context.transform.localScale.y, context.transform.localScale.z);
                }
            }

            return State.Success;  // Action completed successfully
        }

        return State.Running;  // Action is still running (if you need to update things over time)
    }
}
