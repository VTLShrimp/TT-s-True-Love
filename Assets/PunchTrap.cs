using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchTrap : MonoBehaviour
{
    public float punchInterval = 2f; // Thời gian giữa các cú đấm
    public int punchDamage = 10; // Sát thương của cú đấm
    public float punchForce = 5f; // Lực của cú đấm
    public Transform punchPoint; // Điểm xuất phát của cú đấm
    public float punchRange = 1f; // Phạm vi cú đấm
    public LayerMask playerLayer; // Lớp của người chơi
    public AudioClip punchSound; // Âm thanh của cú đấm
    private Animator animator;
    private AudioSource audioSource;
    private bool isPlayerInRange = false; // Cờ để kiểm tra người chơi có trong phạm vi không

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu đối tượng va chạm là người chơi
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isPlayerInRange) // Đảm bảo chỉ bắt đầu nếu chưa trong phạm vi
            {
                isPlayerInRange = true;
                StartCoroutine(PunchRoutine()); // Bắt đầu chu trình đấm khi người chơi vào phạm vi
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Khi người chơi rời khỏi phạm vi, dừng chu trình đấm
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            StopCoroutine(PunchRoutine()); // Dừng bẫy hoạt động khi người chơi rời đi
        }
    }

    IEnumerator PunchRoutine()
    {
        while (isPlayerInRange) // Chỉ đấm khi người chơi trong phạm vi
        {
            yield return new WaitForSeconds(punchInterval);
            Punch();
        }
    }

    void Punch()
    {
        // Chạy hoạt ảnh cú đấm
        if (animator != null)
        {
            animator.SetTrigger("Punch");
        }
        // Phát âm thanh cú đấm
        if (audioSource != null && punchSound != null)
        {
            audioSource.PlayOneShot(punchSound);
        }
        // Phát hiện người chơi trong phạm vi
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(punchPoint.position, punchRange, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Gây sát thương cho người chơi
                Vector2 knockbackDirection = (player.transform.position - punchPoint.position).normalized;
                playerHealth.TakeDamage(punchDamage);
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Tác động lực lên người chơi
                    ApplyForce(rb, player.transform.position);
                }
            }
        }

        void ApplyDamage(PlayerHealth playerHealth)
        {
            playerHealth.TakeDamage(punchDamage);
        }

        void ApplyForce(Rigidbody2D rb, Vector3 playerPosition)
        {
            Vector2 direction = playerPosition - punchPoint.position;
            rb.AddForce(direction.normalized * punchForce, ForceMode2D.Impulse);
        }

        void OnDrawGizmosSelected()
        {
            if (punchPoint == null)
                return;
            Gizmos.DrawWireSphere(punchPoint.position, punchRange);
        }
    }
}
