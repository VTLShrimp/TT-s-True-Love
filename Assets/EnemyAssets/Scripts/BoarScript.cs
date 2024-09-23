using UnityEngine;

public class BoarScript : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    public float speed;
    private Transform currentTarget;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentTarget = PointB;
        animator.SetBool("IsRunning", true);
    }

    void Update()
    {
        // Calculate direction towards the current target
        Vector2 direction = (currentTarget.position - transform.position).normalized;
        
        // Set the velocity to move towards the target
        rb.velocity = direction * speed;

        // Check the distance to the current target
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f)
        {
            // Switch target when close enough
            if (currentTarget == PointB)
            {
                currentTarget = PointA;
            }
            else
            {
                currentTarget = PointB;
            }
        }
    }
}