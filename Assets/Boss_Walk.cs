using UnityEngine;

public class Boss_Walk : StateMachineBehaviour
{
    public float attackRange = 5f; // Phạm vi để boss tấn công người chơi
    public float speed = 10f;
    Transform player;
    Rigidbody2D rb;
    Boss boss;




    // OnStateEnter is called khi bắt đầu trạng thái di chuyển
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform; // Tìm đối tượng người chơi
        rb = animator.GetComponent<Rigidbody2D>(); // Lấy rigidbody của boss
        boss = animator.GetComponent<Boss>(); // Lấy script Boss
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookatPlayer(); // Boss nhìn về phía người chơi
        
        // Di chuyển boss về phía người chơi
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.transform.position = newPos;

        // Kiểm tra nếu boss ở trong phạm vi tấn công
        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("Attack"); // Kích hoạt hành động tấn công
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack"); // Reset lại trigger sau khi boss tấn công
    }
}
