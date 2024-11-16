using UnityEngine;
using TheKiwiCoder;

public class JumpAction : ActionNode
{
    public float jumpUpForce = 15f; // Lực nhảy lên
    public float jumpTowardPlayerForce = 10f; // Lực nhảy hướng về người chơi
    public float jumpCooldown = 1f; // Thời gian chờ giữa các lần nhảy

    private Rigidbody2D rb;
    private Transform player;
    private Animator animator;

    private bool isJumping = false;  // Kiểm tra trạng thái nhảy
    private bool hasLanded = false;  // Kiểm tra trạng thái đã đáp đất chưa
    private float lastJumpTime = 0f; // Thời điểm boss thực hiện lần nhảy cuối

    protected override void OnStart()
    {
        var agent = context.gameObject; // context.gameObject là gameObject mà node đang xử lý
        rb = agent.GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D từ agent
        animator = agent.GetComponent<Animator>(); // Lấy Animator từ agent

        if (rb == null)
        {
            Debug.LogError("Không tìm thấy Rigidbody2D trên agent.");
        }

        if (animator == null)
        {
            Debug.LogError("Không tìm thấy Animator trên agent.");
        }

        // Lấy đối tượng người chơi từ scene
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Không tìm thấy người chơi trong scene.");
        }
    }

    protected override void OnStop()
    {
        // Làm sạch hoặc reset nếu cần thiết
    }

    protected override State OnUpdate()
    {
        if (Time.time < lastJumpTime + jumpCooldown)
        {
            return State.Running; // Đợi đến khi cooldown kết thúc
        }

        if (rb != null)
        {
            if (!isJumping)
            {
                // Animation chuẩn bị nhảy
                animator.SetTrigger("PrepareJump");

                // Tính toán khoảng cách và hướng từ boss tới người chơi
                Vector2 direction = (player.position - rb.transform.position).normalized;

                // Tạo lực nhảy lên
                rb.velocity = new Vector2(direction.x * jumpTowardPlayerForce, jumpUpForce);
                animator.SetTrigger("Jumping"); // Animation nhảy
                isJumping = true; // Đánh dấu rằng boss đang nhảy lên
                lastJumpTime = Time.time; // Cập nhật thời điểm nhảy cuối
                hasLanded = false; // Đặt lại trạng thái đã đáp đất
                return State.Running; // Đang nhảy
            }
            else
            {
                // Kiểm tra nếu boss đang trên không và chuẩn bị chạm đất
                if (Mathf.Abs(rb.velocity.y) < 0.1f && rb.velocity.y <= 0f && !hasLanded)
                {
                    // Animation đáp đất ngay lập tức khi boss chạm đất
                    animator.SetTrigger("Landing");
                    hasLanded = true; // Đánh dấu đã đáp đất
                    isJumping = false; // Đặt lại trạng thái nhảy
                    return State.Success; // Hoàn thành hành động
                }
            }
        }

        return State.Running; // Nhảy đang diễn ra
    }
}
