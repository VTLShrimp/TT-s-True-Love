using UnityEngine;

public class Boss_Walk : StateMachineBehaviour
{
    public float attackRange = 5f; // Range within which the boss will attack the player
    public float speed = 10f;
    Transform player;
    Rigidbody2D rb;
    Boss boss;

    // OnStateEnter is called when the state starts
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player")?.transform; // Find the player object
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player object has the 'Player' tag.");
        }

        rb = animator.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component of the boss
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found! Make sure the animator is on a GameObject with a Rigidbody2D.");
        }

        boss = animator.GetComponent<Boss>(); // Get the Boss script
        if (boss == null)
        {
            Debug.LogError("Boss script not found! Make sure the animator is on a GameObject with the Boss script.");
        }
    }


    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.playerInzone)
        {
            boss.LookatPlayer(); // Boss looks at the player

            // Move the boss towards the player
            Vector2 target = new(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.transform.position = newPos;

            // Check if the boss is within attack range
            if (Vector2.Distance(player.position, rb.position) <= attackRange)
            {
                animator.SetTrigger("Attack"); // Trigger the attack action
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack"); // Reset the attack trigger after the boss attacks
    }
}